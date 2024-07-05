using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace VkStatusChanger.Worker.Tests.IntegrationTests
{
    [Collection(nameof(CommandsOutputTests))]
    public class CommandsOutputTests
    {
        const string settingsFile = "settings.json";
        const string settingsBufferFile = $"settings_buffer.json";

        [Fact]
        public async Task Settings_type_set_command_shows_expected_output_to_console()
        {
            const string command = "settings type set --settings-type Every";
            var sut = await ExecuteCommand(command).ConfigureAwait(false);
            RestoreSettings();

            var output = await sut.StandardOutput.ReadLineAsync().ConfigureAwait(false);

            output.Should().Be("Тип настроек изменён.");
        }

        [Fact]
        public async Task Settings_type_show_command_shows_expected_output_to_console()
        {
            // Arrange
            const string typeSetCommand = "settings type set --settings-type Schedule";
            await ExecuteCommand(typeSetCommand).ConfigureAwait(false);

            const string typeShowCommand = "settings type show";
            var sut = await ExecuteCommand(typeShowCommand).ConfigureAwait(false);
            RestoreSettings();

            // Act
            var output = await sut.StandardOutput.ReadLineAsync().ConfigureAwait(false);

            // Assert
            output.Should().Be("Тип настроек: Schedule");
        }

        [Fact]
        public async Task Settings_auth_set_command_shows_expected_output_to_console()
        {
            const string command = "settings auth set --access-token NewAccessToken";
            var sut = await ExecuteCommand(command).ConfigureAwait(false);
            RestoreSettings();

            var output = await sut.StandardOutput.ReadLineAsync().ConfigureAwait(false);

            // Assert
            output.Should().Be("Токен авторизации изменён.");
        }

        [Fact]
        public async Task Settings_auth_show_command_shows_expected_output_to_console()
        {
            // Arrange
            const string authSetCommand = "settings auth set --access-token NewAccessToken";
            await ExecuteCommand(authSetCommand).ConfigureAwait(false);

            const string authShowCommand = "settings auth show";
            var sut = await ExecuteCommand(authShowCommand).ConfigureAwait(false);
            RestoreSettings();
            
            // Act
            var output = await sut.StandardOutput.ReadLineAsync().ConfigureAwait(false);

            // Assert
            output.Should().Be("Токен авторизации: NewAccessToken");
        }

        [return: NotNull]
        private async Task<Process> ExecuteCommand(string command)
        {
            if (!File.Exists(settingsBufferFile) && File.Exists(settingsFile))
                File.Copy(settingsFile, settingsBufferFile);

            var processStartInfo = new ProcessStartInfo();
            processStartInfo.WorkingDirectory = Directory.GetCurrentDirectory();
            processStartInfo.FileName = $"{Assembly.GetAssembly(typeof(Program))!.GetName().Name}.exe";
            processStartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            processStartInfo.CreateNoWindow = true;
            processStartInfo.UseShellExecute = false;
            processStartInfo.Arguments = command;
            processStartInfo.RedirectStandardInput = true;
            processStartInfo.RedirectStandardOutput = true;

            var process = Process.Start(processStartInfo);
            if (process is null)
                throw new Exception("Не удалось создать процесс");

            await process.WaitForExitAsync().ConfigureAwait(false);
            process.Kill();

            return process;
        }

        private void RestoreSettings()
        {
            if(File.Exists(settingsFile))
                File.Delete(settingsFile);

            if (File.Exists(settingsBufferFile))
            {
                File.Copy(settingsBufferFile, settingsFile);
                File.Delete(settingsBufferFile);
            }
        }
    }

    [CollectionDefinition(nameof(CommandsOutputTests), DisableParallelization = true)]
    public class ConsoleCommandsCollection
    {

    }
}
