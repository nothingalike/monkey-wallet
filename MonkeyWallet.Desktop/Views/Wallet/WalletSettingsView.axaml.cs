using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using MonkeyWallet.Desktop.ViewModels.Wallet;
using ReactiveUI;

namespace MonkeyWallet.Desktop.Views.Wallet;

public partial class WalletSettingsView : ReactiveUserControl<WalletSettingsViewModel>
{
    public static readonly StyledProperty<string> WalletIdProperty
        = AvaloniaProperty.Register<WalletSettingsView, string>(
            nameof(WalletId));

    public string WalletId
    {
        get => GetValue(WalletIdProperty);
        set
        {
            SetValue(WalletIdProperty, value);
            ViewModel.WalletId = value;
        }
    }
    
    private TextBox walletName => this.FindControl<TextBox>("tbWalletName");

    public WalletSettingsView()
    {
        DataContext = new WalletSettingsViewModel();
        InitializeComponent();

        this.WhenActivated(d =>
        {
            walletName.Text = ViewModel.WalletId.ToString();
        });
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}