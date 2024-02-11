using VkStatusChanger.Worker.Contracts;
using VkStatusChanger.Worker.Contracts.Infrastructure;

namespace VkStatusChanger.Worker.Commands.Impl;

internal class Settings_Type_Set_Command : Command<Routes.Settings.Type.Set>
{
    private readonly ISettingsManager _settingsManager;

    public Settings_Type_Set_Command(ISettingsManager settingsManager)
    {
        _settingsManager = settingsManager;    
    }

    public async override Task<string> Execute(Routes.Settings.Type.Set request)
    {
        if (!ModelState.IsValid)
            return await BadCommand();

        var settings = await _settingsManager.Read();

        settings.Type = request.SettingsType;

        await _settingsManager.Write(settings);

        return await Ok("Тип настроек изменён.");
    }
}
