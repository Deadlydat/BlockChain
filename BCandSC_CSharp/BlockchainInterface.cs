using System.Data.SqlClient;
using System.Data;
using System.Numerics;
using static BCandSC_CSharp.Player;
using Nethereum.Contracts.Standards.ENS.PublicResolver.ContractDefinition;
using Nethereum.Web3;
using Nethereum.Web3.Accounts;
using Betting.Betting.ContractDefinition;
using Betting.Betting;


namespace BCandSC_CSharp
{
    public class BlockchainInterface
    {
        private const string Url = "HTTP://192.168.178.27:7545";
        private const string PrivateKey = "0x840af416508488a7ef3f7e822d71b381235bae6c1286f9203d1d89818a7dcb47";
        private const string ContractAddress = "0x9314658625A4a4c212859Abd15D6A168aE6d6731";
        private Account _account;
        private Web3 _web3;
        private BettingService _bettingService;
        private const long gas = 20000000000;
        private const long value = 100000000000000;


        public BlockchainInterface()
        {
            _account = new Account(PrivateKey);
            _web3 = new Web3(_account, Url);
            _bettingService = new BettingService(_web3, ContractAddress);
            Demo().Wait();
        }

        public static void Main()
        {
            Console.WriteLine("test");
        }


        public void Bet(string teamRepresentation)
        {
            _bettingService.BetRequestAndWaitForReceiptAsync(new BetFunction()
                { TeamRepresentation = teamRepresentation, Gas = gas, AmountToSend = value });
        }

        public BigInteger GetMoneyPool()
        {
            return _bettingService.GetMoneyPoolQueryAsync().Result;
        }


        async Task Demo()
        {
            try
            {
                // Setup
                // Here we're using local chain eg Geth https://github.com/Nethereum/TestChains#geth
                var account = new Account(PrivateKey);
                var web3 = new Web3(account, Url);

                //Console.WriteLine("Deploying...");
                //var deployment = new BettingDeployment();
                // var receipt = await BettingService.DeployContractAndWaitForReceiptAsync(web3, deployment);
                var service = new BettingService(web3, ContractAddress);
                //Console.WriteLine($"Contract Deployment Tx Status: {receipt.Status.Value}");
                //Console.WriteLine($"Contract Address: {service.ContractHandler.ContractAddress}");
                //Console.WriteLine("");

                Console.WriteLine("Sending a transaction to the function set()...");

                Console.WriteLine(GetMoneyPool());
                // var receiptForSetFunctionCall = await service.SetRequestAndWaitForReceiptAsync(
                //     new SetFunction() { X = 42, Gas = 400000 });
                // Console.WriteLine($"Finished storing an int: Tx Hash: {receiptForSetFunctionCall.TransactionHash}");
                // Console.WriteLine($"Finished storing an int: Tx Status: {receiptForSetFunctionCall.Status.Value}");
                // Console.WriteLine("");
                //
                // Console.WriteLine("Calling the function get()...");
                // var intValueFromGetFunctionCall = await service.GetQueryAsync();
                // Console.WriteLine($"Int value: {intValueFromGetFunctionCall} (expecting value 42)");
                // Console.WriteLine("");
                
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