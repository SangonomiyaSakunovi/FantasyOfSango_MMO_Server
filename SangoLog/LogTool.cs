using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;

//Developer: SangonomiyaSakunovi

public static class ExtensionMethods
{
    #region Log
    public static void LogInfo(this object obj, string log, params object[] args)
    {
        SangoLog.LogTool.LogInfo(string.Format(log,args));
    }

    public static void LogInfo(this object obj, object logObj)
    {
        SangoLog.LogTool.LogInfo(logObj);
    }

    public static void ColorLog(this object obj, SangoLog.LogColor color, string log, params object[] args)
    {
        SangoLog.LogTool.ColorLog(color,string.Format(log, args));
    }

    public static void ColorLog(this object obj, SangoLog.LogColor color, object logObj)
    {
        SangoLog.LogTool.ColorLog(color,logObj);
    }

    public static void LogTraceInfo(this object obj, string log, params object[] args)
    {
        SangoLog.LogTool.LogTraceInfo(string.Format(log, args));
    }

    public static void LogTraceInfo(this object obj, object logObj)
    {
        SangoLog.LogTool.LogTraceInfo(logObj);
    }

    public static void LogWarn(this object obj, string log, params object[] args)
    {
        SangoLog.LogTool.LogWarn(string.Format(log, args));
    }

    public static void LogWarn(this object obj, object logObj)
    {
        SangoLog.LogTool.LogWarn(logObj);
    }

    public static void LogError(this object obj, string log, params object[] args)
    {
        SangoLog.LogTool.LogError(string.Format(log, args));
    }

    public static void LogError(this object obj, object logObj)
    {
        SangoLog.LogTool.LogError(logObj);
    }

    public static void LogProcessing(this object obj, string log, params object[] args)
    {
        SangoLog.LogTool.LogProcessing(string.Format(log, args));
    }

    public static void LogProcessing(this object obj, object logObj)
    {
        SangoLog.LogTool.LogProcessing(logObj);
    }

    public static void LogDone(this object obj, string log, params object[] args)
    {
        SangoLog.LogTool.LogDone(string.Format(log, args));
    }

    public static void LogDone(this object obj, object logObj)
    {
        SangoLog.LogTool.LogDone(logObj);
    }
    #endregion
}

namespace SangoLog
{
    public class LogTool
    {
        public static LogConfig config;
        private static ILogger logger;
        private static StreamWriter logFileWriter = null;

        public static void InitSettings(LogConfig cfg = null)
        {
            if (cfg == null)
            {
                cfg = new LogConfig();
            }
            config = cfg;
            if (config.loggerType == LoggerType.Console)
            {
                logger = new ConsoleLogger();
            }
            else
            {
                logger = new UnityLogger();
            }
            if (config.enableSaveLog == false) { return; }
            if (config.enableCoverLog)
            {
                string path = config.saveLogPath + config.saveLogName;
                try
                {
                    if (Directory.Exists(config.saveLogPath))
                    {
                        if (File.Exists(path))
                        {
                            File.Delete(path);
                        }
                    }
                    else
                    {
                        Directory.CreateDirectory(config.saveLogPath);
                    }
                    logFileWriter = File.AppendText(path);
                    logFileWriter.AutoFlush = true;
                }
                catch
                {
                    logFileWriter = null;
                }
            }
            else
            {
                string prefix = DateTime.Now.ToString("yyyyMMdd@HH-mm-ss");
                string path = config.saveLogPath + prefix + config.saveLogName;
                try
                {
                    if (Directory.Exists(config.saveLogPath) == false)
                    {
                        Directory.CreateDirectory(config.saveLogPath);
                    }
                    logFileWriter = File.AppendText(path);
                    logFileWriter.AutoFlush = true;
                }
                catch
                {

                }
            }
        }

        #region Log
        public static void LogInfo(string log, params object[] args)
        {
            if (config.enableSangoLog == false) { return; }
            log = DecorateLog(string.Format(log, args));
            logger.Log(log);
            if (config.enableSaveLog)
            {
                WriteToFile(string.Format("[LogInfo]{0}", log));
            }
        }

        public static void LogInfo(object logObj)
        {
            if (config.enableSangoLog == false) { return; }
            string log = DecorateLog(logObj.ToString());
            logger.Log(log);
            if (config.enableSaveLog)
            {
                WriteToFile(string.Format("[LogInfo]{0}", log));
            }
        }

