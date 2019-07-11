using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Workflow;
using System.Activities;
using Microsoft.Xrm.Sdk.Client;

namespace Customworkflows
{
    
    public class GetConfig : CodeActivity
    {
       
        [Input("Name of the key")]
        public InArgument<string> Name { get; set; }

        [Output("Value of the key")]
        public OutArgument<string> Value { get; set; }
        protected override void Execute(CodeActivityContext executionContext)
        {
            ITracingService tracingService = executionContext.GetExtension<ITracingService>();

            IWorkflowContext context = executionContext.GetExtension<IWorkflowContext>();
            IOrganizationServiceFactory serviceFactory = executionContext.GetExtension<IOrganizationServiceFactory>();
            IOrganizationService service = serviceFactory.CreateOrganizationService(context.UserId);
            //retreiving records using LINQ
            String keyName = Name.Get(executionContext);


            using (OrganizationServiceContext queryContext = new OrganizationServiceContext(service))
            {
                //var result = queryContext.CreateQuery("rev_config").First().Attributes["rev_value"].TOString();

                var result = from item in queryContext.CreateQuery("rev_config")
                             where item.Attributes["rev_name"].Equals(keyName + "-TAX")
                             select item.Attributes["rev_value"].ToString();


                Value.Set(executionContext, result.First().ToString());
            }
        }
    }
}
