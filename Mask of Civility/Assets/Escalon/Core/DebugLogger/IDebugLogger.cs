namespace Escalon
{
    /// <summary>
    /// Used to create an interface between Escalon and an engine or OS's logging
    /// </summary>
    public interface IDebugLogger
    {
        public void Assert(bool condition, string message);
        public void Log(LogType logType, string message, LogFilter filterType);
    }
}
