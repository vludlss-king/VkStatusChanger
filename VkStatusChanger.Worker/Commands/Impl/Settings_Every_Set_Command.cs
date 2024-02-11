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
        if (!ModelState.IsValid)
            return await BadCommand();

        var settings = await _settingsManager.Read();

        settings.EverySecondsSchedule.Statuses = request.Statuses.ToList();
        settings.EverySecondsSchedule.Seconds = request.Seconds;

        await _settingsManager.Write(settings);

        return await Ok("Настройки Every изменены.");
    }
}
