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

        public Policy(string firstName, string lastName, string country, string numCars, string drivingRecord)
        {
            FirstName = firstName;
            LastName = lastName;
            Country = country;
            NumCars = Convert.ToInt32(numCars);
            switch (drivingRecord)
            {
                case "Perfect":
                    DrivingRecord = 283210000;
                    break;
                case "Good":
                    DrivingRecord = 283210001;
                    break;
                case "Bad":
                    DrivingRecord = 283210002;
                    break;
                case "Horrible":
                    DrivingRecord = 283210003;
                    break;
            }
        }
    }
}