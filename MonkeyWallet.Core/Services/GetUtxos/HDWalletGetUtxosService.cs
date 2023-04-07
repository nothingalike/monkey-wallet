using CardanoSharp.Koios.Client;
using CardanoSharp.Wallet.Enums;
using CardanoSharp.Wallet.Extensions.Models;
using CardanoSharp.Wallet.Models;
using CardanoSharp.Wallet.Models.Keys;
using CardanoSharp.Wallet.Utilities;
using MonkeyWallet.Core.Contracts;
using MonkeyWallet.Core.Data;
using MonkeyWallet.Core.Data.Models;
using MonkeyWallet.Core.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using MonkeyWallet.Core.Common;

namespace MonkeyWallet.Core.Services.GetUtxos;

public class HDWalletGetUtxosService : IGetUtxosService
{
    private readonly IWalletDatabase _walletDatabase;
    private readonly IWalletKeyDatabase _walletKeyDatabase;
    private readonly IAddressClient _addressClient;
    private readonly ISettingsDatabase _settingsDatabase;

    public HDWalletGetUtxosService(IAddressClient addressClient, IWalletKeyDatabase walletKeyDatabase, IWalletDatabase walletDatabase, ISettingsDatabase settingsDatabase)
    {
        _addressClient = addressClient;
        _walletKeyDatabase = walletKeyDatabase;
        _walletDatabase = walletDatabase;
        _settingsDatabase = settingsDatabase;
    }

    public async Task<List<AddressUtxoSet>> Execute(Wallet wallet)
    {
        //determine network type that is set in wallet
        NetworkType networkType = NetworkType.Mainnet;
        var settings = await _settingsDatabase.GetByKeyAsync("Network");
        if (settings is not null)
            networkType = settings.Value switch
            {
                NetworkOptions.MAINNET => NetworkType.Mainnet,
                NetworkOptions.PREPROD => NetworkType.Preprod,
                NetworkOptions.PREVIEW => NetworkType.Preview,
                _ => NetworkType.Mainnet
            };

        //get the wallet keys
        var walletKey = (await _walletKeyDatabase.GetWalletKeysAsync(wallet.Id)).FirstOrDefault();

        //derive stake key/address
        var stakeKey = JsonSerializer.Deserialize<PublicKey>(walletKey.Vkey);

        List<AddressUtxoSet> addresses = new List<AddressUtxoSet>();
        AddressBulkRequest addressBulkRequest = new AddressBulkRequest()
        {
            Addresses = new List<string>()
        };

        for (int i = 0; i < 50; i++)
        {
            var paymentKey = stakeKey.Derive(RoleType.ExternalChain).Derive(i);
            var paymentAddress = AddressUtility.GetBaseAddress(paymentKey.PublicKey, stakeKey, networkType);
            addresses.Add(new AddressUtxoSet()
            {
                Index = i,
                Address = paymentAddress.ToString(),
                Utxos = new List<Utxo>()
            });
            addressBulkRequest.Addresses.Add(paymentAddress.ToString());
        }

        //get utxos by payment address
        var utxos = await _addressClient.GetAddressInformation(addressBulkRequest);
        foreach(var addrInfo in utxos.Content)
        {
            var aus = addresses.FirstOrDefault(x => x.Address == addrInfo.Address);
            foreach (var utxo in addrInfo.UtxoSets)
            {
                var balance = new Balance()
                {
                    Lovelaces = ulong.Parse(utxo.Value),
                    Assets = new List<Asset>()
                };

                foreach(var asset in utxo.AssetList)
                {
                    balance.Assets.Add(new Asset()
                    {
                        PolicyId = asset.PolicyId,
                        Name = asset.AssetName,
                        Quantity = long.Parse(asset.Quantity)
                    });
                }

                aus.Utxos.Add(new Utxo()
                {
                    TxHash = utxo.TxHash,
                    TxIndex = utxo.TxIndex,
                    Balance = balance
                });
            }
        }

        return addresses;
    }
}
