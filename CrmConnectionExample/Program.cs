using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using Microsoft.Xrm.Tooling.Connector;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace CrmConnectionExample
{
    class Program
    {
        static void Main(string[] args)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["CRM"].ConnectionString;

            CrmServiceClient crmService = new CrmServiceClient(connectionString); // This might take a moment

            string uniqueName = $"Test account - {Guid.NewGuid().ToString("N")}";

            // Create
            Entity newAccount = new Entity("account");
            newAccount["name"] = uniqueName;

            crmService.Create(newAccount); // This returns the ID of the created record

            // Read
            QueryExpression query = new QueryExpression("account");
            query.ColumnSet = new ColumnSet("accountid", "name");
            query.Criteria.AddCondition("name", ConditionOperator.Equal, uniqueName);

            EntityCollection result = crmService.RetrieveMultiple(query);

            // Update
            if(result.Entities.Count > 0)
            {
                Entity foundAccount = result.Entities[0];

                Entity accountToUpdate = new Entity("account");
                accountToUpdate.Id = foundAccount.Id;
                accountToUpdate["name"] = (string) foundAccount["name"] + " - updated";

                crmService.Update(accountToUpdate);
            }

            // Delete
            if (result.Entities.Count > 0)
            {
                Entity foundAccount = result.Entities[0];

                crmService.Delete("account", foundAccount.Id);
            }
        }
    }
}
