using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using MonkeyWallet.Core.Data;
using ReactiveUI;

namespace MonkeyWallet.Desktop.ViewModels.Settings;

public class MonkeySettingsViewModel: ViewModelBase, IRoutableViewModel
{
    private List<Core.Data.Models.Settings> _settings;
    private readonly ISettingsDatabase _settingsDatabase;
    
    public string? UrlPathSegment { get; }
    public IScreen HostScreen { get; }
    
    public ICommand Submit { get; set; }

    public int NetworkIndex { get; set; } = 0;

    public MonkeySettingsViewModel(ISettingsDatabase settingsDatabase)
    {
        _settingsDatabase = settingsDatabase;
        Submit = ReactiveCommand.CreateFromTask(SubmitHandler);

        Task.Run(() => LoadSettings());
    }

    private async Task SubmitHandler()
    {
        var networkSetting = _settings.FirstOrDefault(x => x.Key == "Network");
        if (networkSetting is null)
            networkSetting = new Core.Data.Models.Settings()
            {
                Key = "Network"
            };
            
        networkSetting.Value = NetworkIndex switch
        {
            0 => "mainnet",
            1 => "preprod",
            2 => "preview",
            _ => "mainnet"
        };
        await _settingsDatabase.SaveAsync(networkSetting);

    }

    private async Task LoadSettings()
    {
        _settings = await _settingsDatabase.ListAsync();

        var networkSettings = _settings.FirstOrDefault(x => x.Key == "Network");
        if (networkSettings is not null)
            NetworkIndex = networkSettings.Value switch
            {
                "mainnet" => 0,
                "preprod" => 1,
                "preview" => 2,
                _ => 0
            };
    }
}