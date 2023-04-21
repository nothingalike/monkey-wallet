namespace MonkeyWallet.Desktop.ViewModels.Transaction;

public class TransactionListItemViewModel : ViewModelBase
{
    public string? TxHash { get; set; }
    
    public uint EpochNo { get; set; }
    
    public uint? BlockHeight { get; set; }
    
    public ulong? BlockTime { get; set; }
}