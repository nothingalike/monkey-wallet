using ReactiveUI;

namespace MonkeyWallet.Desktop.ViewModels.Wallet;

public class WalletListViewModel : ViewModelBase, IRoutableViewModel
{
    public string UrlPathSegment => nameof(WalletListViewModel);
    public IScreen HostScreen { get; }

    public WalletListViewModel(IScreen screen)
    {
        HostScreen = screen;
    }
}