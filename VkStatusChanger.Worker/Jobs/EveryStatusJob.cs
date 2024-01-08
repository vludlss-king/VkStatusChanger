using Quartz;
using VkStatusChanger.Worker.Contracts.Infrastructure;

namespace VkStatusChanger.Worker.Jobs
{
    internal class EveryStatusJob : IJob
    {
        private readonly IVkStatusHttpClient _vkHttpClient;

        private static int _refireCount = 0;

        public EveryStatusJob(IVkStatusHttpClient vkHttpClient)
        {
            _vkHttpClient = vkHttpClient;    
        }

        public async Task Execute(IJobExecutionContext context)
        {
            if(context.JobDetail.JobDataMap.TryGetValue("statusText", out object statusesTextsAsObject))
            {
                var statusesTexts = (List<string>)statusesTextsAsObject;
                var statusText = statusesTexts[_refireCount];

                if(statusText is not null)
                    await _vkHttpClient.SetStatus(statusText);

                _refireCount++;
                if (_refireCount > statusesTexts.Count - 1)
                    _refireCount = 0;
            }
        }
    }
}
