﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Input;
using CardanoSharp.Koios.Client;
using CardanoSharp.Wallet.Models.Keys;
using MonkeyWallet.Core.Data;
using MonkeyWallet.Core.Data.Models;
using MonkeyWallet.Core.Services;
using MonkeyWallet.Desktop.Models;
using ReactiveUI;
using Splat;

namespace MonkeyWallet.Desktop.ViewModels.Wallet;

public class WalletDetailViewModel : ViewModelBase, IRoutableViewModel
{
    public string UrlPathSegment => nameof(WalletDetailViewModel);
    public IScreen HostScreen { get; }

    public ICommand GotToWalletListView { get; set; }

    private string _walletName;

    private readonly Core.Data.Models.Wallet _wallet;

    public string WalletName
    {
        get => _walletName;
        set => this.RaiseAndSetIfChanged(ref _walletName, value);
    }

    private string _walletType;

    public string WalletType
    {
        get => _walletType;
        set => this.RaiseAndSetIfChanged(ref _walletType, value);
    }

    private decimal _adaBalance;

    public decimal AdaBalance
    {
        get => _adaBalance;
        set => this.RaiseAndSetIfChanged(ref _adaBalance, value);
    }

    private string _adaPrice;

    public string? AdaPrice
    {
        get => _adaPrice;
        set => this.RaiseAndSetIfChanged(ref _adaPrice, value);
    }

    private int _walletId;

    public int WalletId
    {
        get => _walletId;
        set => this.RaiseAndSetIfChanged(ref _walletId, value);
    }

    public WalletDetailViewModel(IScreen screen, Core.Data.Models.Wallet wallet)
    {
        HostScreen = screen;
        _wallet = wallet;
        WalletId = wallet.Id;
        WalletName = wallet.Name;
        WalletType = wallet.WalletType is 1 ? "HD" : "Key/Pair";
        GotToWalletListView = ReactiveCommand.Create(RouteToWalletListView);
        Task.Run(GetCardanoPrice);
    }

    private void RouteToWalletListView()
    {
        HostScreen.Router.NavigateAndReset.Execute(new WalletListViewModel(HostScreen, Locator.Current.GetService<IWalletDatabase>(),
            Locator.Current.GetService<SelectedWalletState>(),
            Locator.Current.GetService<IWalletKeyDatabase>(),
            Locator.Current.GetService<ISettingsDatabase>(), Locator.Current.GetService<IAccountClient>()));
    }

    private async Task GetCardanoPrice()
    {
        HttpResponseMessage response;
        using (HttpClient web = new HttpClient())
        {
            response = await web.SendAsync(new HttpRequestMessage()
            {
                RequestUri = new Uri(@"https://api.binance.us/api/v3/trades?symbol=ADAUSDT")
            });
        }

        var obj = Newtonsoft.Json.JsonConvert.DeserializeObject<List<dynamic>>(await response.Content.ReadAsStringAsync())!;
        AdaPrice = "$" + Math.Round(decimal.Parse((string)obj[0].price), 3).ToString(CultureInfo.InvariantCulture);
    }
}