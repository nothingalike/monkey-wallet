using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using MonkeyWallet.Core.Data;
using MonkeyWallet.Desktop.Models;
using MonkeyWallet.Desktop.ViewModels.Wallet;
using ReactiveUI;

namespace MonkeyWallet.Desktop.ViewModels.Transaction;

public class TransactionListViewModel : ViewModelBase, IRoutableViewModel
{
    public string UrlPathSegment => nameof(TransactionListViewModel);
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
    
    public TransactionListViewModel(IScreen screen)
    {
        HostScreen = screen;
        GetTransactions();
    }
    
    private void GetTransactions()
    {
        WalletTransactions.Add(new TransactionListItemViewModel());
        WalletTransactions.Add(new TransactionListItemViewModel());
    }
}