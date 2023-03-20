using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LoggerForServices
{
    public class Logger
    {
        public string TestLogDir = @"C:\Logs\Test\";
        public string LogDir = @"C:\Logs\";
        public string DefaultFileName = @"Logs";
        public string DefaultExt = ".log";
        public int LogNum = 0;
        public int TestLogNum = 0;
        public int MaxFileSize = 30; //max log filesize
        private int steps = 0;

        public enum LogType
        {
            INFO = 1,
            ERROR = 2,
            FATAL = 3,
            WARNING = 4,
            END = 5,
            TESTS = 6,
            DEBUG = 7,
            DETAILS = 8
        }
        /// <summary>
        /// Initialize logger, check if path exists and get current log file number
        /// </summary>
        private void CheckFileSize(bool isTest = false)
        {
            var directory = isTest ? TestLogDir : LogDir;
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            while (true)
            {
                string path = string.Concat(directory, DefaultFileName, LogNum.ToString(), DefaultExt);
                if (File.Exists(path))
                {
                    if (new FileInfo(path).Length >= MaxFileSize * 1024 * 1024)
                    {
                        LogNum++;
                    }
                    else
                    {
                        break;
                    }
                }
                else
                {
                    break;
                }
            }
        }

        public void WriteLog(string message, LogType type)
        {
            try
            {
                if (steps < 10)
                {
                    CheckFileSize();
                    string path = string.Concat(LogDir, DefaultFileName, LogNum.ToString(), DefaultExt);
                    using (StreamWriter w = File.AppendText(path))
                    {
                        switch (type)
                        {
                            case LogType.ERROR:
                                w.WriteLine("ERROR " + DateTime.Now + ": " + message);
                                break;
                            case LogType.FATAL:
                                w.WriteLine("FATAL " + DateTime.Now + ": " + message);
                                break;
                            case LogType.INFO:
                                w.WriteLine("INFO " + DateTime.Now + ": " + message);
                                break;
                            case LogType.WARNING:
                                w.WriteLine("WARNING " + DateTime.Now + ": " + message);
                                break;
                            case LogType.END:
                                w.WriteLine(">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>" + Environment.NewLine);
                                break;
                            case LogType.TESTS:
                                WriteTestLog(message);
                                break;
                            case LogType.DEBUG:
                                w.WriteLine("DEBUG " + DateTime.Now + ": " + message);
                                break;
                            case LogType.DETAILS:
                                w.WriteLine("DETAILES " + DateTime.Now + ": " + message);
                                break;
                        }
                    }
                    steps = 0;
                }
                else
                {
                    throw new Exception("Unable to access log file"); 
                }
            }
            catch(IOException)
            {
                steps++;
                Thread.Sleep(100);
                WriteLog(message, type);
            }
        }

        private void WriteTestLog(string message)
        {
            CheckFileSize(isTest: true);
            string path = string.Concat(TestLogDir, DefaultFileName, LogNum.ToString(), DefaultExt);
            using (StreamWriter w = File.AppendText(path))
            {
                w.WriteLine("TESTS " + DateTime.Now + ": " + message);
            }
        }
    }
}
