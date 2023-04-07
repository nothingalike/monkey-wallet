using System.Linq;
using MonkeyWallet.Core.Data;
using MonkeyWallet.Desktop.Models;
using MonkeyWallet.Desktop.ViewModels.Wallet;
using ReactiveUI;
using Splat;

namespace MonkeyWallet.Desktop.ViewModels.Transaction;

public class BaseTransactionViewModel : ReactiveObject, IActivatableViewModel, IScreen
{
    public ViewModelActivator Activator { get; } = new();
    public RoutingState Router { get; } = new();

    public BaseTransactionViewModel()
    {
        Router.Navigate.Execute(new TransactionListViewModel(this));
    }

}