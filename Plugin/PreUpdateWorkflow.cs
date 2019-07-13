//THIS IS ACTUALLY PRE
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
    public class PreUpdateWorkflow: IPlugin
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
                Entity claim = (Entity)context.InputParameters["Target"];

                // Obtain the organization service reference which you will need for  
                // web service calls.  
                IOrganizationServiceFactory serviceFactory =
                    (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
                IOrganizationService service = serviceFactory.CreateOrganizationService(context.UserId);

                try
                {
                    //ADD IF RESOLVED
                    if (claim.Attributes.Contains("statecode"))
                    {
                        tracingService.Trace("quick test: " + claim.Attributes["statecode"].ToString());
                        int resolved = ((OptionSetValue)claim["statecode"]).Value;

                        tracingService.Trace("quick test2: " + resolved);
                        if (resolved == 1)
                        {
                            Entity accountImage = context.PreEntityImages["incidentPreImage"];

                            if (accountImage.Attributes.Contains("ownerid"))
                            {
                                tracingService.Trace("does contain");

                                tracingService.Trace(accountImage.Attributes["ownerid"].ToString());
                                EntityReference c = ((EntityReference)accountImage.Attributes["ownerid"]);
                                Entity owner = service.Retrieve(c.LogicalName, c.Id,
                                new ColumnSet("firstname", "lastname", "address1_country"));

                                tracingService.Trace("trace after entity: " + owner.Id.ToString());
                                String ownerIdString = owner.Id.ToString();
                                Guid ownerId = new Guid(owner.Id.ToString());

                                ColumnSet cols = new ColumnSet(
                                    new String[] { "firstname" });

                                Entity ownerEntity = (Entity)service.Retrieve("systemuser", ownerId, cols);

                                tracingService.Trace("after retrieving: " + ownerEntity.Attributes["firstname"].ToString());

                                QueryExpression numberQuery2 = new QueryExpression("rev_config");
                                numberQuery2.ColumnSet.AddColumn("rev_numberofclaims");

                                numberQuery2.Criteria.AddCondition("rev_name", ConditionOperator.Equal, ownerIdString);
                                EntityCollection numbercases2 = service.RetrieveMultiple(numberQuery2);

                                if (numbercases2.Entities.Count > 0)
                                {
                                    foreach (Entity nj in numbercases2.Entities)
                                    {
                                        int nClaims = Int32.Parse(nj.Attributes["rev_numberofclaims"].ToString());

                                        tracingService.Trace("old number of claims: " + nClaims);
                                        nClaims--;

                                        tracingService.Trace("new number of claims: " + nClaims);
                                        nj.Attributes["rev_numberofclaims"] = nClaims;

                                        service.Update(nj);
                                    }
                                }

                            }
                        }
                    } 
                     
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