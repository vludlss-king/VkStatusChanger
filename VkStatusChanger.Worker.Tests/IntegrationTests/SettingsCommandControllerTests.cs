using Microsoft.Extensions.Options;
using VkStatusChanger.Worker.Contracts.Infrastructure;
using VkStatusChanger.Worker.Controllers;
using VkStatusChanger.Worker.Enums;
using VkStatusChanger.Worker.Infrastructure;
using VkStatusChanger.Worker.Models;
using VkStatusChanger.Worker.Models.Commands;

namespace VkStatusChanger.Worker.Tests.IntegrationTests
{
    public class SettingsCommandControllerTests
    {
        public SettingsCommandControllerTests()
        {
            
        }

        [Fact]
        public async Task Settings_auth_set_command_works_properly()
        {
            const string fileName = "settings_auth_set.json";
            var (settingsHelper, sut) = Startup(fileName);
            Command.Settings.Auth.Set command = new Command.Settings.Auth.Set
            {
                AccessToken = "NewAccessToken"
            };

            await sut.Execute(command);

            var settings = await settingsHelper.Read();
            if (File.Exists(fileName))
                File.Delete(fileName);

            settings.AccessToken.Should().Be(command.AccessToken);
            settings.EverySecondsSchedule.Statuses.Count.Should().Be(0);
            settings.EverySecondsSchedule.Seconds.Should().Be(0);
            settings.DateTimeSchedule.Items.Count.Should().Be(0);
        }

        [Fact]
        public async Task Settings_type_set_command_works_properly()
        {
            const string fileName = "settings_type_set.json";
            var (settingsHelper, sut) = Startup(fileName);
            Command.Settings.Type.Set command = new Command.Settings.Type.Set
            {
                SettingsType = SettingsType.Schedule
            };

            await sut.Execute(command);

            var settings = await settingsHelper.Read();
            if (File.Exists(fileName))
                File.Delete(fileName);

            settings.Type.Should().Be(command.SettingsType);
            settings.EverySecondsSchedule.Statuses.Count.Should().Be(0);
            settings.EverySecondsSchedule.Seconds.Should().Be(0);
            settings.DateTimeSchedule.Items.Count.Should().Be(0);
        }

        [Fact]
        public async Task Settings_every_set_command_works_properly()
        {
            const string fileName = "settings_every_set.json";
            var (settingsHelper, sut) = Startup(fileName);
            Command.Settings.Every.Set command = new Command.Settings.Every.Set
            {
                Statuses = new List<string> { "Status1", "Status2", "Status3" },
                Seconds = 60,
            };

            await sut.Execute(command);

            var settings = await settingsHelper.Read();
            if (File.Exists(fileName))
                File.Delete(fileName);

            settings.EverySecondsSchedule.Statuses.Should().BeEquivalentTo(command.Statuses);
            settings.EverySecondsSchedule.Seconds.Should().Be(command.Seconds);
            settings.DateTimeSchedule.Items.Should().BeEmpty();
        }

        [Fact]
        public async Task Settings_schedule_add_command_works_properly()
        {
            const string fileName = "settings_schedule_add.json";
            var (settingsHelper, sut) = Startup(fileName);
            Command.Settings.Schedule.Add command = new Command.Settings.Schedule.Add
            {
                Status = "Added",
                Date = new DateTime(2024, 1, 13),
                Time = new TimeSpan(6, 5, 30),
            };

            await sut.Execute(command);

            var settings = await settingsHelper.Read();
            if (File.Exists(fileName))
                File.Delete(fileName);

            settings.DateTimeSchedule.Items.Count.Should().Be(1);
            settings.DateTimeSchedule.Items.First().Status.Should().Be(command.Status);
            settings.DateTimeSchedule.Items.First().Date.Should().Be(command.Date);
            settings.DateTimeSchedule.Items.First().Time.Should().Be(command.Time);
            settings.EverySecondsSchedule.Statuses.Count.Should().Be(0);
            settings.EverySecondsSchedule.Seconds.Should().Be(0);
        }

        [Fact]
        public async Task Settings_schedule_edit_command_works_properly()
        {
            // Arrange
            const string fileName = "settings_schedule_edit.json";
            var (settingsHelper, sut) = Startup(fileName);
            Command.Settings.Schedule.Add addCommand = new Command.Settings.Schedule.Add
            {
                Status = "Added",
                Date = new DateTime(2024, 1, 13),
                Time = new TimeSpan(6, 5, 30),
            };
            await sut.Execute(addCommand);

            Command.Settings.Schedule.Edit editCommand = new Command.Settings.Schedule.Edit
            {
                Id = 1,
                Status = "Edited",
                Date = new DateTime(2024, 2, 13),
                Time = new TimeSpan(7, 5, 30),
            };

            // Act
            await sut.Execute(editCommand);

            var settings = await settingsHelper.Read();
            if (File.Exists(fileName))
                File.Delete(fileName);

            // Assert
            settings.DateTimeSchedule.Items.Count.Should().Be(1);
            settings.DateTimeSchedule.Items.First().Status.Should().Be(editCommand.Status);
            settings.DateTimeSchedule.Items.First().Date.Should().Be(editCommand.Date);
            settings.DateTimeSchedule.Items.First().Time.Should().Be(editCommand.Time);
            settings.EverySecondsSchedule.Statuses.Count.Should().Be(0);
            settings.EverySecondsSchedule.Seconds.Should().Be(0);
        }

        [Fact]
        public async Task Settings_schedule_remove_command_works_properly()
        {
            // Arrange
            const string fileName = "settings_schedule_remove.json";
            var (settingsHelper, sut) = Startup(fileName);
            Command.Settings.Schedule.Add addCommand = new Command.Settings.Schedule.Add
            {
                Status = "Added",
                Date = new DateTime(2024, 1, 13),
                Time = new TimeSpan(6, 5, 30),
            };
            await sut.Execute(addCommand);

            Command.Settings.Schedule.Remove removeCommand = new Command.Settings.Schedule.Remove
            {
                Id = 1
            };

            // Act
            await sut.Execute(removeCommand);

            var settings = await settingsHelper.Read();
            if (File.Exists(fileName))
                File.Delete(fileName);

            // Assert
            settings.DateTimeSchedule.Items.Count.Should().Be(0);
            settings.EverySecondsSchedule.Statuses.Count.Should().Be(0);
            settings.EverySecondsSchedule.Seconds.Should().Be(0);
        }

        private (ISettingsManager settingsManager, CommandController sut) Startup(string fileName)
        {
            var settingsFileStub = new Mock<IOptions<SettingsFile>>();
            settingsFileStub
                .Setup(setup => setup.Value)
                .Returns(new SettingsFile { Name = fileName });
            var settingsHelper = new SettingsManager(settingsFileStub.Object);

            var parserResultStub = new Mock<ICustomParserResult>();
            var sut = new CommandController(parserResultStub.Object, settingsHelper);

            return (settingsHelper, sut);
        }
    }
}
