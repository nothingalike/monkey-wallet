using ReactiveUI;

namespace MonkeyWallet.Desktop.ViewModels.Wallet.ReceiveFunds;

public class ReceiveFundListViewModel : ViewModelBase, IRoutableViewModel
{
    public string? UrlPathSegment => nameof(ReceiveFundListViewModel);
    public IScreen HostScreen { get; }

    public ReceiveFundListViewModel(IScreen screen)
    {
        HostScreen = screen;
    }
}