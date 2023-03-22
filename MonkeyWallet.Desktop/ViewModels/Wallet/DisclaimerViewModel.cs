using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MonkeyWallet.Desktop.ViewModels.Wallet;

public class DisclaimerViewModel: ViewModelBase, IRoutableViewModel
{
    public string UrlPathSegment => nameof(AddWalletViewModel);
    public IScreen HostScreen { get; }
    public ICommand Next { get; set; }
    public ICommand Previous { get; set; }

    private bool _accepted;
    public bool Accepted
    {
        get => _accepted;
        set => this.RaiseAndSetIfChanged(ref _accepted, value);
    }

    public DisclaimerViewModel()
    {
        Next = ReactiveCommand.CreateFromTask(NextHandler);
        Previous = ReactiveCommand.CreateFromTask(PreviousHandler);
    }

    public DisclaimerViewModel(IScreen screen)
    {
        HostScreen = screen;
        Next = ReactiveCommand.CreateFromTask(NextHandler);
        Previous = ReactiveCommand.CreateFromTask(PreviousHandler);
    }

    private async Task NextHandler(CancellationToken arg)
    {
        //go to Show Mnemonic View
        HostScreen.Router.Navigate.Execute(new ShowMnemonicViewModel(HostScreen));
    }

    private async Task PreviousHandler(CancellationToken arg)
    {
        //go to Decision View
        HostScreen.Router.NavigateBack.Execute();
    }
}
