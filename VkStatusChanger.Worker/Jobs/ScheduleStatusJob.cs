using Quartz;
using VkStatusChanger.Worker.Contracts.Infrastructure;

namespace VkStatusChanger.Worker.Jobs
{
    internal class ScheduleStatusJob : IJob
    {
        private readonly IVkStatusHttpClient _vkHttpClient;

        public ScheduleStatusJob(IVkStatusHttpClient vkHttpClient)
        {
            _vkHttpClient = vkHttpClient;    
        }

        public async Task Execute(IJobExecutionContext context)
        {
            var statusText = context.JobDetail.JobDataMap.GetString("statusText");

            if(statusText is not null)
                await _vkHttpClient.SetStatus(statusText);
        }
    }
}
