using System.Diagnostics;
using VkStatusChanger.Worker.Models.Commands.Common;

namespace VkStatusChanger.Worker.Tests.IntegrationTests
{
    public class ConsoleCommandsTests
    {
        [Fact]
        public async Task Settings_type_set_command_shows_expected_output_to_console()
        {
            const string command = "settings type set --settings-type Every";
            var sut = StartProcess(command);

            var output = await sut.StandardOutput.ReadLineAsync();

            output.Should().Be("Тип настроек изменён.");
        }

        private Process StartProcess(string command)
        {
            File.Copy("settings.json", "settings_buffer.json");

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

            File.Delete("settings.json");
            File.Copy("settings_buffer.json", "settings.json");
            File.Delete("settings_buffer.json");

            return process;
        }
    }
}
