using Microsoft.Extensions.Logging;
using Quartz;
using VkStatusChanger.Worker.Contracts.Infrastructure;

namespace VkStatusChanger.Worker.Jobs
{
    internal class EveryStatusJob : IJob
    {
        private readonly IVkStatusHttpClient _vkHttpClient;
        private readonly ILogger<EveryStatusJob> _logger;

        private static int _refireCount = 0;

        public EveryStatusJob(IVkStatusHttpClient vkHttpClient, ILogger<EveryStatusJob> logger)
        {
            _vkHttpClient = vkHttpClient;
            _logger = logger;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            _logger.LogInformation("Изменяю статус...");

            if (context.MergedJobDataMap.TryGetValue("statusText", out object statusesTextsAsObject))
            {
                var statusesTexts = (List<string>)statusesTextsAsObject;
                var statusText = statusesTexts[_refireCount];

                if(statusText is not null)
                {
                    var isSet = await _vkHttpClient.SetStatus(statusText);
                    if (isSet)
                        _logger.LogInformation("Статус успешно изменён!");
                }

                _refireCount++;
                if (_refireCount > statusesTexts.Count - 1)
                    _refireCount = 0;
            }
        }
    }
}
