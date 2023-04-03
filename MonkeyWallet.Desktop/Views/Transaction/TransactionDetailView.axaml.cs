using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace MonkeyWallet.Desktop.Views.Transaction;

public partial class TransactionDetailView : UserControl
{
    public TransactionDetailView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}