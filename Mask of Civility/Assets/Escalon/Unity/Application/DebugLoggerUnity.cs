
using UnityEngine;

namespace Escalon.Unity
{
    public class DebugLoggerUnity : IDebugLogger
    {
        public void Assert(bool condition, string message)
        {
            UnityEngine.Debug.Assert(condition, message);
        }
        
        public void Log(LogType logType, string message, LogFilter filterType)
        {
            LogFilter filter = (LogFilter)PlayerPrefs.GetInt("DebugLogFilter");
            if (filterType == LogFilter.None || (filter & filterType) != 0)
            {
                UnityEngine.Debug.unityLogger.Log((UnityEngine.LogType)logType, message);
            }
        }
    }
}