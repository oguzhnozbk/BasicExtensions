using NLog;
using System;

namespace BasicExtensions
{
    public static class NLogExtensions
    {
        public static string LoggerName { get; set; } = string.Empty;
        private static NLog.Logger _logger;
        private static void AddLog(this string message, string loggerName = "",string fileNames = "", LogLevel level = null, Exception ex = null)
        {
            if(LogManager.Configuration != null && LogManager.Configuration.Variables["fileNames"] != null)
                LogManager.Configuration.Variables["fileNames"] = string.IsNullOrWhiteSpace(fileNames) ? "Logs" : fileNames;
            loggerName = string.IsNullOrWhiteSpace(loggerName) ? LoggerName : loggerName;
            _logger = !string.IsNullOrWhiteSpace(loggerName) ? LogManager.GetLogger(loggerName) : LogManager.GetCurrentClassLogger();
            _logger.Log(level == null ? LogLevel.Info : level, message, ex);
        }
        public static void ErrorLog(this string message, string loggerName = "", string fileNames = "", Exception ex = null) => message.AddLog(loggerName, fileNames, LogLevel.Error, ex);
        public static void InfoLog(this string message, string loggerName = "", string fileNames = "", Exception ex = null) => message.AddLog(loggerName, fileNames, LogLevel.Info, ex);
        public static void DebugLog(this string message, string loggerName = "", string fileNames = "", Exception ex = null) => message.AddLog(loggerName, fileNames, LogLevel.Debug, ex);
        public static void TraceLog(this string message, string loggerName = "", string fileNames = "", Exception ex = null) => message.AddLog(loggerName, fileNames, LogLevel.Trace, ex);
        public static void WarnLog(this string message, string loggerName = "", string fileNames = "", Exception ex = null) => message.AddLog(loggerName, fileNames, LogLevel.Warn, ex);
        public static void FatalLog(this string message, string loggerName = "", string fileNames = "", Exception ex = null) => message.AddLog(loggerName, fileNames, LogLevel.Fatal, ex);
        public static void OffLog(this string message, string loggerName = "", string fileNames = "", Exception ex = null) => message.AddLog(loggerName,fileNames, LogLevel.Off, ex);
        public static void Log(this Exception exception, string loggerName = "", string fileNames = "") => exception.Message.ErrorLog(loggerName,fileNames, exception);
    }
}
