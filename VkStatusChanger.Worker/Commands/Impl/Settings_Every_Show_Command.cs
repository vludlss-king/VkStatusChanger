using System.Text;
using VkStatusChanger.Worker.Contracts;
using VkStatusChanger.Worker.Contracts.Infrastructure;

namespace VkStatusChanger.Worker.Commands.Impl;

internal class Settings_Every_Show_Command : Command<Routes.Settings.Every.Show>
{
    private readonly ISettingsManager _settingsManager;

    public Settings_Every_Show_Command(ISettingsManager settingsManager)
    {
        _settingsManager = settingsManager;    
    }

    public async override Task<string> Execute(Routes.Settings.Every.Show request)
    {
        if (!ModelState.IsValid)
            return BadCommand();

        var settings = await _settingsManager.Read();

        var outputBuilder = new StringBuilder();
        outputBuilder.AppendLine($"Статусы меняются каждые {settings.EverySecondsSchedule.Seconds} секунд, список статусов:");

        if (!settings.EverySecondsSchedule.Statuses.Any())
        {
            outputBuilder.AppendLine("отсутствуют");
        }
        else
        {
            for (int index = 0; index < settings.EverySecondsSchedule.Statuses.Count; index++)
            {
                var statusText = settings.EverySecondsSchedule.Statuses[index];
                outputBuilder.AppendLine($"{index + 1}. {statusText}");
            }
        }

        return outputBuilder.ToString();
    }
}
