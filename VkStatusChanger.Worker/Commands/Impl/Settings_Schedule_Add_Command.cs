using VkStatusChanger.Worker.Contracts;
using VkStatusChanger.Worker.Contracts.Infrastructure;
using VkStatusChanger.Worker.Models.UserSettings;

namespace VkStatusChanger.Worker.Commands.Impl;

internal class Settings_Schedule_Add_Command : Command<Routes.Settings.Schedule.Add>
{
    private readonly ISettingsManager _settingsManager;

    public Settings_Schedule_Add_Command(ISettingsManager settingsManager)
    {
        _settingsManager = settingsManager;    
    }

    public async override Task<string> Execute(Routes.Settings.Schedule.Add request)
    {
        if (!ModelState.IsValid)
            return await BadCommand();

        var settings = await _settingsManager.Read();

        var scheduleItem = new DateTimeScheduleItem
        {
            Status = request.Status,
            Date = request.Date,
            Time = request.Time,
        };
        settings.DateTimeSchedule.Items.Add(scheduleItem);

        await _settingsManager.Write(settings);

        return await Ok("Расписание добавлено.");
    }
}
