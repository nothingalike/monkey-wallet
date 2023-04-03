using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace MonkeyWallet.Desktop.Views.Transaction;

public partial class TransactionListItemView : UserControl
{
    public TransactionListItemView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}