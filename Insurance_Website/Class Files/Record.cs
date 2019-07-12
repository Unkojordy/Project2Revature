using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Tooling.Connector;

namespace Insurance_Website.Class_Files
{
    public class Record
    {
        public static void Create(Policy policy)
        {
            string CrmConnectionString = "AuthType=Office365;Url=https://dynamictraining.crm.dynamics.com;UserName=EricBooker@dynamictraining.onmicrosoft.com;Password=teddy1500!@#$";

            CrmServiceClient Service = new CrmServiceClient(CrmConnectionString);

            Entity entity = new Entity("rev_policy");
            entity.Attributes.Add("rev_firstname", policy.FirstName);
            entity.Attributes.Add("rev_lastname", policy.LastName);
            entity.Attributes.Add("rev_country", policy.Country);
            entity.Attributes.Add("rev_numberofcars", policy.NumCars);
            entity.Attributes.Add("rev_drivingrecord", new OptionSetValue(policy.DrivingRecord));
            Service.Create(entity);
        }

        public static void Create(Claim claim)
        {
            string CrmConnectionString = "AuthType=Office365;Url=https://dynamictraining.crm.dynamics.com;UserName=EricBooker@dynamictraining.onmicrosoft.com;Password=teddy1500!@#$";

            CrmServiceClient Service = new CrmServiceClient(CrmConnectionString);

            Entity entity = new Entity("incident");
            //entity.Attributes.Add("title", claim.Title);
            entity.Attributes.Add("subjectid", claim.Subject);
            entity.Attributes.Add("rev_policynumber", claim.PolicyNumber);
            entity.Attributes.Add("rev_numberofcars", policy.NumCars);
            entity.Attributes.Add("rev_drivingrecord", new OptionSetValue(policy.DrivingRecord));
            Service.Create(entity);
        }
    }
}