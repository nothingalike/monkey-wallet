using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Mixins;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using MonkeyWallet.Desktop.Models;
using MonkeyWallet.Desktop.ViewModels.Wallet;
using ReactiveUI;
using ReactiveUI.Validation.Extensions;
using Splat;
using System.Threading.Tasks;

namespace MonkeyWallet.Desktop.Views.Wallet;

public partial class WalletSettingsView : ReactiveUserControl<WalletSettingsViewModel>
{
    private TextBox walletName => this.FindControl<TextBox>("tbWalletName");
    private TextBlock walletNameError => this.FindControl<TextBlock>("tbWalletNameError");
    private TextBlock walletNameSuccess => this.FindControl<TextBlock>("tbWalletNameSuccess");

    private TextBox currentPassword => this.FindControl<TextBox>("tbCurrentPassword");
    private TextBox newPassword => this.FindControl<TextBox>("tbNewPassword");
    private TextBox confirmPassword => this.FindControl<TextBox>("tbConfirmPassword");
    private TextBlock currentPasswordError => this.FindControl<TextBlock>("tbCurrentPasswordError");
    private TextBlock newPasswordError => this.FindControl<TextBlock>("tbNewPasswordError");
    private TextBlock confirmPasswordError => this.FindControl<TextBlock>("tbConfirmPasswordError");
    private TextBlock passwordSuccess => this.FindControl<TextBlock>("tbPasswordSuccess");
    public WalletSettingsView()
    {
        DataContext = new WalletSettingsViewModel(Locator.Current.GetService<SelectedWalletState>());
        InitializeComponent();

        this.WhenActivated(d =>
        {
            //Wallet Name Change
            this.Bind(ViewModel,
                vm => vm.WalletName,
                v => v.walletName.Text);
            this.Bind(ViewModel,
                vm => vm.WalletNameError,
                v => v.walletNameError.Text);
            this.Bind(ViewModel,
                vm => vm.UpdateNameSuccessMessage,
                v => v.walletNameSuccess.Text);

            //Password Change
            this.Bind(ViewModel,
                vm => vm.CurrentPassword,
                v => v.currentPassword.Text);
            this.Bind(ViewModel,
                vm => vm.NewPassword,
                v => v.newPassword.Text);
            this.Bind(ViewModel,
                vm => vm.ConfirmPassword,
                v => v.confirmPassword.Text);
            this.Bind(ViewModel,
                vm => vm.CurrentPasswordError,
                v => v.currentPasswordError.Text);
            this.Bind(ViewModel,
                vm => vm.NewPasswordError,
                v => v.newPasswordError.Text);
            this.Bind(ViewModel,
                vm => vm.ConfirmPasswordError,
                v => v.confirmPasswordError.Text);
            this.Bind(ViewModel,
                vm => vm.UpdatePasswordSuccessMessage,
                v => v.passwordSuccess.Text);
        });
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}