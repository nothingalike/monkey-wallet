using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using MonkeyWallet.Desktop.ViewModels.Wallet;

namespace MonkeyWallet.Desktop.Views.Wallet;

public partial class AddWalletView : ReactiveUserControl<AddWalletViewModel>
{
    public AddWalletView()
    {
        InitializeComponent();
    }
}