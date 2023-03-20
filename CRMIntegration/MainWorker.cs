using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using System.Threading;
using System.Threading.Tasks;
using Contacts;
using Opportunities;
using Timer = System.Timers.Timer;
using Common.Helpers;

namespace CRMIntegration
{
    public class MainWorker
    {
        private static Timer Timer1H { get; set; } = null;
        private static Timer Timer12H { get; set; } = null;
        private static Timer TimerMidnight { get; set; } = null;
        public static void DoLaunch()
        {
            try
            {
                Thread host = new Thread(new ThreadStart(HostLauncher.StartHost));
                host.Start();
                StartTimers();
            }
            catch(Exception ex)
            {
                Data.Logger.WriteLog("Main: Fatal error on launcher! Reason - " + ex.Message, LoggerForServices.Logger.LogType.FATAL);
                StartTimers();
            }
        }
        private static void StartTimers()
        {
            if (Timer1H == null)
            {
                int delay = DateTimeHelper.GetUntilNextHour();
                Timer1H = new Timer(delay);
                Timer1H.Elapsed += (s, e) => Run1H();
                Timer1H.Start();
                Data.Logger.WriteLog("Main: Timer 1H started. Next run at " + DateTime.Now.AddMilliseconds(delay), LoggerForServices.Logger.LogType.INFO);
            }
            if (Timer12H == null)
            {
                int delay = DateTimeHelper.GetNext12H();
                Timer12H = new Timer(delay);
                Timer12H.Elapsed += (s, e) => Run12H();
                Timer12H.Start();
                Data.Logger.WriteLog("Main: Timer 12H started. Next run at " + DateTime.Now.AddMilliseconds(delay), LoggerForServices.Logger.LogType.INFO);
            }
            if (TimerMidnight == null)
            {
                int delay = DateTimeHelper.GetUntilMidnight();
                TimerMidnight = new Timer(delay);
                TimerMidnight.Elapsed += (s, e) => RunMidnight();
                TimerMidnight.Start();
                Data.Logger.WriteLog("Main: Midnight timer started. Next run at " + DateTime.Now.AddMilliseconds(delay), LoggerForServices.Logger.LogType.INFO);
            }
        }
        private static void Run1H()
        {
            try
            {
                if (Timer1H != null)
                {
                    Timer1H.Stop();
                    Timer1H = null;
                }
                try
                {
                    Thread contactUpdate = new Thread(new ThreadStart(ContactWorker.RunContactCheck));
                    contactUpdate.Start();
                    while (contactUpdate.IsAlive)
                    {
                        Thread.Sleep(100);
                    }
                }
                catch(Exception ex)
                {
                    Data.Logger.WriteLog("Main: Fatal error on 1h treads (contact update) launch. Reason - " + ex.Message, LoggerForServices.Logger.LogType.FATAL);
                }
                try
                {
                    ApiUpdate api = new ApiUpdate();
                    Thread apiUpdate = new Thread(new ThreadStart(api.Update));
                    apiUpdate.Start();
                    while (apiUpdate.IsAlive)
                    {
                        Thread.Sleep(100);
                    }
                }
                catch (Exception ex)
                {
                    Data.Logger.WriteLog("Main: Fatal error on 1h treads (api update) launch. Reason - " + ex.Message, LoggerForServices.Logger.LogType.FATAL);
                }
                try
                {
                    Thread ftpUpdate = new Thread(new ThreadStart(FtpUpdate.Update));
                    ftpUpdate.Start();
                    while (ftpUpdate.IsAlive)
                    {
                        Thread.Sleep(100);
                    }
                }
                catch (Exception ex)
                {
                    Data.Logger.WriteLog("Main: Fatal error on 1h treads (ftp update) launch. Reason - " + ex.Message, LoggerForServices.Logger.LogType.FATAL);
                }
            }
            catch(Exception ex)
            {
                Data.Logger.WriteLog("Main: Fatal error on 1h treads (contact, ftp and api update) launch. Reason - " + ex.Message, LoggerForServices.Logger.LogType.FATAL);
            }
            finally
            {
                 StartTimers();
            }
        }
        private static void Run12H()
        {
            try
            {
                Timer12H.Stop();
                Timer12H = null;
                Thread unpaid = new Thread(new ThreadStart(UnpaidInvoices.Update));
                unpaid.Start();
            }
            catch (Exception ex)
            {
                Data.Logger.WriteLog("Main: Fatal error on 12h treads (unpaid invoices) launch. Reason - " + ex.Message, LoggerForServices.Logger.LogType.FATAL);
            }
            finally
            {
                StartTimers();
            }
        }
        public static void RunMidnight()
        {
            try
            {
                TimerMidnight.Stop();
                TimerMidnight = null;
                try
                {
                    Thread oppUpdate = new Thread(new ParameterizedThreadStart(UpdateIsInCRM.Update));
                    oppUpdate.Start();
                    while (oppUpdate.IsAlive)
                    {
                        Thread.Sleep(100);
                    }
                }
                catch(Exception ex)
                {
                    Data.Logger.WriteLog("Main: Unable to update IsInCRM field. Reason - " + ex.Message, LoggerForServices.Logger.LogType.ERROR);
                }
                try
                {
                    Thread dates = new Thread(() => UpdateLastOrderDate.UpdateOrderDeliveryDate());
                    dates.Start();
                    while(dates.IsAlive)
                    {
                        Thread.Sleep(100);
                    }
                }
                catch(Exception ex)
                {
                    Data.Logger.WriteLog("Main: Unable to update LastOrderDelivered field. Reason - " + ex.Message, LoggerForServices.Logger.LogType.ERROR);
                }
            }
            catch(Exception ex)
            {
                Data.Logger.WriteLog("Main: Fatal error on midnight threads (update if orders are in CRM, LastOrderDelivered) launch. Reason - " + ex.Message, LoggerForServices.Logger.LogType.FATAL);
            }
            finally
            {
                StartTimers();
            }
        }
    }
}
