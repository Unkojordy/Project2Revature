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
            string crmConnectionString = "AuthType=Office365;Url=https://org.crm.dynamics.com;UserName=djmendez@org.onmicrosoft.com;Password=ZWARRIORwar555";
            CrmServiceClient Service = new CrmServiceClient(crmConnectionString);

            Entity contact = new Entity("contact");
            contact.Attributes.Add("lastname", "Console App");

            //Console.WriteLine(Service.Create(contact));
        }
    }
}
