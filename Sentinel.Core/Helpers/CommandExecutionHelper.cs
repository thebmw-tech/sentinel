using System;
using System.Diagnostics;
using System.Text;
using Sentinel.Core.Models;

namespace Sentinel.Core.Helpers
{
    public interface ICommandExecutionHelper
    {
        CommandExecutionResult Execute(string executable, string args = "");
    }
    public class CommandExecutionHelper : ICommandExecutionHelper
    {
        public CommandExecutionResult Execute(string executable, string args = "")
        {
            var outStringBuilder = new StringBuilder();
            var errorStringBuilder = new StringBuilder();

            var startInfo = new ProcessStartInfo()
            {
                FileName = executable,
                Arguments = args,
                RedirectStandardError = true,
                RedirectStandardOutput = true,
                UseShellExecute = false
            };

            var process = new Process
            {
                StartInfo = startInfo
            };

            process.ErrorDataReceived += (sender, eventArgs) => errorStringBuilder.Append(eventArgs.Data);
            process.OutputDataReceived += (sender, eventArgs) => outStringBuilder.Append(eventArgs.Data);

            process.Start();
            process.BeginErrorReadLine();
            process.BeginOutputReadLine();
            process.WaitForExit();

            return new CommandExecutionResult()
            {
                Output = outStringBuilder.ToString(),
                Error = errorStringBuilder.ToString(),
                ExitCode = process.ExitCode
            };
        }
    }
}