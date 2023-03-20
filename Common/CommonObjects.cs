using Microsoft.Xrm.Sdk.Query;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class CommonObjects
    {
        public class CRMInput
        {
            public string Type { get; set; }
            public List<string> Columns { get; set; }
            public string Value { get; set; }
            public string[] Values { get; set; }
            public LogicalOperator Logic { get; set; } = LogicalOperator.And;
        }
        public class ActionResult
        {
            public bool Success { get; set; } = true;
            public bool Result { get; set; }
            public string Message { get; set; } = string.Empty;
            public object Object { get; set; } = null;
        }
        public class UpdateAttribute
        {
            public string Attribute { get; set; }
            public string UserField { get; set; }
        }
        public class OrderSearchCriteria
        {
            public short? IsInCRM { get; set; }
        }
        public class CRMRequest
        {
            public string TaskToRun { get; set; }
            public string Name { get; set; }
            public string Username { get; set; }
            public string[] Additional { get; set; }
        }
        public class TaskToRun
        {
            [JsonProperty("TaskToExecute")]
            public string TaskToExecute { get; set; }
            [JsonProperty("Param")]
            public dynamic Param { get; set; }
            [JsonProperty("Type")]
            public int Type { get; set; }
            public TaskToRun() { }
        }
        public class LockedOrder
        {
            public string OrderName { get; set; }
            public string UserName { get; set; }
        }
        public enum OpportunityType
        {
            onlineOrder = 100000010,
            purchaseOrder = 100000020
        }
        public enum OpportunityProbability
        {
            IdentifiedOpportunity = 881990000,
            QualifiedOpportunity = 881990001,
            SolutionProposed = 881990002,
            SolutionAccepted = 881990003,
            OrderPlaced = 881990004
        }
    }
}
