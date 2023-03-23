using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Extensions.Hosting;
using MonkeyWallet.Core.Data;
using ReactiveUI;

namespace MonkeyWallet.Desktop.ViewModels.Wallet;

public class WalletListViewModel : ViewModelBase, IRoutableViewModel
{
    public string UrlPathSegment => nameof(WalletListViewModel);
    public IScreen HostScreen { get; }

    private readonly IWalletDatabase _walletDatabase;
    public ObservableCollection<WalletListItemViewModel> UserWallets { get; } = new(); 
    
    private WalletListItemViewModel? _selectedWallet;
    public WalletListItemViewModel? SelectedWallet
    {
        get => _selectedWallet;
        set
        {
            this.RaiseAndSetIfChanged(ref _selectedWallet, value);
            HostScreen.Router.Navigate.Execute(new WalletDetailViewModel(HostScreen, SelectedWallet!.Wallet));
        }
    }

    private bool _hasNoWallet;
    public bool HasNoWallet
    {
        get => _hasNoWallet;
        set => this.RaiseAndSetIfChanged(ref _hasNoWallet, value);
    }

    public ICommand GoToAddWallet { get; set; }

    public WalletListViewModel(IScreen screen, IWalletDatabase walletDatabase)
    {
        HostScreen = screen;
        _walletDatabase = walletDatabase;
        GoToAddWallet = ReactiveCommand.CreateFromTask(NavigateToAddWalletView);
        Task.Run(() => CheckHasWallets());
    }

    private async Task CheckHasWallets()
    {
        List<Core.Data.Models.Wallet> wallets = await _walletDatabase.ListAsync();
        
        foreach (var wallet in wallets)
        {
            UserWallets.Add(new WalletListItemViewModel(wallet));
        }
        UserWallets.Add(new WalletListItemViewModel(new Core.Data.Models.Wallet(){Name = "Test Wallet"}));
        UserWallets.Add(new WalletListItemViewModel(new Core.Data.Models.Wallet(){Name = "Test Wallet 2"}));
        if (!wallets.Any()) HasNoWallet = true;
    }

    private async Task NavigateToAddWalletView()
    {
        HostScreen.Router.Navigate.Execute(new AddWalletViewModel(HostScreen));
    }
}