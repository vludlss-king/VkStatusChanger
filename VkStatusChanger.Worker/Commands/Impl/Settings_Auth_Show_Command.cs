using VkStatusChanger.Worker.Contracts;
using VkStatusChanger.Worker.Contracts.Infrastructure;

namespace VkStatusChanger.Worker.Commands.Impl;

internal class Settings_Auth_Show_Command : Command<Routes.Settings.Auth.Show>
{
    private readonly ISettingsManager _settingsManager;

    public Settings_Auth_Show_Command(ISettingsManager settingsManager)
    {
        _settingsManager = settingsManager;    
    }

    public async override Task<string> Execute(Routes.Settings.Auth.Show request)
    {
        if (!ModelState.IsValid)
            return await BadCommand();

        var settings = await _settingsManager.Read();

        var token = string.IsNullOrWhiteSpace(settings.AccessToken)
            ? "отсутствует"
            : settings.AccessToken;

        return await Ok($"Токен авторизации: {token}");
    }
}
