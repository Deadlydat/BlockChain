using System.Data.SqlClient;
using System.Data;
using System.Numerics;
using static BCandSC_CSharp.Player;
using Nethereum.Contracts.Standards.ENS.PublicResolver.ContractDefinition;
using Nethereum.Web3;
using Nethereum.Web3.Accounts;
using BettingContract.Betting.ContractDefinition;
using BettingContract.Betting;
using Nethereum.RPC.Eth.DTOs;
using Newtonsoft.Json.Linq;


namespace BCandSC_CSharp
{
    public class BlockchainInterface
    {
        private const string Url = "https://rpc.ankr.com/eth_goerli";
        private const string ContractAddress = "0x58387EEA716c5E07AEeF2bf92e1731E1e39cA7Fb";
        private const long gas = 20000000000;

        private Account _account;
        private Web3 _web3;
        private BettingService _bettingService;

        public BlockchainInterface(User user)
        {
            _account = new Account(user.PrivateKey);
            _web3 = new Web3(_account, Url);
            _bettingService = new BettingService(_web3, ContractAddress);
        }

        public JArray GiveMoneyBack()
        {
            return _bettingService.GiveMoneyBackRequestAndWaitForReceiptAsync(new GiveMoneyBackFunction()
                { GasPrice = gas }).Result.Logs;
        }

        public JArray Bet(string teamRepresentation, int amountToSend)
        {
            return _bettingService.BetRequestAndWaitForReceiptAsync(new BetFunction()
                    { TeamRepresentation = teamRepresentation, GasPrice = gas, AmountToSend = amountToSend })
                .Result.Logs;
        }

        public JArray DistributePrices(List<string> teamRepresentationsOfWinners)
        {
            return _bettingService.DistributePrizesRequestAndWaitForReceiptAsync(new DistributePrizesFunction()
                    { TeamRepresentationOfWinners = teamRepresentationsOfWinners, GasPrice = gas })
                .Result.Logs;
        }

        public JArray StartGame()
        {
            return _bettingService.StartGameRequestAndWaitForReceiptAsync(new StartGameFunction() { GasPrice = gas })
                .Result.Logs;
        }

        public JArray FinishGame()
        {
            return _bettingService.FinishGameRequestAndWaitForReceiptAsync(new FinishGameFunction() { GasPrice = gas })
                .Result.Logs;
        }

        public BigInteger GetMoneyPool()
        {
            return _bettingService.GetMoneyPoolQueryAsync().Result;
        }

        public BigInteger GetPlayerCount()
        {
            return _bettingService.GetPlayerCountQueryAsync().Result;
        }

        public decimal GetAccountBalance()
        {
            var weiBalance = _web3.Eth.GetBalance.SendRequestAsync(_account.Address).Result;
            return Web3.Convert.FromWei(weiBalance);
        }

        public static long GetGasPrice()
        {
            return gas;
        }
    }
}