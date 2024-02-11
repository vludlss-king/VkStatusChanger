using VkStatusChanger.Worker.Contracts;
using VkStatusChanger.Worker.Contracts.Infrastructure;

namespace VkStatusChanger.Worker.Commands.Impl;

internal class Settings_Schedule_Remove_Command : Command<Routes.Settings.Schedule.Remove>
{
    private readonly ISettingsManager _settingsManager;

    public Settings_Schedule_Remove_Command(ISettingsManager settingsManager)
    {
        _settingsManager = settingsManager;    
    }

    public async override Task<string> Execute(Routes.Settings.Schedule.Remove request)
    {
        if (!ModelState.IsValid)
            return BadCommand();

        var settings = await _settingsManager.Read();

        if (request.Id.HasValue)
            settings.DateTimeSchedule.Items.RemoveAt(request.Id.Value - 1);
        else
            settings.DateTimeSchedule.Items.RemoveAll(_ => true);

        await _settingsManager.Write(settings);

        var output = "Расписание удалено.";
        return output;
    }
}
