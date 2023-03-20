using Common;
using DataAccess.DAO;
using DataAccess.Entities;
using DataAccess.Helpers;
using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Common.CommonObjects;
using static DataAccess.Helpers.EntityHelper;

namespace Opportunities
{
    public class CreateOpportunity
    {
        //Params { {DateTime}[0] order_date, {decimal}[1] price, {string}[2] currency, {string}[3] acc_name, {string}[4] contact, {string}[5] segment, {int}[6] updateProbability, {int}[7] CloseAsWon }
        public string Create(CRMRequest request, OpportunityType type)
        {
            CRMDAO crmDAO = new CRMDAO();
            try
            {
                Entity entity = new Entity("opportunity");
                Entity opportunity = crmDAO.GetObjectsByValue(new CRMInput() { Columns = new List<string>() { "name" }, Type = "opportunity", Value = request.Name }).FirstOrDefault();
                if (opportunity != null)
                {
                    if (string.IsNullOrEmpty(request.Additional[6]) && (string.IsNullOrEmpty(request.Additional[7]) || request.Additional[7] == "0"))
                    {
                        Data.Logger.WriteLog("Create opportunity: opportunity with name " + request.Name + " already exists", LoggerForServices.Logger.LogType.INFO);
                        return "Opportunity already exist!";
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(request.Additional[6]) && request.Additional[6] == "-1")
                        {
                            opportunity.UpdateValue("sh_probability", (int)OpportunityProbability.OrderPlaced);
                            crmDAO.UpdateEntity(opportunity);
                            Data.Logger.WriteLog("Create opportunity: Opportunity with name " + request.Name + " probability updated", LoggerForServices.Logger.LogType.INFO);
                        }
                        if(!string.IsNullOrEmpty(request.Additional[7]) && request.Additional[7] == "-1")
                        {
                            crmDAO.CloseAsWon(opportunity);
                        }
                        return string.Empty;
                    }
                }

                Entity ownerObj = null;
                if (!string.IsNullOrEmpty(request.Username))
                {
                    ownerObj = crmDAO.GetObjectsByValue(new CRMInput() { Type = "systemuser", Columns = new List<string>() { "internalemailaddress" }, Value = request.Username }).FirstOrDefault();
                    if (ownerObj == null)
                    {
                        Data.Logger.WriteLog("Create opportunity: Could not find CRM object \"account\" with name " + request.Username + ". Default owner will be applied", LoggerForServices.Logger.LogType.ERROR);

                    }
                    else
                    {
                        entity.Attributes.Add("ownerid", ownerObj.ToEntityReference());
                    }
                }
                else
                {
                    Data.Logger.WriteLog("Create opportunity: Owner for opportunity " +request.Name + " not specified!", LoggerForServices.Logger.LogType.WARNING);
                }

                if (string.IsNullOrEmpty(request.Additional[3]))
                {
                    Data.Logger.WriteLog("Create opportunity: Account for opportunity " + request.Name + " not specified!", LoggerForServices.Logger.LogType.ERROR);
                    return "Could not create opportunity - Account not specified!";
                }

                Entity account = crmDAO.GetObjectsByValue(new CRMInput() { Type = "account", Columns = new List<string>() { "accountnumber" }, Value = request.Additional[3] }).FirstOrDefault();
                if (account == null)
                {
                    Data.Logger.WriteLog("Create opportunity: Could not find CRM object \"account\" with name " + request.Additional[3], LoggerForServices.Logger.LogType.ERROR);
                    return "Could not find CRM object \"account\" with name " + request.Additional[3];
                }
                entity.Attributes["new_oportunity_type"] = new OptionSetValue((int)type);
                if (type == OpportunityType.onlineOrder)
                {
                    if(string.IsNullOrEmpty(request.Additional[4]))
                    {
                        Data.Logger.WriteLog("Create opportunity: Contact for opportunity not sended. Unable to continue.", LoggerForServices.Logger.LogType.ERROR);
                        return "Contact for opportunity not sended. Unable to continue.";
                    }
                    Entity contact = crmDAO.GetObjectsByValue(new CRMInput()
                    {
                        Logic = Microsoft.Xrm.Sdk.Query.LogicalOperator.Or,
                        Columns = new List<string>() { "emailaddress1", "emailaddress2" },
                        Type = "contact",
                        Value = request.Additional[4]
                    }).FirstOrDefault();
                    if(contact == null)
                    {
                        Data.Logger.WriteLog("Create opportunity: Could not find CRM object \"contact\" with name " + request.Additional[4], LoggerForServices.Logger.LogType.ERROR);
                        return "Contact with name " + request.Additional[4] + " for opportunity not found in crm.";
                    }
                    entity.Attributes["parentcontactid"] = contact.ToEntityReference();
                }
                if(string.IsNullOrEmpty(request.Additional[5]))
                {
                    entity.Attributes["new_market_segment"] = new OptionSetValue(100000099);
                }
                else
                {
                    entity.Attributes["new_market_segment"] = new OptionSetValue(Convert.ToInt32(request.Additional[5]));
                }
                if(!string.IsNullOrEmpty(request.Additional[2]))
                {
                    Entity currency = crmDAO.GetObjectsByValue(new CRMInput() { Columns = new List<string>() { "currencyname" }, Type = "transactioncurrency", Value = request.Additional[2] }).FirstOrDefault();
                    if(currency != null)
                    {
                        entity.Attributes.Add("transactioncurrencyid", currency.ToEntityReference());
                    }
                }
                entity.Attributes.Add("name", request.Name);
                entity.Attributes.Add("statuscode", new OptionSetValue(1));
                entity.Attributes.Add("customerid", account.ToEntityReference());
                entity.Attributes.Add("estimatedvalue", new Money(Convert.ToDecimal(request.Additional[1])));
                entity.Attributes.Add("sh_probability", new OptionSetValue(881990004));
                if (request.Additional.Length > 8)
                {
                    var format = new CultureInfo(request.Additional[8]);
                    entity.Attributes.Add("sh_estimatedorderdate", DateTime.Parse(request.Additional[0], format));
                }
                else
                {
                    entity.Attributes.Add("sh_estimatedorderdate", Convert.ToDateTime(request.Additional[0]));
                }
                entity.Attributes.Add("sh_requestforquotetype", new OptionSetValue(881990000));
                entity.Attributes.Add("sh_quoteadjustmentsrequired", new OptionSetValue(881990000));
                entity.Attributes.Add("stepname", "9-Close");
                crmDAO.CreateEntity(entity);
                Data.Logger.WriteLog("Create opportunity: Opportunity with name " + request.Name + " created", LoggerForServices.Logger.LogType.INFO);
                if (!string.IsNullOrEmpty(request.Additional[7]) && request.Additional[7] == "-1")
                {
                    crmDAO.CloseAsWon(entity);
                }
                return string.Empty;
            }
            catch (Exception ex)
            {
                Data.Logger.WriteLog("Create opportunity: unable to parse opportunity. Reason - " + ex.Message, LoggerForServices.Logger.LogType.ERROR);
                return "Failed!";
            }
        }

    }
}
