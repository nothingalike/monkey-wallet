using System.Net.NetworkInformation;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using MonkeyWallet.Core.Data;
using MonkeyWallet.Desktop.ViewModels.Settings;
using ReactiveUI;
using Splat;

namespace MonkeyWallet.Desktop.Views.Settings;

public partial class MonkeySettingsView : ReactiveUserControl<MonkeySettingsViewModel>
{
    private ComboBox networkInput => this.FindControl<ComboBox>("cbNetwork");
    public MonkeySettingsView()
    {
        DataContext = new MonkeySettingsViewModel(Locator.Current.GetService<ISettingsDatabase>());
        InitializeComponent();
        
        this.WhenActivated(d =>
        {
            networkInput.PropertyChanged += NetworkChange;
        });
    }

    private void NetworkChange(object? sender, AvaloniaPropertyChangedEventArgs e)
    {
        ViewModel.NetworkIndex = networkInput.SelectedIndex;
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}