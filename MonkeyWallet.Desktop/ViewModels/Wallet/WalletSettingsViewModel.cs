using ReactiveUI;

namespace MonkeyWallet.Desktop.ViewModels.Wallet;

public class WalletSettingsViewModel: ViewModelBase
{
    private string _walletId;
    public string WalletId
    {
        get => _walletId;
        set => this.RaiseAndSetIfChanged(ref _walletId, value);
    }
}