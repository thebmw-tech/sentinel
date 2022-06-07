namespace Sentinel.Core.Command.Output;

public class CommandOutput<TOutput, TError>
{
    public TOutput Output { get; set; }
    public TError Error { get; set; }
}