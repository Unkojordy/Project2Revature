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
    public class COntactPreCreate : IPlugin
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
                Entity contact = (Entity)context.InputParameters["Target"];

                // Obtain the organization service reference which you will need for  
                // web service calls.  
                IOrganizationServiceFactory serviceFactory =
                    (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
                IOrganizationService service = serviceFactory.CreateOrganizationService(context.UserId);

                try
                {
                  string firstname = contact.Attributes["firstname"].ToString();

                    string email = contact.Attributes["emailaddress1"].ToString();


                    /* Different ways of retreiving the data from the API
                          1.QueryExpression
                          2.QueryByAttribute
                          3.FetchXML
                          4.LINQ*/

                   
                    //SQL statement
                    //select * from contact where emailaddress1= email;

                    QueryExpression query = new QueryExpression("contact");
                    query.ColumnSet.AddColumn("emailaddress1");
                    query.Criteria.AddCondition("emailaddress1", ConditionOperator.Equal, email);

                    EntityCollection collection = service.RetrieveMultiple(query);
                    //you can use foreach() if you have more than one item 
                    if (collection.Entities.Count > 0)
                    {
                        throw new InvalidPluginExecutionException("Enter unique Email address");
                     }
                    // Plug-in business logic goes here.  
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
