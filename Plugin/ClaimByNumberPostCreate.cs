﻿using System;
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
                Entity claim= (Entity)context.InputParameters["Target"];

                // Obtain the organization service reference which you will need for  
                // web service calls.  
                IOrganizationServiceFactory serviceFactory =
                    (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
                IOrganizationService service = serviceFactory.CreateOrganizationService(context.UserId);

                try
                {
                    int count = 0;
                    QueryExpression query = new QueryExpression("rev_config");
                    query.ColumnSet.AddColumn("rev_numberofclaims");

                    Guid teamId = new Guid("d5ec2386-1da3-e911-a964-000d3a37fb59");
                    QueryExpression userQuery = new QueryExpression("systemuser");
                    userQuery.ColumnSet = new ColumnSet(true);
                    LinkEntity teamLink = new LinkEntity("systemuser", "teammembership", "systemuserid", "systemuserid", JoinOperator.Inner);
                    ConditionExpression teamCondition = new ConditionExpression("teamid", ConditionOperator.Equal, teamId);
                    teamLink.LinkCriteria.AddCondition(teamCondition);
                    userQuery.LinkEntities.Add(teamLink);

                    EntityCollection numberOfClaims = service.RetrieveMultiple(query);

                    EntityCollection retrievedUsers = service.RetrieveMultiple(userQuery);
                    var userlist = new List<Entity>();
                    
                    foreach (Entity user in retrievedUsers.Entities)
                    {
                        tracingService.Trace("user id for each " + user.Id.ToString());
                        QueryExpression priceQuery = new QueryExpression("rev_config");
                        priceQuery.ColumnSet.AddColumn("rev_numberofclaims");
                        priceQuery.Criteria.AddCondition("rev_name", ConditionOperator.Equal, user.Id.ToString());
                        EntityCollection numbercases = service.RetrieveMultiple(priceQuery);
                        
                        if (numbercases.Entities.Count==0)
                        {
                            Entity k = new Entity("rev_config");
                            k.Attributes.Add("rev_name", user.Id.ToString());
                            k.Attributes.Add("rev_numberofclaims", 0);
                            service.Create(k);
                        }
                            userlist.Add(user);
                            count++;
                    }
                     
                        Entity leastwork = userlist[0];
         
                        for (int i = 0; i < count; i++)
                        {
                        QueryExpression priceQuery = new QueryExpression("rev_config");
                        priceQuery.ColumnSet.AddColumn("rev_numberofclaims");
                        string st = userlist[i].Id.ToString();
                        priceQuery.Criteria.AddCondition("rev_name", ConditionOperator.Equal, st);
                        EntityCollection numbercases = service.RetrieveMultiple(priceQuery);
                        int currc = 0;

                        if(numbercases.Entities.Count > 0)
                        {
                            foreach (Entity a in numbercases.Entities)
                            {
                               // tracingService.Trace("not inside the if " + Int32.Parse(a.Attributes["rev_numberofclaims"].ToString()));
                                int curra = Int32.Parse(a.Attributes["rev_numberofclaims"].ToString());
                                int currb=0;
                                QueryExpression uery = new QueryExpression("rev_config");
                                uery.ColumnSet.AddColumn("rev_numberofclaims");
                                string sd = leastwork.Id.ToString();
                                uery.Criteria.AddCondition("rev_name", ConditionOperator.Equal, leastwork.Id.ToString());
                                EntityCollection nc = service.RetrieveMultiple(uery);
                                if(nc.Entities.Count == 1)
                                {
                                    foreach(Entity m in nc.Entities)
                                    {
                                        //tracingService.Trace("fuck this: " + m.Attributes["rev_numberofclaims"].ToString());
                                        currb = (Int32.Parse(m.Attributes["rev_numberofclaims"].ToString()));
                                      
                                    }
                                }
                                tracingService.Trace("curra: " + curra);
                                tracingService.Trace("currb: " + currb);
                                if (curra < currb)
                                {
                                    tracingService.Trace("inside currb: " + currb);
                                    leastwork = userlist[i];
                                    tracingService.Trace("work can start: " + userlist[i].Id.ToString());
                                }

                             
                               // tracingService.Trace("number of claisadasdms dasd: " + (a.Attributes["rev_numberofclaims"].ToString()));
                                 //}
                                
                            }
                        }

                    }

                    QueryExpression priceQuery2 = new QueryExpression("rev_config");
                    priceQuery2.ColumnSet.AddColumn("rev_numberofclaims");
                    string st2 = leastwork.Id.ToString();
                    priceQuery2.Criteria.AddCondition("rev_name", ConditionOperator.Equal, st2);
                    EntityCollection numbercases2 = service.RetrieveMultiple(priceQuery2);

                    if(numbercases2.Entities.Count > 0)
                    {
                        foreach(Entity nj in numbercases2.Entities)
                        {
                            int c = Int32.Parse(nj.Attributes["rev_numberofclaims"].ToString());
                            c++;
                            nj.Attributes["rev_numberofclaims"] = c;

                            service.Update(nj);
                        }
                    }

                    claim.Attributes["ownerid"] = leastwork.ToEntityReference();

                   
                    service.Update(claim);





                    // int curr = 0;
                    //int numberOfUSers = 0;
                    //  QueryExpression query = new QueryExpression("rev_count");
                    //query.ColumnSet.AddColumn("rev_counterp2");
                    // QueryExpression query = new QueryExpression("rev_config");
                    //query.ColumnSet.AddColumn("rev_counter");

                    // query.Criteria.AddCondition("parentcustomerid", ConditionOperator.Equal, account.Id);

                    /* Guid teamId = new Guid("d5ec2386-1da3-e911-a964-000d3a37fb59");



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
                            // service.Update(user);

                         }
                         else
                         {
                            // tracingService.Trace("number of claims: " + user["rev_number_of_claims"] );

                         }*/

                    /*var current = user.Contains("fullname") ? user["fullname"].ToString() : "";
                    //tracingService.Trace("user at place" + user.ToString());
                    userlist.Add(user);
                    //tracingService.Trace("name of users " + current);
                    numberOfUSers++;*/



                    // } 
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
