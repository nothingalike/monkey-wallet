using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using MonkeyWallet.Desktop.Models;
using MonkeyWallet.Desktop.ViewModels.Wallet;
using ReactiveUI;
using Splat;

namespace MonkeyWallet.Desktop.Views.Wallet;

public partial class WalletSettingsView : ReactiveUserControl<WalletSettingsViewModel>
{    
    private TextBox walletName => this.FindControl<TextBox>("tbWalletName");

    public WalletSettingsView()
    {
        DataContext = new WalletSettingsViewModel(Locator.Current.GetService<SelectedWalletState>());
        InitializeComponent();

        this.WhenActivated(d =>
        {
            walletName.Text = ViewModel.SelectedWallet.GetWallet().Name;
        });
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}