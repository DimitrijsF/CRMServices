using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Common;
using LoggerForServices;
using CRMIntegration;

namespace CRMIntegrationService
{
    public partial class CRMIntegrationService : ServiceBase
    {
        public CRMIntegrationService()
        {
            InitializeComponent();
        }
        Thread Launcher { get; set; } = new Thread(() => MainWorker.DoLaunch())
        {
            IsBackground = true
        };
        protected override void OnStart(string[] args)
        {
            Data.Logger = new Logger() { LogDir = @"C:\ServiceLogs\CRMIntegration\" };
            Launcher = new Thread(() => MainWorker.DoLaunch())
            {
                IsBackground = true
            };
            try
            {
                Data.Logger.WriteLog("Service: Started", Logger.LogType.INFO);
                if(args != null && args.Length > 0)
                {
                    if(args.First() == "1")
                    {
                        Data.Debug = true;
                        Data.DebugLogger = new Logger() { LogDir = @"C:\ServiceLogs\CRMIntegration\Debug\" };
                        Data.Logger.WriteLog("Service: Debug parameter applied", Logger.LogType.INFO);
                    }
                }
                Launcher.Start();
            }
            catch(Exception ex)
            {
                if (Data.Logger != null)
                {
                    Data.Logger.WriteLog("Service: Fatal error. " + ex.Message, Logger.LogType.FATAL);
                }
            }
        }

        protected override void OnStop()
        {
            try
            {
                Launcher.Abort();
                if (Data.Listener != null)
                {
                    Data.StopHost = true;
                    Data.Listener.Close(0);
                }
            }
            catch (Exception ex)
            {
                Data.Logger.WriteLog("Service: Fatal error. " + ex.Message, Logger.LogType.FATAL);
            }
            finally
            {
                Data.Logger.WriteLog("Service: Stopped", Logger.LogType.INFO);
                Data.Logger.WriteLog(string.Empty, Logger.LogType.END);
            }
        }
    }
}
