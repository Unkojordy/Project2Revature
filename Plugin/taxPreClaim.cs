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
    public class taxPreClaim : IPlugin
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
                    decimal tax = 0;
                    decimal amount = ((Money)claim.Attributes["rev_amount"]).Value;
                  EntityReference  c = ((EntityReference)claim.Attributes["customerid"]);
                    Entity contact = service.Retrieve(c.LogicalName, c.Id,
                    new ColumnSet("firstname", "lastname", "address1_country"));
                    string country = contact["address1_country"].ToString();
                    if (string.Equals(country, "United States"))
                    {
                        QueryExpression query = new QueryExpression("rev_config");
                        query.ColumnSet.AddColumn("rev_ustax");
                        query.Criteria.AddCondition("rev_name", ConditionOperator.Equal, "usatax");
                        EntityCollection usat = service.RetrieveMultiple(query);
                        
                        if (usat.Entities.Count == 0)
                        {
                            Entity newcount = new Entity("rev_config");
                            newcount.Attributes.Add("rev_name", "usatax");
                            newcount.Attributes.Add("rev_ustax", .05m);
                            service.Create(newcount);
                            tracingService.Trace("usa tax is empty");
                        }
                        else
                       {
                            foreach (Entity count in usat.Entities)
                            {
                                tracingService.Trace("usa tax is not empty: "+count["rev_ustax"].ToString());
                                tax = (decimal)count["rev_ustax"];
                                // tracingService.Trace("usa tax is ", count["rev_ustax"]);
                           }
                        }
                        decimal total = amount + (amount *  tax);
                        claim.Attributes.Add("rev_amountaftertax", new Money(Math.Round(total)));
                        service.Update(claim);
                    }//end if US
                    else if (string.Equals(country, "Canada")){
                        QueryExpression query = new QueryExpression("rev_config");
                        query.ColumnSet.AddColumn("rev_catax");
                        query.Criteria.AddCondition("rev_name", ConditionOperator.Equal, "cantax");
                        EntityCollection usat = service.RetrieveMultiple(query);

                        if (usat.Entities.Count == 0)
                        {
                            Entity newcount = new Entity("rev_config");
                            newcount.Attributes.Add("rev_name", "cantax");
                            newcount.Attributes.Add("rev_catax", .04m);
                            service.Create(newcount);
                            tracingService.Trace("canada tax is empty");
                        }
                        else
                        {
                            foreach (Entity count in usat.Entities)
                            {
                                tracingService.Trace("canada tax is not empty: " + count["rev_catax"].ToString());
                                tax = (decimal)count["rev_catax"];
                                // tracingService.Trace("usa tax is ", count["rev_ustax"]);
                            }
                        }
                        decimal total = amount + (amount * tax);
                        claim.Attributes.Add("rev_amountaftertax", new Money(Math.Round(total)));
                        service.Update(claim);
                    }
                    else
                    {
                        throw new InvalidPluginExecutionException("Needs to be between US and Canada");
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
