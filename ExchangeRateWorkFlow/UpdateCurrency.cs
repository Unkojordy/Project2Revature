using Microsoft.Xrm.Tooling.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRate
{
    class UpdateCurrency
    {
        public static void CrmConnection()
        {
            string CrmConnectionString = "AuthType=Office365;Url=https://dynamictraining.crm.dynamics.com;UserName=EricBooker@dynamictraining.onmicrosoft.com;Password=teddy1500!@#$";
            CrmServiceClient Service = new CrmServiceClient(CrmConnectionString);
        }

    }
}
