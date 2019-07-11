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

            Entity policy = new Entity("rev_policy");
            policy.Attributes.Add("rev_firstname", "Eric");
            policy.Attributes.Add("rev_lastname", "Booker");
            policy.Attributes.Add("rev_country", "United States");
            policy.Attributes.Add("rev_numberofcars", 1);
            policy.Attributes.Add("rev_drivingrecord", new OptionSetValue(283210001));
            Console.WriteLine(Service.Create(policy));
        }
    }
}
