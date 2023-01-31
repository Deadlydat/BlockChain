using System.Data.SqlClient;
using System.Data;
using static BCandSC_CSharp.Player;
using Nethereum.Contracts.Standards.ENS.PublicResolver.ContractDefinition;
using Nethereum.Web3;
using Nethereum.Web3.Accounts;

using Nethereum.Contracts;
using System.Numerics;
using Newtonsoft.Json.Linq;
using Nethereum.Hex.HexTypes;
using Nethereum.Signer;

namespace BCandSC_CSharp
{
    public class BlockchainInterface
    {
        private const string Abi = @"[
    {
      ""inputs"": [],
      ""stateMutability"": ""nonpayable"",
      ""type"": ""constructor""
    },
    {
      ""inputs"": [],
      ""name"": ""betAmount"",
      ""outputs"": [
        {
          ""internalType"": ""uint256"",
          ""name"": """",
          ""type"": ""uint256""
        }
      ],
      ""stateMutability"": ""view"",
      ""type"": ""function"",
      ""constant"": true
    },
    {
      ""inputs"": [],
      ""name"": ""moneyPool"",
      ""outputs"": [
        {
          ""internalType"": ""uint256"",
          ""name"": """",
          ""type"": ""uint256""
        }
      ],
      ""stateMutability"": ""view"",
      ""type"": ""function"",
      ""constant"": true
    },
    {
      ""inputs"": [],
      ""name"": ""owner"",
      ""outputs"": [
        {
          ""internalType"": ""address payable"",
          ""name"": """",
          ""type"": ""address""
        }
      ],
      ""stateMutability"": ""view"",
      ""type"": ""function"",
      ""constant"": true
    },
    {
      ""inputs"": [
        {
          ""internalType"": ""address"",
          ""name"": """",
          ""type"": ""address""
        }
      ],
      ""name"": ""playerInfo"",
      ""outputs"": [
        {
          ""internalType"": ""uint256"",
          ""name"": ""amountBet"",
          ""type"": ""uint256""
        },
        {
          ""internalType"": ""bytes32"",
          ""name"": ""hashOfTeam"",
          ""type"": ""bytes32""
        }
      ],
      ""stateMutability"": ""view"",
      ""type"": ""function"",
      ""constant"": true
    },
    {
      ""inputs"": [
        {
          ""internalType"": ""uint256"",
          ""name"": """",
          ""type"": ""uint256""
        }
      ],
      ""name"": ""players"",
      ""outputs"": [
        {
          ""internalType"": ""address payable"",
          ""name"": """",
          ""type"": ""address""
        }
      ],
      ""stateMutability"": ""view"",
      ""type"": ""function"",
      ""constant"": true
    },
    {
      ""inputs"": [],
      ""name"": ""getPlayerCount"",
      ""outputs"": [
        {
          ""internalType"": ""uint256"",
          ""name"": """",
          ""type"": ""uint256""
        }
      ],
      ""stateMutability"": ""view"",
      ""type"": ""function"",
      ""constant"": true
    },
    {
      ""inputs"": [],
      ""name"": ""kill"",
      ""outputs"": [],
      ""stateMutability"": ""nonpayable"",
      ""type"": ""function""
    },
    {
      ""inputs"": [
        {
          ""internalType"": ""string"",
          ""name"": ""teamRepresentation"",
          ""type"": ""string""
        }
      ],
      ""name"": ""bet"",
      ""outputs"": [],
      ""stateMutability"": ""payable"",
      ""type"": ""function"",
      ""payable"": true
    },
    {
      ""inputs"": [],
      ""name"": ""startGame"",
      ""outputs"": [],
      ""stateMutability"": ""nonpayable"",
      ""type"": ""function""
    },
    {
      ""inputs"": [],
      ""name"": ""finishGame"",
      ""outputs"": [],
      ""stateMutability"": ""nonpayable"",
      ""type"": ""function""
    },
    {
      ""inputs"": [
        {
          ""internalType"": ""string"",
          ""name"": ""teamRepresentationOfWinner"",
          ""type"": ""string""
        }
      ],
      ""name"": ""distributePrizes"",
      ""outputs"": [],
      ""stateMutability"": ""nonpayable"",
      ""type"": ""function""
    },
    {
      ""inputs"": [],
      ""name"": ""getMoneyPool"",
      ""outputs"": [
        {
          ""internalType"": ""uint256"",
          ""name"": """",
          ""type"": ""uint256""
        }
      ],
      ""stateMutability"": ""view"",
      ""type"": ""function"",
      ""constant"": true
    }
  ]";
        public BlockchainInterface()
        {
            Demo().Wait();
        }


