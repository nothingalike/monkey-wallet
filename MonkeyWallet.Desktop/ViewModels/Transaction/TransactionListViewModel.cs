using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using CardanoSharp.Koios.Client;
using CardanoSharp.Koios.Client.Contracts;
using CardanoSharp.Wallet;
using CardanoSharp.Wallet.Enums;
using CardanoSharp.Wallet.Extensions.Models;
using CardanoSharp.Wallet.Models.Addresses;
using CardanoSharp.Wallet.Models.Derivations;
using CardanoSharp.Wallet.Models.Keys;
using DynamicData;
using MonkeyWallet.Core.Common;
using MonkeyWallet.Core.Data;
using MonkeyWallet.Core.Data.Models;
using MonkeyWallet.Desktop.ViewModels.Wallet;
using MonkeyWallet.Desktop.Views.Wallet;
using ReactiveUI;

namespace MonkeyWallet.Desktop.ViewModels.Transaction;

public class TransactionListViewModel : ViewModelBase, IRoutableViewModel
{
    public string UrlPathSegment => nameof(TransactionListViewModel);
    private readonly IWalletKeyDatabase _walletKeyDatabase;
    private readonly IAddressClient _addressClient;
    private readonly ISettingsDatabase _settingsDatabase;

    public IScreen HostScreen { get; }
    public ObservableCollection<TransactionListItemViewModel> WalletTransactions { get; } = new();

    private TransactionListItemViewModel? _selectedTransaction;

    public TransactionListItemViewModel? SelectedTransaction
    {
        get => _selectedTransaction;
        set
        {
            this.RaiseAndSetIfChanged(ref _selectedTransaction, value);
            HostScreen.Router.Navigate.Execute(new TransactionDetailViewModel(HostScreen));
        }
    }
    
    public TransactionListViewModel(IScreen screen, IWalletKeyDatabase walletKeyDatabase, IAddressClient addressClient, ISettingsDatabase settingsDatabase)
    {
        HostScreen = screen;
        _walletKeyDatabase = walletKeyDatabase;
        _addressClient = addressClient;
        _settingsDatabase = settingsDatabase;
        Task.Run(GetTransactions);
    }
    
    private async Task GetTransactions()
    {
        List<WalletKey> keys = await _walletKeyDatabase.GetWalletKeysAsync(WalletDetailViewModel.Wallet.Id);
        WalletKey validKey = keys.First();
        PublicKey? publicKey = JsonSerializer.Deserialize<PublicKey>(validKey.Vkey);
        
        IIndexNodeDerivation paymentNode = publicKey
            .Derive(RoleType.ExternalChain)
            .Derive(0);

        IIndexNodeDerivation stakingNode = publicKey
            .Derive(RoleType.Staking)
            .Derive(0);

        NetworkType networkType = NetworkType.Mainnet;

        Core.Data.Models.Settings? settings = await _settingsDatabase.GetByKeyAsync("Network");

        if (settings is not null)
            networkType = settings.Value.ToUpperInvariant() switch
            {
                NetworkOptions.MAINNET => NetworkType.Mainnet,
                NetworkOptions.PREPROD => NetworkType.Preprod,
                NetworkOptions.PREVIEW => NetworkType.Preview,
                _ => NetworkType.Mainnet
            };
        
        Address delegationAddress = new AddressService()
            .GetBaseAddress(
                paymentNode.PublicKey,
                stakingNode.PublicKey,
                networkType);
        
        AddressTransactionRequest addressTransactionRequest = new()
        {
            Addresses = new List<string>()
            {
                delegationAddress.ToString(),
            }
        };
        AddressTransaction[]? addressTransactions = (await _addressClient.GetAddressTransactions(addressTransactionRequest)).Content;
        foreach (AddressTransaction transaction in addressTransactions)
        {
            WalletTransactions.Add(new TransactionListItemViewModel(){TxHash = transaction.TxHash, BlockHeight = transaction.BlockHeight, BlockTime = transaction.BlockTime, EpochNo = transaction.EpochNo});
        }
    }
}