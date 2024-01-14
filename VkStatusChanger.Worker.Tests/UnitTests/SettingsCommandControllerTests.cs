﻿using Newtonsoft.Json.Linq;
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

        [Fact]
        public async Task Settings_auth_set_command_shows_expected_output()
        {
            const string accessToken = "NewAccessToken";
            var parserResultStub = new Mock<ICustomParserResult>();
            var settingsHelperStub = new Mock<ISettingsHelper>();
            settingsHelperStub
                .Setup(setup => setup.ReadSettings())
                .Returns(Task.FromResult(new SettingsModel()
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
                .Setup(setup => setup.ReadSettings())
                .Returns(Task.FromResult(new SettingsModel()
                {
                    AccessToken = accessToken
                }));
            var sut = new SettingsCommandController(parserResultStub.Object, settingsHelperStub.Object);
            SettingsCommand.AuthCommand.ShowCommand command = new SettingsCommand.AuthCommand.ShowCommand();

            var output = await sut.AuthShow(command);

            output.Should().Be($"Токен авторизации: {accessToken}");
        }

        [Fact]
        public async Task Settings_reset_command_shows_expected_output()
        {
            const string accessToken = "NewAccessToken";
            var parserResultStub = new Mock<ICustomParserResult>();
            var settingsHelperStub = new Mock<ISettingsHelper>();
            settingsHelperStub
                .Setup(setup => setup.ReadSettings())
                .Returns(Task.FromResult(new SettingsModel()
                {
                    AccessToken = accessToken
                }));
            var sut = new SettingsCommandController(parserResultStub.Object, settingsHelperStub.Object);
            SettingsCommand.ResetCommand command = new SettingsCommand.ResetCommand();

            var output = sut.Reset(command);

            output.Should().Be("Настройки сброшены.");
        }
    }
}