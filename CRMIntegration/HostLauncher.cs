using Common;
using LoggerForServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CRMIntegration
{
    public class HostLauncher
    {
        private static bool AwaitStart { get; set; } = false;
        public static void StartHost()
        {
            AwaitStart = false;
            HostServer server = new HostServer();
            Data.Logger.WriteLog("Host server: Starting", Logger.LogType.INFO);
            Thread host = new Thread(() => server.StartListener());
            host.Start();
            while (!Data.StopHost)
            {
                if (!host.IsAlive && !AwaitStart)
                {
                    Data.Logger.WriteLog("Host server: Host running error. Will try to restart host", Logger.LogType.FATAL);
                    RestartHost();
                    break;
                }
                Thread.Sleep(1000);
            }
        }
        private static void RestartHost()
        {
            AwaitStart = true;
            System.Timers.Timer timer = new System.Timers.Timer(60000);
            timer.Elapsed += (s, e) =>
            {
                timer.Stop();
                StartHost();
            };
            timer.Start();
        }
    }
}
