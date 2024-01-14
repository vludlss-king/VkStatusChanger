using VkNet.Enums.Filters;
using VkStatusChanger.Worker.Contracts.Helpers;
using VkStatusChanger.Worker.Contracts.Infrastructure;
using VkStatusChanger.Worker.Controllers;
using VkStatusChanger.Worker.Enums;
using VkStatusChanger.Worker.Models.Commands;
using VkStatusChanger.Worker.Models.Settings;

namespace VkStatusChanger.Worker.Tests.UnitTests
{
    public class SettingsCommandControllerTests
    {
        [Fact]
        public async Task Settings_type_set_command_shows_expected_output()
        {
            var parserResultStub = new Mock<ICustomParserResult>();
            var settingsHelperStub = new Mock<ISettingsHelper>();
            settingsHelperStub
                .Setup(setup => setup.ReadSettings())
                .Returns(Task.FromResult(new SettingsModel()
                {
                    SettingsType = SettingsType.Schedule
                }));
            var sut = new SettingsCommandController(parserResultStub.Object, settingsHelperStub.Object);
            SettingsCommand.TypeCommand.SetCommand command = new SettingsCommand.TypeCommand.SetCommand
            {
                SettingsType = SettingsType.Schedule
            };

            var output = await sut.TypeSet(command);

            output.Should().Be("Тип настроек изменён.");
        }

        [Fact]
        public async Task Settings_type_show_command_shows_expected_output()
        {
            var parserResultStub = new Mock<ICustomParserResult>();
            var settingsHelperStub = new Mock<ISettingsHelper>();
            settingsHelperStub
                .Setup(setup => setup.ReadSettings())
                .Returns(Task.FromResult(new SettingsModel()
                {
                    SettingsType = SettingsType.Schedule
                }));
            var sut = new SettingsCommandController(parserResultStub.Object, settingsHelperStub.Object);
            SettingsCommand.TypeCommand.ShowCommand command = new SettingsCommand.TypeCommand.ShowCommand();

            var output = await sut.TypeShow(command);

            output.Should().Be($"Тип настроек: {SettingsType.Schedule}");
        }
    }
}
