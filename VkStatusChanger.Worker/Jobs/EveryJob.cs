using Microsoft.Extensions.Logging;
using Quartz;
using VkStatusChanger.Worker.Contracts.Infrastructure;

namespace VkStatusChanger.Worker.Jobs
{
    internal class EveryJob : IJob
    {
        private readonly IVkStatusHttpClient _vkHttpClient;
        private readonly ILogger<EveryJob> _logger;

        private static int _refireCount = 0;

        public EveryJob(IVkStatusHttpClient vkHttpClient, ILogger<EveryJob> logger)
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
