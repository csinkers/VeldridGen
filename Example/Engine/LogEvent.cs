namespace VeldridGen.Example.Engine;

public class LogEvent : IEvent
{
    public LogLevel Severity { get; }
    public string Message { get; }
    public string File { get; }
    public string Member { get; }
    public int? Line { get; }

    public LogEvent(LogLevel severity, string message, string file = null, string member = null, int? line = null)
    {
        Severity = severity;
        Message = message;
        File = file;
        Member = member;
        Line = line;
    }
}