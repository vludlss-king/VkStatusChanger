using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

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
            const string command = "settings type set --settings-type Every";
            var sut = await StartProcess(command);
            RestoreSettings();

            var output = await sut.StandardOutput.ReadLineAsync();

            output.Should().Be("Тип настроек изменён.");
        }

        [Fact]
        public async Task Settings_type_show_command_shows_expected_output_to_console()
        {
            // Arrange
            const string typeSetCommand = "settings type set --settings-type Schedule";
            await StartProcess(typeSetCommand);

            const string typeShowCommand = "settings type show";
            var sut = await StartProcess(typeShowCommand);
            RestoreSettings();

            // Act
            var output = await sut.StandardOutput.ReadLineAsync();

            // Assert
            output.Should().Be("Тип настроек: Schedule");
        }

        [Fact]
        public async Task Settings_auth_set_command_shows_expected_output_to_console()
        {
            const string command = "settings auth set --access-token NewAccessToken";
            var sut = await StartProcess(command);
            RestoreSettings();

            var output = await sut.StandardOutput.ReadLineAsync();

            // Assert
            output.Should().Be("Токен авторизации изменён.");
        }

        [Fact]
        public async Task Settings_auth_show_command_shows_expected_output_to_console()
        {
            // Arrange
            const string authSetCommand = "settings auth set --access-token NewAccessToken";
            await StartProcess(authSetCommand);

            const string authShowCommand = "settings auth show";
            var sut = await StartProcess(authShowCommand);
            RestoreSettings();
            
            // Act
            var output = await sut.StandardOutput.ReadLineAsync();

            // Assert
            output.Should().Be("Токен авторизации: NewAccessToken");
        }

        [return: NotNull]
        private async Task<Process> StartProcess(string command)
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
            if (process is null)
                throw new Exception("Не удалось создать процесс");
            await process.WaitForExitAsync();
            process.Kill();

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
