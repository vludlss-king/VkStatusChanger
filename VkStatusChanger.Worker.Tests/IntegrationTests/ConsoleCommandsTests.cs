using System.Diagnostics;
using VkStatusChanger.Worker.Tests.Attributes;

namespace VkStatusChanger.Worker.Tests.IntegrationTests
{
    [Collection(nameof(ConsoleCommandsTests))]
    public class ConsoleCommandsTests
    {
        const string settingsFile = "settings.json";
        const string settingsBufferFile = $"settings_buffer.json";

        [Fact]
        public async Task Settings_type_set_command_shows_expected_output_to_console()
        {
            // Arrange
            const string command = "settings type set --settings-type Every";

            var sut = StartProcess(command);
            await sut.WaitForExitAsync();
            sut.Kill();

            RestoreSettings();

            // Act
            var output = await sut!.StandardOutput.ReadLineAsync();

            // Assert
            output.Should().Be("Тип настроек изменён.");
        }

        [Fact]
        public async Task Settings_type_show_command_shows_expected_output_to_console()
        {
            // Arrange
            const string typeSetCommand = "settings type set --settings-type Schedule";

            var sut = StartProcess(typeSetCommand);
            await sut.WaitForExitAsync();
            sut.Kill();

            const string typeShowCommand = "settings type show";
            sut = StartProcess(typeShowCommand);
            await sut.WaitForExitAsync();
            sut.Kill();

            RestoreSettings();

            // Act
            var output = await sut!.StandardOutput.ReadLineAsync();

            // Assert
            output.Should().Be("Тип настроек: Schedule");
        }

        private Process? StartProcess(string command)
        {
            if(!File.Exists(settingsBufferFile))
                File.Copy(settingsFile, settingsBufferFile);

            var processStartInfo = new ProcessStartInfo();
            processStartInfo.WorkingDirectory = Directory.GetCurrentDirectory();
            processStartInfo.FileName = "VkStatusChanger.Worker.exe";
            processStartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            processStartInfo.CreateNoWindow = true;
            processStartInfo.UseShellExecute = false;
            processStartInfo.Arguments = command;
            processStartInfo.RedirectStandardInput = true;
            processStartInfo.RedirectStandardOutput = true;

            var process = Process.Start(processStartInfo);

            return process;
        }

        private void RestoreSettings()
        {
            File.Delete(settingsFile);
            File.Copy(settingsBufferFile, settingsFile);
            File.Delete(settingsBufferFile);
        }
    }

    [CollectionDefinition(nameof(ConsoleCommandsTests), DisableParallelization = true)]
    public class ConsoleCommandsCollection
    {

    }
}
