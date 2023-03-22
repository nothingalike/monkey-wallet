using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using MonkeyWallet.Desktop.ViewModels.Wallet;
using ReactiveUI;

namespace MonkeyWallet.Desktop.Views.Wallet;

public partial class DisclaimerView : ReactiveUserControl<DisclaimerViewModel>
{
    public CheckBox AcceptedCheckBox => this.FindControl<CheckBox>("cbAccept");
    public DisclaimerView()
    {
        InitializeComponent();

        this.WhenActivated(d =>
        {
            AcceptedCheckBox.Command = ReactiveCommand.CreateFromTask(async () =>
            {
                if (AcceptedCheckBox.IsChecked.HasValue)
                    ViewModel.Accepted = AcceptedCheckBox.IsChecked.Value;
            });
        });
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}