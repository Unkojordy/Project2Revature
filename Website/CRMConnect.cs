//using Microsoft.Xrm.Sdk;
//using Microsoft.Xrm.Tooling.Connector;


//namespace Website
//{
//    public class CRMConnect
//    {
//        public IOrganizationService service =null;
//        //connect to CRM
//        public CRMConnect()
//        {
//            string crmConnectionString = "AuthType=Office365;Url=https://org.crm.dynamics.com;UserName=djmendez@org.onmicrosoft.com;Password=ZWARRIORwar555";
//             service = new CrmServiceClient(crmConnectionString);

//        }
//        protected void CreatePolicy()
//        {
//            //service.Create();
//        }
//        public void CreateAccount(Accounts account)
//        {
//            Entity record = new Entity("account");
//            record.Attributes.Add("name", account.name);
//            service.Create(record);
//        }

//    }
//}
   