using CardanoSharp.Koios.Client;
using MonkeyWallet.Core.Data;
using ReactiveUI;
using Splat;

namespace MonkeyWallet.Desktop.ViewModels.Transaction;

public class BaseTransactionViewModel : ReactiveObject, IActivatableViewModel, IScreen
{
    public ViewModelActivator Activator { get; } = new();
    public RoutingState Router { get; } = new();

    public BaseTransactionViewModel()
    {
        Router.Navigate.Execute(new TransactionListViewModel(this, Locator.Current.GetService<IWalletKeyDatabase>(), Locator.Current.GetService<IAddressClient>(),Locator.Current.GetService<ISettingsDatabase>()));
    }

}