using ReactiveUI;

namespace MonkeyWallet.Desktop.ViewModels.Wallet;

public class WalletDetailViewModel : ViewModelBase, IRoutableViewModel
{
    public string UrlPathSegment => nameof(WalletDetailViewModel);
    public IScreen HostScreen { get; }

    public WalletDetailViewModel(IScreen screen)
    {
        HostScreen = screen;
    }
}