        public static void ColorLog(LogColor color, string log, params object[] args)
        {
            if (config.enableSangoLog == false) { return; }
            log = DecorateLog(string.Format(log, args));
            logger.Log(log, color);
            if (config.enableSaveLog)
            {
                WriteToFile(string.Format("[LogInfo]{0}", log));
            }
        }

        public static void ColorLog(LogColor color, object logObj)
        {
            if (config.enableSangoLog == false) { return; }
            string log = DecorateLog(logObj.ToString());
            logger.Log(log, color);
            if (config.enableSaveLog)
            {
                WriteToFile(string.Format("[LogInfo]{0}", log));
            }
        }

        public static void LogTraceInfo(string log, params object[] args)
        {
            if (config.enableSangoLog == false) { return; }
            log = DecorateLog(string.Format(log, args), true);
            logger.Log(log, LogColor.Magenta);
            if (config.enableSaveLog)
            {
                WriteToFile(string.Format("[LogTraceInfo]{0}", log));
            }
        }

        public static void LogTraceInfo(object logObj)
        {
            if (config.enableSangoLog == false) { return; }
            string log = DecorateLog(logObj.ToString(), true);
            logger.Log(log, LogColor.Magenta);
            if (config.enableSaveLog)
            {
                WriteToFile(string.Format("[LogTraceInfo]{0}", log));
            }
        }

        public static void LogWarn(string log, params object[] args)
        {
            if (config.enableSangoLog == false) { return; }
            log = DecorateLog(string.Format(log, args));
            logger.Warn(log);
            if (config.enableSaveLog)
            {
                WriteToFile(string.Format("[LogWarn]{0}", log));
            }
        }

        public static void LogWarn(object logObj)
        {
            if (config.enableSangoLog == false) { return; }
            string log = DecorateLog(logObj.ToString());
            logger.Warn(log);
            if (config.enableSaveLog)
            {
                WriteToFile(string.Format("[LogWarn]{0}", log));
            }
        }

        public static void LogError(string log, params object[] args)
        {
            if (config.enableSangoLog == false) { return; }
            log = DecorateLog(string.Format(log, args));
            logger.Error(log);
            if (config.enableSaveLog)
            {
                WriteToFile(string.Format("[LogError]{0}", log));
            }
        }

        public static void LogError(object logObj)
        {
            if (config.enableSangoLog == false) { return; }
            string log = DecorateLog(logObj.ToString());
            logger.Error(log);
            if (config.enableSaveLog)
            {
                WriteToFile(string.Format("[LogError]{0}", log));
            }
        }

        public static void LogProcessing(string log, params object[] args)
        {
            if (config.enableSangoLog == false) { return; }
            log = DecorateLog(string.Format(log, args));
            logger.Processing(log);
            if (config.enableSaveLog)
            {
                WriteToFile(string.Format("[LogProcessing]{0}", log));
            }
        }

        public static void LogProcessing(object logObj)
        {
            if (config.enableSangoLog == false) { return; }
            string log = DecorateLog(logObj.ToString());
            logger.Processing(log);
            if (config.enableSaveLog)
            {
                WriteToFile(string.Format("[LogProcessing]{0}", log));
            }
        }

        public static void LogDone(string log, params object[] args)
        {
            if (config.enableSangoLog == false) { return; }
            log = DecorateLog(string.Format(log, args));
            logger.Done(log);
            if (config.enableSaveLog)
            {
                WriteToFile(string.Format("[LogDone]{0}", log));
            }
        }

        public static void LogDone(object logObj)
        {
            if (config.enableSangoLog == false) { return; }
            string log = DecorateLog(logObj.ToString());
            logger.Done(log);
            if (config.enableSaveLog)
            {
                WriteToFile(string.Format("[LogDone]{0}", log));
            }
        }
        #endregion

        #region Decorate
        private static string DecorateLog(string log, bool isTraceInfo = false)
        {
            StringBuilder sb = new StringBuilder(config.logPrefix, 100);
            if (config.enableTimestamp)
            {
                sb.AppendFormat(" {0}", DateTime.Now.ToString("hh:mm:ss--fff"));
            }
            if (config.enableThreadID)
            {
                sb.AppendFormat(" {0}", GetThreadID());
            }
            sb.AppendFormat(" {0} {1}", config.logSeparate, log);
            if (isTraceInfo)
            {
                sb.AppendFormat(" \nStackTrace: {0}", GetTraceInfo());
            }
            return sb.ToString();
        }

        private static string GetThreadID()
        {
            return string.Format("ThreadID:{0}", Thread.CurrentThread.ManagedThreadId);
        }

