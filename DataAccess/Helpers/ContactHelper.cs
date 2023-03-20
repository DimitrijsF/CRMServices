using Common;
using DataAccess.DAO;
using DataAccess.Entities;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static Common.CommonObjects;

namespace DataAccess.Helpers
{
    public class ContactHelper
    {
        public CRMDAO dao = null;
        public object ParseFromUser(ExtendedUser user, object contact)
        {
            try
            {
                if (contact == null)
                {
                    contact = CreateContact(user);
                }
                PropertyInfo[] fields = typeof(ExtendedUser).GetProperties();
                foreach (UpdateAttribute field in Data.ContactFields)
                {
                    PropertyInfo userField = fields.Where(x => x.Name.ToLower() == field.UserField.ToLower()).FirstOrDefault();
                    if (userField != null)
                    {
                        if (userField.GetValue(user) != null)
                        {
                            if (((Entity)contact).Attributes.Contains(field.Attribute))
                            {
                                if (field.Attribute == "new_listsearchitemacountinlast3months" || field.Attribute == "sh_listsearchusecountinlast3months")
                                {
                                    ((Entity)contact).Attributes[field.Attribute] = userField.GetValue(user).ToString();
                                }
                                else
                                {
                                    ((Entity)contact).Attributes[field.Attribute] = userField.GetValue(user);
                                }
                            }
                            else
                            {
                                if (field.Attribute == "new_listsearchitemacountinlast3months" || field.Attribute == "sh_listsearchusecountinlast3months")
                                {
                                    ((Entity)contact).Attributes.Add(field.Attribute, userField.GetValue(user).ToString());
                                }
                                else
                                {
                                    ((Entity)contact).Attributes.Add(field.Attribute, userField.GetValue(user));
                                }

                            }
                        }
                    }
                    else
                    {
                        Data.Logger.WriteLog("Contacts: Unable to get " + field.UserField + " field in user object", LoggerForServices.Logger.LogType.ERROR);
                    }
                }
                bool userDeleted = false;
                if (user.Deleted == "Y")
                {
                    userDeleted = true;
                    ((Entity)contact).UpdateValue("sh_webaccesstermiateddate", user.Modified);
                }
                ((Entity)contact).UpdateValue("new_registered_user", !userDeleted);
                ((Entity)contact).UpdateValue("fullname", user.Firstname + " " + user.Lastname);
                bool isactive = true;
                if (user.Isactive == 0)
                {
                    isactive = false;
                }
                 ((Entity)contact).UpdateValue("new_activated_registration", isactive);
            }
            catch (Exception ex)
            {
                contact = null;
                Data.Logger.WriteLog("Contacts: Unable to parse CRM contact from user. Reason - " + ex.Message, LoggerForServices.Logger.LogType.ERROR);
            }
            return contact;
        }
        private Entity CreateContact(ExtendedUser user)
        {
            Entity contact = new Entity("contact");
            contact.LogicalName = "contact";
            contact.UpdateValue("emailaddress1", user.Username);
            contact.UpdateValue("jobtitle", "Research");
            contact.UpdateValue("new_job_functional_area", new OptionSetValue() { Value = 100000010 });
            {
                EntityReference value = (EntityReference)dao.GetObjectsByValue(new CRMInput() { Type = "contact", Columns =  new List<string>() { "emailaddress1" }, Value = user.Username.Split('@').Last() })?
                    .Where(x => x.Attributes.Contains("parentcustomerid"))?.FirstOrDefault()?.Attributes["parentcustomerid"];
                if (value == null)
                {
                    value = dao.GetObjectsByValue(new CRMInput() { Type = "account", Columns = new List<string>() { "name" }, Value = "Unknown Account (USE IT)" }).FirstOrDefault()?.ToEntityReference();
                }
                contact.UpdateValue("parentcustomerid", value);
            }
            {
                Entity value = dao.GetObjectsByValue(new CRMInput() { Type = "systemuser", Columns = new List<string>() { "fullname" }, Value = "system" }).FirstOrDefault();
                if (value == null)
                {
                    return null;
                }
                contact.UpdateValue("ownerid", value.ToEntityReference());
            }
            return contact;
        }
        public bool IsApiChanges(Entity input, APIUser user)
        {
            if(user.Active == -1)
            {
                if(!input.Attributes.Contains("new_api_access") || !Convert.ToBoolean(input.Attributes["new_api_access"]))
                { 
                    return true;
                }
                if (!input.Attributes.Contains("new_api_given_date") || Convert.ToDateTime(input.Attributes["new_api_given_date"]).Date != user.Modified.Value.Date)
                {
                    return true;
                }
                if (input.Attributes.Contains("new_apilastaccessdatedate") && user.LastRequested.HasValue && Convert.ToDateTime(input.Attributes["new_apilastaccessdatedate"]).Date != user.LastRequested.Value.Date) 
                {
                    return true;
                }
                if (!input.Attributes.Contains("new_api_last_12_week_activity") || input.Attributes["new_api_last_12_week_activity"].ToString() != user.ActiveWeeks.ToString())
                {
                    return true;
                }
                if (!input.Attributes.Contains("sh_apitotalrequestcountinlast12weeksnumber") || input.Attributes["sh_apitotalrequestcountinlast12weeksnumber"].ToString() != user.RequestCount.ToString())
                {
                    return true;
                }
                if (input.Attributes.Contains("sh_apiaccessterminateddate"))
                {
                    return true;
                }
            }
            else
            {
                if (!input.Attributes.Contains("new_api_access") || Convert.ToBoolean(input.Attributes["new_api_access"]))
                {
                    return true;
                }
                if ((!input.Attributes.Contains("sh_apiaccessterminateddate") && user.Modified.Value.Date != user.Created.Value.Date) || 
                    (input.Attributes.Contains("sh_apiaccessterminateddate") && Convert.ToDateTime(input.Attributes["sh_apiaccessterminateddate"]).Date != user.Modified.Value.Date) ||
                    (input.Attributes.Contains("sh_apiaccessterminateddate") && user.Modified.Value.Date == user.Created.Value.Date))
                {
                    return true;
                }
            }
            return false;
        }
        public Entity AppliyApiAttributes(Entity input, APIUser user, bool updateActive)
        {
            if (user.Active == -1)
            {
                if (updateActive)
                {
                    input.UpdateValue("new_api_access", true);
                    input.UpdateValue("new_api_given_date",DateTime.Now.Date);
                }
                input.UpdateValue("new_apilastaccessdatedate", user.LastRequested);
                input.UpdateValue("new_api_last_12_week_activity", user.ActiveWeeks.ToString());
                input.UpdateValue("sh_apitotalrequestcountinlast12weeksnumber", user.RequestCount.ToString());
                input.UpdateValue("sh_apiaccessterminateddate", null);
            }
            else
            {
                input.UpdateValue("new_api_access", false);
                input.UpdateValue("new_api_given_date", null);
                input.UpdateValue("new_apilastaccessdatedate", null);
                input.UpdateValue("new_api_last_12_week_activity", null);
                input.UpdateValue("sh_apitotalrequestcountinlast12weeksnumber", null);
                if (user.Created.Value.Date != user.Modified.Value.Date)
                {
                    input.UpdateValue("sh_apiaccessterminateddate", user.Modified);
                }
                else
                {
                    input.UpdateValue("sh_apiaccessterminateddate", null);
                }
            }
            return input;
        }
        public bool IsFtpChanges(Entity input, FTPUser user)
        {
            if (user.Active == -1)
            {
                if (!input.Attributes.Contains("new_ftp_access") || !Convert.ToBoolean(input.Attributes["new_ftp_access"]))
                {
                    return true;
                }
                if (!input.Attributes.Contains("new_ftp_given_date") || Convert.ToDateTime(input.Attributes["new_ftp_given_date"]).Date != user.Modified.Value.Date)
                {
                    return true;
                }
                if (!string.IsNullOrEmpty(user.FTPUsername) && (!input.Attributes.Contains("new_ftp_username") || input.Attributes["new_ftp_username"].ToString() != user.FTPUsername))
                {
                    return true;
                }
                if (user.LastActivity != null && (!input.Attributes.Contains("new_ftp_last_accessed") || Convert.ToDateTime(input.Attributes["new_ftp_last_accessed"]) != user.LastActivity))
                {
                    return true;
                }
                if (!input.Attributes.Contains("sh_last6monthsactivitiy") || input.Attributes["sh_last6monthsactivitiy"].ToString() != user.ActiveMonth.ToString())
                {
                    return true;
                }
                if (!input.Attributes.Contains("sh_last6monthfiledownloads") || input.Attributes["sh_last6monthfiledownloads"].ToString() != user.RequestCount.ToString())
                {
                    return true;
                }
                if (input.Attributes.Contains("sh_ftpaccessterminateddate"))
                {
                    return true;
                }
            }
            else
            {
                if (!input.Attributes.Contains("new_ftp_access") || Convert.ToBoolean(input.Attributes["new_ftp_access"]))
                {
                    return true;
                }
                if (!input.Attributes.Contains("sh_ftpaccessterminateddate") || Convert.ToDateTime(input.Attributes["sh_ftpaccessterminateddate"]).Date != user.Modified.Value.Date)
                {
                    return true;
                }
            }
            return false;
        }
        public Entity ApplyFtpAttributes(Entity input, FTPUser user)
        {
            if (user.Active == -1)
            {
                input.UpdateValue("new_ftp_access", true);

                if (!string.IsNullOrEmpty(user.FTPUsername))
                {
                    input.UpdateValue("new_ftp_username", user.FTPUsername);
                }
                if (user.Modified.HasValue)
                {
                    input.UpdateValue("new_ftp_given_date", user.Modified);
                }
                if (user.LastActivity.HasValue)
                {
                    input.UpdateValue("new_ftp_last_accessed", user.LastActivity);
                }
                input.UpdateValue("sh_last6monthsactivitiy", user.ActiveMonth.ToString());
                input.UpdateValue("sh_last6monthfiledownloads", user.RequestCount.ToString());
                if (input.Attributes.Contains("sh_ftpaccessterminateddate"))
                {
                    input.Attributes["sh_ftpaccessterminateddate"] = null;
                }
            }
            else
            {
                input.UpdateValue("new_ftp_access", false);
                input.UpdateValue("sh_ftpaccessterminateddate", user.Modified);
                if (input.Attributes.Contains("new_ftp_username"))
                {
                    input.Attributes["new_ftp_username"] = null;
                }
                if (input.Attributes.Contains("new_ftp_given_date"))
                {
                    input.Attributes["new_ftp_given_date"] = null;
                }
                if (input.Attributes.Contains("new_ftp_last_accessed"))
                {
                    input.Attributes["new_ftp_last_accessed"] =null;
                }
                if (input.Attributes.Contains("sh_last6monthsactivitiy"))
                {
                    input.Attributes["sh_last6monthsactivitiy"] = null;
                }
                if (input.Attributes.Contains("sh_last6monthfiledownloads"))
                {
                    input.Attributes["sh_last6monthfiledownloads"] = null;
                }
            }
            return input;
        }
    }
}
