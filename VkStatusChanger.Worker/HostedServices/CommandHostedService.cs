using CommandLine;
using Microsoft.Extensions.Hosting;
using VkStatusChanger.Worker.Controllers;
using VkStatusChanger.Worker.Models;

namespace VkStatusChanger.Worker.HostedServices
{
    internal class CommandHostedService : IHostedService
    {
        private readonly ParserResult<object> _parserResult;
        private readonly ConfigCommandController _configCommandController;

        public CommandHostedService(ParserResult<object> parserResult, ConfigCommandController configCommandController)
        {
            _parserResult = parserResult;
            _configCommandController = configCommandController;    
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await _parserResult.WithParsedAsync<Config.Every.Set>(_configCommandController.EverySet);
            await _parserResult.WithParsedAsync<Config.Every.Show>(_configCommandController.EveryShow);

            await _parserResult.WithParsedAsync<Config.Schedule.Add>(_configCommandController.ScheduleAdd);
            await _parserResult.WithParsedAsync<Config.Schedule.Edit>(_configCommandController.ScheduleUpdate);
            await _parserResult.WithParsedAsync<Config.Schedule.Remove>(_configCommandController.ScheduleRemove);
            await _parserResult.WithParsedAsync<Config.Schedule.List>(_configCommandController.ScheduleList);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
