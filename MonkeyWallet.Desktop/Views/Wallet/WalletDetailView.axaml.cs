using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using MonkeyWallet.Desktop.ViewModels.Wallet;

namespace MonkeyWallet.Desktop.Views.Wallet;

public partial class WalletDetailView : ReactiveUserControl<WalletDetailViewModel>
{
    public WalletDetailView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}