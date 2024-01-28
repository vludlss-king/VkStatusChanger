using VkStatusChanger.Worker.Contracts;
using VkStatusChanger.Worker.Contracts.Infrastructure;

namespace VkStatusChanger.Worker.Commands.Impl;

internal class Settings_Reset_Command : ICommand<Routes.Settings.Reset>
{
    private readonly ISettingsManager _settingsManager;

    public Settings_Reset_Command(ISettingsManager settingsManager)
    {
        _settingsManager = settingsManager;    
    }

    public Task<string> Execute(Routes.Settings.Reset request)
    {
        _settingsManager.Reset();

        var output = "Настройки сброшены.";
        return Task.FromResult(output);
    }
}
