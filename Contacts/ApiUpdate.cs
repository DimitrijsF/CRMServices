using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using DataAccess.DAO;
using DataAccess.Entities;
using DataAccess.Helpers;
using LoggerForServices;
using Microsoft.Xrm.Sdk;
using static Common.CommonObjects;

namespace Contacts
{
    public class ApiUpdate
    {
        public void Update()
        {
            try
            {
                Data.Logger.WriteLog("API Update: Starting check", Logger.LogType.INFO);
                APIUserDAO apiDAO = new APIUserDAO();
                List<APIUser> users = apiDAO.GetListFor1HUpdate();
                if (users.Count > 0)
                {
                    foreach (APIUser user in users)
                    {
                        try
                        {
                            if (string.IsNullOrEmpty(user.Email))
                            {
                                Data.Logger.WriteLog("API update: No email field for " + user.Username + ". Will try with username", Logger.LogType.WARNING);
                                user.Email = user.Username;
                            }
                            if (user.Email.ToLower().Contains(".com"))
                            {
                                continue;
                            }
                            UpdateApiUser(string.Empty, false, user);
                        }
                        catch (Exception ex)
                        {
                            Data.Logger.WriteLog("API Update: Unable to update API user " + user.Username + ". Reason: " + ex.Message, Logger.LogType.WARNING);
                        }
                    }
                    Data.Logger.WriteLog("API Update: Completed check", Logger.LogType.INFO);
                }
                else
                {
                    Data.Logger.WriteLog("API Update: No users found to update", Logger.LogType.INFO);
                }
            }
            catch (Exception ex)
            {
                Data.Logger.WriteLog("API Update: Error while getting user list! Reason: " + ex.Message, Logger.LogType.ERROR);
            }
        }
        public string UpdateApiUser(string username, bool updateActive, APIUser user = null)
        {
            try
            {
                if (updateActive)
                {
                    Data.Logger.WriteLog("API Update: Starting active change", Logger.LogType.INFO);
                }
                APIUserDAO apiDAO = new APIUserDAO();
                CRMDAO crmDAO = new CRMDAO();
                ContactHelper contactHelper = new ContactHelper();
                if (user == null)
                {
                    user = apiDAO.GetByName(username);
                }
                if (user == null)
                {
                    string msg = "API user with username " + username + " not found!";
                    Data.Logger.WriteLog("API Update: " + msg, Logger.LogType.ERROR);
                    return msg;
                }
                Entity contact = crmDAO.GetObjectsByValue(new CRMInput()
                {
                    Logic = Microsoft.Xrm.Sdk.Query.LogicalOperator.Or,
                    Columns = new List<string>() { "emailaddress1", "emailaddress2" },
                    Type = "contact",
                    Value = user.Email
                }).FirstOrDefault();
                if (contact == null)
                {
                    string msg = "Contact with emailaddress1 or emailaddress2 " + user.Username + " not found in CRM!";
                    Data.Logger.WriteLog("API Update: " + msg, Logger.LogType.ERROR);
                    return msg;
                }
                if (contactHelper.IsApiChanges(contact, user))
                {
                    ActionResult result = crmDAO.UpdateEntity(contactHelper.AppliyApiAttributes(contact, user, false));
                    if (result.Success)
                    {
                        Data.Logger.WriteLog("API Update: Contact " + user.Email + " saved", Logger.LogType.INFO);
                    }
                    else
                    {
                        Data.Logger.WriteLog("API Update: Unable to update contact " + user.Email + ". Reason: " + result.Message, Logger.LogType.ERROR);
                    }
                }
                return string.Empty;
            }
            catch(Exception ex)
            {
                Data.Logger.WriteLog("API Update: Changing API user " + username + " active field! Reason: " + ex.Message, Logger.LogType.ERROR);
                return ex.Message;
            }
        }
    }
}
