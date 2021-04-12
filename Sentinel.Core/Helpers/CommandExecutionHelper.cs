using System;
using System.Diagnostics;
using System.Text;
using Sentinel.Core.Models;

namespace Sentinel.Core.Helpers
{
    public interface ICommandExecutionHelper
    {
        public Process BuildProcess(string executable, string args = "", bool rdrOutput = false, bool rdrError = false,
            bool rdrInput = false);
        CommandExecutionResult Execute(string executable, string args = "");
    }
    public class CommandExecutionHelper : ICommandExecutionHelper
    {
        public Process BuildProcess(string executable, string args = "", bool rdrOutput = false, bool rdrError = false, bool rdrInput = false)
        {
            var startInfo = new ProcessStartInfo()
            {
                FileName = executable,
                Arguments = args,
                RedirectStandardError = rdrError,
                RedirectStandardOutput = rdrOutput,
                RedirectStandardInput = rdrInput,
                UseShellExecute = false,
            };

            var process = new Process
            {
                StartInfo = startInfo
            };

            return process;
        }

        public CommandExecutionResult Execute(string executable, string args = "")
        {
            var outStringBuilder = new StringBuilder();
            var errorStringBuilder = new StringBuilder();

            var process = BuildProcess(executable, args, true, true);

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