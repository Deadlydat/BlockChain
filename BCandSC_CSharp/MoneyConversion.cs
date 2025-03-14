﻿using Nethereum.JsonRpc.Client;
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
            HttpResponseMessage response = await client.GetAsync(
                "https://min-api.cryptocompare.com/data/price?fsym=ETH&tsyms=USD,EUR" +
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

            dataObject.USD = Math.Round(Decimal.ToDouble(balance) * data.USD, 2);
            dataObject.EUR = Math.Round(Decimal.ToDouble(balance) * data.EUR, 2);

            return dataObject;
        }


        public static void BetCertainAmount(string teamRepresantion, int betAmount, User user)
        {
            DataObject data = GetETHValueFromApi().Result;

            double gasFee = 0;
            BlockchainInterface blockchainInterface = new(user);
            var balance = Decimal.ToDouble(blockchainInterface.GetAccountBalance());

            double betETH = betAmount / data.EUR;

            if (balance > betETH + gasFee)
            {
                double betWei = (betAmount / data.EUR) * 1000000000000000000;

                blockchainInterface.Bet(teamRepresantion, Convert.ToInt64(betWei));
            }

        }


        public static void CalculateTransactionForParticipants(int matchDay)
        {
            Gamelogic gamelogic = new(matchDay);

            List<int> participantsId = gamelogic.GetListOfParticipantsForTheMatchday();

            foreach (var item in participantsId)
            {
                User user = new User();
                user = user.GetUser(item);
                BlockchainInterface blockchainInterface = new(user);

                var currentUserBalance = TurnAccountBalanceInFiatMoney(user.GetUserBalance(user.Id));
                var newBalance = TurnAccountBalanceInFiatMoney(blockchainInterface.GetAccountBalance());

                double transaction = newBalance.EUR - currentUserBalance.EUR;

                user.SetUserTransaction(user.Id, matchDay, (decimal)transaction);


            }
        }
    }
}