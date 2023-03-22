using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MonkeyWallet.Desktop.ViewModels.Wallet;

public class EnterMnemonicViewModel: ViewModelBase, IRoutableViewModel
{
    public string UrlPathSegment => nameof(AddWalletViewModel);
    public IScreen HostScreen { get; }
    public List<string> ConfirmMnemonic { get; set; }
    public List<string> CurrentMnemonic { get; set; }
    public ICommand Previous { get; set; }
    public ICommand Next { get; set; }

    private bool _match;

    public bool Match
    {
        get => _match;
        set => this.RaiseAndSetIfChanged(ref _match, value);
    }

    public EnterMnemonicViewModel(List<string> generatedWords)
    {
        CurrentMnemonic = generatedWords;
        ConfirmMnemonic = new List<string>()
        {
            "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", ""
        };
        Next = ReactiveCommand.CreateFromTask(NextHandler);
        Previous = ReactiveCommand.CreateFromTask(PreviousHandler);
    }

    public EnterMnemonicViewModel(List<string> generatedWords, IScreen screen)
    {
        HostScreen = screen; 
        CurrentMnemonic = generatedWords;
        ConfirmMnemonic = new List<string>()
        {
            "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", ""
        };
        Next = ReactiveCommand.CreateFromTask(NextHandler);
        Previous = ReactiveCommand.CreateFromTask(PreviousHandler);
    }

    private async Task NextHandler(CancellationToken arg)
    {
        //go to Show Mnemonic View
        //HostScreen.Router.Navigate.Execute(new NameAndSecureViewModel(HostScreen));
    }

    private async Task PreviousHandler(CancellationToken arg)
    {
        //go to Disclaimer View
        HostScreen.Router.NavigateBack.Execute();
    }

    public void WordCheck()
    {
        if (CurrentMnemonic is not null)
        {
            var result = true;
            for (var i = 0; i < CurrentMnemonic.Count(); i++)
            {
                if (!ConfirmMnemonic[i].Equals(CurrentMnemonic[i]))
                {
                    result = false;
                }
            }

            Match = result;
        }
        else
        {
            Match = ConfirmMnemonic.All(x => !string.IsNullOrEmpty(x));
        }
    }
}
