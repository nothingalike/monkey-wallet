using System.Linq;
using ReactiveUI;

namespace MonkeyWallet.Desktop.ViewModels.Wallet;

public class BaseWalletViewModel : ReactiveObject, IActivatableViewModel, IScreen
{
    public ViewModelActivator Activator { get; } = new();
    public RoutingState Router { get; } = new();

    public BaseWalletViewModel()
    {
        DirectToRouterWalletView();
    }

    private void DirectToRouterWalletView()
    {
        if (!Router.NavigationStack.Any())
        {
            Router.Navigate.Execute(new WalletListViewModel(this));
            return;
        }
        Router.Navigate.Execute(Router.NavigationStack.First());
    }
}