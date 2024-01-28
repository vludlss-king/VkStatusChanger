using VkStatusChanger.Worker.Commands;
using VkStatusChanger.Worker.Commands.Impl;
using VkStatusChanger.Worker.Contracts.Infrastructure;
using VkStatusChanger.Worker.Enums;
using VkStatusChanger.Worker.Models.UserSettings;

namespace VkStatusChanger.Worker.Tests.UnitTests
{
    public class CommandsOutputTests
    {
        [Fact]
        public async Task Settings_type_set_command_shows_expected_output()
        {
            var settingsManagerStub = new Mock<ISettingsManager>();
            settingsManagerStub
                .Setup(setup => setup.Read())
                .Returns(Task.FromResult(new UserSettingsModel()
                {
                    Type = SettingsType.Schedule
                }));
            var sut = new Settings_Type_Set_Command(settingsManagerStub.Object);
            Routes.Settings.Type.Set request = new Routes.Settings.Type.Set
            {
                SettingsType = SettingsType.Schedule
            };

            var output = await sut.Execute(request);

            output.Should().Be("Тип настроек изменён.");
        }

        [Fact]
        public async Task Settings_type_show_command_shows_expected_output()
        {
            var settingsManagerStub = new Mock<ISettingsManager>();
            settingsManagerStub
                .Setup(setup => setup.Read())
                .Returns(Task.FromResult(new UserSettingsModel()
                {
                    Type = SettingsType.Schedule
                }));
            var sut = new Settings_Type_Show_Command(settingsManagerStub.Object);
            Routes.Settings.Type.Show command = new Routes.Settings.Type.Show();

            var output = await sut.Execute(command);

            output.Should().Be($"Тип настроек: {SettingsType.Schedule}");
        }

        [Fact]
        public async Task Settings_auth_set_command_shows_expected_output()
        {
            const string accessToken = "NewAccessToken";
            var settingsManagerStub = new Mock<ISettingsManager>();
            settingsManagerStub
                .Setup(setup => setup.Read())
                .Returns(Task.FromResult(new UserSettingsModel()
                {
                    AccessToken = accessToken
                }));
            var sut = new Settings_Auth_Set_Command(settingsManagerStub.Object);
            Routes.Settings.Auth.Set command = new Routes.Settings.Auth.Set();

            var output = await sut.Execute(command);

            output.Should().Be("Токен авторизации изменён.");
        }

        [Fact]
        public async Task Settings_auth_show_command_shows_expected_output()
        {
            const string accessToken = "NewAccessToken";
            var settingsManagerStub = new Mock<ISettingsManager>();
            settingsManagerStub
                .Setup(setup => setup.Read())
                .Returns(Task.FromResult(new UserSettingsModel()
                {
                    AccessToken = accessToken
                }));
            var sut = new Settings_Auth_Show_Command(settingsManagerStub.Object);
            Routes.Settings.Auth.Show command = new Routes.Settings.Auth.Show();

            var output = await sut.Execute(command);

            output.Should().Be($"Токен авторизации: {accessToken}");
        }

        [Fact]
        public async Task Settings_reset_command_shows_expected_output()
        {
            const string accessToken = "NewAccessToken";
            var settingsManagerStub = new Mock<ISettingsManager>();
            settingsManagerStub
                .Setup(setup => setup.Read())
                .Returns(Task.FromResult(new UserSettingsModel()
                {
                    AccessToken = accessToken
                }));
            var sut = new Settings_Reset_Command(settingsManagerStub.Object);
            Routes.Settings.Reset command = new Routes.Settings.Reset();

            var output = await sut.Execute(command);

            output.Should().Be("Настройки сброшены.");
        }

        [Fact]
        public async Task Settings_every_set_command_shows_expected_output()
        {
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
            var sut = new Settings_Every_Set_Command(settingsManagerStub.Object);
            Routes.Settings.Every.Set command = new Routes.Settings.Every.Set()
            {
                Statuses = new List<string> { "Status1" },
                Seconds = 30,
            };

            var output = await sut.Execute(command);

            output.Should().Be("Настройки Every изменены.");
        }
    }
}
