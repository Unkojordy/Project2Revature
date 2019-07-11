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
    public class ClaimByNumberPostCreate : IPlugin
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
                    int curr = 0;
                    int numberOfUSers = 0;
                    //  QueryExpression query = new QueryExpression("rev_count");
                    //query.ColumnSet.AddColumn("rev_counterp2");
                   // QueryExpression query = new QueryExpression("rev_config");
                    //query.ColumnSet.AddColumn("rev_counter");

                    // query.Criteria.AddCondition("parentcustomerid", ConditionOperator.Equal, account.Id);

                    Guid teamId = new Guid("d5ec2386-1da3-e911-a964-000d3a37fb59");

                    

                    QueryExpression userQuery = new QueryExpression("systemuser");
                    userQuery.ColumnSet = new ColumnSet(true);
                    LinkEntity teamLink = new LinkEntity("systemuser", "teammembership", "systemuserid", "systemuserid", JoinOperator.Inner);
                    ConditionExpression teamCondition = new ConditionExpression("teamid", ConditionOperator.Equal, teamId);
                    teamLink.LinkCriteria.AddCondition(teamCondition);
                    userQuery.LinkEntities.Add(teamLink);

                    EntityCollection retrievedUsers = service.RetrieveMultiple(userQuery);
                    var userlist = new List<Entity>();
                    foreach (Entity user in retrievedUsers.Entities)
                    {
                        var userId = user.Id;

                        if (!user.Attributes.Contains("rev_numberclaims"))
                        {
                            tracingService.Trace("no claims yet ");
                            user.Attributes.Add("rev_numberclaims", 0);

                            service.Update(user);

                        }
                        else
                        {
                           // tracingService.Trace("number of claims: " + user["rev_number_of_claims"] );

                        }

                        /*var current = user.Contains("fullname") ? user["fullname"].ToString() : "";
                        //tracingService.Trace("user at place" + user.ToString());
                        userlist.Add(user);
                        //tracingService.Trace("name of users " + current);
                        numberOfUSers++;*/



                    }
                    /*
                    EntityCollection counter = service.RetrieveMultiple(query);
                    if (counter.Entities.Count == 0)
                    {
                        Entity newcount = new Entity("rev_config");
                        newcount.Attributes.Add("rev_name", "counterName");
                        newcount.Attributes.Add("rev_counter", 0);
                        service.Create(newcount);
                        tracingService.Trace("counter is empty");
                    }
                    if (counter.Entities.Count == 1)
                    {
                        foreach (Entity count in counter.Entities)
                        {
                            var count1 = count.Contains("rev_counter") ? count["rev_counter"].ToString() : "";
                            curr = Int32.Parse(count1);
                            int next = Int32.Parse(count1);
                            next = next + 1;
                            if (next + 1 > numberOfUSers)
                            {
                                next = 0;
                            }
                            tracingService.Trace("next: " + next);
                            count["rev_counter"] = next;
                            service.Update(count);
                        }
                    }

                    claim.Attributes["ownerid"] = userlist[curr].ToEntityReference();


                    tracingService.Trace("user list at 0" + userlist[curr]["fullname"].ToString());
                    service.Update(claim);*/

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
