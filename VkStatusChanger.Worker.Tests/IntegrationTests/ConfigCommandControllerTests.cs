﻿using Microsoft.Extensions.Options;
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
        public async Task Settings_every_set_command_works_properly()
        {
            const string fileName = "settings_every_set.json";
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

        [Fact]
        public async Task Settings_schedule_add_command_works_property()
        {
            const string fileName = "settings_schedule_add.json";
            var (settingsHelper, sut) = Startup(fileName);
            SettingsCommand.ScheduleCommand.AddCommand command = new SettingsCommand.ScheduleCommand.AddCommand
            {
                StatusText = "Status1",
                Date = new DateTime(2024, 1, 13),
                Time = new TimeSpan(6, 5, 30),
            };

            await sut.ScheduleAdd(command);

            var settings = await settingsHelper.ReadSettings();
            if (File.Exists(fileName))
                File.Delete(fileName);

            settings.Schedule.Items.Count.Should().Be(1);
            settings.Schedule.Items.First().StatusText.Should().Be(command.StatusText);
            settings.Schedule.Items.First().Date.Should().Be(command.Date);
            settings.Schedule.Items.First().Time.Should().Be(command.Time);
            settings.Every.StatusesTexts.Count.Should().Be(0);
            settings.Every.Seconds.Should().Be(0);
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
