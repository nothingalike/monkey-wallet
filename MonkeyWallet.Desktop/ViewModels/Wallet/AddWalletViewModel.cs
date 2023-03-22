﻿using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MonkeyWallet.Desktop.ViewModels.Wallet;

public class AddWalletViewModel : ViewModelBase, IRoutableViewModel
{
    public string UrlPathSegment => nameof(AddWalletViewModel);
    public IScreen HostScreen { get; }
    public ICommand CreateWallet { get; set; }
    public ICommand RestoreWallet { get; set; }

    public AddWalletViewModel(IScreen screen)
    {
        HostScreen = screen;
        CreateWallet = ReactiveCommand.CreateFromTask(CreateWalletHandler);
        RestoreWallet = ReactiveCommand.CreateFromTask(RestoreWalletHandler);
    }

    public AddWalletViewModel()
    {
        CreateWallet = ReactiveCommand.CreateFromTask(CreateWalletHandler);
        RestoreWallet = ReactiveCommand.CreateFromTask(RestoreWalletHandler);
    }

    private async Task CreateWalletHandler(CancellationToken arg)
    {
        //go to Disclaimer View
        HostScreen.Router.Navigate.Execute(new DisclaimerViewModel(HostScreen));
    }

    private async Task RestoreWalletHandler(CancellationToken arg)
    {
        //go to Enter Mnemonic View
    }
}
