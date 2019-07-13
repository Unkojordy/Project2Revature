using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Workflow;
using Microsoft.Xrm.Tooling.Connector;
using System;
using System.Activities;

namespace ExchangeRateWorkFlow
{
    class UpdateCurrency
    {
        protected void Execute(CodeActivityContext executionContext)
        {
            var apiUrl = "http://apilayer.net/api/live?access_key=9c63c5d26713d53732db01f976b86580&currencies=CAD&source=USD&format=1";

            ExchangeRate exchangeRate = ApiCall.GetExchangeRate(apiUrl);

            string CrmConnectionString = "AuthType=Office365;Url=https://dynamictraining.crm.dynamics.com;UserName=EricBooker@dynamictraining.onmicrosoft.com;Password=teddy1500!@#$";
            CrmServiceClient Service = new CrmServiceClient(CrmConnectionString);

            //Create the tracing service
            ITracingService tracingService = executionContext.GetExtension<ITracingService>();

            //Create the context
            IWorkflowContext context = executionContext.GetExtension<IWorkflowContext>();
            IOrganizationServiceFactory serviceFactory = executionContext.GetExtension<IOrganizationServiceFactory>();
            IOrganizationService service = serviceFactory.CreateOrganizationService(context.UserId);

            var canadianRecord = new Entity("transactioncurrency", new Guid("9e6d3598-5f8f-e911-a9c1-000d3a33afd1"));

            canadianRecord.Attributes.Add("exchangerate", exchangeRate.Value);
            Service.Update(canadianRecord);
        }
    }
}
