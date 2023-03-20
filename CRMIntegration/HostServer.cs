using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Common;
using Common.Helpers;
using Contacts;
using DataAccess.DAO;
using DataAccess.Helpers;
using LoggerForServices;
using Newtonsoft.Json;
using Opportunities;
using static Common.CommonObjects;

namespace CRMIntegration
{
    public class HostServer
    {
        private Socket Client { get; set; }

        public void StartListener()
        {
            try
            {
                Data.LockedOrders = new List<LockedOrder>();
                IPHostEntry hosts = Dns.GetHostEntry(Dns.GetHostName());
                IPAddress ipAddress = hosts.AddressList.Where(x => x.AddressFamily == AddressFamily.InterNetwork).FirstOrDefault();
                if (ipAddress != null)
                {
                    IPEndPoint localEndPoint = new IPEndPoint(ipAddress, 0);
                    Data.Listener = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                    Data.Listener.Bind(new IPEndPoint(IPAddress.Any, 7843));
                    Data.Listener.Listen(100);
                    Data.Logger.WriteLog("Host server: Started", Logger.LogType.INFO);
                    DoListen();
                }
                else
                {
                    Data.Logger.WriteLog("Host server: Error on starting. Reason - unable to get correct ip address of machine", Logger.LogType.FATAL);
                }
            }
            catch (Exception ex)
            {
                Data.Logger.WriteLog("Host server: Error on starting. Reason - " + ex.Message, Logger.LogType.FATAL);
            }
        }
        private void DoListen()
        {
            try
            {
                while (true)
                {
                    CRMHelper helper = new CRMHelper();
                    Client = Data.Listener.Accept();
                    byte[] bytes = new byte[1024];
                    string data = null;
                    while (true)
                    {
                        int numByte = Client.Receive(bytes);
                        data += Encoding.UTF8.GetString(bytes, 0, numByte);
                        if (data.IndexOf("<EOF>") > -1)
                        {
                            break;
                        }
                    }
                    byte[] response = Encoding.UTF8.GetBytes(string.Empty);
                    dynamic decrypted = CryptHelper.Decrypt(data);
                    if (Data.Debug)
                    {
                        Data.DebugLogger.WriteLog("Host server: Decrypted text - " + decrypted, Logger.LogType.DEBUG);
                    }
                    CRMRequest request = JsonConvert.DeserializeObject<CRMRequest>(decrypted);
                    switch (request.TaskToRun)
                    {
                        case "OpportunityUPD":
                            try
                            {
                                if (!string.IsNullOrEmpty(request.Name))
                                {
                                    Data.Logger.WriteLog("Host server: Starting opportunity update", Logger.LogType.INFO);
                                    UpdateOwner.UpdateOwners(request.Name);
                                }
                            }
                            catch (Exception ex)
                            {
                                Data.Logger.WriteLog("Host server: Failed Opportunity update start. " + ex.Message, Logger.LogType.ERROR);
                                response = Encoding.UTF8.GetBytes("Failed$");
                            }
                            break;
                        case "AddNewOpportunity":
                            try
                            {
                                if (!string.IsNullOrEmpty(request.Name) && request.Additional != null && request.Additional.Length > 0)
                                {
                                    CreateOpportunity create = new CreateOpportunity();
                                    Data.Logger.WriteLog("Host server: Starting opportunity create", Logger.LogType.INFO);
                                    response = Encoding.UTF8.GetBytes(create.Create(request, OpportunityType.onlineOrder));
                                }
                            }
                            catch (Exception ex)
                            {
                                Data.Logger.WriteLog("Host server: Failed Opportunity create start. " + ex.Message, Logger.LogType.ERROR);
                                response = Encoding.UTF8.GetBytes("Failed$");
                            }
                            break;
                        case "OpenedDashboard":
                            try
                            {
                                if (!string.IsNullOrEmpty(request.Username) && !string.IsNullOrEmpty(request.Name))
                                {
                                    Data.Logger.WriteLog("Host server: Form opened: Username = " + request.Username + "; Form = " + request.Name, Logger.LogType.INFO);
                                    response = Encoding.UTF8.GetBytes("Saved!");
                                }
                            }
                            catch { }
                            break;
                        case "Opportunity_Exist":
                            try
                            {
                                if (!string.IsNullOrEmpty(request.Name))
                                {
                                    Data.Logger.WriteLog("Host server: Starting opportunity exist checking", Logger.LogType.INFO);
                                    ActionResult result = helper.CheckEntityExists(new CRMInput() { Columns = new List<string>() { "name" }, Type = "opportunity", Value = request.Name }, new CRMDAO());
                                    if (result.Success)
                                    {
                                        if (result.Result)
                                        {
                                            response = Encoding.UTF8.GetBytes("Opportunity already exist!");
                                        }
                                        else
                                        {
                                            response = Encoding.UTF8.GetBytes(string.Empty);
                                        }
                                    }
                                    else
                                    {
                                        Data.Logger.WriteLog("Host server: unable to check opportunity exist. " + result.Message, Logger.LogType.ERROR);
                                        response = Encoding.UTF8.GetBytes("Failed!");
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                Data.Logger.WriteLog("Host server: Failed checking opportunity. " + ex.Message, Logger.LogType.ERROR);
                                response = Encoding.UTF8.GetBytes("Failed$");
                            }
                            break;
                        case "lundbeckUPD":
                            try
                            {
                                if (!string.IsNullOrEmpty(request.Name) && request.Additional != null && request.Additional.Length > 0)
                                {
                                    CreateOpportunity create = new CreateOpportunity();
                                    Data.Logger.WriteLog("Host server: Starting opportunity create", Logger.LogType.INFO);
                                    response = Encoding.UTF8.GetBytes(create.Create(request, OpportunityType.onlineOrder));
                                }
                            }
                            catch (Exception ex)
                            {
                                Data.Logger.WriteLog("Host server: Failed Punchout update. " + ex.Message, Logger.LogType.ERROR);
                                response = Encoding.UTF8.GetBytes("Failed$");
                            }

                            break;
                        case "SetLocked":
                            try
                            {
                                Data.Logger.WriteLog("Host server: Locking order " + request.Name, Logger.LogType.INFO);
                                if (!Data.LockedOrders.Any(x => x.OrderName == request.Name))
                                {
                                    Data.LockedOrders.Add(new LockedOrder() { OrderName = request.Name, UserName = request.Username });
                                }
                            }
                            catch (Exception ex)
                            {
                                Data.Logger.WriteLog("Host server: Failed to lock order. " + ex.Message, Logger.LogType.ERROR);
                                response = Encoding.UTF8.GetBytes("Failed$");
                            }

                            break;
                        case "CheckLocked":
                            try
                            {
                                Data.Logger.WriteLog("Host server: Checking lock on order " + request.Name, Logger.LogType.INFO);
                                LockedOrder order = Data.LockedOrders.Find(x => x.OrderName == request.Name);
                                if (order != null)
                                {
                                    response = Encoding.UTF8.GetBytes(order.UserName);
                                }
                            }
                            catch (Exception ex)
                            {
                                Data.Logger.WriteLog("Host server: Failed to check order lock. " + ex.Message, Logger.LogType.ERROR);
                                response = Encoding.UTF8.GetBytes("Failed$");
                            }

                            break;
                        case "ClearLocked":
                            try
                            {
                                Data.Logger.WriteLog("Host server: Clearing lock on order " + request.Name, Logger.LogType.INFO);
                                if (Data.LockedOrders.Any(x => x.OrderName == request.Name))
                                {
                                    Data.LockedOrders.Remove(Data.LockedOrders.Find(x => x.OrderName == request.Name));
                                }
                            }
                            catch (Exception ex)
                            {
                                Data.Logger.WriteLog("Host server: Failed to clear order lock. " + ex.Message, Logger.LogType.ERROR);
                                response = Encoding.UTF8.GetBytes("Failed$");
                            }
                            break;
                        case "ActivateAPI":
                            try
                            {
                                Data.Logger.WriteLog("Host server: Activating API user " + request.Username, Logger.LogType.INFO);
                                ApiUpdate api = new ApiUpdate();
                                response = Encoding.UTF8.GetBytes(api.UpdateApiUser(request.Username, true, null));
                            }
                            catch (Exception ex)
                            {
                                response = Encoding.ASCII.GetBytes("Failed. " + ex.Message);
                            }
                            break;
                        default:
                            Data.Logger.WriteLog("Host server: Unexpected function call: " + request.TaskToRun, Logger.LogType.INFO);
                            response = Encoding.ASCII.GetBytes("Unexpected function call!");
                            break;
                    }
                    try
                    {
                        Client.Send(response);
                    }
                    catch
                    {
                        Data.Logger.WriteLog("Host server: Failed to send response for client - looks like client was disconnected", Logger.LogType.WARNING);
                    }
                    finally
                    {
                        if (Client.Connected)
                        {
                            Client.Shutdown(SocketShutdown.Both);
                            Client.Disconnect(true);
                        }
                        Client.Close(0);
                        Client.Dispose();
                    }
                }
            }
            catch (Exception ex)
            {
                if (!Data.StopHost)
                {
                    Data.Logger.WriteLog("Host server: Error on running. Reason - " + ex.Message, Logger.LogType.FATAL);
                }
                if (Data.Listener.Connected)
                {
                    Data.Listener.Shutdown(SocketShutdown.Both);
                    Data.Listener.Disconnect(true);
                }
                Data.Listener.Close(0);
                Data.Listener.Dispose();
            }
        }
    }
}
