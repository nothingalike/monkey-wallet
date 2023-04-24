using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Input;
using CardanoSharp.Koios.Client;
using CardanoSharp.Koios.Client.Contracts;
using CardanoSharp.Wallet.Enums;
using CardanoSharp.Wallet.Extensions.Models;
using CardanoSharp.Wallet.Models.Addresses;
using CardanoSharp.Wallet.Models.Keys;
using CardanoSharp.Wallet.Utilities;
using MonkeyWallet.Core.Common;
using MonkeyWallet.Core.Data;
using MonkeyWallet.Core.Data.Models;
using MonkeyWallet.Core.Services;
using MonkeyWallet.Desktop.Models;
using ReactiveUI;
using Splat;

namespace MonkeyWallet.Desktop.ViewModels.Wallet;

public class WalletListViewModel : ViewModelBase, IRoutableViewModel
{
    public string UrlPathSegment => nameof(WalletListViewModel);
    public IScreen HostScreen { get; }

    private readonly IWalletDatabase _walletDatabase;
    private readonly IWalletKeyDatabase _walletKeyDatabase;
    private readonly ISettingsDatabase _settingsDatabase;
    private readonly IAccountClient _accountClient;

    public ObservableCollection<WalletListItemViewModel> UserWallets { get; } = new();

    private WalletListItemViewModel? _selectedWallet;

    public WalletListItemViewModel? SelectedWallet
    {
        get => _selectedWallet;
        set
        {
            this.RaiseAndSetIfChanged(ref _selectedWallet, value);
            _selectedWalletState.SetWallet(SelectedWallet!.Wallet);
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

    private readonly SelectedWalletState _selectedWalletState;

    public WalletListViewModel(IScreen screen, IWalletDatabase walletDatabase, SelectedWalletState selectedWalletState, IWalletKeyDatabase walletKeyDatabase, ISettingsDatabase settingsDatabase, IAccountClient accountClient)
    {
        HostScreen = screen;
        _walletDatabase = walletDatabase;
        _selectedWalletState = selectedWalletState;
        _walletKeyDatabase = walletKeyDatabase;
        _settingsDatabase = settingsDatabase;
        _accountClient = accountClient;
        GoToAddWallet = ReactiveCommand.Create(NavigateToAddWalletView);
        Task.Run(CheckHasWallets);
    }

    private async Task CheckHasWallets()
    {
        List<Core.Data.Models.Wallet> wallets = await _walletDatabase.ListAsync();


        foreach (Core.Data.Models.Wallet wallet in wallets)
        {
            List<WalletKey> keys = await _walletKeyDatabase.GetWalletKeysAsync(wallet.Id);

            AccountInformation[]? response = null;
            if (wallet.WalletType is (int)WalletType.HD)
            {
                WalletKey validKey = keys.First();
                PublicKey? publicKey = JsonSerializer.Deserialize<PublicKey>(validKey.Vkey).Derive(RoleType.Staking).Derive(0).PublicKey;
                NetworkType networkType = NetworkType.Mainnet;

                Core.Data.Models.Settings? settings = await _settingsDatabase.GetByKeyAsync("Network");

                if (settings is not null)
                    networkType = settings.Value.ToUpperInvariant() switch
                    {
                        NetworkOptions.MAINNET => NetworkType.Mainnet,
                        NetworkOptions.PREPROD => NetworkType.Preprod,
                        NetworkOptions.PREVIEW => NetworkType.Preview,
                        _ => NetworkType.Mainnet
                    };

                Address? stakeAddress = AddressUtility.GetRewardAddress(publicKey, networkType);
                 response = (await _accountClient.GetAccountInformation(new AccountBulkRequest()
                {
                    StakeAddresses = new[] { stakeAddress.ToString() }
                })).Content;
            }

            double totalBalanceCalculation = double.TryParse(response?.FirstOrDefault()?.TotalBalance, out double answer) ? answer / 1000000000 : 0;
            UserWallets.Add(new WalletListItemViewModel(wallet.Id)
            {
                TotalBalance = totalBalanceCalculation.ToString(CultureInfo.InvariantCulture) + " ADA",
                WalletName = wallet.Name,
                Wallet = wallet
            });

        }

        if (!wallets.Any()) HasNoWallet = true;
    }

    private void NavigateToAddWalletView()
    {
        HostScreen.Router.Navigate.Execute(new AddWalletViewModel(HostScreen));
    }
}