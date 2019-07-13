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
    public class accountPostCreate : IPlugin
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
                    //create task record

                    Entity taskRecord = new Entity("task");
                    taskRecord.Attributes.Add("subject","follow up with account" );

                    //Lookups 
                    // taskRecord.Attributes.Add("regardingobjectid", new EntityReference("account", account.Id));
                    taskRecord.Attributes.Add("regardingobjectid", account.ToEntityReference());
                    //service.Create()  

                    //option set

                    taskRecord.Attributes.Add("prioritycode", new OptionSetValue(2));

                    //datetime

                    taskRecord.Attributes.Add("scheduledend", DateTime.UtcNow.AddDays(2));

                    Guid taskGuid =service.Create(taskRecord);

                    //SORT OF A CONSOLE.LOG
                    tracingService.Trace("Task is created " + taskGuid);
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
