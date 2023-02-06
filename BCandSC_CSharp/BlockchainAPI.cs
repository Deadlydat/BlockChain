using Nethereum.JsonRpc.Client;
using Newtonsoft.Json;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Net.Http.Headers;
using System.Numerics;
using System.Security.Cryptography.Xml;
using System.Threading.Tasks;


namespace BCandSC_CSharp
{
    public class BlockchainAPI
    {

        public class DataObject
        {
            public double USD { get; set; }
            public double EUR { get; set; }

        }

        public static async void GetETHValueFromApi()
        {

            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync("https://min-api.cryptocompare.com/data/price?fsym=ETH&tsyms=USD,EUR" +
                "&api_key=b88e111c6b5fae28acc9c6f997f2ae98be60fea07467d6094984e9d9421b4fb8");

            if (response.IsSuccessStatusCode)
            {
                var responseBody = await response.Content.ReadAsStringAsync();

                var result = JsonConvert.DeserializeObject<DataObject>(responseBody);

                Console.WriteLine("USD: " + result.USD + "   EUR: " + result.EUR);
            }

        }

    }
}
