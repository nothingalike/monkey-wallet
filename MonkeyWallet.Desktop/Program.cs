using Avalonia;
using Avalonia.ReactiveUI;
using System;
using System.Reflection;
using MonkeyWallet.Core.Data;
using ReactiveUI;
using Splat;
using IMonkeyWalletService = MonkeyWallet.Core.Services.IWalletService;
using MonkeyWalletService = MonkeyWallet.Core.Services.WalletService;
using CardanoSharp.Wallet;

namespace MonkeyWallet.Desktop
{
    internal class Program
    {
        // Initialization code. Don't use any Avalonia, third-party APIs or any
        // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
        // yet and stuff might break.
        [STAThread]
        public static void Main(string[] args) => BuildAvaloniaApp()
            .StartWithClassicDesktopLifetime(args);

        // Avalonia configuration, don't remove; also used by visual designer.
        public static AppBuilder BuildAvaloniaApp()
        {
            Register();
            return AppBuilder.Configure<App>()
                .UseReactiveUI()
                .UsePlatformDetect()
                .LogToTrace();
        }
        
        private static void Register()
        {
            Locator.CurrentMutable.Register<IWalletDatabase>(() => new WalletDatabase());
            Locator.CurrentMutable.Register<IWalletKeyDatabase>(() => new WalletKeyDatabase());
            Locator.CurrentMutable.Register<IMonkeyWalletService>(() => new MonkeyWalletService(
                new MnemonicService(),
                Locator.Current.GetService<IWalletKeyDatabase>(),
                Locator.Current.GetService<IWalletDatabase>()
            ));
        }
    }
}