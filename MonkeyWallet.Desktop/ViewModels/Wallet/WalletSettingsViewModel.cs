using MonkeyWallet.Desktop.Models;
using ReactiveUI;

namespace MonkeyWallet.Desktop.ViewModels.Wallet;

public class WalletSettingsViewModel: ViewModelBase
{
    public SelectedWalletState SelectedWallet;

    public WalletSettingsViewModel(SelectedWalletState selectedWalletState)
    {
        SelectedWallet = selectedWalletState;
    }
}