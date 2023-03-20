using Common;
using DataAccess.DAO;
using DataAccess.Entities;
using DataAccess.Helpers;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Common.CommonObjects;

namespace Contacts
{
    public class ContactWorker
    {
        public static void RunContactCheck()
        {
            try
            {
                CRMDAO crmDAO = new CRMDAO();
                ExtendedUserDAO userDAO = new ExtendedUserDAO();
                CRMHelper crmHelper = new CRMHelper();
                ContactHelper contactHelper = new ContactHelper() { dao = crmDAO };
                Data.Logger.WriteLog("Contacts: Starting", LoggerForServices.Logger.LogType.INFO);
                List<ExtendedUser> users = userDAO.GetListByDates(DateTime.Now.AddHours(-3.5)).Where(x => !x.Username.ToLower().Contains(".com")).ToList();
                if (users.Count == 0)
                {
                    Data.Logger.WriteLog("Contacts: No users to update", LoggerForServices.Logger.LogType.INFO);
                    return;
                }

                Data.Logger.WriteLog("Contacts: Found " + users.Count + " users to update", LoggerForServices.Logger.LogType.INFO);
                foreach (ExtendedUser user in users)
                {
                    try
                    {
                        //check if username is correct email form
                        string[] check = user.Username.Split('@');
                        if (check.Length > 1)
                        {
                            if (check.Last().Length < 5 || !check.Last().Contains("."))
                            {
                                Data.Logger.WriteLog("Contacts: Username " + user.Username + " is not valid to process", LoggerForServices.Logger.LogType.INFO);
                                continue;
                            }
                        }
                        else
                        {
                            Data.Logger.WriteLog("Contacts: Username " + user.Username + " is not valid to process", LoggerForServices.Logger.LogType.INFO);
                            continue;
                        }

                        Entity contact = crmDAO.GetObjectsByValue(new CRMInput()
                        {
                            Logic = Microsoft.Xrm.Sdk.Query.LogicalOperator.Or,
                            Columns = new List<string>() { "emailaddress1", "emailaddress2" },
                            Type = "contact",
                            Value = user.Username
                        }).FirstOrDefault();
                        ActionResult result = null;

                        if (contact == null)
                        {
                            result = crmDAO.CreateEntity(contactHelper.ParseFromUser(user, null));
                        }
                        else
                        {
                            result = crmDAO.UpdateEntity(contactHelper.ParseFromUser(user, contact));
                        }

                        if (result.Success)
                        {
                            Data.Logger.WriteLog("Contacts: Contact " + user.Username + " saved", LoggerForServices.Logger.LogType.INFO);
                        }
                        else
                        {
                            Data.Logger.WriteLog("Contacts: Unable to save " + user.Username + " user! Reason - " + result.Message, LoggerForServices.Logger.LogType.WARNING);
                        }
                    }
                    catch (Exception ex)
                    {
                        Data.Logger.WriteLog("Contacts: Unable to process user! Reason - " + ex.Message, LoggerForServices.Logger.LogType.WARNING);
                    }
                }
                Data.Logger.WriteLog("Contacts: Completed", LoggerForServices.Logger.LogType.INFO);
            }
            catch (Exception ex)
            {
                Data.Logger.WriteLog("Contacts: Unable to get users list to check! Reason - " + ex.Message, LoggerForServices.Logger.LogType.ERROR);
            }
        }
    }
}
