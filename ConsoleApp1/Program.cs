using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Services.Description;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Tooling.Connector;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            string crmConnectionString = "AuthType=Office365;Url=https://dynamictraining.crm.dynamics.com;UserName=EricBooker@dynamictraining.onmicrosoft.com;Password=teddy1500!@#$";
            CrmServiceClient Service = new CrmServiceClient(crmConnectionString);
            var perfect = 283210000;
            var good = 283210001;
            var bad = 283210002;
            var horrible = 283210003;

            Entity policy = new Entity("rev_policy");
            policy.Attributes.Add("rev_name", "Jordan's Policy");
            policy.Attributes.Add("rev_firstname", "Jordan");
            policy.Attributes.Add("rev_lastname", "Eror");
            policy.Attributes.Add("rev_country", "United States");
            policy.Attributes.Add("rev_numberofcars", 1);
            policy.Attributes.Add("rev_drivingrecord", new OptionSetValue(horrible));
            Console.WriteLine(Service.Create(policy));
            Console.ReadKey();
        }
    }
}
