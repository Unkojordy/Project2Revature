using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services.Description;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Tooling.Connector;

namespace Insurance_Website.Class_Files
{
    public class Policy
    {
        public string Name { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Country { get; set; }
        public int NumCars { get; set; }
        public int DrivingRecord { get; set; }
        public string CrmConnectionString { get; set; }

        public Policy(string name, string firstName, string lastName, string country, string numCars, string drivingRecord)
        {
            Name = name;
            FirstName = firstName;
            LastName = lastName;
            Country = country;
            NumCars = Int32.Parse(numCars);
            switch (drivingRecord)
            {
                case "perfect":
                    DrivingRecord = 283210000;
                    break;
                case "good":
                    DrivingRecord = 283210001;
                    break;
                case "bad":
                    DrivingRecord = 283210002;
                    break;
                case "horrible":
                    DrivingRecord = 283210003;
                    break;
            }
            CrmConnectionString = "AuthType=Office365;Url=https://dynamictraining.crm.dynamics.com;UserName=EricBooker@dynamictraining.onmicrosoft.com;Password=teddy1500!@#$";         
        }
        
        public void Create()
        {
            CrmServiceClient Service = new CrmServiceClient(CrmConnectionString);

            Entity policy = new Entity("rev_policy");
            policy.Attributes.Add("rev_name", Name);
            policy.Attributes.Add("rev_firstname", FirstName);
            policy.Attributes.Add("rev_lastname", LastName);
            policy.Attributes.Add("rev_country", Country);
            policy.Attributes.Add("rev_numberofcars", NumCars);
            policy.Attributes.Add("rev_drivingrecord", new OptionSetValue(DrivingRecord));
            Console.WriteLine(Service.Create(policy));
            Console.ReadKey();
        }
    }
}