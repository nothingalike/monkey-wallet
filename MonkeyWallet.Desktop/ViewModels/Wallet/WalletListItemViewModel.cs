namespace MonkeyWallet.Desktop.ViewModels.Wallet;
using Wallet =  MonkeyWallet.Core.Data.Models.Wallet;



public class WalletListItemViewModel : ViewModelBase
{
    private readonly Wallet _wallet;

    public WalletListItemViewModel(Wallet wallet)
    {
        _wallet = wallet;
    }

    public string WalletName => _wallet.Name;
}