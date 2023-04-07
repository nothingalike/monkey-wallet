using System.Threading.Tasks;
using System.Windows.Input;
using MonkeyWallet.Desktop.ViewModels.Wallet;
using ReactiveUI;

namespace MonkeyWallet.Desktop.ViewModels.Transaction;

public class TransactionDetailViewModel : ViewModelBase, IRoutableViewModel
{
    public string UrlPathSegment => nameof(TransactionDetailViewModel);
    public IScreen HostScreen { get; }
    
    public ICommand GotToWalletListView { get; set; }

    private string _walletName;
    public string WalletName
    {
        get => _walletName;
        set => this.RaiseAndSetIfChanged(ref _walletName, value);
    }
    
    private string _walletType;
    public string WalletType
    {
        get => _walletType;
        set => this.RaiseAndSetIfChanged(ref _walletType, value);
    }
    
    private decimal _adaBalance;
    public decimal AdaBalance
    {
        get => _adaBalance;
        set => this.RaiseAndSetIfChanged(ref _adaBalance, value);
    }

    private string _adaPrice;
    public string? AdaPrice
    {
        get => _adaPrice;
        set => this.RaiseAndSetIfChanged(ref _adaPrice, value);
    }
    
    private int _walletId;
    public int WalletId
    {
        get => _walletId;
        set => this.RaiseAndSetIfChanged(ref _walletId, value);
    }

    public TransactionDetailViewModel(IScreen screen)
    {
        HostScreen = screen;
    }

}