        private static string GetTraceInfo()
        {
            StackTrace st = new StackTrace(3, true);    //The method called DecorateLog has 3 calls should be ignore
            string traceInfo = "";
            for (int i = 0; i < st.FrameCount; i++)
            {
                StackFrame sf = st.GetFrame(i);
                traceInfo += string.Format("\n    {0}::{1}  line:{2}", sf.GetFileName(), sf.GetMethod(), sf.GetFileLineNumber());
            }
            return traceInfo;
        }
        #endregion

        private static void WriteToFile(string log)
        {
            if (logFileWriter != null)
            {
                try
                {
                    logFileWriter.WriteLine(log);
                }
                catch
                {
                    logFileWriter = null;
                }
            }
        }

        private class UnityLogger : ILogger
        {
            Type type = Type.GetType("UnityEngine.Debug,UnityEngine");

            public void Log(string log, LogColor color = LogColor.None)
            {
                log = GetUnityLogColorString(log, color);
                type.GetMethod("Log", new Type[] { typeof(object) }).Invoke(null, new object[] { log });
            }

            public void Processing(string log)
            {
                log = GetUnityLogColorString(log, LogColor.Cyan);
                type.GetMethod("Log", new Type[] { typeof(object) }).Invoke(null, new object[] { log });
            }

            public void Done(string log)
            {
                log = GetUnityLogColorString(log, LogColor.Green);
                type.GetMethod("Log", new Type[] { typeof(object) }).Invoke(null, new object[] { log });
            }

            public void Warn(string log)
            {
                log = GetUnityLogColorString(log, LogColor.Yellow);
                type.GetMethod("LogWarning", new Type[] { typeof(object) }).Invoke(null, new object[] { log });
            }

            public void Error(string log)
            {
                log = GetUnityLogColorString(log, LogColor.Red);
                type.GetMethod("LogError", new Type[] { typeof(object) }).Invoke(null, new object[] { log });
            }

            private string GetUnityLogColorString(string log, LogColor color)
            {
                switch (color)
                {
                    case LogColor.None:
                        break;
                    case LogColor.Yellow:
                        log = string.Format("<color=#FFFF00>{0}</color>", log);
                        break;
                    case LogColor.Red:
                        log = string.Format("<color=#FF0000>{0}</color>", log);
                        break;
                    case LogColor.Green:
                        log = string.Format("<color=#00FF00>{0}</color>", log);
                        break;
                    case LogColor.Blue:
                        log = string.Format("<color=#0000FF>{0}</color>", log);
                        break;
                    case LogColor.Magenta:
                        log = string.Format("<color=#FF00FF>{0}</color>", log);
                        break;
                    case LogColor.Cyan:
                        log = string.Format("<color=#00FFFF>{0}</color>", log);
                        break;
                    default:
                        Console.WriteLine(log);
                        break;
                }
                return log;
            }
        }

        private class ConsoleLogger : ILogger
        {
            public void Log(string log, LogColor color = LogColor.None)
            {
                ConsoleLog(log, color);
            }

            public void Processing(string log)
            {
                ConsoleLog(log, LogColor.Cyan);
            }

            public void Done(string log)
            {
                ConsoleLog(log, LogColor.Green);
            }

            public void Warn(string log)
            {
                ConsoleLog(log, LogColor.Yellow);
            }

            public void Error(string log)
            {
                ConsoleLog(log, LogColor.Red);
            }

            private void ConsoleLog(string log, LogColor color)
            {
                switch (color)
                {
                    case LogColor.None:
                        Console.WriteLine(log);
                        break;
                    case LogColor.Yellow:
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine(log);
                        Console.ForegroundColor = ConsoleColor.Gray;
                        break;
                    case LogColor.Red:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine(log);
                        Console.ForegroundColor = ConsoleColor.Gray;
                        break;
                    case LogColor.Green:
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine(log);
                        Console.ForegroundColor = ConsoleColor.Gray;
                        break;
                    case LogColor.Blue:
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.WriteLine(log);
                        Console.ForegroundColor = ConsoleColor.Gray;
                        break;
                    case LogColor.Magenta:
                        Console.ForegroundColor = ConsoleColor.Magenta;
                        Console.WriteLine(log);
                        Console.ForegroundColor = ConsoleColor.Gray;
                        break;
                    case LogColor.Cyan:
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.WriteLine(log);
                        Console.ForegroundColor = ConsoleColor.Gray;
                        break;
                    default:
                        Console.WriteLine(log);
                        break;
                }
            }
        }
    }
}
