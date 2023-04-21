namespace MonkeyWallet.Desktop.ViewModels.Wallet;
using Wallet =  MonkeyWallet.Core.Data.Models.Wallet;



public class WalletListItemViewModel : ViewModelBase
{
    public int Id { get; init; }
    public string? TotalBalance { get; set; }

    public Wallet? Wallet { get; set; }

    public WalletListItemViewModel(int id)
    {
        Id = id;
    }

    public string WalletName { get; set; }
}