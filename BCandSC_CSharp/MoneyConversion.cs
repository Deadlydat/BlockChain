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
    public class MoneyConversion
    {


        public class DataObject
        {
            public double USD { get; set; } = 0;
            public double EUR { get; set; } = 0;

        }

        public static async Task<DataObject> GetETHValueFromApi()
        {

            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync("https://min-api.cryptocompare.com/data/price?fsym=ETH&tsyms=USD,EUR" +
                "&api_key=b88e111c6b5fae28acc9c6f997f2ae98be60fea07467d6094984e9d9421b4fb8");



            if (response.IsSuccessStatusCode)
            {
                var responseBody = await response.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject<DataObject>(responseBody);



            }
            return new DataObject();
        }



        public static DataObject TurnAccountBalanceInFiatMoney(decimal balance)
        {
            DataObject dataObject = new DataObject();
            DataObject data = GetETHValueFromApi().Result;

            //BlockchainInterface blockchainInterface = new();

            //double balance = Decimal.ToDouble(blockchainInterface.GetAccountBalance(user.Address));

            dataObject.USD = Math.Round(Decimal.ToDouble(balance) * data.USD, 2);
            dataObject.EUR = Math.Round(Decimal.ToDouble(balance) * data.EUR, 2);

            return dataObject;
        }



        public static void BetCertainAmount(string teamRepresantion, int betAmount, User user)
        {
            DataObject dataObject = new DataObject();
            DataObject data = GetETHValueFromApi().Result;

            double gasFee = 0;


            BlockchainInterface blockchainInterface = new();
            double balance = Decimal.ToDouble(blockchainInterface.GetAccountBalance(user.Address));


            double betETH = betAmount / data.EUR;


            if (balance > betETH + gasFee)
            {

                //hier neue bet funktion

                blockchainInterface.Bet(teamRepresantion, user.PrivateKey);
            }
            else
            {

                //warning for user
            }

        }





        public static void CalculateTransactionForParticipants(int matchDay)
        {

            Gamelogic gamelogic = new(matchDay);
            BlockchainInterface blockchainInterface = new();

            List<int> participantsId = gamelogic.GetListOfParticipantsForTheMatchday();

            foreach (var item in participantsId)
            {
                User user = new User();
                user = user.GetUser(item);

                var currentUserBalance = TurnAccountBalanceInFiatMoney(user.GetUserBalance(user.Id));
                var newBalance = TurnAccountBalanceInFiatMoney(blockchainInterface.GetAccountBalance(user.Address));

                double transaction = currentUserBalance.EUR - newBalance.EUR;

        
                user.SetUserTransaction(user.Id, matchDay, (int)transaction);


            }

        }


    }
}
