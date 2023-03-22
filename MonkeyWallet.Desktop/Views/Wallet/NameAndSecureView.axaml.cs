using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using MonkeyWallet.Desktop.ViewModels.Wallet;
using ReactiveUI;

namespace MonkeyWallet.Desktop.Views.Wallet;

public partial class NameAndSecureView : ReactiveUserControl<NameAndSecureViewModel>
{
    private TextBox walletName => this.FindControl<TextBox>("tbWalletName");
    private TextBox spendingPassword => this.FindControl<TextBox>("tbSpendingPassword");
    private TextBox confirmPassword => this.FindControl<TextBox>("tbConfirmPassword");
    public NameAndSecureView()
    {
        InitializeComponent();
        this.WhenActivated(d =>
        {
            walletName.PropertyChanged += (sender, args) =>
            {
                if (sender is TextBox)
                    ViewModel.Name = ((TextBox)sender).Text ?? string.Empty;
            };

            spendingPassword.PropertyChanged += (sender, args) =>
            {
                if (sender is TextBox)
                    ViewModel.SpendingPassword = ((TextBox)sender).Text ?? string.Empty;
            };

            confirmPassword.PropertyChanged += (sender, args) =>
            {
                if (sender is TextBox)
                    ViewModel.ConfirmPassword = ((TextBox)sender).Text ?? string.Empty;
            };
        });
    }
}