using VkStatusChanger.Worker.Contracts.Helpers;
using VkStatusChanger.Worker.Contracts.Infrastructure;
using VkStatusChanger.Worker.Controllers;
using VkStatusChanger.Worker.Enums;
using VkStatusChanger.Worker.Models.Commands;
using VkStatusChanger.Worker.Models.UserSettings;

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
                .Setup(setup => setup.Read())
                .Returns(Task.FromResult(new Settings()
                {
                    Type = SettingsType.Schedule
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
                .Setup(setup => setup.Read())
                .Returns(Task.FromResult(new Settings()
                {
                    Type = SettingsType.Schedule
                }));
            var sut = new SettingsCommandController(parserResultStub.Object, settingsHelperStub.Object);
            SettingsCommand.TypeCommand.ShowCommand command = new SettingsCommand.TypeCommand.ShowCommand();

            var output = await sut.TypeShow(command);

            output.Should().Be($"Тип настроек: {SettingsType.Schedule}");
        }

        [Fact]
        public async Task Settings_auth_set_command_shows_expected_output()
        {
            const string accessToken = "NewAccessToken";
            var parserResultStub = new Mock<ICustomParserResult>();
            var settingsHelperStub = new Mock<ISettingsHelper>();
            settingsHelperStub
                .Setup(setup => setup.Read())
                .Returns(Task.FromResult(new Settings()
                {
                    AccessToken = accessToken
                }));
            var sut = new SettingsCommandController(parserResultStub.Object, settingsHelperStub.Object);
            SettingsCommand.AuthCommand.SetCommand command = new SettingsCommand.AuthCommand.SetCommand();

            var output = await sut.AuthSet(command);

            output.Should().Be("Токен авторизации изменён.");
        }

        [Fact]
        public async Task Settings_auth_show_command_shows_expected_output()
        {
            const string accessToken = "NewAccessToken";
            var parserResultStub = new Mock<ICustomParserResult>();
            var settingsHelperStub = new Mock<ISettingsHelper>();
            settingsHelperStub
                .Setup(setup => setup.Read())
                .Returns(Task.FromResult(new Settings()
                {
                    AccessToken = accessToken
                }));
            var sut = new SettingsCommandController(parserResultStub.Object, settingsHelperStub.Object);
            SettingsCommand.AuthCommand.ShowCommand command = new SettingsCommand.AuthCommand.ShowCommand();

            var output = await sut.AuthShow(command);

            output.Should().Be($"Токен авторизации: {accessToken}");
        }

        [Fact]
        public void Settings_reset_command_shows_expected_output()
        {
            const string accessToken = "NewAccessToken";
            var parserResultStub = new Mock<ICustomParserResult>();
            var settingsHelperStub = new Mock<ISettingsHelper>();
            settingsHelperStub
                .Setup(setup => setup.Read())
                .Returns(Task.FromResult(new Settings()
                {
                    AccessToken = accessToken
                }));
            var sut = new SettingsCommandController(parserResultStub.Object, settingsHelperStub.Object);
            SettingsCommand.ResetCommand command = new SettingsCommand.ResetCommand();

            var output = sut.Reset(command);

            output.Should().Be("Настройки сброшены.");
        }

        [Fact]
        public async Task Settings_every_set_command_shows_expected_output()
        {
            var parserResultStub = new Mock<ICustomParserResult>();
            var settingsHelperStub = new Mock<ISettingsHelper>();
            settingsHelperStub
                .Setup(setup => setup.Read())
                .Returns(Task.FromResult(new Settings()
                {
                    Every = new Every
                    {
                        StatusesTexts = new List<string> { "Status1" },
                        Seconds = 30,
                    }
                }));
            var sut = new SettingsCommandController(parserResultStub.Object, settingsHelperStub.Object);
            SettingsCommand.EveryCommand.SetCommand command = new SettingsCommand.EveryCommand.SetCommand()
            {
                StatusesTexts = new List<string> { "Status1" },
                Seconds = 30,
            };

            var output = await sut.EverySet(command);

            output.Should().Be("Настройки Every изменены.");
        }
    }
}
