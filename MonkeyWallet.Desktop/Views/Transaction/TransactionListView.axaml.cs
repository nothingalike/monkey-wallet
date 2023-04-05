using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using MonkeyWallet.Desktop.ViewModels.Transaction;
using ReactiveUI;

namespace MonkeyWallet.Desktop.Views.Transaction;

public partial class TransactionListView : ReactiveUserControl<TransactionListViewModel>
{
    public TransactionListView()
    {
        this.WhenActivated(disposables => { });
        AvaloniaXamlLoader.Load(this);
    }
}