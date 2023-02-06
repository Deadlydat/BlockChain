using System.Data.SqlClient;
using System.Data;
using System.Numerics;
using static BCandSC_CSharp.Player;
using Nethereum.Contracts.Standards.ENS.PublicResolver.ContractDefinition;
using Nethereum.Web3;
using Nethereum.Web3.Accounts;
using Betting.Betting.ContractDefinition;
using Betting.Betting;
using Nethereum.RPC.Eth.DTOs;
using Newtonsoft.Json.Linq;


namespace BCandSC_CSharp
{
    public class BlockchainInterface
    {
        private const string Url = "https://rpc.ankr.com/eth_goerli";
        // this is the private key of the contract owner
        private const string PrivateKey = "0xc815fa507181842ea2c85b12d75dc162b633d1c69d87e351bb86406f3db5b72f";
        private const string ContractAddress = "0xE1D9739D0CF03fC0396892ba682ed1E6d008C730";
        private Account _account;
        private Web3 _web3;
        private BettingService _bettingService;
        private const long gas = 20000000000;
        // TODO increase the betting value for easier testing
        private const long value = 100000000000000;


        public BlockchainInterface()
        {
            _account = new Account(PrivateKey);
            _web3 = new Web3(_account, Url);
            _bettingService = new BettingService(_web3, ContractAddress);
            Demo().Wait();
        }

        //public static void Main()
        //{
        //    var blockchainInterface = new BlockchainInterface();
        //}

        public JArray Bet(string teamRepresentation, string privateKey)
        {
            _account = new Account(privateKey);
            _web3 = new Web3(_account, Url);
            _bettingService = new BettingService(_web3, ContractAddress);

            return _bettingService.BetRequestAndWaitForReceiptAsync(new BetFunction()
                    { TeamRepresentation = teamRepresentation, GasPrice = gas, AmountToSend = value })
                .Result.Logs;
        }

        public JArray DistributePrices(string teamRepresentationOfWinner, string privateKey)
        {
            _account = new Account(privateKey);
            _web3 = new Web3(_account, Url);
            _bettingService = new BettingService(_web3, ContractAddress);

            return _bettingService.DistributePrizesRequestAndWaitForReceiptAsync(new DistributePrizesFunction()
                    { TeamRepresentationOfWinner = teamRepresentationOfWinner, GasPrice = gas })
                .Result.Logs;
        }

        public JArray StartGame(string privateKey)
        {
            _account = new Account(privateKey);
            _web3 = new Web3(_account, Url);
            _bettingService = new BettingService(_web3, ContractAddress);
            
            return _bettingService.StartGameRequestAndWaitForReceiptAsync(new StartGameFunction() { GasPrice = gas })
                .Result.Logs;
        }
        
        public JArray FinishGame(string privateKey)
        {
            _account = new Account(privateKey);
            _web3 = new Web3(_account, Url);
            _bettingService = new BettingService(_web3, ContractAddress);
            
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
        
        public decimal GetAccountBalance(string accountAddress)
        {
            var weiBalance = _web3.Eth.GetBalance.SendRequestAsync(accountAddress).Result;
            return Web3.Convert.FromWei(weiBalance);
        }


        async Task Demo()
        {
            try
            {
                // Setup
                // Here we're using local chain eg Geth https://github.com/Nethereum/TestChains#geth
                //var account = new Account(PrivateKey);
                //var web3 = new Web3(account, Url);

                //Console.WriteLine("Deploying...");
                //var deployment = new BettingDeployment();
                // var receipt = await BettingService.DeployContractAndWaitForReceiptAsync(web3, deployment);
                //var service = new BettingService(web3, ContractAddress);
                //Console.WriteLine($"Contract Deployment Tx Status: {receipt.Status.Value}");
                //Console.WriteLine($"Contract Address: {service.ContractHandler.ContractAddress}");
                //Console.WriteLine("");

                Console.WriteLine("Start.");

                //Console.WriteLine("First Bet...");
                //Console.WriteLine(Bet("HAHA", "0x1c7a3d64bf03314cd99cbadc1aac27a96512efb1c5adc2e3994f0533be1571da"));
                //Console.WriteLine("Second Bet...");
                //Console.WriteLine(Bet("BEBE", "0xf3b96df77469e17a768461fd086071ee06fdf662bad857bc56dfa1d50dd164fb"));
                //Console.WriteLine("Start Game...");
                //Console.WriteLine(StartGame(PrivateKey));
                //Console.WriteLine("Finish Game...");
                //Console.WriteLine(FinishGame(PrivateKey));
                Console.WriteLine("MoneyPool:");
                Console.WriteLine(GetMoneyPool());
                Console.WriteLine("Player count:");
                Console.WriteLine(GetPlayerCount());

                Console.WriteLine("GetBalance");
                Console.WriteLine(GetAccountBalance("0xa145E3bCe112EB732aeeBefa3Cc2C9Ad82275B0C"));


                Console.WriteLine("First Bet...");
                Console.WriteLine(Bet("HAHA", "0x6ddb585187ab98da4515f1618bcd49327d7ae76b60aa832bc39a3e14c86a374a"));
                

                Console.WriteLine("MoneyPool:");
                Console.WriteLine(GetMoneyPool());
                Console.WriteLine("Player count:");
                Console.WriteLine(GetPlayerCount());
                Console.WriteLine("GetBalance");
                Console.WriteLine(GetAccountBalance("0xa145E3bCe112EB732aeeBefa3Cc2C9Ad82275B0C"));


                //Console.WriteLine("Distribute Prices:");
                //Console.WriteLine(DistributePrices("BEBE", PrivateKey));
                //Console.WriteLine("MoneyPool:");
                //Console.WriteLine(GetMoneyPool());
                //Console.WriteLine("Player count:");
                //Console.WriteLine(GetPlayerCount());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            Console.WriteLine("Finished");
            Console.ReadLine();
        }
    }
}