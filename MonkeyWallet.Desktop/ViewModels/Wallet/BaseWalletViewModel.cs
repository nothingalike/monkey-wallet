using System.Linq;
using CardanoSharp.Koios.Client;
using MonkeyWallet.Core.Data;
using MonkeyWallet.Desktop.Models;
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
            Router.Navigate.Execute(new WalletListViewModel(this, Locator.Current.GetService<IWalletDatabase>(), Locator.Current.GetService<SelectedWalletState>(), Locator.Current.GetService<IWalletKeyDatabase>(), Locator.Current.GetService<ISettingsDatabase>(), Locator.Current.GetService<IAccountClient>()));
            return;
        }
        Router.Navigate.Execute(Router.NavigationStack.First());
    }
}