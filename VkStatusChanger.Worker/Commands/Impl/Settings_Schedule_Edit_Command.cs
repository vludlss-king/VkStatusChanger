using VkStatusChanger.Worker.Contracts;
using VkStatusChanger.Worker.Contracts.Infrastructure;

namespace VkStatusChanger.Worker.Commands.Impl;

internal class Settings_Schedule_Edit_Command : ICommand<Routes.Settings.Schedule.Edit>
{
    private readonly ISettingsManager _settingsManager;

    public Settings_Schedule_Edit_Command(ISettingsManager settingsManager)
    {
        _settingsManager = settingsManager;    
    }

    public async Task<string> Execute(Routes.Settings.Schedule.Edit request)
    {
        var settings = await _settingsManager.Read();

        var item = settings.DateTimeSchedule.Items[request.Id - 1];

        item.Status = request.Status;
        item.Date = request.Date;
        item.Time = request.Time;

        await _settingsManager.Write(settings);

        var output = "Расписание изменено.";
        return output;
    }
}
