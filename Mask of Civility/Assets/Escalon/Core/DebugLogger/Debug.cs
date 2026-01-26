using Escalon.Utility;

namespace Escalon
{
    public class DebugSingleton : Singleton<DebugSingleton>, IDebugLogger
    {
        private IDebugLogger _debugLogger;
        
        public void Init(IDebugLogger debugLogger)
        {
            _debugLogger = debugLogger;
        }
        
        public void Assert(bool condition, string message)
        {
            _debugLogger.Assert(condition, message);
        }

        public void Log(LogType logType, string message, LogFilter filterType)
        {
            _debugLogger.Log(logType, message, filterType);
        }
    }
    
    public static class Debug
    {
        public static void Init(IDebugLogger debugLogger)
        {
            DebugSingleton.Instance.Init(debugLogger);
        }
        
        public static void Assert(bool condition, string message)
        {
            DebugSingleton.Instance.Assert(condition, message);
        }

        public static void Log(string message, LogFilter filterType = LogFilter.None)
        {
            DebugSingleton.Instance.Log(LogType.Log, message, filterType);
        }
        
        public static void LogError(string message)
        {
            DebugSingleton.Instance.Log(LogType.Error, message, LogFilter.None);
        }
        
        public static void LogWarning(string message)
        {
            DebugSingleton.Instance.Log(LogType.Warning, message, LogFilter.None);
        }
        
    }

}