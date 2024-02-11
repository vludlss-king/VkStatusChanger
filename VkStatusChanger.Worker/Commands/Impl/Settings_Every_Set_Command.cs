using VkStatusChanger.Worker.Contracts;
using VkStatusChanger.Worker.Contracts.Infrastructure;

namespace VkStatusChanger.Worker.Commands.Impl;

internal class Settings_Every_Set_Command : Command<Routes.Settings.Every.Set>
{
    private readonly ISettingsManager _settingsManager;
        
    public Settings_Every_Set_Command(ISettingsManager settingsManager)
    {
        _settingsManager = settingsManager;
    }

    public async override Task<string> Execute(Routes.Settings.Every.Set request)
    {
        var settings = await _settingsManager.Read();

        settings.EverySecondsSchedule.Statuses = request.Statuses.ToList();
        settings.EverySecondsSchedule.Seconds = request.Seconds;

        await _settingsManager.Write(settings);

        var output = "Настройки Every изменены.";
        return output;
    }
}
