using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using MonkeyWallet.Desktop.ViewModels.Wallet.SendFunds;

namespace MonkeyWallet.Desktop.Views.Wallet.SendFunds;

public partial class SendFundsBuildView : ReactiveUserControl<SendFundsBuildViewModel>
{
    public SendFundsBuildView()
    {
        InitializeComponent();
    }
}