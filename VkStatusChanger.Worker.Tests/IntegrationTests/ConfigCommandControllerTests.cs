using Microsoft.Extensions.Options;
using VkStatusChanger.Worker.Contracts.Helpers;
using VkStatusChanger.Worker.Controllers;
using VkStatusChanger.Worker.Helpers;
using VkStatusChanger.Worker.Models;

namespace VkStatusChanger.Worker.Tests.IntegrationTests
{
    public class ConfigCommandControllerTests
    {
        private readonly ISettingsHelper _settingsHelper;
        private readonly ConfigCommandController _sut;

        public ConfigCommandControllerTests()
        {
            var settingsFileStub = new Mock<IOptions<SettingsFile>>();
            settingsFileStub
                .Setup(setup => setup.Value)
                .Returns(new SettingsFile { Name = "settings_test.json" });

            _settingsHelper = new SettingsHelper(settingsFileStub.Object);

            _sut = new ConfigCommandController(_settingsHelper);
        }

        [Fact]
        public async Task Config_every_set_command_works_properly()
        {
            SettingsCommand.EveryCommand.SetCommand command = new SettingsCommand.EveryCommand.SetCommand
            {
                StatusesTexts = new List<string> { "Status1", "Status2", "Status3" },
                Seconds = 60,
            };

            await _sut.EverySet(command);

            var settings = await _settingsHelper.ReadSettings();

            settings.Every.StatusesTexts.Should().BeEquivalentTo(command.StatusesTexts);
            settings.Every.Seconds.Should().Be(command.Seconds);
        }
    }
}
