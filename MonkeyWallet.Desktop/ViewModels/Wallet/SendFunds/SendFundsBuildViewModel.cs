using MonkeyWallet.Desktop.ViewModels.Transaction;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonkeyWallet.Desktop.ViewModels.Wallet.SendFunds;

public class SendFundsBuildViewModel: IRoutableViewModel
{
    public string UrlPathSegment => nameof(TransactionListViewModel);
    public IScreen HostScreen { get; }

    public SendFundsBuildViewModel(IScreen screen)
    {
        HostScreen = screen;
    }
}
