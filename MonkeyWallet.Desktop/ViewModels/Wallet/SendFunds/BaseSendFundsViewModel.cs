using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonkeyWallet.Desktop.ViewModels.Wallet.SendFunds;

public class BaseSendFundsViewModel : ReactiveObject, IActivatableViewModel, IScreen
{
    public ViewModelActivator Activator { get; } = new();
    public RoutingState Router { get; } = new();

    public BaseSendFundsViewModel()
    {
        Router.Navigate.Execute(new SendFundsBuildViewModel(this));
    }
}
