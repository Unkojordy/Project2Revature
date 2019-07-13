using System.Web;
using System.Net.Http;
using System;
using System.Web.Script.Serialization;

namespace ExchangeRateWorkFlow
{
    public class ApiCall
    {     
        public static ExchangeRate GetExchangeRate(string url)
        {
            using (var httpClient = new HttpClient())
            {

                httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
                var response = httpClient.GetStringAsync(new Uri(url)).Result;

                var serializer = new JavaScriptSerializer();
                dynamic jsonObject = serializer.Deserialize<dynamic>(response);
                decimal value = jsonObject["quotes"]["USDCAD"];

                return new ExchangeRate(value);
            }
        }
    }
}
