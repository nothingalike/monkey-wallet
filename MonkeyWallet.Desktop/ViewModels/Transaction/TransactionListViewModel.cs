using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CardanoSharp.Koios.Client;
using CardanoSharp.Koios.Client.Contracts;
using DynamicData;
using MonkeyWallet.Core.Data;
using ReactiveUI;

namespace MonkeyWallet.Desktop.ViewModels.Transaction;

public class TransactionListViewModel : ViewModelBase, IRoutableViewModel
{
    public string UrlPathSegment => nameof(TransactionListViewModel);
    private readonly IWalletKeyDatabase _walletKeyDatabase;
    private readonly IAddressClient _addressClient;

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
    
    public TransactionListViewModel(IScreen screen, IWalletKeyDatabase walletKeyDatabase, IAddressClient addressClient)
    {
        HostScreen = screen;
        _walletKeyDatabase = walletKeyDatabase;
        _addressClient = addressClient;
        Task.Run(GetTransactions);
    }
    
    private async Task GetTransactions()
    {
        AddressTransactionRequest addressTransactionRequest = new()
        {
            Addresses = new List<string>()
            {
                "addr_test1vzpwq95z3xyum8vqndgdd9mdnmafh3djcxnc6jemlgdmswcve6tkw",
            }
        };
        AddressTransaction[]? addressTransactions = (await _addressClient.GetAddressTransactions(addressTransactionRequest)).Content;
        foreach (AddressTransaction transaction in addressTransactions)
        {
            WalletTransactions.Add(new TransactionListItemViewModel(){TxHash = transaction.TxHash, BlockHeight = transaction.BlockHeight, BlockTime = transaction.BlockTime, EpochNo = transaction.EpochNo});
        }
    }
}