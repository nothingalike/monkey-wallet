﻿using CardanoSharp.Koios.Client;
using CardanoSharp.Koios.Client.Contracts;
using CardanoSharp.Wallet;
using CardanoSharp.Wallet.Enums;
using CardanoSharp.Wallet.Extensions.Models;
using CardanoSharp.Wallet.Models.Keys;
using MonkeyWallet.Core.Common;
using MonkeyWallet.Core.Data;
using MonkeyWallet.Core.Data.Models;
using CardanoSharpAsset = CardanoSharp.Wallet.Models.Asset;
using CardanoSharp.Wallet.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Refit;
using Utxo = CardanoSharp.Wallet.Models.Utxo;

namespace MonkeyWallet.Core.Services
{
    public interface IAddressService
    {
        Task<string?> GetWalletAddress(int? addressIndex = null);
        Task<string?> GetChangeAddress(int? addressIndex = null);
        Task<List<Utxo>> GetUtxos(string address);
        Task<Wallet> AddWallet(string name, string recoveryPhrase, string spendingPassword);
        Task<AddressTransaction[]> GetWalletTransactions(AddressTransactionRequest request);
    }

    public class AddressService : IAddressService
    {
        private readonly IAddressClient _addressClient;
        private IMnemonicService _mnemonicService;
        private IWalletDatabase _walletDatabase;
        private IWalletKeyDatabase _walletKeyDatabase;
        private readonly ILogger<IAddressClient> _logger;

        public AddressService(
            IAddressClient addressClient,
            IMnemonicService mnemonicService,
            IWalletDatabase walletDatabase,
            IWalletKeyDatabase walletKeyDatabase, ILogger<IAddressClient> logger)
        {
            _addressClient = addressClient;
            _mnemonicService = mnemonicService;
            _walletDatabase = walletDatabase;
            _walletKeyDatabase = walletKeyDatabase;
            _logger = logger;
        }

        public async Task<string?> GetWalletAddress(int? addressIndex = null)
        {
            addressIndex ??= 0;

            var wallet = await _walletKeyDatabase.GetFirstAsync();
            var publicKey = JsonConvert.DeserializeObject<PublicKey>(wallet.Vkey);
            if (publicKey is null)
                throw new Exception("Wallet not found");
            var payment = publicKey
                .Derive(RoleType.ExternalChain)
                .Derive((int)addressIndex);

            var stake = publicKey
                .Derive(RoleType.Staking)
                .Derive(0);

            var address = new CardanoSharp.Wallet.AddressService()
                .GetBaseAddress(payment.PublicKey, stake.PublicKey, NetworkType.Testnet);

            return address.ToString();
        }

        public async Task<string?> GetChangeAddress(int? addressIndex = null)
        {
            addressIndex ??= 0;

            var wallet = await _walletKeyDatabase.GetFirstAsync();
            var publicKey = JsonConvert.DeserializeObject<PublicKey>(wallet.Vkey);
            if (publicKey is null)
                throw new Exception("Wallet not found");
            var payment = publicKey
                .Derive(RoleType.InternalChain)
                .Derive((int)addressIndex);

            var stake = publicKey
                .Derive(RoleType.Staking)
                .Derive(0);

            var address = new CardanoSharp.Wallet.AddressService()
                .GetBaseAddress(payment.PublicKey, stake.PublicKey, NetworkType.Testnet);

            return address.ToString();
        }

        public async Task<List<Utxo>> GetUtxos(string address)
        {
            try
            {
                var addressResponse = (await _addressClient.GetAddressInformation(new AddressBulkRequest() { Addresses = new List<string>() { address } }));
                var addressInfo = addressResponse.Content;
                var utxos = new List<Utxo>();

                foreach (var ai in addressInfo.SelectMany(x => x.UtxoSets))
                {
                    if (ai is null) continue;
                    var utxo = new Utxo()
                    {
                        TxIndex = ai.TxIndex,
                        TxHash = ai.TxHash,
                        Balance = new Balance()
                        {
                            Lovelaces = ulong.Parse(ai.Value)
                        }
                    };

                    var assetList = new List<CardanoSharpAsset>();
                    foreach (var aa in ai.AssetList)
                    {
                        assetList.Add(new CardanoSharpAsset()
                        {
                            Name = aa.AssetName,
                            PolicyId = aa.PolicyId,
                            Quantity = long.Parse(aa.Quantity)
                        });
                    }

                    utxo.Balance.Assets = assetList;
                    utxos.Add(utxo);
                }

                return utxos;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public async Task<Wallet> AddWallet(string name, string recoveryPhrase, string spendingPassword)
        {
            // Restore a Mnemonic
            var mnemonic = _mnemonicService.Restore(recoveryPhrase);
            Wallet? newlyCreatedWallet;

            if (await _walletDatabase.ExistsAsync(name))
            {
                throw new Exception("Wallet already exists");
            }

            int accountIx = 0;

            await _walletDatabase.SaveAsync(new Wallet
            {
                Name = name,
                WalletType = (int)WalletType.HD,
            });

            newlyCreatedWallet = await _walletDatabase.GetByNameAsync(name);

            if (newlyCreatedWallet == null)
            {
                throw new Exception("Unable to create wallet.");
            }

            var accountNode = mnemonic.GetMasterNode()
                .Derive(PurposeType.Shelley)
                .Derive(CoinType.Ada)
                .Derive(accountIx);
            accountNode.SetPublicKey();

            await _walletKeyDatabase.SaveWalletAsync(new WalletKey
            {
                WalletId = newlyCreatedWallet.Id,
                KeyType = (int)KeyType.Account,
                Skey = JsonConvert.SerializeObject(accountNode.PrivateKey.Encrypt(spendingPassword)),
                Vkey = JsonConvert.SerializeObject(accountNode.PublicKey),
                KeyIndex = accountIx,
                AccountIndex = accountIx
            });


            var wallet = await _walletDatabase.GetByNameAsync(name);
            wallet.Keys = await _walletKeyDatabase.GetWalletKeysAsync(wallet.Id);
            return wallet;
        }

        public async Task<AddressTransaction[]> GetWalletTransactions(AddressTransactionRequest request)
        {
            AddressTransaction[]? walletTransactions;
            try
            {
                walletTransactions = (await _addressClient.GetAddressTransactions(request)).Content;
            }
            catch (Exception e)
            {
                _logger.LogError("There was an error getting the transactions for the wallet with error {Error}", e);
                throw;
            }

            return walletTransactions ?? Array.Empty<AddressTransaction>();
        }
    }
}