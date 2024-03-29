﻿using System;
using MonkeyWallet.Desktop.ViewModels.Transaction;
using MonkeyWallet.Desktop.ViewModels.Wallet;
using MonkeyWallet.Desktop.Views.Transaction;
using MonkeyWallet.Desktop.Views.Wallet;
using ReactiveUI;
using Splat;

namespace MonkeyWallet.Desktop.Utility;

public class AppViewLocator : ReactiveUI.IViewLocator
{
    public IViewFor ResolveView<T>(T viewModel, string contract = null) => viewModel switch
    {
        WalletListViewModel context => new WalletListView() { DataContext = context },
        AddWalletViewModel context => new AddWalletView() { DataContext = context },
        DisclaimerViewModel context => new DisclaimerView() { DataContext = context },
        ShowMnemonicViewModel context => new ShowMnemonicView() { DataContext = context },
        EnterMnemonicViewModel context => new EnterMnemonicView() { DataContext = context },
        NameAndSecureViewModel context => new NameAndSecureView() { DataContext = context },
        WalletDetailViewModel context => new WalletDetailView() { DataContext = context },
        TransactionDetailViewModel context => new TransactionDetailView(){DataContext = context},
        TransactionListViewModel context => new TransactionListView() { DataContext = context },

        _ => throw new ArgumentOutOfRangeException(nameof(viewModel))
    };
}