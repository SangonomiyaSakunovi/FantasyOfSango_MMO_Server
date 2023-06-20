using System;

//Developer: SangonomiyaSakunovi

namespace SangoLog
{
    public enum LoggerType
    {
        Unity,
        Console
    }

    public enum LogColor
    {
        None,
        Red,
        Green,
        Blue,
        Cyan,
        Magenta,
        Yellow
    }


    public class LogConfig
    {
        public bool enableSangoLog = true;
        public string logPrefix = "#";
        public bool enableTimestamp = true;
        public string logSeparate = ">>";
        public bool enableThreadID = true;
        public bool enableTraceInfo = true;
        public bool enableSaveLog = true;
        public bool enableCoverLog = true;
        public string saveLogPath = string.Format("{0}Logs\\", AppDomain.CurrentDomain.BaseDirectory);
        public string saveLogName = "SangoLog.txt";
        public LoggerType loggerType = LoggerType.Console;
    }

    public interface ILogger
    {
        void Log(string message, LogColor color = LogColor.None);
        void Processing(string message);
        void Done(string message);
        void Warn(string message);
        void Error(string message);
    }
}
