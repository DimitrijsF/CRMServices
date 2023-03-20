using Common;
using DataAccess.DAO;
using DataAccess.Entities;
using DataAccess.Helpers;
using LoggerForServices;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Common.CommonObjects;

namespace Contacts
{
    public class FtpUpdate
    {
        public static void Update()
        {
            try
            {
                Data.Logger.WriteLog("FTP Update: Starting", Logger.LogType.INFO);
                CRMDAO crmDAO = new CRMDAO();
                FTPUserDAO ftpDAO = new FTPUserDAO();
                UserDAO userDAO = new UserDAO();
                ContactHelper contactHelper = new ContactHelper();
                List<FTPUser> users = ftpDAO.GetDataFromOracle();
                List<FTPUserMS> usersMs = ftpDAO.GetDataFromMs();
                foreach(FTPUserMS ms in usersMs)
                {
                    try
                    {
                        FTPUser user = users.Find(x => x.Username == ms.FTPUsername);
                        if(user == null)
                        {
                            user = ftpDAO.GetByUsername(ms.FTPUsername);
                        }
                        else
                        {
                            users.Remove(user);
                        }
                        if(user == null)
                        {
                            Data.Logger.WriteLog("FTP Update: Unable to find FTP user with username " + ms.FTPUsername, Logger.LogType.ERROR);
                            continue;
                        }
                        if (user.Email.ToLower().Contains(".com"))
                        {
                            continue;
                        }

                        user.MergeWithMS(ms);
                        Entity contact = crmDAO.GetObjectsByValue(new CRMInput()
                        {
                            Logic = Microsoft.Xrm.Sdk.Query.LogicalOperator.Or,
                            Columns = new List<string>() { "emailaddress1", "emailaddress2" },
                            Type = "contact",
                            Value = user.Email
                        }).FirstOrDefault();

                        if (contact == null)
                        {
                            Data.Logger.WriteLog("FTP Update: Contact with emailaddress1 or emailaddress2 " + user.Email + " not found in CRM!", Logger.LogType.ERROR);
                            continue;
                        }

                        if (contactHelper.IsFtpChanges(contact, user))
                        {
                            ActionResult result = crmDAO.UpdateEntity(contactHelper.ApplyFtpAttributes(contact, user));
                            if (result.Success)
                            {
                                Data.Logger.WriteLog("FTP Update: Contact " + user.Email + " saved", Logger.LogType.INFO);
                            }
                            else
                            {
                                Data.Logger.WriteLog("FTP Update: Unable to update contact " + user.Email + ". Reason: " + result.Message, Logger.LogType.ERROR);
                            }
                        }
                    }
                    catch(Exception ex)
                    {
                        Data.Logger.WriteLog("FTP Update: Unable to process FTP user " + ms.FTPUsername + "! Reason: " + ex.Message, Logger.LogType.ERROR);
                    }
                }
                if(users.Count > 0)
                {
                    foreach(FTPUser user in users)
                    {
                        try
                        {
                            if (user.Email.ToLower().Contains(".com"))
                            {
                                continue;
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
                                Data.Logger.WriteLog("FTP Update: Contact with emailaddress1 or emailaddress2 " + user.Email + " not found in CRM!", Logger.LogType.ERROR);
                                continue;
                            }

                            if (contactHelper.IsFtpChanges(contact, user))
                            {
                                ActionResult result = crmDAO.UpdateEntity(contactHelper.ApplyFtpAttributes(contact, user));
                                if (result.Success)
                                {
                                    Data.Logger.WriteLog("FTP Update: Contact " + user.Email + " saved", Logger.LogType.INFO);
                                }
                                else
                                {
                                    Data.Logger.WriteLog("FTP Update: Unable to update contact " + user.Email + ". Reason: " + result.Message, Logger.LogType.ERROR);
                                }
                            }
                        }
                        catch(Exception ex)
                        {
                            Data.Logger.WriteLog("FTP Update: Unable to process user with email " + user.Email + ". Reason - " + ex.Message, Logger.LogType.ERROR);
                        }
                    }
                }
                Data.Logger.WriteLog("FTP Update: Completed", Logger.LogType.INFO);
            }
            catch (Exception ex)
            {
                Data.Logger.WriteLog("FTP Update: Error while getting user list! Reason: " + ex.Message, Logger.LogType.ERROR);
            }
        }
    }
}
