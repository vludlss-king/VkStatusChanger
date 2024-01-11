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
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using VkStatusChanger.Worker.Contracts.Infrastructure;
using VkStatusChanger.Worker.Infrastructure.HttpClients;

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
                    AccessToken = inputArgs.AccessToken
                };
                
                vkApi.Authorize(authParams);

                return vkApi;
            });

            services.AddScoped<IVkStatusHttpClient, VkStatusHttpClient>();

            return services;
        }

        public static IServiceCollection AddJobScheduler(this IServiceCollection services, SettingsModel settingsModel)
        {
            services.AddQuartz(q =>
            {
                q.UseInMemoryStore();
                q.UseDefaultThreadPool(1);

                const string jobDataKey = "statusText";
                if (settingsModel!.Every is not null && settingsModel!.Schedule is null)
                {
                    var everyStatusJobKey = new JobKey(nameof(EveryStatusJob));
                    q.AddJob<EveryStatusJob>(everyStatusJobKey);

                    q.AddTrigger(t =>
                    {
                        t.ForJob(everyStatusJobKey)
                            .StartNow()
                            .WithSimpleSchedule(s => s.WithIntervalInSeconds(settingsModel!.Every!.Seconds).RepeatForever());

                        var dict = new Dictionary<string, object>()
                        {
                            [jobDataKey] = settingsModel!.Every!.StatusesTexts!
                        };
                        var jobData = new JobDataMap((IDictionary<string, object>)dict);
                        t.UsingJobData(jobData);
                    });
                }
                if (settingsModel!.Schedule is not null && settingsModel!.Every is null)
                {
                    var scheduleStatusJobKey = new JobKey(nameof(ScheduleStatusJob));
                    q.AddJob<ScheduleStatusJob>(scheduleStatusJobKey);

                    var dateNow = DateTime.Now;
                    foreach (var scheduleItem in settingsModel!.Schedule!.Items!.Where(item => item.Date > dateNow))
                    {
                        q.AddTrigger(t =>
                        {
                            t.ForJob(scheduleStatusJobKey)
                                .StartAt(scheduleItem.Date);

                            t.UsingJobData(jobDataKey, scheduleItem.StatusText!);
                        });
                    }
                }
            });

            services.AddQuartzHostedService(options =>
            {
                options.WaitForJobsToComplete = true;
            });

            return services;
        }

        public static IServiceCollection AddConfiguration(this IServiceCollection services, ConfigurationManager configuration, SettingsModel settingsModel, InputArgs inputArgs)
        {
            services.AddSingleton(provider => Options.Create(settingsModel));

            configuration.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddUserSecrets<InputArgs>();
            services.AddSingleton(provider =>
            {
                var env = provider.GetRequiredService<IHostEnvironment>();
                var configuration = provider.GetRequiredService<IConfiguration>();

                if (env.IsDevelopment())
                    return Options.Create(configuration.Get<InputArgs>()!);
                else
                    return Options.Create(inputArgs);
            });

            return services;
        }
    }
}
