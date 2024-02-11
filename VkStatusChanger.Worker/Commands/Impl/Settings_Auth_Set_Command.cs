using VkStatusChanger.Worker.Contracts;
using VkStatusChanger.Worker.Contracts.Infrastructure;

namespace VkStatusChanger.Worker.Commands.Impl;

internal class Settings_Auth_Set_Command : Command<Routes.Settings.Auth.Set>
{
    private readonly ISettingsManager _settingsManager;

    public Settings_Auth_Set_Command(ISettingsManager settingsManager)
    {
        _settingsManager = settingsManager;    
    }

    public async override Task<string> Execute(Routes.Settings.Auth.Set request)
    {
        if (!ModelState.IsValid)
            return await BadCommand();

        var settings = await _settingsManager.Read();

        settings.AccessToken = request.AccessToken;

        await _settingsManager.Write(settings);

        return await Ok("Токен авторизации изменён.");
    }
}
