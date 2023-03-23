using CardanoSharp.Wallet;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MonkeyWallet.Desktop.ViewModels.Wallet;

public class ShowMnemonicViewModel : ViewModelBase, IRoutableViewModel
{
    public string UrlPathSegment => nameof(AddWalletViewModel);
    public IScreen HostScreen { get; }

    public List<string> Mnemonic { get; set; }

    public ICommand Previous { get; set; }
    public ICommand Next { get; set; }

    public ShowMnemonicViewModel()
    {
        Mnemonic = new List<string>();
        Next = ReactiveCommand.CreateFromTask(NextHandler);
        Previous = ReactiveCommand.CreateFromTask(PreviousHandler);
        GenerateMnemonic();
    }

    public ShowMnemonicViewModel(IScreen screen)
    {
        HostScreen = screen;
        Mnemonic = new List<string>();
        Next = ReactiveCommand.CreateFromTask(NextHandler);
        Previous = ReactiveCommand.CreateFromTask(PreviousHandler);
        GenerateMnemonic();
    }

    private void GenerateMnemonic()
    {
        if(Mnemonic is null || !Mnemonic.Any())
            Mnemonic = new MnemonicService().Generate(24).Words.Split(" ").ToList();
        
    }

    private async Task NextHandler(CancellationToken arg)
    {
        //go to Show Mnemonic View
        HostScreen.Router.Navigate.Execute(new EnterMnemonicViewModel(Mnemonic, HostScreen));
    }

    private async Task PreviousHandler(CancellationToken arg)
    {
        //go to Disclaimer View
        HostScreen.Router.NavigateBack.Execute();
    }
}
