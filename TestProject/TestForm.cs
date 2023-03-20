using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using CRMIntegration;
using Common;
using Contacts;
using DataAccess.DAO;
using Opportunities;
using LoggerForServices;
using DataAccess.Entities;
using static Common.CommonObjects;
using DataAccess.Helpers;
using Microsoft.Xrm.Sdk;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Globalization;

namespace TestProject
{
    public partial class TestForm : Form
    {
        public TestForm()
        {
            InitializeComponent();
        }
        Thread Launcher { get; set; } 

        private void btRunFull_Click(object sender, EventArgs e)
        {
            Launcher = new Thread(() => MainWorker.DoLaunch())
            {
                IsBackground = true
            };
            Data.Logger.WriteLog("Tests: Starting full", Logger.LogType.DEBUG);
            Launcher.Start();
        }

        private void TestForm_Load(object sender, EventArgs e)
        {
            Data.Logger = new Logger() { LogDir = @"C:\ServiceLogs\CRMIntegration\" };
            Data.DebugLogger = new Logger() { LogDir = @"C:\ServiceLogs\CRMIntegration\" };
           // Data.Debug = true;
        }

        private void btRunUsers_Click(object sender, EventArgs e)
        {
            Data.GetAll = true;
            Data.Logger.WriteLog("Tests: Starting users", Logger.LogType.DEBUG);
            Thread contactUpdate = new Thread(new ThreadStart(ContactWorker.RunContactCheck));
            contactUpdate.Start();
        }
        private void btTestSearch_Click(object sender, EventArgs e)
        {
            CRMDAO dao = new CRMDAO();
          
        }

        private void btIsInCRM_Click(object sender, EventArgs e)
        {
            Data.Logger.WriteLog("Tests: Starting IsInCRM", Logger.LogType.DEBUG);
            Thread test = new Thread(new ParameterizedThreadStart(UpdateIsInCRM.Update));
            test.Start();
        }

        private void btStartHost_Click(object sender, EventArgs e)
        {
            Data.Logger.WriteLog("Tests: Starting host", Logger.LogType.DEBUG);
            Thread test = new Thread(new ThreadStart(HostLauncher.StartHost));
            test.Start();
        }

        private void btUnpaidInvoices_Click(object sender, EventArgs e)
        {
            Data.Logger.WriteLog("Tests: Starting unpaid invoices test", Logger.LogType.DEBUG);
            Thread test = new Thread(new ThreadStart(UnpaidInvoices.Update));
            test.Start();
        }

        private void btUpdateOwner_Click(object sender, EventArgs e)
        {
            Data.Logger.WriteLog("Tests: Starting owners update test", Logger.LogType.DEBUG);
            Thread test = new Thread(() => UpdateOwner.UpdateOwners(txtOppName.Text));
            test.Start();
        }

        private void btStopFull_Click(object sender, EventArgs e)
        {
            try
            {
                Launcher.Abort();
                Data.StopHost = true;
                Data.Listener.Close(0);
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

        private void btRunSpecificUser_Click(object sender, EventArgs e)
        {
            Data.Logger.WriteLog("Tests: Starting users", Logger.LogType.DEBUG);
            Thread contactUpdate = new Thread(new ThreadStart(ContactWorker.RunContactCheck));
            contactUpdate.Start();
        }
        System.Threading.Timer Timer1H { get; set; } = null;
        private void btStartUserByTimer_Click(object sender, EventArgs e)
        {
            Run1H(null);
        }
        private void StartTimer()
        {
            int delay = 3600000;
            Timer1H = new System.Threading.Timer(Run1H, null, delay, Timeout.Infinite);
            Data.Logger.WriteLog("Main: Timer 1H started. Next run at " + DateTime.Now.AddMilliseconds(delay), LoggerForServices.Logger.LogType.INFO);
        }
        private void Run1H(object state)
        {
            try
            {
                Thread contactUpdate = new Thread(new ThreadStart(ContactWorker.RunContactCheck));
                contactUpdate.Start();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                StartTimer();
            }
        }

        private void btCreateOpportunity_Click(object sender, EventArgs e)
        {
            try
            {
                CreateOpportunity create = new CreateOpportunity();
                create.Create(new CRMRequest() { Additional = new string[]{
                "13.12.2022 00:00:00", 
                "",
                "",
                "",
                "",
                "100000004",
                null, null},
                 Name = "SKF08J3934589",
                   TaskToRun =  "AddNewOpportunity",
                    Username = ""
                }, OpportunityType.onlineOrder);
                //create.Create(txtOppName.Text, OpportunityType.onlineOrder);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void btTestSeach_Click(object sender, EventArgs e)
        {
            CRMDAO dao = new CRMDAO();
           // dao.SearchObjectsByEnd(new CommonObjects.CRMInput() { Type = "user", Column = "email", Value = "test" });
        }

        private void btFtpApiUpdate_Click(object sender, EventArgs e)
        {
            ApiUpdate api = new ApiUpdate();
            api.Update();
        }

        private void btFtpUpdate_Click(object sender, EventArgs e)
        {
            FtpUpdate.Update();
        }

        private void btGetName_Click(object sender, EventArgs e)
        {
            MessageBox.Show(Environment.MachineName);
        }

        private void btFtpApiByTimer_Click(object sender, EventArgs e)
        {
            //MainWorker.StartApiFtpTest();
        }

        private void btCheckIsInCRM_Click(object sender, EventArgs e)
        {
            OrderDAO dao = new OrderDAO();
            CRMHelper crm = new CRMHelper();
            CRMDAO crmDAO = new CRMDAO();
            
            List<Order> orders = dao.GetByCreatedDate(DateTime.Now.AddDays(-30),DateTime.Now).OrderBy(x=> x.Created).ToList();
           List<Entity> opps =  crmDAO.GetObjectsByList(new CRMInput() { Columns = new List<string>() { "name" }, Type = "opportunity", Values = orders.Select(x => x.MolportOrderNumber).Distinct().ToArray() });
            int count = 0;
            foreach(Order order in orders)
            {
                Entity opp = opps.Find(x => x.Attributes["name"].ToString() == order.MolportOrderNumber);
                if(opp== null)
                {
                    order.IsInCRM = 0;
                }
                else
                {
                    if (opp.Attributes.ContainsKey("new_oportunity_type"))
                    {
                        OptionSetValue val = (OptionSetValue)opp.Attributes["new_oportunity_type"];
                        if (val.Value.ToString() == ((int)OpportunityType.purchaseOrder).ToString())
                        {
                            order.IsInCRM = -1;
                        }
                        else
                        {
                            order.IsInCRM = 0;
                        }
                    }
                    else
                    {
                        order.IsInCRM = 0;
                    }
                }
                dao.Update(order);
                count++;
            }
        }

        private void btCloseasWon_Click(object sender, EventArgs e)
        {
            CRMDAO crmDAO = new CRMDAO();
            Entity opp = crmDAO.GetObjectsByValue(new CRMInput() { Columns = new List<string>() { "name" }, Type = "opportunity", Value = "" }).FirstOrDefault();
            if (opp != null)
            {
                var a = opp.Attributes["transactioncurrencyid"];
                crmDAO.CloseAsWon(opp);
            }
        }

        private void btTestCurrency_Click(object sender, EventArgs e)
        {
            CRMDAO crmDAO = new CRMDAO();
            Entity curr = crmDAO.GetObjectsByValue(new CRMInput() { Columns = new List<string>() { "currencyname" }, Type = "transactioncurrency", Value = "USD" }).FirstOrDefault();
        }

        private void btTestSocket_Click(object sender, EventArgs e)
        {
            IPHostEntry hosts = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress ipAddress = hosts.AddressList.Where(x => x.AddressFamily == AddressFamily.InterNetwork).FirstOrDefault();
            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, 7843);
            Socket client = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            client.Connect(localEndPoint);
            byte[] bytes = new byte[1024];

            BinaryFormatter bf = new BinaryFormatter();
            MemoryStream ms = new MemoryStream();
            bf.Serialize(ms, "test<EOF>");
            byte[] test = ms.ToArray();

            int bytesSent = client.Send(test);
            client.Shutdown(SocketShutdown.Both);
            client.Close();           
        }

        private void btUpdateDeliveryDates_Click(object sender, EventArgs e)
        {
            Thread test = new Thread(() => UpdateLastOrderDate.UpdateOrderDeliveryDate(new DateTime(2022, 1, 1)));
            btUpdateDeliveryDates.Enabled = false;
            test.Start();
            while(test.IsAlive)
            {
                Application.DoEvents();
            }
            btUpdateDeliveryDates.Enabled = true;
        }

        private void btTestBO_Click(object sender, EventArgs e)
        {
            BoIntegrationTests test = new BoIntegrationTests();
            string tt = test.GetReponse();
        }

        private void btTestPunchOUT_Click(object sender, EventArgs e)
        {
            OrderDAO dao = new OrderDAO();
            Order order = dao.GetByName("");
            CreateOpportunity create = new CreateOpportunity();
        }
    }
}
