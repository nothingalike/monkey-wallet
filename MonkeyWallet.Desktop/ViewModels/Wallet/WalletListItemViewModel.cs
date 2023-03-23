namespace MonkeyWallet.Desktop.ViewModels.Wallet;
using Wallet =  MonkeyWallet.Core.Data.Models.Wallet;



public class WalletListItemViewModel : ViewModelBase
{
    public Wallet Wallet { get; set; }

    public WalletListItemViewModel(Wallet wallet)
    {
        Wallet = wallet;
    }

    public string WalletName => Wallet.Name;
}