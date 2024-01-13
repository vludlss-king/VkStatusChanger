using Microsoft.Extensions.Options;
using VkStatusChanger.Worker.Contracts.Helpers;
using VkStatusChanger.Worker.Controllers;
using VkStatusChanger.Worker.Helpers;
using VkStatusChanger.Worker.Models;

namespace VkStatusChanger.Worker.Tests.IntegrationTests
{
    public class ConfigCommandControllerTests
    {
        public ConfigCommandControllerTests()
        {
            
        }

        [Fact]
        public async Task Config_every_set_command_works_properly()
        {
            const string fileName = "settings_config_every_set.json";
            var (settingsHelper, sut) = Startup(fileName);
            SettingsCommand.EveryCommand.SetCommand command = new SettingsCommand.EveryCommand.SetCommand
            {
                StatusesTexts = new List<string> { "Status1", "Status2", "Status3" },
                Seconds = 60,
            };

            await sut.EverySet(command);

            var settings = await settingsHelper.ReadSettings();
            if (File.Exists(fileName))
                File.Delete(fileName);

            settings.Every!.StatusesTexts.Should().BeEquivalentTo(command.StatusesTexts);
            settings.Every.Seconds.Should().Be(command.Seconds);
            settings.Schedule!.Items.Should().BeEmpty();
        }

        private (ISettingsHelper settingsHelper, ConfigCommandController sut) Startup(string fileName)
        {
            var settingsFileStub = new Mock<IOptions<SettingsFile>>();
            settingsFileStub
                .Setup(setup => setup.Value)
                .Returns(new SettingsFile { Name = fileName });

            var settingsHelper = new SettingsHelper(settingsFileStub.Object);
            var sut = new ConfigCommandController(settingsHelper);

            return (settingsHelper, sut);
        }
    }
}