        static async Task Demo()
        {
            try
            {


                const string url = "HTTP://192.168.178.27:7545";
                var gas = new HexBigInteger(new BigInteger(20000000000));
                var value = new HexBigInteger(new BigInteger(100000000000000));

                const string mainPrivateKey = "840af416508488a7ef3f7e822d71b381235bae6c1286f9203d1d89818a7dcb47";
                Web3 web3 = new Web3(url);
                const string contractAddress = "0x9314658625A4a4c212859Abd15D6A168aE6d6731";
                var mainAccount = new EthECKey(mainPrivateKey);


                Console.WriteLine("Getting Contract");

                Contract bettingContract = web3.Eth.GetContract(Abi, contractAddress);
                var betFunction = bettingContract.GetFunction("bet");

                var txManager = web3.Eth.TransactionManager;

                var betInput = "HAHA";
                var transactionInput =
                    betFunction.CreateTransactionInput(contractAddress, gas, new HexBigInteger(0), value, betInput);
                var signedTransaction = txManager.SignTransactionAsync(transactionInput);
                var transactionHash = web3.Eth.TransactionManager.SendTransactionAsync(transactionInput);
                Console.WriteLine(transactionHash);
                Console.WriteLine("Result = " + transactionHash.Result);
























                //Setup
                //Here we're using local chain eg Geth https://github.com/Nethereum/TestChains#geth
                //var url = "HTTP://192.168.178.27:7545";
                //var url = "http://localhost:8545";
                //var privateKey = "0xb5b1870957d373ef0eeffecc6e4812c0fd08f554b37b233526acc331bf1544f7";
                //var account = new Account(privateKey);
                //var web3 = new Web3(account, url);

                //Web3 web3 = new Web3(url);

                //var privateKey2 = "0x9314658625A4a4c212859Abd15D6A168aE6d6731";
                //var account2 = new Account(privateKey2);



                //Console.WriteLine("Getting Contract");

                //Contract voteContract = web3.Eth.GetContract(ABI, privateKey2);

                //string accountAddress = "0x9fca5e364e4702dD3eA148e604128B39E9AcA666";
                //HexBigInteger gas = new HexBigInteger(new BigInteger(20000000000));
                //HexBigInteger value = new HexBigInteger(new BigInteger(100000000000000));

                //Task<string> castVoteFunction = voteContract.GetFunction("bet").SendTransactionAsync(accountAddress, gas, value);



                //Console.WriteLine(castVoteFunction.Result);

                //castVoteFunction.Wait();




                //Console.WriteLine("Deploying...");
                //var deployment = new BettingDeployment();
                //var receipt = await BettingService.DeployContractAndWaitForReceiptAsync(web3, deployment);
                //var service = new BettingService(web3, receipt.ContractAddress);
                //Console.WriteLine($"Contract Deployment Tx Status: {receipt.Status.Value}");
                //Console.WriteLine($"Contract Address: {service.ContractHandler.ContractAddress}");
                //Console.WriteLine("");


                //Console.WriteLine("Sending a transaction to the function set()...");



                //var receiptForSetFunctionCall = await service.BetRequestAndWaitForReceiptAsync(
                //    new BetFunction() { TeamRepresentation = "Haga", Gas = 400000 });



                ////var receiptForSetFunctionCall = await service.SetRequestAndWaitForReceiptAsync(
                ////    new SetFunction() { X = 42, Gas = 400000 });
                //Console.WriteLine($"Finished storing an int: Tx Hash: {receiptForSetFunctionCall.TransactionHash}");
                //Console.WriteLine($"Finished storing an int: Tx Status: {receiptForSetFunctionCall.Status.Value}");
                //Console.WriteLine("");


                //var intValueFromGetFunctionCall = await service.GetMoneyPoolQueryAsync();
                //Console.WriteLine("money is " + intValueFromGetFunctionCall);

                ////Console.WriteLine("Calling the function get()...");
                ////var intValueFromGetFunctionCall = await service.GetQueryAsync();
                ////Console.WriteLine($"Int value: {intValueFromGetFunctionCall} (expecting value 42)");
                ////Console.WriteLine("");
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




