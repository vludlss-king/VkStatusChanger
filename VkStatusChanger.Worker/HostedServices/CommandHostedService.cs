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
            await _parserResult.WithParsedAsync<SettingsCommand.EveryCommand.SetCommand>(_configCommandController.EverySet);
            await _parserResult.WithParsedAsync<SettingsCommand.EveryCommand.ShowCommand>(_configCommandController.EveryShow);

            await _parserResult.WithParsedAsync<SettingsCommand.ScheduleCommand.AddCommand>(_configCommandController.ScheduleAdd);
            await _parserResult.WithParsedAsync<SettingsCommand.ScheduleCommand.EditCommand>(_configCommandController.ScheduleUpdate);
            await _parserResult.WithParsedAsync<SettingsCommand.ScheduleCommand.RemoveCommand>(_configCommandController.ScheduleRemove);
            await _parserResult.WithParsedAsync<SettingsCommand.ScheduleCommand.ListCommand>(_configCommandController.ScheduleList);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
