using VkStatusChanger.Worker.Contracts;
using VkStatusChanger.Worker.Contracts.Infrastructure;

namespace VkStatusChanger.Worker.Commands.Impl;

internal class Settings_Reset_Command : Command<Routes.Settings.Reset>
{
    private readonly ISettingsManager _settingsManager;

    public Settings_Reset_Command(ISettingsManager settingsManager)
    {
        _settingsManager = settingsManager;    
    }

    public override async Task<string> Execute(Routes.Settings.Reset request)
    {
        if (!ModelState.IsValid)
            return await BadCommand();

        _settingsManager.Reset();

        return await Ok("Настройки сброшены.");
    }
}
