using VkStatusChanger.Worker.Contracts;
using VkStatusChanger.Worker.Contracts.Infrastructure;

namespace VkStatusChanger.Worker.Commands.Impl;

internal class Settings_Type_Set_Command : ICommand<Routes.Settings.Type.Set>
{
    private readonly ISettingsManager _settingsManager;

    public Settings_Type_Set_Command(ISettingsManager settingsManager)
    {
        _settingsManager = settingsManager;    
    }

    public async Task<string> Execute(Routes.Settings.Type.Set request)
    {
        var settings = await _settingsManager.Read();

        settings.Type = request.SettingsType;

        await _settingsManager.Write(settings);

        var output = "Тип настроек изменён.";
        return output;
    }
}
