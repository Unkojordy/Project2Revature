using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System.ServiceModel;
namespace Plugin
{
    public class AccountPostUpdate: IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {

            // Obtain the tracing service
            ITracingService tracingService =
            (ITracingService)serviceProvider.GetService(typeof(ITracingService));

            // Obtain the execution context from the service provider.  
            IPluginExecutionContext context = (IPluginExecutionContext)
                serviceProvider.GetService(typeof(IPluginExecutionContext));

            // The InputParameters collection contains all the data passed in the message request.  
            if (context.InputParameters.Contains("Target") &&
                context.InputParameters["Target"] is Entity)
            {
                // Obtain the target entity from the input parameters.  
                Entity account = (Entity)context.InputParameters["Target"];

                // Obtain the organization service reference which you will need for  
                // web service calls.  
                IOrganizationServiceFactory serviceFactory =
                    (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
                IOrganizationService service = serviceFactory.CreateOrganizationService(context.UserId);

                try
                {

                    string city = account.Attributes["address1_city"].ToString();
                    //query contacts 
                    QueryExpression query = new QueryExpression("contact");

                    query.ColumnSet.AddColumn("address1_city");
                    query.Criteria.AddCondition("parentcustomerid", ConditionOperator.Equal, account.Id);


                    //using queryattribute

                    QueryByAttribute query2 = new QueryByAttribute("contact");
                    query2.ColumnSet.AddColumn("address1_city");
                    query2.AddAttributeValue("parentcustomerid", account.Id);

                    EntityCollection collection = service.RetrieveMultiple(query);
               
                    foreach (Entity item in collection.Entities)
                    {
                        if (item.Attributes.Contains("address1_city"))
                            {

                            item.Attributes["address1_city"] = city;
                        }
                        else
                        {
                            item.Attributes.Add("address1_city", city);
                        }
                        service.Update(item);
                    }
                    /*string desc = String.Empty;
                    if (account.Attributes.Contains("description"))
                    {
                        desc = account.Attributes["description"].ToString();
                        account.Attributes["description"].ToString();

                        account.Attributes["description"] = desc + "account is updated with " + city;

                    }
                    else
                    {
                        Entity retrieved = service.Retrieve("account", account.Id, new ColumnSet())
                            if (retrieved.Attributes.Contains("description"))
                        {
                            desc = retrieved.Attributes["description"].ToString();
                        }
                        account.Attributes["description"] = desc + "account is updated with " + city;
                    }

                    service.Update(account);*/
                }
                catch (FaultException<OrganizationServiceFault> ex)
                {
                    throw new InvalidPluginExecutionException("An error occurred in FollowUpPlugin.", ex);
                }

                catch (Exception ex)
                {
                    tracingService.Trace("FollowUpPlugin: {0}", ex.ToString());
                    throw;
                }
            }
        }
    }
}

