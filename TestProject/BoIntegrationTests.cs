using Common.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace TestProject
{
    public class BoIntegrationTests
    {
        [Serializable]
        public class CRMRequest
        {
            public string TaskToRun { get; set; }
            public string Name { get; set; }
            public string Username { get; set; }
            public string[] Additional { get; set; }
        }
        public string GetReponse()
        {
            //for opp create use param[] { {DateTime} order_date, {decimal} price, {string} currency, {string} acc_name, {string} contact, {string} segment, {int} updateProbability, {int} CloseAsWon }
            CRMRequest input = new CRMRequest()
            {
                TaskToRun = "AddNewOpportunity",
                Username = "Username",
                Name = "None",
                Additional = new string[] 
                { 
                    new DateTime(2022, 1, 1).ToString(),
                    Convert.ToDecimal(100.23).ToString(),
                    "USD",
                    "TestAcc",
                    "TestContact",
                    "Segment",
                    "0",
                    "0"
                }
            };
            string result = string.Empty;
            string js0 = JsonConvert.SerializeObject(input);
            string js1 = CryptHelper.Encrypt(js0);
            string js2 = js1 + "<EOF>";
            IPHostEntry hosts = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress ipAddress = hosts.AddressList.Where(x => x.AddressFamily == AddressFamily.InterNetwork).FirstOrDefault();
            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, 0);
            Socket client = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            client.Connect(localEndPoint);
            byte[] bytes = new byte[1024];
            byte[] data = Encoding.UTF8.GetBytes(js2);
            int bytesSent = client.Send(data);
            client.Receive(bytes);
            string r0 = Encoding.UTF8.GetString(bytes);
            client.Shutdown(SocketShutdown.Both);
            client.Close();
            return result;
        }
    }
}
