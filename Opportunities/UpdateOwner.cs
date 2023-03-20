using Common;
using DataAccess.DAO;
using DataAccess.Entities;
using LoggerForServices;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Opportunities
{
    public static class UpdateOwner
    {
        public static void UpdateOwners(string input)
        {
            OrderDAO orderDAO = new OrderDAO();
            CRMDAO crmDAO = new CRMDAO();
            
            try
            {
                Data.Logger.WriteLog("Owners update: Starting", Logger.LogType.INFO);
                Order order = orderDAO.GetByName(input);
                
                Entity opportunity = crmDAO.GetObjectsByValue(new CommonObjects.CRMInput() { Columns = new List<string>() { "name" }, Type = "opportunity", Value = input }).FirstOrDefault();
                if (opportunity == null)
                {
                    Data.Logger.WriteLog("Owners update: No opportunity with name " + input + " found", Logger.LogType.WARNING);
                    return;
                }

                CheckOwnerId(opportunity, crmDAO, order, orderDAO, input);

                CheckParentAccountId(opportunity, crmDAO, order.Id, input);

            }
            catch (Exception ex)
            {
                Data.Logger.WriteLog("Owners update: Unable to update. Reason - " + ex.Message, Logger.LogType.ERROR);
            }
        }

        private static void CheckOwnerId(Entity opportunity, CRMDAO crmDAO, Order order, OrderDAO orderDAO, string input)
        {
            UserDAO userDAO = new UserDAO();
            if (!opportunity.Attributes.ContainsKey("ownerid"))
            {
                Data.Logger.WriteLog("Owners update: Opportunity with name " + input + " does not contains Account Manager field", Logger.LogType.WARNING);
                return;
            }

            Entity accManager = crmDAO.GetObjectsByValue(new CommonObjects.CRMInput() { Type = "systemuser", Columns = new List<string>() { "fullname" }, Value = ((EntityReference)opportunity.Attributes["ownerid"]).Name }).FirstOrDefault();
            if (accManager == null)
            {
                Data.Logger.WriteLog("Owners update: unable to get opportunity " + input + " owner contact by name " + ((EntityReference)opportunity.Attributes["ownerid"]).Name, Logger.LogType.WARNING);
                return;
            }

            if (!accManager.Attributes.Contains("internalemailaddress"))
            {
                Data.Logger.WriteLog("Owners update: Contact with name " + accManager.Attributes["emailaddress1"] + " does not contains internalemailaddress field", Logger.LogType.WARNING);
                return;
            }

            User user = userDAO.GetByUserName(accManager.Attributes["internalemailaddress"].ToString());
            if (user == null)
            {
                Data.Logger.WriteLog("Owners update: User with name " + accManager.Attributes["internalemailaddress"].ToString() + " not found in database", Logger.LogType.WARNING);
                return;
            }

            order.AccountManagerId = user.Id;
            orderDAO.Update(order);
            Data.Logger.WriteLog("Owners update: order " + input + " updated", Logger.LogType.INFO);
        }

        private static void CheckParentAccountId(Entity opportunity, CRMDAO crmDAO, long orderId, string input)
        {
            InvoiceDAO invoiceDAO = new InvoiceDAO();
            List<Invoice> invoices = invoiceDAO.GetByOrderID(orderId);

            if (!opportunity.Attributes.ContainsKey("parentaccountid"))
            {
                Data.Logger.WriteLog("Owners update: Opportunity with name " + input + " does not contains Parent account field ", Logger.LogType.WARNING);
                return;
            }

            Entity parent = crmDAO.GetObjectsByValue(new CommonObjects.CRMInput() { Type = "account", Columns = new List<string>() { "name" }, Value = ((EntityReference)opportunity.Attributes["parentaccountid"]).Name }).FirstOrDefault();
            if (parent == null)
            {
                Data.Logger.WriteLog("Owners update: unable to get opportunity " + input + " parent account by name " + ((EntityReference)opportunity.Attributes["ownerid"]).Name, Logger.LogType.WARNING);
                return;
            }

            if (!parent.Attributes.Contains("emailaddress1"))
            {
                Data.Logger.WriteLog("Owners update: Parent account of opportunity " + input + " does not contains emailaddress1 field", Logger.LogType.WARNING);
                return;
            }

            invoices.ForEach(x => x.CrmOpportunityOwner = parent.Attributes["emailaddress1"].ToString());
            foreach (Invoice invoice in invoices)
            {
                try
                {
                    invoice.CrmOpportunityOwner = parent.Attributes["emailaddress1"].ToString();
                    invoiceDAO.Update(invoice);
                    Data.Logger.WriteLog("Owners update: invoice " + invoice.InvoiceNumber + " updated", Logger.LogType.INFO);
                }
                catch (Exception ex)
                {
                    Data.Logger.WriteLog("Owners update: invoice " + invoice.InvoiceNumber + " update failed. Reason - " + ex.Message, Logger.LogType.WARNING);
                }
            }
        }
    }
}
