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
    public class PreTaxPolicy : IPlugin
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
                Entity policy = (Entity)context.InputParameters["Target"];

                // Obtain the organization service reference which you will need for  
                // web service calls.  
                IOrganizationServiceFactory serviceFactory =
                    (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
                IOrganizationService service = serviceFactory.CreateOrganizationService(context.UserId);

                try
                {
                   decimal tax = 0;
                    decimal pricePerCar = 0;
                    decimal total = 0;
                    decimal totalPostTax = 0;
                    if (policy.Attributes.Contains("rev_country") && policy.Attributes.Contains("rev_firstname")&& policy.Attributes.Contains("rev_lastname")&& policy.Attributes.Contains("rev_drivingrecord")&& policy.Attributes.Contains("rev_numberofcars"))
                    {
                        string country = policy.Attributes["rev_country"].ToString();
                        string firstname = policy.Attributes["rev_firstname"].ToString();
                        string lastname = policy.Attributes["rev_lastname"].ToString();
                        String record = policy.FormattedValues["rev_drivingrecord"].ToString();
                        int cars = Convert.ToInt32(policy.Attributes["rev_numberofcars"]);

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
                                    tracingService.Trace("usa tax is not empty: " + count["rev_ustax"].ToString());
                                    tax = (decimal)count["rev_ustax"];

                                }
                            }
                        }
                        else if (string.Equals(country, "Canada"))
                        {
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

                        }

                        tracingService.Trace("record: " + record);
                        Entity contact = new Entity("contact");
                        contact.Attributes.Add("firstname", firstname);
                        contact.Attributes.Add("lastname", lastname);
                        contact.Attributes.Add("address1_country", country);
                        service.Create(contact);

                        QueryExpression priceQuery = new QueryExpression("rev_config");
                        priceQuery.ColumnSet.AddColumn("rev_priceperrecord");
                        priceQuery.Criteria.AddCondition("rev_name", ConditionOperator.Equal, record);
                        EntityCollection recordCollection = service.RetrieveMultiple(priceQuery);
                        if (recordCollection.Entities.Count == 0)
                        {
                            decimal setPrice = 0;
                            if (string.Equals(record, "Perfect"))
                            {
                                setPrice = 40m;
                            }
                            else if (string.Equals(record, "Good"))
                            {
                                setPrice = 50m;
                            }
                            else if (string.Equals(record, "Bad"))
                            {
                                setPrice = 70m;
                            }
                            else if (string.Equals(record, "Horrible"))
                            {
                                setPrice = 90m;
                            }
                            Entity records = new Entity("rev_config");
                            records.Attributes.Add("rev_name", record);
                            records.Attributes.Add("rev_priceperrecord", new Money(Math.Round(setPrice)));
                            service.Create(records);
                        }
                        else
                        {
                            foreach (Entity p in recordCollection.Entities)
                            {

                                pricePerCar = ((Money)p["rev_priceperrecord"]).Value;
                            }

                            tracingService.Trace("price : " + pricePerCar);


                        }
                        total = pricePerCar * cars;
                        tracingService.Trace("total before tax : " + total);


                        totalPostTax = (total + (total * tax));
                        tracingService.Trace("total before tax : " + totalPostTax);
                        policy.Attributes.Add("rev_price", new Money(Math.Round(total)));
                        policy.Attributes.Add("rev_priceaftertax", new Money(Math.Round(totalPostTax)));
                        service.Update(policy);


                        QueryExpression t = new QueryExpression("contact");
                        t.ColumnSet.AddColumn("firstname");
                        t.Criteria.AddCondition("firstname", ConditionOperator.Equal, firstname);
                        t.Criteria.AddCondition("lastname", ConditionOperator.Equal, lastname);
                        EntityCollection tc = service.RetrieveMultiple(t);

                        if (tc.Entities.Count == 0)
                        {
                            tracingService.Trace("not found");
                        }
                        else
                        {
                            foreach (Entity l in tc.Entities)
                            {
                                policy.Attributes["rev_contact"] = l.ToEntityReference();
                                policy.Attributes["rev_contact"] = l.ToEntityReference();
                                tracingService.Trace("found: " + l["firstname"].ToString());
                                service.Update(policy);
                            }

                        }

                    }
                    else
                    {
                        throw new InvalidPluginExecutionException("missing attributes");
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