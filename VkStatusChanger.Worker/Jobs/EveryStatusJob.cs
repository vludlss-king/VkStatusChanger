using Quartz;
using VkNet.Abstractions;

namespace VkStatusChanger.Worker.Jobs
{
    internal class EveryStatusJob : IJob
    {
        private readonly IVkApi _vkHttpClient;

        private static int _refireCount = 0;

        public EveryStatusJob(IVkApi vkHttpClient)
        {
            _vkHttpClient = vkHttpClient;    
        }

        public async Task Execute(IJobExecutionContext context)
        {
            if(context.JobDetail.JobDataMap.TryGetValue("statusText", out object statusesTextsAsObject))
            {
                var statusesTexts = (List<string>)statusesTextsAsObject;
                var statusText = statusesTexts[_refireCount];

                await _vkHttpClient.Status.SetAsync(statusText);

                _refireCount++;
                if (_refireCount > statusesTexts.Count - 1)
                    _refireCount = 0;
            }
        }
    }
}
