﻿using VkStatusChanger.Worker.Contracts;
using VkStatusChanger.Worker.Contracts.Infrastructure;

namespace VkStatusChanger.Worker.Commands.Impl;

internal class Settings_Type_Show_Command : ICommand<Routes.Settings.Type.Show>
{
    private readonly ISettingsManager _settingsManager;

    public Settings_Type_Show_Command(ISettingsManager settingsManager)
    {
        _settingsManager = settingsManager;    
    }

    public async Task<string> Execute(Routes.Settings.Type.Show request)
    {
        var settings = await _settingsManager.Read();

        var output = $"Тип настроек: {settings.Type}";
        return output;
    }
}
