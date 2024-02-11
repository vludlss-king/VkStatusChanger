using VkStatusChanger.Worker.Contracts;
using VkStatusChanger.Worker.Contracts.Infrastructure;

namespace VkStatusChanger.Worker.Commands.Impl;

internal class Settings_Type_Show_Command : Command<Routes.Settings.Type.Show>
{
    private readonly ISettingsManager _settingsManager;

    public Settings_Type_Show_Command(ISettingsManager settingsManager)
    {
        _settingsManager = settingsManager;    
    }

    public async override Task<string> Execute(Routes.Settings.Type.Show request)
    {
        if (!ModelState.IsValid)
            return BadCommand();

        var settings = await _settingsManager.Read();

        var output = $"Тип настроек: {settings.Type}";
        return output;
    }
}
