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
            var settingsManagerStub = new Mock<ISettingsManager>();
            settingsManagerStub
                .Setup(setup => setup.Read())
                .Returns(Task.FromResult(new UserSettingsModel()
                {
                    Type = SettingsType.Schedule
                }));
            var sut = new CommandController(parserResultStub.Object, settingsManagerStub.Object);
            Command.Settings.Type.Set command = new Command.Settings.Type.Set
            {
                SettingsType = SettingsType.Schedule
            };

            var output = await sut.Execute(command);

            output.Should().Be("Тип настроек изменён.");
        }

        [Fact]
        public async Task Settings_type_show_command_shows_expected_output()
        {
            var parserResultStub = new Mock<ICustomParserResult>();
            var settingsManagerStub = new Mock<ISettingsManager>();
            settingsManagerStub
                .Setup(setup => setup.Read())
                .Returns(Task.FromResult(new UserSettingsModel()
                {
                    Type = SettingsType.Schedule
                }));
            var sut = new CommandController(parserResultStub.Object, settingsManagerStub.Object);
            Command.Settings.Type.Show command = new Command.Settings.Type.Show();

            var output = await sut.Execute(command);

            output.Should().Be($"Тип настроек: {SettingsType.Schedule}");
        }

        [Fact]
        public async Task Settings_auth_set_command_shows_expected_output()
        {
            const string accessToken = "NewAccessToken";
            var parserResultStub = new Mock<ICustomParserResult>();
            var settingsManagerStub = new Mock<ISettingsManager>();
            settingsManagerStub
                .Setup(setup => setup.Read())
                .Returns(Task.FromResult(new UserSettingsModel()
                {
                    AccessToken = accessToken
                }));
            var sut = new CommandController(parserResultStub.Object, settingsManagerStub.Object);
            Command.Settings.Auth.Set command = new Command.Settings.Auth.Set();

            var output = await sut.Execute(command);

            output.Should().Be("Токен авторизации изменён.");
        }

        [Fact]
        public async Task Settings_auth_show_command_shows_expected_output()
        {
            const string accessToken = "NewAccessToken";
            var parserResultStub = new Mock<ICustomParserResult>();
            var settingsManagerStub = new Mock<ISettingsManager>();
            settingsManagerStub
                .Setup(setup => setup.Read())
                .Returns(Task.FromResult(new UserSettingsModel()
                {
                    AccessToken = accessToken
                }));
            var sut = new CommandController(parserResultStub.Object, settingsManagerStub.Object);
            Command.Settings.Auth.Show command = new Command.Settings.Auth.Show();

            var output = await sut.Execute(command);

            output.Should().Be($"Токен авторизации: {accessToken}");
        }

        [Fact]
        public void Settings_reset_command_shows_expected_output()
        {
            const string accessToken = "NewAccessToken";
            var parserResultStub = new Mock<ICustomParserResult>();
            var settingsManagerStub = new Mock<ISettingsManager>();
            settingsManagerStub
                .Setup(setup => setup.Read())
                .Returns(Task.FromResult(new UserSettingsModel()
                {
                    AccessToken = accessToken
                }));
            var sut = new CommandController(parserResultStub.Object, settingsManagerStub.Object);
            Command.Settings.Reset command = new Command.Settings.Reset();

            var output = sut.Execute(command);

            output.Should().Be("Настройки сброшены.");
        }

        [Fact]
        public async Task Settings_every_set_command_shows_expected_output()
        {
            var parserResultStub = new Mock<ICustomParserResult>();
            var settingsManagerStub = new Mock<ISettingsManager>();
            settingsManagerStub
                .Setup(setup => setup.Read())
                .Returns(Task.FromResult(new UserSettingsModel()
                {
                    EverySecondsSchedule = new EverySecondsSchedule
                    {
                        Statuses = new List<string> { "Status1" },
                        Seconds = 30,
                    }
                }));
            var sut = new CommandController(parserResultStub.Object, settingsManagerStub.Object);
            Command.Settings.Every.Set command = new Command.Settings.Every.Set()
            {
                StatusesTexts = new List<string> { "Status1" },
                Seconds = 30,
            };

            var output = await sut.Execute(command);

            output.Should().Be("Настройки Every изменены.");
        }
    }
}
