namespace Sentinel.Core.Models
{
    public class CommandExecutionResult
    {
        public string Output { get; set; }
        public string Error { get; set; }
        public int ExitCode { get; set; }
    }
}