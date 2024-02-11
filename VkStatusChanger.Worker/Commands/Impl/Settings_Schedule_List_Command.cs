using VkStatusChanger.Worker.Contracts;
using VkStatusChanger.Worker.Contracts.Infrastructure;

namespace VkStatusChanger.Worker.Commands.Impl;

internal class Settings_Schedule_List_Command : Command<Routes.Settings.Schedule.List>
{
    private readonly ISettingsManager _settingsManager;

    public Settings_Schedule_List_Command(ISettingsManager settingsManager)
    {
        _settingsManager = settingsManager;    
    }

    public async override Task<string> Execute(Routes.Settings.Schedule.List request)
    {
        if (!ModelState.IsValid)
            return BadCommand();

        var settings = await _settingsManager.Read();

        string output = "";
        if (!settings.DateTimeSchedule.Items.Any())
            output = "Расписания отсутствуют.";
        else
            for (int index = 0; index < settings.DateTimeSchedule.Items.Count; index++)
            {
                var scheduleItem = settings.DateTimeSchedule.Items[index];
                output = $"{index + 1}. Статус: {scheduleItem.Status}, Дата: {scheduleItem.Date:dd.MM.yyyy}, Время: {scheduleItem.Time}";
            }

        return output;
    }
}
