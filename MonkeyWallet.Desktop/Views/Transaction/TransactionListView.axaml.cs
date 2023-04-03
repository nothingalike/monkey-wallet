using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using MonkeyWallet.Desktop.ViewModels.Transaction;

namespace MonkeyWallet.Desktop.Views.Transaction;

public partial class TransactionListView : ReactiveUserControl<TransactionListView>
{
    public TransactionListView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}