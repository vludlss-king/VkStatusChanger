using Microsoft.Extensions.DependencyInjection;
using Quartz;
using VkNet.Abstractions;
using VkNet.Enums.Filters;
using VkNet.Model;
using VkNet;
using VkStatusChanger.Worker.Jobs;
using VkStatusChanger.Worker.Models;
using VkStatusChanger.Worker.Models.Settings;
using Microsoft.Extensions.Options;

namespace VkStatusChanger.Worker.Extensions
{
    internal static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddVkHttpClient(this IServiceCollection services)
        {
            services.AddScoped<IVkApi>(provider =>
            {
                var inputArgs = provider.GetRequiredService<IOptions<InputArgs>>().Value;

                var vkApi = new VkApi();

                var authParams = new ApiAuthParams
                {
                    ApplicationId = inputArgs.ApplicationId,
                    Login = inputArgs.Login,
                    Password = inputArgs.Password,
                    Settings = Settings.Status,
                };

                if (inputArgs.TwoFactorAuth)
                {
                    authParams.TwoFactorAuthorization = () =>
                    {
                        Console.WriteLine("Введите код двух факторной авторизации");
                        return Console.ReadLine()!.Trim();
                    };
                }

                vkApi.Authorize(authParams);

                return vkApi;
            });

            return services;
        }

        public static IServiceCollection AddJobScheduler(this IServiceCollection services, SettingsModel settingsModel)
        {
            services.AddQuartz(q =>
            {
                q.UseInMemoryStore();
                q.UseDefaultThreadPool(1);

                var jobKey = new JobKey(nameof(ScheduleStatusJob));
                q.AddJob<ScheduleStatusJob>(jobKey);

                if (settingsModel!.Every is not null && settingsModel!.Schedule is null)
                {
                    q.AddTrigger(t =>
                    {
                        t.ForJob(jobKey);
                        t.StartNow();
                        t.WithSimpleSchedule(s => s.WithIntervalInSeconds(settingsModel!.Every!.Seconds));

                        var dict = new Dictionary<string, object>()
                        {
                            ["statusText"] = settingsModel!.Every!.StatusesTexts
                        };
                        var jobData = new JobDataMap((IDictionary<string, object>)dict);
                        t.UsingJobData(jobData);
                    });
                }

                if (settingsModel!.Schedule is not null && settingsModel!.Every is null)
                {
                    foreach (var scheduleItem in settingsModel!.Schedule!.Items!)
                    {
                        q.AddTrigger(t =>
                        {
                            t.ForJob(jobKey);
                            t.StartAt(scheduleItem.Date);

                            t.UsingJobData("statusText", scheduleItem.StatusText!);
                        });
                    }
                }
            });

            return services;
        }
    }
}
