using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonkeyWallet.Desktop.ViewModels.Wallet;

public class ShowMnemonicViewModel : ViewModelBase, IRoutableViewModel
{
    public string UrlPathSegment => nameof(AddWalletViewModel);
    public IScreen HostScreen { get; }
}
