using Microsoft.Extensions.Options;
using VkStatusChanger.Worker.Commands;
using VkStatusChanger.Worker.Commands.Impl;
using VkStatusChanger.Worker.Contracts;
using VkStatusChanger.Worker.Contracts.Infrastructure;
using VkStatusChanger.Worker.Enums;
using VkStatusChanger.Worker.Infrastructure;
using VkStatusChanger.Worker.Models;

namespace VkStatusChanger.Worker.Tests.IntegrationTests
{
    public class CommandsSettingsTests
    {
        public CommandsSettingsTests()
        {
            
        }

        [Fact]
        public async Task Settings_auth_set_command_changes_settings_file()
        {
            // Arrange
            const string fileName = "settings_auth_set.json";
            if (File.Exists(fileName))
                File.Delete(fileName);

            var settingsManager = Startup(fileName);
            var sut = new Settings_Auth_Set_Command(settingsManager);
            Routes.Settings.Auth.Set command = new Routes.Settings.Auth.Set
            {
                AccessToken = "NewAccessToken"
            };

            // Act
            await sut.Execute(command);

            // Assert
            var settings = await settingsManager.Read();

            settings.AccessToken.Should().Be(command.AccessToken);
            settings.EverySecondsSchedule.Statuses.Count.Should().Be(0);
            settings.EverySecondsSchedule.Seconds.Should().Be(0);
            settings.DateTimeSchedule.Items.Count.Should().Be(0);
        }

        [Fact]
        public async Task Settings_type_set_command_changes_settings_file()
        {
            // Arrange
            const string fileName = "settings_type_set.json";
            if (File.Exists(fileName))
                File.Delete(fileName);

            var settingsManager = Startup(fileName);
            var sut = new Settings_Type_Set_Command(settingsManager);
            Routes.Settings.Type.Set command = new Routes.Settings.Type.Set
            {
                SettingsType = SettingsType.Schedule
            };

            // Act
            await sut.Execute(command);

            // Assert
            var settings = await settingsManager.Read();

            settings.Type.Should().Be(command.SettingsType);
            settings.EverySecondsSchedule.Statuses.Count.Should().Be(0);
            settings.EverySecondsSchedule.Seconds.Should().Be(0);
            settings.DateTimeSchedule.Items.Count.Should().Be(0);
        }

        [Fact]
        public async Task Settings_every_set_command_changes_settings_file()
        {
            // Arrange
            const string fileName = "settings_every_set.json";
            if (File.Exists(fileName))
                File.Delete(fileName);

            var settingsManager = Startup(fileName);
            var sut = new Settings_Every_Set_Command(settingsManager);
            Routes.Settings.Every.Set command = new Routes.Settings.Every.Set
            {
                Statuses = new List<string> { "Status1", "Status2", "Status3" },
                Seconds = 60,
            };

            // Act
            await sut.Execute(command);

            // Assert
            var settings = await settingsManager.Read();

            settings.EverySecondsSchedule.Statuses.Should().BeEquivalentTo(command.Statuses);
            settings.EverySecondsSchedule.Seconds.Should().Be(command.Seconds);
            settings.DateTimeSchedule.Items.Should().BeEmpty();
        }

        [Fact]
        public async Task Settings_schedule_add_command_changes_settings_file()
        {
            // Arrange
            const string fileName = "settings_schedule_add.json";
            if (File.Exists(fileName))
                File.Delete(fileName);

            var settingsManager = Startup(fileName);
            var sut = new Settings_Schedule_Add_Command(settingsManager);
            Routes.Settings.Schedule.Add command = new Routes.Settings.Schedule.Add
            {
                Status = "Added",
                Date = new DateTime(2024, 1, 13),
                Time = new TimeSpan(6, 5, 30),
            };

            // Act
            await sut.Execute(command);

            // Assert
            var settings = await settingsManager.Read();

            settings.DateTimeSchedule.Items.Count.Should().Be(1);
            settings.DateTimeSchedule.Items.First().Status.Should().Be(command.Status);
            settings.DateTimeSchedule.Items.First().Date.Should().Be(command.Date);
            settings.DateTimeSchedule.Items.First().Time.Should().Be(command.Time);
            settings.EverySecondsSchedule.Statuses.Count.Should().Be(0);
            settings.EverySecondsSchedule.Seconds.Should().Be(0);
        }

        [Fact]
        public async Task Settings_schedule_edit_command_changes_settings_file()
        {
            // Arrange
            const string fileName = "settings_schedule_edit.json";
            if (File.Exists(fileName))
                File.Delete(fileName);

            var settingsManager = Startup(fileName);

            var addCommand = new Settings_Schedule_Add_Command(settingsManager);
            Routes.Settings.Schedule.Add addRequest = new Routes.Settings.Schedule.Add
            {
                Status = "Added",
                Date = new DateTime(2024, 1, 13),
                Time = new TimeSpan(6, 5, 30),
            };
            await addCommand.Execute(addRequest);

            var sut = new Settings_Schedule_Edit_Command(settingsManager);
            Routes.Settings.Schedule.Edit sutRequest = new Routes.Settings.Schedule.Edit
            {
                Id = 1,
                Status = "Edited",
                Date = new DateTime(2024, 2, 13),
                Time = new TimeSpan(7, 5, 30),
            };

            // Act
            await sut.Execute(sutRequest);

            // Assert
            var settings = await settingsManager.Read();

            settings.DateTimeSchedule.Items.Count.Should().Be(1);
            settings.DateTimeSchedule.Items.First().Status.Should().Be(sutRequest.Status);
            settings.DateTimeSchedule.Items.First().Date.Should().Be(sutRequest.Date);
            settings.DateTimeSchedule.Items.First().Time.Should().Be(sutRequest.Time);
            settings.EverySecondsSchedule.Statuses.Count.Should().Be(0);
            settings.EverySecondsSchedule.Seconds.Should().Be(0);
        }

        [Fact]
        public async Task Settings_schedule_remove_command_changes_settings_file()
        {
            // Arrange
            const string fileName = "settings_schedule_remove.json";
            if (File.Exists(fileName))
                File.Delete(fileName);

            var settingsManager = Startup(fileName);

            var addCommand = new Settings_Schedule_Add_Command(settingsManager);
            Routes.Settings.Schedule.Add addRequest = new Routes.Settings.Schedule.Add
            {
                Status = "Added",
                Date = new DateTime(2024, 1, 13),
                Time = new TimeSpan(6, 5, 30),
            };
            await addCommand.Execute(addRequest);

            var sut = new Settings_Schedule_Remove_Command(settingsManager);
            Routes.Settings.Schedule.Remove sutRequest = new Routes.Settings.Schedule.Remove
            {
                Id = 1
            };

            // Act
            await sut.Execute(sutRequest);

            // Assert
            var settings = await settingsManager.Read();

            settings.DateTimeSchedule.Items.Count.Should().Be(0);
            settings.EverySecondsSchedule.Statuses.Count.Should().Be(0);
            settings.EverySecondsSchedule.Seconds.Should().Be(0);
        }

        private ISettingsManager Startup(string fileName)
        {
            var settingsFileStub = new Mock<IOptions<SettingsFile>>();
            settingsFileStub
                .Setup(setup => setup.Value)
                .Returns(new SettingsFile { Name = fileName });
            var settingsHelper = new SettingsManager(settingsFileStub.Object);

            return settingsHelper;
        }
    }
}
