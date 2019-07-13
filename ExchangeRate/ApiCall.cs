using System.Web;
using System.Net.Http;
using System;
using System.Web.Script.Serialization;

namespace ExchangeRate
{
    class ApiCall
    {
        private string apiUrl = "http://apilayer.net/api/live?access_key=9c63c5d26713d53732db01f976b86580&currencies=CAD&source=USD&format=1";
        
        protected ExchangeRate GetExchangeRate(string url)
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
