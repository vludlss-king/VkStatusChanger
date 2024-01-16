using Microsoft.Extensions.Options;
using VkStatusChanger.Worker.Contracts.Helpers;
using VkStatusChanger.Worker.Contracts.Infrastructure;
using VkStatusChanger.Worker.Controllers;
using VkStatusChanger.Worker.Enums;
using VkStatusChanger.Worker.Helpers;
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
            SettingsCommand.AuthCommand.SetCommand command = new SettingsCommand.AuthCommand.SetCommand
            {
                AccessToken = "NewAccessToken"
            };

            await sut.AuthSet(command);

            var settings = await settingsHelper.Read();
            if (File.Exists(fileName))
                File.Delete(fileName);

            settings.AccessToken.Should().Be(command.AccessToken);
            settings.Every!.StatusesTexts!.Count.Should().Be(0);
            settings.Every.Seconds.Should().Be(0);
            settings.Schedule!.Items!.Count.Should().Be(0);
        }

        [Fact]
        public async Task Settings_type_set_command_works_properly()
        {
            const string fileName = "settings_type_set.json";
            var (settingsHelper, sut) = Startup(fileName);
            SettingsCommand.TypeCommand.SetCommand command = new SettingsCommand.TypeCommand.SetCommand
            {
                SettingsType = SettingsType.Schedule
            };

            await sut.TypeSet(command);

            var settings = await settingsHelper.Read();
            if (File.Exists(fileName))
                File.Delete(fileName);

            settings.SettingsType.Should().Be(command.SettingsType);
            settings.Every!.StatusesTexts!.Count.Should().Be(0);
            settings.Every.Seconds.Should().Be(0);
            settings.Schedule!.Items!.Count.Should().Be(0);
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

            var settings = await settingsHelper.Read();
            if (File.Exists(fileName))
                File.Delete(fileName);

            settings.Every!.StatusesTexts.Should().BeEquivalentTo(command.StatusesTexts);
            settings.Every.Seconds.Should().Be(command.Seconds);
            settings.Schedule!.Items.Should().BeEmpty();
        }

        [Fact]
        public async Task Settings_schedule_add_command_works_properly()
        {
            const string fileName = "settings_schedule_add.json";
            var (settingsHelper, sut) = Startup(fileName);
            SettingsCommand.ScheduleCommand.AddCommand command = new SettingsCommand.ScheduleCommand.AddCommand
            {
                StatusText = "Added",
                Date = new DateTime(2024, 1, 13),
                Time = new TimeSpan(6, 5, 30),
            };

            await sut.ScheduleAdd(command);

            var settings = await settingsHelper.Read();
            if (File.Exists(fileName))
                File.Delete(fileName);

            settings.Schedule!.Items!.Count.Should().Be(1);
            settings.Schedule.Items.First().StatusText.Should().Be(command.StatusText);
            settings.Schedule.Items.First().Date.Should().Be(command.Date);
            settings.Schedule.Items.First().Time.Should().Be(command.Time);
            settings.Every!.StatusesTexts!.Count.Should().Be(0);
            settings.Every.Seconds.Should().Be(0);
        }

        [Fact]
        public async Task Settings_schedule_edit_command_works_properly()
        {
            // Arrange
            const string fileName = "settings_schedule_edit.json";
            var (settingsHelper, sut) = Startup(fileName);
            SettingsCommand.ScheduleCommand.AddCommand addCommand = new SettingsCommand.ScheduleCommand.AddCommand
            {
                StatusText = "Added",
                Date = new DateTime(2024, 1, 13),
                Time = new TimeSpan(6, 5, 30),
            };
            await sut.ScheduleAdd(addCommand);

            SettingsCommand.ScheduleCommand.EditCommand editCommand = new SettingsCommand.ScheduleCommand.EditCommand
            {
                Id = 1,
                StatusText = "Edited",
                Date = new DateTime(2024, 2, 13),
                Time = new TimeSpan(7, 5, 30),
            };

            // Act
            await sut.ScheduleEdit(editCommand);

            var settings = await settingsHelper.Read();
            if (File.Exists(fileName))
                File.Delete(fileName);

            // Assert
            settings.Schedule!.Items!.Count.Should().Be(1);
            settings.Schedule.Items.First().StatusText.Should().Be(editCommand.StatusText);
            settings.Schedule.Items.First().Date.Should().Be(editCommand.Date);
            settings.Schedule.Items.First().Time.Should().Be(editCommand.Time);
            settings.Every!.StatusesTexts!.Count.Should().Be(0);
            settings.Every.Seconds.Should().Be(0);
        }

        [Fact]
        public async Task Settings_schedule_remove_command_works_properly()
        {
            // Arrange
            const string fileName = "settings_schedule_remove.json";
            var (settingsHelper, sut) = Startup(fileName);
            SettingsCommand.ScheduleCommand.AddCommand addCommand = new SettingsCommand.ScheduleCommand.AddCommand
            {
                StatusText = "Added",
                Date = new DateTime(2024, 1, 13),
                Time = new TimeSpan(6, 5, 30),
            };
            await sut.ScheduleAdd(addCommand);

            SettingsCommand.ScheduleCommand.RemoveCommand removeCommand = new SettingsCommand.ScheduleCommand.RemoveCommand
            {
                Id = 1
            };

            // Act
            await sut.ScheduleRemove(removeCommand);

            var settings = await settingsHelper.Read();
            if (File.Exists(fileName))
                File.Delete(fileName);

            // Assert
            settings.Schedule!.Items!.Count.Should().Be(0);
            settings.Every!.StatusesTexts!.Count.Should().Be(0);
            settings.Every.Seconds.Should().Be(0);
        }

        private (ISettingsHelper settingsHelper, SettingsCommandController sut) Startup(string fileName)
        {
            var settingsFileStub = new Mock<IOptions<SettingsFile>>();
            settingsFileStub
                .Setup(setup => setup.Value)
                .Returns(new SettingsFile { Name = fileName });
            var settingsHelper = new SettingsHelper(settingsFileStub.Object);

            var parserResultStub = new Mock<ICustomParserResult>();
            var sut = new SettingsCommandController(parserResultStub.Object, settingsHelper);

            return (settingsHelper, sut);
        }
    }
}
