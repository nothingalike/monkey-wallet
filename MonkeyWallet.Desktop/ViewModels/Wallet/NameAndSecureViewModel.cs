using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using IWalletService = .IWalletService;

namespace MonkeyWallet.Desktop.ViewModels.Wallet;

public class NameAndSecureViewModel : ViewModelBase, IRoutableViewModel
{
    public string UrlPathSegment => nameof(AddWalletViewModel);
    public IScreen HostScreen { get; }
    public List<string> Mnemonic { get; set; }
    public string Name { get; set; }
    public string SpendingPassword { get; set; }
    public string ConfirmPassword { get; set; }
    public ICommand Previous { get; set; }
    public ICommand Next { get; set; }

    public List<string> Errors { get; set; }

    public NameAndSecureViewModel(List<string> mnemonic, IScreen screen)
    {
        Mnemonic = mnemonic;    
        HostScreen = screen;
        Next = ReactiveCommand.CreateFromTask(NextHandler);
        Previous = ReactiveCommand.CreateFromTask(PreviousHandler);
    }

    public NameAndSecureViewModel(List<string> mnemonic)
    {
        Mnemonic = mnemonic;
        Next = ReactiveCommand.CreateFromTask(NextHandler);
        Previous = ReactiveCommand.CreateFromTask(PreviousHandler);
    }

    private async Task NextHandler(CancellationToken arg)
    {
        //save wallet
        //go to Show Mnemonic View
        HostScreen.Router.NavigateAndReset.Execute(new WalletListViewModel(HostScreen));
    }

    private async Task PreviousHandler(CancellationToken arg)
    {
        //go to Disclaimer View
        HostScreen.Router.NavigateBack.Execute();
    }
}
