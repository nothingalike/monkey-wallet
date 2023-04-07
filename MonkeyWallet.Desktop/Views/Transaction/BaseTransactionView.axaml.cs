using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using MonkeyWallet.Desktop.ViewModels.Transaction;

namespace MonkeyWallet.Desktop.Views.Transaction;

public partial class BaseTransactionView : ReactiveUserControl<BaseTransactionViewModel>
{
    public BaseTransactionView()
    {
        DataContext = new BaseTransactionViewModel();
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}