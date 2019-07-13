using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Script.Serialization;

namespace Testing
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var httpClient = new HttpClient())
            {
                var url = "http://apilayer.net/api/live?access_key=9c63c5d26713d53732db01f976b86580&currencies=CAD&source=USD&format=1";

                httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
                var response = httpClient.GetStringAsync(new Uri(url)).Result;

                var serializer = new JavaScriptSerializer();
                dynamic jsonObject = serializer.Deserialize<dynamic>(response);
                var value = jsonObject["quotes"]["USDCAD"];
                Console.WriteLine(value);
                Console.ReadKey();
            }
        }
    }
}
