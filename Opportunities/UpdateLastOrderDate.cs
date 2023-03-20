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

namespace Opportunities
{
    public static class UpdateLastOrderDate
    {
        private class OrdersToUpdate
        {
            public string OrderName { get; set; }
            public DateTime? DeliveryDate { get; set; }
        }

        public static void UpdateOrderDeliveryDate(DateTime? input = null)
        {
            OrderItemDAO itemDAO = new OrderItemDAO();
            OrderDAO orderDAO = new OrderDAO();
            CRMDAO crmDAO = new CRMDAO();
            try
            {
                Data.Logger.WriteLog("DeliveryDates: Starting", Logger.LogType.INFO);
                List<long> orderids = new List<long>();
                if (input == null)
                {
                    orderids = itemDAO.GetByModifiedDate(DateTime.Now.AddDays(-1).Date);
                }
                else
                {
                    orderids = itemDAO.GetByModifiedDate(input.Value);
                }
                if (orderids == null || orderids.Count == 0)
                {
                    Data.Logger.WriteLog("DeliveryDates: No any modified orders found", Logger.LogType.INFO);
                    return;
                }
                Data.Logger.WriteLog("DeliveryDates: Found " + orderids.Count + " modified orders. Starting process.", Logger.LogType.INFO);
                List<OrdersToUpdate> toUpdate = new List<OrdersToUpdate>();
                foreach (long order in orderids)
                {
                    DateTime? maxDelivered = itemDAO.MaxDeliveryDateIfAllDelivered(order);
                    if (maxDelivered.HasValue)
                    {
                        toUpdate.Add(new OrdersToUpdate() { OrderName = orderDAO.GetNameById(order), DeliveryDate = maxDelivered.Value });
                    }
                }
                List<Entity> opps = crmDAO.GetObjectsByList(new CRMInput()
                {
                    Columns = new List<string>() { "name" },
                    Type = "opportunity",
                    Values = toUpdate.Select(x => x.OrderName).ToArray()
                });
                List<string> updated = new List<string>();
                foreach(Entity opp in opps)
                {
                    DateTime? date = toUpdate.Find(x => x.OrderName == opp.Attributes["name"].ToString())?.DeliveryDate;
                    if (date.HasValue)
                    {
                        opp.UpdateValue("sh_orderdeliverydate", date);
                        crmDAO.UpdateEntity(opp);
                        toUpdate.Remove(toUpdate.Find(x => x.OrderName == opp.Attributes["name"].ToString()));
                        Data.Logger.WriteLog("DeliveryDates: Opportunity " + opp.Attributes["name"].ToString() + " updated! New value = " + date, Logger.LogType.INFO);
                        updated.Add(opp.Attributes["name"].ToString());
                    }
                    else
                    {
                        if (!updated.Contains(opp.Attributes["name"].ToString()))
                        {
                            Data.Logger.WriteLog("DeliveryDates: Unable to get delivery date for " + opp.Attributes["name"].ToString() + " opportunity", Logger.LogType.WARNING);
                        }
                    }
                }
                foreach (OrdersToUpdate order in toUpdate)
                {
                    Data.Logger.WriteLog("DeliveryDates: Opportunity " + order.OrderName + " not found in CRM", Logger.LogType.WARNING);
                }
                Data.Logger.WriteLog("DeliveryDates: Finished", Logger.LogType.INFO);
            }
            catch (Exception ex)
            {
                Data.Logger.WriteLog("DeliveryDates: Failed. Reason - " + ex.Message, Logger.LogType.ERROR);
            }
        }
    }
}
