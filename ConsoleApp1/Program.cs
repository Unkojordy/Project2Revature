using System;
using System.Web.Services.Description;
using Microsoft.Xrm.Sdk;
using ServiceStack;
using RestSharp;
using Newtonsoft.Json.Linq;
using Microsoft.Xrm.Tooling.Connector;

namespace CurrencyExchange
{
    class Program
    {

        static void Main(string[] args)
        {
            var exchangeRate = ApiCall("http://apilayer.net/api/live?access_key=9c63c5d26713d53732db01f976b86580&currencies=CAD&source=USD&format=1");

            Console.WriteLine(ConvertCurrency(exchangeRate, 100));
            Console.ReadKey();

        }


        public static ExchangeRate ApiCall(string url)
        {
            string CrmConnectionString = "AuthType=Office365;Url=https://dynamictraining.crm.dynamics.com;UserName=EricBooker@dynamictraining.onmicrosoft.com;Password=teddy1500!@#$";
            CrmServiceClient Service = new CrmServiceClient(CrmConnectionString);

            var Client = new RestClient(url);
            var response = Client.Execute(new RestRequest());
            var jObject = JObject.Parse(response.Content);

            ExchangeRate exchangeRate = new ExchangeRate(jObject["quotes"]["USDCAD"].ToString());

            return exchangeRate;
        }

        public static decimal ConvertCurrency(ExchangeRate exchangeRate, decimal amount)
        {
            return amount * exchangeRate.Value;
        }
    }
}
