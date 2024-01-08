using Microsoft.Extensions.Logging;
using Quartz;
using VkStatusChanger.Worker.Contracts.Infrastructure;

namespace VkStatusChanger.Worker.Jobs
{
    internal class ScheduleStatusJob : IJob
    {
        private readonly IVkStatusHttpClient _vkHttpClient;
        private readonly ILogger<ScheduleStatusJob> _logger;

        public ScheduleStatusJob(IVkStatusHttpClient vkHttpClient, ILogger<ScheduleStatusJob> logger)
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
}
