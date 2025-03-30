namespace VeldridGen.Example.Engine;

public class LogEvent(LogLevel severity, string message, string file = null, string member = null, int? line = null)
    : IEvent
{
    public LogLevel Severity { get; } = severity;
    public string Message { get; } = message;
    public string File { get; } = file;
    public string Member { get; } = member;
    public int? Line { get; } = line;
}
