using Microsoft.Extensions.DependencyInjection;
using Quartz;
using VkNet.Abstractions;
using VkNet.Model;
using VkNet;
using VkStatusChanger.Worker.Jobs;
using VkStatusChanger.Worker.Models;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using VkStatusChanger.Worker.Contracts.Infrastructure;
using VkStatusChanger.Worker.Infrastructure.HttpClients;
using VkStatusChanger.Worker.Enums;
using VkStatusChanger.Worker.Controllers;
using VkStatusChanger.Worker.Infrastructure;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;

namespace VkStatusChanger.Worker.Extensions
{
    internal static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddHttpClients(this IServiceCollection services)
        {
            services.AddVkApi();
            services.AddScoped<IVkStatusHttpClient, VkStatusHttpClient>();

            return services;
        }

        private static IServiceCollection AddVkApi(this IServiceCollection services)
        {
            services.AddScoped<IVkApi>(provider =>
            {
                var inputArgs = provider.GetRequiredService<IOptions<Authorization>>().Value;

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
                var settingsManager = provider.GetRequiredService<ISettingsManager>();
                var logger = provider.GetRequiredService<ILogger<Program>>();

                var settingsModel = settingsManager.Read().GetAwaiter().GetResult();

                const string jobDataKey = "statusText";
                switch (settingsModel.Type)
                {
                    case SettingsType.Every:
                        {
                            var everyStatusJobKey = new JobKey(nameof(EveryJob));
                            quartzCfg.AddJob<EveryJob>(everyStatusJobKey);

                            quartzCfg.AddTrigger(triggerCfg =>
                            {
                                triggerCfg.ForJob(everyStatusJobKey)
                                    .StartNow()
                                    .WithSimpleSchedule(builder => builder.WithIntervalInSeconds(settingsModel.Every.Seconds).RepeatForever());

                                var dict = new Dictionary<string, object>()
                                {
                                    [jobDataKey] = settingsModel.Every.StatusesTexts
                                };
                                var jobData = new JobDataMap((IDictionary<string, object>)dict);
                                triggerCfg.UsingJobData(jobData);
                            });

                            logger.LogInformation("Выбран тип настроек Every");

                            break;
                        }
                    case SettingsType.Schedule:
                        {
                            var scheduleStatusJobKey = new JobKey(nameof(ScheduleJob));
                            quartzCfg.AddJob<ScheduleJob>(scheduleStatusJobKey);

                            var dateTimeNow = DateTime.Now;
                            var scheduleItems = settingsModel.Schedule.Items.Select(item => new
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

                            logger.LogInformation("Выбран тип настроек Schedule");

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
            services.AddSingleton<ISettingsManager, SettingsManager>();

            services.AddSingleton(provider =>
            {
                var settingsManager = provider.GetRequiredService<ISettingsManager>();
                var settingsModel = settingsManager.Read().GetAwaiter().GetResult();
                return Options.Create(settingsModel);
            });

            configuration.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddUserSecrets<Authorization>();
            services.AddSingleton(provider =>
            {
                var env = provider.GetRequiredService<IHostEnvironment>();
                var configuration = provider.GetRequiredService<IConfiguration>();
                var settingsManager = provider.GetRequiredService<ISettingsManager>();
                var settingsModel = settingsManager.Read().GetAwaiter().GetResult();

                if (env.IsDevelopment())
                    return Options.Create(configuration.Get<Authorization>()!);
                else
                    return Options.Create(new Authorization { AccessToken = settingsModel.AccessToken });
            });

            return services;
        }

        public static IServiceCollection AddControllers(this IServiceCollection services)
        {
            services.AddHostedService<SettingsCommandController>();

            return services;
        }

        public static IServiceCollection AddSerilog(this IServiceCollection services)
        {
            services.AddSerilog((provider, cfg) =>
            {
                var env = provider.GetRequiredService<IHostEnvironment>();

                cfg.Enrich.FromLogContext()
                    .WriteTo.Console();

                if (env.IsProduction())
                {
                    cfg.MinimumLevel.Override("Microsoft", LogEventLevel.Fatal);
                    cfg.MinimumLevel.Override("Quartz", LogEventLevel.Fatal);
                }
                else
                    cfg.MinimumLevel.Information();
            });

            return services;
        }
    }
}
