using CardanoSharp.Wallet.Extensions;
using CardanoSharp.Wallet.Extensions.Models;
using CardanoSharp.Wallet.Models.Keys;
using MonkeyWallet.Core.Common;
using MonkeyWallet.Core.Data;
using MonkeyWallet.Desktop.Models;
using ReactiveUI;
using ReactiveUI.Validation.Abstractions;
using ReactiveUI.Validation.Contexts;
using ReactiveUI.Validation.Extensions;
using Splat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MonkeyWallet.Desktop.ViewModels.Wallet;

public class WalletSettingsViewModel: ViewModelBase
{
    #region Update Name Properties
    public ICommand UpdateNameCommand { get; set; }
    public ICommand ResetNameCommand { get; set; }
    private string _walletName;
    public string WalletName
    {
        get => _walletName;
        set => this.RaiseAndSetIfChanged(ref _walletName, value);
    }
    private string _walletNameError;
    public string WalletNameError
    {
        get => _walletNameError;
        set => this.RaiseAndSetIfChanged(ref _walletNameError, value);
    }
    private string _updateNameSuccessMessage;
    public string UpdateNameSuccessMessage
    {
        get => _updateNameSuccessMessage;
        set => this.RaiseAndSetIfChanged(ref _updateNameSuccessMessage, value);
    }
    #endregion

    #region Update Password Properties
    public ICommand UpdatePasswordCommand { get; set; }
    public ICommand ResetPasswordCommand { get; set; }
    private string _currentPassword;
    public string CurrentPassword
    {
        get => _currentPassword;
        set => this.RaiseAndSetIfChanged(ref _currentPassword, value);
    }
    private string _newPassword;
    public string NewPassword
    {
        get => _newPassword;
        set => this.RaiseAndSetIfChanged(ref _newPassword, value);
    }
    private string _confirmPassword;
    public string ConfirmPassword
    {
        get => _confirmPassword;
        set => this.RaiseAndSetIfChanged(ref _confirmPassword, value);
    }
    private string _currentPasswordError;
    public string CurrentPasswordError
    {
        get => _currentPasswordError;
        set => this.RaiseAndSetIfChanged(ref _currentPasswordError, value);
    }
    private string _newPasswordError;
    public string NewPasswordError
    {
        get => _newPasswordError;
        set => this.RaiseAndSetIfChanged(ref _newPasswordError, value);
    }
    private string _confirmPasswordError;
    public string ConfirmPasswordError
    {
        get => _confirmPasswordError;
        set => this.RaiseAndSetIfChanged(ref _confirmPasswordError, value);
    }
    private string _updatePasswordSuccessMessage;
    public string UpdatePasswordSuccessMessage
    {
        get => _updatePasswordSuccessMessage;
        set => this.RaiseAndSetIfChanged(ref _updatePasswordSuccessMessage, value);
    }
    #endregion

    public SelectedWalletState SelectedWallet;
    public List<Core.Data.Models.Wallet> Wallets { get; set; }

    public WalletSettingsViewModel(SelectedWalletState selectedWalletState)
    {
        SelectedWallet = selectedWalletState;

        SetWalletName();
        UpdateNameCommand = ReactiveCommand.CreateFromTask(UpdateNameHandler);
        ResetNameCommand = ReactiveCommand.Create(SetWalletName);
        UpdatePasswordCommand = ReactiveCommand.CreateFromTask(UpdatePasswordHandler);
        ResetPasswordCommand = ReactiveCommand.Create(ResetPasswordHandler);
    }

    private void ResetPasswordHandler()
    {
        CurrentPassword = string.Empty;
        NewPassword = string.Empty;
        ConfirmPassword = string.Empty;
        CurrentPasswordError = string.Empty;
        NewPasswordError = string.Empty;
        ConfirmPasswordError = string.Empty;
    }

    private async Task UpdatePasswordHandler()
    {
        CurrentPasswordError = string.Empty;
        NewPasswordError = string.Empty;
        ConfirmPasswordError = string.Empty;

        if (string.IsNullOrEmpty(CurrentPassword))
        {
            CurrentPasswordError = "Current Password is required";
            return;
        }

        if (string.IsNullOrEmpty(NewPassword))
        {
            NewPasswordError = "New Password is required";
            return;
        }

        if (string.IsNullOrEmpty(ConfirmPassword))
        {
            ConfirmPasswordError = "Confirm Password is required";
            return;
        }

        if(NewPassword != ConfirmPassword)
        {
            ConfirmPasswordError = "New/Confirm Password mismatch";
            return;
        }

        var wallet = SelectedWallet.GetWallet();
        var walletKeyDatabase = Locator.Current.GetService<IWalletKeyDatabase>();
        var walletKey = (await walletKeyDatabase.GetWalletKeysAsync(wallet.Id)).FirstOrDefault();
        if(wallet.WalletType == (int)WalletType.HD)
        {
            if(walletKey != null)
            {
                var decryptedSkey = JsonSerializer.Deserialize<PrivateKey>(walletKey.Skey).Decrypt(CurrentPassword);
                var messageByte = "test".HexToByteArray();
                var signature = decryptedSkey.Sign(messageByte);
                var verified = JsonSerializer.Deserialize<PublicKey>(walletKey.Vkey).Verify(messageByte, signature);
                if(!verified)
                {
                    CurrentPasswordError = "Current password is incorrect";
                    return;
                }

                walletKey.Skey = JsonSerializer.Serialize(decryptedSkey.Encrypt(ConfirmPassword));
                await walletKeyDatabase.SaveWalletAsync(walletKey);
                ResetPasswordHandler();
                UpdatePasswordSuccessMessage = "Successfully updated password";
                await Task.Delay(3000);
                UpdatePasswordSuccessMessage = string.Empty;
            }
        }
    }

    private void SetWalletName()
    {
        WalletNameError = string.Empty;
        WalletName = SelectedWallet.GetWallet().Name;
    }

    private async Task UpdateNameHandler()
    {
        WalletNameError = string.Empty;

        if (string.IsNullOrEmpty(WalletName))
        {
            WalletNameError = "Name is required";
            return;
        }

        var walletDatabase = Locator.Current.GetService<IWalletDatabase>();
        var wallets = await walletDatabase.ListAsync();
        var dupName = wallets.Where(x => x.Id != SelectedWallet.GetWallet().Id).Any(x => x.Name == WalletName);
        if(dupName)
        {
            WalletNameError = "Name is already taken";
            return;
        }

        var wallet = SelectedWallet.GetWallet();
        wallet.Name = WalletName;

        await walletDatabase.SaveAsync(wallet);

        SelectedWallet.SetWallet(wallet);
        UpdateNameSuccessMessage = "Successfully updated name";
        await Task.Delay(3000);
        UpdateNameSuccessMessage = string.Empty;
    }

    #region Update Name Handlers
    private async Task UpdateName(string newName)
    {

    }
    #endregion

    #region Update Password Handlers
    private async Task UpdatePassword(string currentPassword, string newPassword, string confirmPassword)
    {

    }
    #endregion
}