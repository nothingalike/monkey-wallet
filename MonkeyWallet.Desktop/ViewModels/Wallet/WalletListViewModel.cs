using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MonkeyWallet.Core.Data;
using ReactiveUI;

namespace MonkeyWallet.Desktop.ViewModels.Wallet;

public class WalletListViewModel : ViewModelBase, IRoutableViewModel
{
    public string UrlPathSegment => nameof(WalletListViewModel);
    public IScreen HostScreen { get; }

    private readonly IWalletDatabase _walletDatabase;

    private bool _hasNoWallet;
    public bool HasNoWallet
    {
        get => _hasNoWallet;
        set => this.RaiseAndSetIfChanged(ref _hasNoWallet, value);
    }

    public WalletListViewModel(IScreen screen, IWalletDatabase walletDatabase)
    {
        HostScreen = screen;
        _walletDatabase = walletDatabase;
        Task.Run(async () => await CheckHasWallets());
    }

    private async Task CheckHasWallets()
    {
        List<Core.Data.Models.Wallet> wallets = await _walletDatabase.ListAsync();

        if (!wallets.Any()) HasNoWallet = true;
    }
}