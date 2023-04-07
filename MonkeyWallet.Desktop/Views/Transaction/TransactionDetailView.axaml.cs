using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using MonkeyWallet.Desktop.ViewModels.Transaction;

namespace MonkeyWallet.Desktop.Views.Transaction;

public partial class TransactionDetailView : ReactiveUserControl<TransactionDetailViewModel>
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