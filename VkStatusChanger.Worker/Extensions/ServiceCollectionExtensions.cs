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
using VkStatusChanger.Worker.Enums;
using VkStatusChanger.Worker.Contracts.Helpers;
using VkStatusChanger.Worker.Helpers;
using VkStatusChanger.Worker.Controllers;

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

        public static IServiceCollection AddJobScheduler(this IServiceCollection services)
        {
            services.AddQuartz(quartzCfg =>
            {
                quartzCfg.UseInMemoryStore();
                quartzCfg.UseDefaultThreadPool(1);

                var provider = services.BuildServiceProvider();
                var settingsHelper = provider.GetRequiredService<ISettingsHelper>();
                var settingsModel = settingsHelper.ReadSettings().GetAwaiter().GetResult();

                const string jobDataKey = "statusText";
                switch (settingsModel.SettingsType)
                {
                    case SettingsType.Every:
                        {
                            var everyStatusJobKey = new JobKey(nameof(EveryStatusJob));
                            quartzCfg.AddJob<EveryStatusJob>(everyStatusJobKey);

                            quartzCfg.AddTrigger(triggerCfg =>
                            {
                                triggerCfg.ForJob(everyStatusJobKey)
                                    .StartNow()
                                    .WithSimpleSchedule(builder => builder.WithIntervalInSeconds(settingsModel!.Every!.Seconds).RepeatForever());

                                var dict = new Dictionary<string, object>()
                                {
                                    [jobDataKey] = settingsModel!.Every!.StatusesTexts!
                                };
                                var jobData = new JobDataMap((IDictionary<string, object>)dict);
                                triggerCfg.UsingJobData(jobData);
                            });

                            break;
                        }
                    case SettingsType.Schedule:
                        {
                            var scheduleStatusJobKey = new JobKey(nameof(ScheduleStatusJob));
                            quartzCfg.AddJob<ScheduleStatusJob>(scheduleStatusJobKey);

                            var dateTimeNow = DateTime.Now;
                            var scheduleItems = settingsModel!.Schedule!.Items!.Select(item => new
                            {
                                DateTime = item.Date.Add(item.Time),
                                StatusText = item.StatusText
                            })
                                .Where(item => item.DateTime > dateTimeNow);
                            foreach (var scheduleItem in scheduleItems)
                            {
                                quartzCfg.AddTrigger(triggerCfg =>
                                {
                                    triggerCfg.ForJob(scheduleStatusJobKey)
                                        .StartAt(scheduleItem.DateTime);

                                    triggerCfg.UsingJobData(jobDataKey, scheduleItem.StatusText!);
                                });
                            }

                            break;
                        }
                }
            });

            services.AddQuartzHostedService(options =>
            {
                options.WaitForJobsToComplete = true;
            });

            return services;
        }

        public static IServiceCollection AddConfiguration(this IServiceCollection services, ConfigurationManager configuration)
        {
            services.AddSingleton(provider => Options.Create(new SettingsFile { Name = "settings.json" }));
            services.AddSingleton<ISettingsHelper, SettingsHelper>();

            services.AddSingleton(provider =>
            {
                var settingsHelper = provider.GetRequiredService<ISettingsHelper>();
                var settingsModel = settingsHelper.ReadSettings().GetAwaiter().GetResult();
                return Options.Create(settingsModel);
            });

            configuration.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddUserSecrets<InputArgs>();
            services.AddSingleton(provider =>
            {
                var env = provider.GetRequiredService<IHostEnvironment>();
                var configuration = provider.GetRequiredService<IConfiguration>();
                var settingsHelper = provider.GetRequiredService<ISettingsHelper>();
                var settingsModel = settingsHelper.ReadSettings().GetAwaiter().GetResult();

                if (env.IsDevelopment())
                    return Options.Create(configuration.Get<InputArgs>()!);
                else
                    return Options.Create(new InputArgs { AccessToken = settingsModel.AccessToken });
            });

            return services;
        }

        public static IServiceCollection AddControllers(this IServiceCollection services)
        {
            services.AddSingleton<ConfigCommandController>();

            return services;
        }
    }
}
