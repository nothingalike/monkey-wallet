using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using MonkeyWallet.Desktop.ViewModels.Wallet;
using ReactiveUI;

namespace MonkeyWallet.Desktop.Views.Wallet;

public partial class WalletListView : ReactiveUserControl<WalletListViewModel>
{
    public WalletListView()
    {
        this.WhenActivated(disposables => { });
        AvaloniaXamlLoader.Load(this);
    }
    
}