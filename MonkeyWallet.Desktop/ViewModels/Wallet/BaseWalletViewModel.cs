using System.Linq;
using MonkeyWallet.Core.Data;
using ReactiveUI;
using Splat;

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
            Router.Navigate.Execute(new WalletListViewModel(this, Locator.Current.GetService<IWalletDatabase>()));
            return;
        }
        Router.Navigate.Execute(Router.NavigationStack.First());
    }
}