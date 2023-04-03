using CardanoSharp.Wallet;
using CardanoSharp.Wallet.Enums;
using CardanoSharp.Wallet.Extensions.Models;
using MonkeyWallet.Core.Common;
using MonkeyWallet.Core.Data;
using MonkeyWallet.Core.Data.Models;
using SQLite;
using System;
using System.Text.Json;
using System.Threading.Tasks;
using CardanoSharp.Koios.Client;
using CardanoSharp.Koios.Client.Contracts;
using Microsoft.Extensions.Logging;

namespace MonkeyWallet.Core.Services
{
    public interface IWalletService
    {
        Task AddWallet(string name, string recoveryPhrase, string spendingPassword);
        Task<AccountInformation[]> GetWalletInformation(string[] stakeAddress);
    }

    public class WalletService : IWalletService
    {
        private readonly IMnemonicService _mnemonicService;
        private readonly IWalletDatabase _walletDatabase;
        private readonly IWalletKeyDatabase _walletKeyDatabase;
        private readonly IAccountClient _accountClient;
        private readonly ILogger<WalletService> _logger;

        public WalletService(
            IMnemonicService mnemonicService,
            IWalletKeyDatabase walletKeyDatabase,
            IWalletDatabase walletDatabase, IAccountClient accountClient, ILogger<WalletService> logger)
        {
            _mnemonicService = mnemonicService;
            _walletDatabase = walletDatabase;
            _accountClient = accountClient;
            _logger = logger;
            _walletKeyDatabase = walletKeyDatabase;
        }

        public async Task AddWallet(string name, string recoveryPhrase, string spendingPassword)
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

            var accountNode = mnemonic.GetMasterNode()
                .Derive(PurposeType.Shelley)
                .Derive(CoinType.Ada)
                .Derive(accountIx);
            accountNode.SetPublicKey();

            await _walletKeyDatabase.SaveWalletAsync(new WalletKey
            {
                WalletId = newlyCreatedWallet.Id,
                KeyType = (int)KeyType.Account,
                Skey = JsonSerializer.Serialize(accountNode.PrivateKey.Encrypt(spendingPassword)),
                Vkey = JsonSerializer.Serialize(accountNode.PublicKey),
                KeyIndex = accountIx,
                AccountIndex = accountIx
            });
        }

        public async Task<AccountInformation[]> GetWalletInformation(string[] stakeAddress)
        {

            AccountInformation[]? accountInformation;
            try
            {
                accountInformation = (await _accountClient.GetAccountInformation(new AccountBulkRequest()
                {
                    StakeAddresses = stakeAddress
                })).Content;
            }
            catch (Exception e)
            {
                _logger.LogError("There was an error getting the transactions for the wallet with error {Error}", e);
                throw;
            }

            return accountInformation ?? Array.Empty<AccountInformation>();
        }
    }
}
