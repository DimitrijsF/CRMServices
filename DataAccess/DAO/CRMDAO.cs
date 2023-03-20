using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Tooling.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using LoggerForServices;
using Microsoft.Xrm.Sdk.Query;
using static Common.CommonObjects;

namespace DataAccess.DAO
{
    public class CRMDAO
    {
        private const string C_URL = ""; //url :)
        private const string C_USERNAME = ""; // username :)
        private const string C_PASSWORD = ""; //password :)
        private IOrganizationService Connection { get; set; } = null;
        private void CheckConnection()
        {
            if(Connection == null)
            {
                CreateConnection();
            }
        }
        private void CreateConnection()
        {
            try
            {
                string connection = $@"
    Url = {C_URL};
    AuthType = OAuth;
    UserName = {C_USERNAME};
    Password = {C_PASSWORD};
    AppId = ;
    RedirectUri = ;
    LoginPrompt=Auto;
    RequireNewInstance = True";
                CrmServiceClient oMSCRMConn = new CrmServiceClient(connection);
                Connection = oMSCRMConn.OrganizationWebProxyClient != null ? oMSCRMConn.OrganizationWebProxyClient : (IOrganizationService)oMSCRMConn.OrganizationServiceProxy;
                if (Connection != null)
                {
                    Guid userid = ((WhoAmIResponse)Connection.Execute(new WhoAmIRequest())).UserId;
                    if (userid != Guid.Empty)
                    {
                        Data.Logger.WriteLog("CRM: Connection Successful", Logger.LogType.INFO);
                    }
                }
                else
                {
                    Data.Logger.WriteLog("CRM: Connection failed", Logger.LogType.FATAL);
                }
            }
            catch (Exception ex)
            {
                Data.Logger.WriteLog("CRM: Connection failed. Reason - " + ex.Message, Logger.LogType.FATAL);
            }
        }
        public List<Entity> GetObjectsByValue(CRMInput input)
        {
            try
            {
                CheckConnection();
                ColumnSet columns = new ColumnSet();
                columns.AllColumns = true;
                var query = new QueryExpression(input.Type)
                {
                    ColumnSet = columns
                };
                FilterExpression filter = new FilterExpression(input.Logic);
                foreach(string column in input.Columns)
                {
                    filter.AddCondition(column, ConditionOperator.Like, "%" + input.Value + "%");
                }
                query.Criteria = filter;
                EntityCollection result = Connection.RetrieveMultiple(query);
                return result.Entities.ToList();
            }
            catch(Exception ex)
            {
                Data.Logger.WriteLog("CRM: Unable to get " + input.Type + " entity by List filters. Reason - " + ex.Message, Logger.LogType.WARNING);
            }
            return null;
        }
        public List<Entity> GetObjectsByList(CRMInput input)
        {
            try
            {
                CheckConnection();
                ColumnSet columns = new ColumnSet();
                columns.AllColumns = true;
                var query = new QueryExpression(input.Type)
                {
                    ColumnSet = columns
                };
                FilterExpression filter = new FilterExpression(input.Logic);
                foreach(string column in input.Columns)
                {
                    filter.AddCondition(column, ConditionOperator.In, input.Values);
                }
                query.Criteria = filter;
                EntityCollection result = Connection.RetrieveMultiple(query);
                return result.Entities.ToList();
            }
            catch (Exception ex)
            {
                Data.Logger.WriteLog("CRM: Unable to get " + input.Type + " entity by List filters. Reason - " + ex.Message, Logger.LogType.WARNING);
            }
            return null;
        }
        public ActionResult UpdateEntity(object input)
        {
            ActionResult result = new ActionResult();
            try
            {
                CheckConnection();
                Connection.Update((Entity)input);
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = ex.Message;
            }
            return result;
        }
        public ActionResult CreateEntity(object input)
        {
            ActionResult result = new ActionResult();
            try
            {
                CheckConnection();
                Connection.Create((Entity)input);
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = ex.Message;
            }
            return result;
        }
        public ActionResult CloseAsWon(Entity input)
        {
            ActionResult result = new ActionResult();
            try
            {
                CheckConnection();
                Entity oppClose = new Entity("opportunityclose");
                oppClose.Attributes.Add("opportunityid", input.ToEntityReference());
                oppClose.Attributes.Add("statuscode", 1);
                WinOpportunityRequest req = new WinOpportunityRequest()
                {
                    OpportunityClose = oppClose,
                    RequestName = "WinOpportunity",
                    Status = new OptionSetValue(3)
                };
                var response = (WinOpportunityResponse)Connection.Execute(req);
            }
            catch(Exception ex)
            {
                result.Success = false;
                result.Message = ex.Message;
            }
            return result;
        }
    }
}
