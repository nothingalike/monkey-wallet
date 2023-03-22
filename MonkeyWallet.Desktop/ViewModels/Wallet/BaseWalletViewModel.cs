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
            var hasWallet = false;
            if (hasWallet)
            {
                Router.Navigate.Execute(new WalletListViewModel(this));
            }else
            {
                Router.Navigate.Execute(new AddWalletViewModel(this));
            }

            return;
        }
        Router.Navigate.Execute(Router.NavigationStack.First());
    }
}