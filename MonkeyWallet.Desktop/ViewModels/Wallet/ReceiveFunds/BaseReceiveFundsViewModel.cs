using ReactiveUI;

namespace MonkeyWallet.Desktop.ViewModels.Wallet.ReceiveFunds;

public class BaseReceiveFundsViewModel  : ReactiveObject, IActivatableViewModel, IScreen
{
    public ViewModelActivator Activator { get; } = new();
    public RoutingState Router { get; } = new();

    public BaseReceiveFundsViewModel()
    {
        Router.Navigate.Execute(new ReceiveFundListViewModel(this));
    }
}