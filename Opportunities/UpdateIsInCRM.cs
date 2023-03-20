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
using static Common.CommonObjects;

namespace Opportunities
{
    public class UpdateIsInCRM
    {
        public static void Update(object inputArray = null)
        {
            OrderDAO orderDAO = new OrderDAO();
            CRMDAO crmDAO = new CRMDAO();
            try
            {
                if (inputArray == null)
                {
                    Data.Logger.WriteLog("IsInCRM: Starting", Logger.LogType.INFO);
                    List<Order> orders = orderDAO.GetOrdersNotInCRM();
                    if (orders.Count == 0)
                    {
                        Data.Logger.WriteLog("IsInCRM: No order found", Logger.LogType.INFO);
                        return;
                    }
                    List<Entity> opps = crmDAO.GetObjectsByList(new CRMInput() { Columns = new List<string>() { "name" }, Type = "opportunity", Values = orders.Select(x => x.MolportOrderNumber).Distinct().ToArray() });
                    foreach (Order order in orders)
                    {
                        Entity opp = opps.Find(x => x.Attributes["name"].ToString() == order.MolportOrderNumber);
                        if (opp == null)
                        {
                            order.IsInCRM = 0;
                        }
                        else
                        {
                            if (opp.Attributes.ContainsKey("new_oportunity_type"))
                            {
                                OptionSetValue val = (OptionSetValue)opp.Attributes["new_oportunity_type"];
                                if (val.Value.ToString() == ((int)OpportunityType.purchaseOrder).ToString())
                                {
                                    order.IsInCRM = -1;
                                }
                                else
                                {
                                    order.IsInCRM = 0;
                                }
                            }
                            else
                            {
                                order.IsInCRM = 0;
                            }
                        }
                        orderDAO.Update(order);
                        Data.Logger.WriteLog("IsInCRM: order " + order.MolportOrderNumber + " updated. New value = " + order.IsInCRM, Logger.LogType.INFO);
                    }
                    Data.Logger.WriteLog("IsInCRM: Completed", Logger.LogType.INFO);
                }
                else
                {
                    string[] data = (string[])inputArray;
                    Order order = orderDAO.GetByName(data[0]);
                    order.IsInCRM = Convert.ToInt16(data[1]);
                    orderDAO.Update(order);
                    Data.Logger.WriteLog("IsInCRM: Updated", Logger.LogType.INFO);
                }
            }
            catch (Exception ex)
            {
                Data.Logger.WriteLog("IsInCRM: Failed. Reason - " + ex.Message, Logger.LogType.ERROR);
            }
        }
    }
}
