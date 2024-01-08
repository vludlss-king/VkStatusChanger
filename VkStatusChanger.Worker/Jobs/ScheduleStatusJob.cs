using Quartz;
using VkNet.Abstractions;

namespace VkStatusChanger.Worker.Jobs
{
    internal class ScheduleStatusJob : IJob
    {
        private readonly IVkApi _vkHttpClient;

        public ScheduleStatusJob(IVkApi vkHttpClient)
        {
            _vkHttpClient = vkHttpClient;    
        }

        public async Task Execute(IJobExecutionContext context)
        {
            var statusText = context.JobDetail.JobDataMap.GetString("statusText");
            await _vkHttpClient.Status.SetAsync(statusText);
        }
    }
}
