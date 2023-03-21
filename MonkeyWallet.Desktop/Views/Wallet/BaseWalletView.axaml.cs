using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using MonkeyWallet.Desktop.ViewModels.Wallet;

namespace MonkeyWallet.Desktop.Views.Wallet;

public partial class BaseWalletView : ReactiveUserControl<BaseWalletViewModel>
{
    public BaseWalletView()
    {
        DataContext = new BaseWalletViewModel();
        AvaloniaXamlLoader.Load(this);
    }
}