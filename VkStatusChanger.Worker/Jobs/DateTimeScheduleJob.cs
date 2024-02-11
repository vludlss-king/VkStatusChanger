using Microsoft.Extensions.Logging;
using Quartz;
using VkStatusChanger.Worker.Contracts.Infrastructure;

namespace VkStatusChanger.Worker.Jobs;

internal class DateTimeScheduleJob : IJob
{
    private readonly IVkStatusHttpClient _vkHttpClient;
    private readonly ILogger<DateTimeScheduleJob> _logger;

    public DateTimeScheduleJob(IVkStatusHttpClient vkHttpClient, ILogger<DateTimeScheduleJob> logger)
    {
        _vkHttpClient = vkHttpClient;
        _logger = logger;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        _logger.LogInformation("Изменяю статус...");

        var statusText = context.MergedJobDataMap.GetString("statusText");
        if(statusText is not null)
        {
            var isSet = await _vkHttpClient.SetStatus(statusText);
            if (isSet)
                _logger.LogInformation("Статус успешно изменён!");
        }
    }
}
