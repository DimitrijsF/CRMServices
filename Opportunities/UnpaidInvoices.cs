using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using LoggerForServices;
using DataAccess.DAO;
using DataAccess.Entities;
using Microsoft.Xrm.Sdk;

namespace Opportunities
{
    public class UnpaidInvoices
    {
        public static void Update()
        {
            try
            {
                CRMDAO crmDAO = new CRMDAO();
                OrderDAO orderDAO = new OrderDAO();
                InvoiceDAO invoiceDAO = new InvoiceDAO();
                UnpaidInvoiceDAO unpaidDAO = new UnpaidInvoiceDAO();
                Data.Logger.WriteLog("Unpaid invoices: Starting", Logger.LogType.INFO);
                List<UnpaidInvoice> invoices = unpaidDAO.GetList().Where(x=> x.InvoiceDueDate < DateTime.Now).ToList();
                foreach(UnpaidInvoice invoice in invoices)
                {
                    Invoice inv = invoiceDAO.GetById(invoice.Id);
                    Order order = orderDAO.GetById(inv.OrderId);
                    Entity opportunity = crmDAO.GetObjectsByValue(new CommonObjects.CRMInput() { Type = "opportunity", Columns = new List<string>() { "name" }, Value = order.MolportOrderNumber }).FirstOrDefault();
                    if(opportunity == null)
                    {
                        Data.Logger.WriteLog("Unpaid invoices: Opportunity with name " + order.MolportOrderNumber + " does not exist in CRM", Logger.LogType.WARNING);
                        continue;
                    }

                    if (opportunity.Attributes.Contains("sh_isoverdueinvoice"))
                    {
                        opportunity.Attributes["sh_isoverdueinvoice"] = true;
                    }
                    else
                    {
                        opportunity.Attributes.Add(new KeyValuePair<string, object>("sh_isoverdueinvoice", true));
                    }

                    if (opportunity.Attributes.Contains("sh_firstoverdueinvoiceduedate30days"))
                    {
                        opportunity.Attributes["sh_firstoverdueinvoiceduedate30days"] = invoice.InvoiceDueDate?.AddDays(30);
                    }
                    else
                    {
                        opportunity.Attributes.Add(new KeyValuePair<string, object>("sh_firstoverdueinvoiceduedate30days", invoice.InvoiceDueDate?.AddDays(30)));
                    }
                    crmDAO.UpdateEntity(opportunity);
                }
                Data.Logger.WriteLog("Unpaid invoices: Opportunity update completed", Logger.LogType.INFO);
                foreach(long billId in invoices.Select(x=> x.BillingOrganizationId).Distinct())
                {
                    Entity account = crmDAO.GetObjectsByValue(new CommonObjects.CRMInput() { Columns = new List<string>() { "accountnumber" }, Type = "account", Value = invoices.Find(x=> x.BillingOrganizationId == billId).BillingCode.ToString() }).FirstOrDefault();
                    if(account == null)
                    {
                        Data.Logger.WriteLog("Unpaid invoices: Account with name " + invoices.Find(x => x.BillingOrganizationId == billId).BillingOrganizationName + " does not exist in CRM", Logger.LogType.WARNING);
                        continue;
                    }

                    if (account.Attributes.Contains("sh_isoverdueinvoice"))
                    {
                        account.Attributes["sh_isoverdueinvoice"] = true;
                    }
                    else
                    {
                        account.Attributes.Add(new KeyValuePair<string, object>("sh_isoverdueinvoice", true));
                    }

                    if (account.Attributes.Contains("sh_firstoverdueinvoiceduedate30days"))
                    {
                        account.Attributes["sh_firstoverdueinvoiceduedate30days"] = invoices.Where(x => x.BillingOrganizationId == billId).Min(x => x.InvoiceDueDate)?.AddDays(30);
                    }
                    else
                    {
                        account.Attributes.Add(new KeyValuePair<string, object>("sh_firstoverdueinvoiceduedate30days", invoices.Where(x => x.BillingOrganizationId == billId).Min(x => x.InvoiceDueDate)?.AddDays(30)));
                    }
                }
                Data.Logger.WriteLog("Unpaid invoices: Account update completed", Logger.LogType.INFO);
            }
            catch(Exception ex)
            {
                Data.Logger.WriteLog("Unpaid invoices: Check failed. Reason - " + ex.Message, Logger.LogType.ERROR);
            }
        }
    }
}
