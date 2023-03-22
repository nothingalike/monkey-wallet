using System;
using MonkeyWallet.Desktop.ViewModels.Wallet;
using MonkeyWallet.Desktop.Views.Wallet;
using ReactiveUI;
using Splat;

namespace MonkeyWallet.Desktop.Utility;

public class AppViewLocator : ReactiveUI.IViewLocator
{
    public IViewFor ResolveView<T>(T viewModel, string contract = null) => viewModel switch
    {
        WalletListViewModel context => new WalletListView() { DataContext = context },
        WalletDetailViewModel context => new WalletDetailView() { DataContext = context },
        _ => throw new ArgumentOutOfRangeException(nameof(viewModel))
    };
}