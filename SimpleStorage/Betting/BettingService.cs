using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Numerics;
using Nethereum.Hex.HexTypes;
using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Web3;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Contracts.CQS;
using Nethereum.Contracts.ContractHandlers;
using Nethereum.Contracts;
using System.Threading;
using BettingContract.Betting.ContractDefinition;

namespace BettingContract.Betting
{
    public partial class BettingService
    {
        public static Task<TransactionReceipt> DeployContractAndWaitForReceiptAsync(Nethereum.Web3.Web3 web3, BettingDeployment bettingDeployment, CancellationTokenSource cancellationTokenSource = null)
        {
            return web3.Eth.GetContractDeploymentHandler<BettingDeployment>().SendRequestAndWaitForReceiptAsync(bettingDeployment, cancellationTokenSource);
        }

        public static Task<string> DeployContractAsync(Nethereum.Web3.Web3 web3, BettingDeployment bettingDeployment)
        {
            return web3.Eth.GetContractDeploymentHandler<BettingDeployment>().SendRequestAsync(bettingDeployment);
        }

        public static async Task<BettingService> DeployContractAndGetServiceAsync(Nethereum.Web3.Web3 web3, BettingDeployment bettingDeployment, CancellationTokenSource cancellationTokenSource = null)
        {
            var receipt = await DeployContractAndWaitForReceiptAsync(web3, bettingDeployment, cancellationTokenSource);
            return new BettingService(web3, receipt.ContractAddress);
        }

        protected Nethereum.Web3.IWeb3 Web3{ get; }

        public ContractHandler ContractHandler { get; }

        public BettingService(Nethereum.Web3.Web3 web3, string contractAddress)
        {
            Web3 = web3;
            ContractHandler = web3.Eth.GetContractHandler(contractAddress);
        }

        public BettingService(Nethereum.Web3.IWeb3 web3, string contractAddress)
        {
            Web3 = web3;
            ContractHandler = web3.Eth.GetContractHandler(contractAddress);
        }

        public Task<string> BetRequestAsync(BetFunction betFunction)
        {
             return ContractHandler.SendRequestAsync(betFunction);
        }

        public Task<TransactionReceipt> BetRequestAndWaitForReceiptAsync(BetFunction betFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(betFunction, cancellationToken);
        }

        public Task<string> BetRequestAsync(string teamRepresentation)
        {
            var betFunction = new BetFunction();
                betFunction.TeamRepresentation = teamRepresentation;
            
             return ContractHandler.SendRequestAsync(betFunction);
        }

        public Task<TransactionReceipt> BetRequestAndWaitForReceiptAsync(string teamRepresentation, CancellationTokenSource cancellationToken = null)
        {
            var betFunction = new BetFunction();
                betFunction.TeamRepresentation = teamRepresentation;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(betFunction, cancellationToken);
        }

        public Task<string> DistributePrizesRequestAsync(DistributePrizesFunction distributePrizesFunction)
        {
             return ContractHandler.SendRequestAsync(distributePrizesFunction);
        }

        public Task<TransactionReceipt> DistributePrizesRequestAndWaitForReceiptAsync(DistributePrizesFunction distributePrizesFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(distributePrizesFunction, cancellationToken);
        }

        public Task<string> DistributePrizesRequestAsync(List<string> teamRepresentationOfWinners)
        {
            var distributePrizesFunction = new DistributePrizesFunction();
                distributePrizesFunction.TeamRepresentationOfWinners = teamRepresentationOfWinners;
            
             return ContractHandler.SendRequestAsync(distributePrizesFunction);
        }

        public Task<TransactionReceipt> DistributePrizesRequestAndWaitForReceiptAsync(List<string> teamRepresentationOfWinners, CancellationTokenSource cancellationToken = null)
        {
            var distributePrizesFunction = new DistributePrizesFunction();
                distributePrizesFunction.TeamRepresentationOfWinners = teamRepresentationOfWinners;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(distributePrizesFunction, cancellationToken);
        }

        public Task<string> FinishGameRequestAsync(FinishGameFunction finishGameFunction)
        {
             return ContractHandler.SendRequestAsync(finishGameFunction);
        }

        public Task<string> FinishGameRequestAsync()
        {
             return ContractHandler.SendRequestAsync<FinishGameFunction>();
        }

        public Task<TransactionReceipt> FinishGameRequestAndWaitForReceiptAsync(FinishGameFunction finishGameFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(finishGameFunction, cancellationToken);
        }

        public Task<TransactionReceipt> FinishGameRequestAndWaitForReceiptAsync(CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync<FinishGameFunction>(null, cancellationToken);
        }

        public Task<BigInteger> GetMoneyPoolQueryAsync(GetMoneyPoolFunction getMoneyPoolFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<GetMoneyPoolFunction, BigInteger>(getMoneyPoolFunction, blockParameter);
        }

        
        public Task<BigInteger> GetMoneyPoolQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<GetMoneyPoolFunction, BigInteger>(null, blockParameter);
        }

        public Task<BigInteger> GetPlayerCountQueryAsync(GetPlayerCountFunction getPlayerCountFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<GetPlayerCountFunction, BigInteger>(getPlayerCountFunction, blockParameter);
        }

        
        public Task<BigInteger> GetPlayerCountQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<GetPlayerCountFunction, BigInteger>(null, blockParameter);
        }

        public Task<string> GiveMoneyBackRequestAsync(GiveMoneyBackFunction giveMoneyBackFunction)
        {
             return ContractHandler.SendRequestAsync(giveMoneyBackFunction);
        }

        public Task<string> GiveMoneyBackRequestAsync()
        {
             return ContractHandler.SendRequestAsync<GiveMoneyBackFunction>();
        }

        public Task<TransactionReceipt> GiveMoneyBackRequestAndWaitForReceiptAsync(GiveMoneyBackFunction giveMoneyBackFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(giveMoneyBackFunction, cancellationToken);
        }

        public Task<TransactionReceipt> GiveMoneyBackRequestAndWaitForReceiptAsync(CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync<GiveMoneyBackFunction>(null, cancellationToken);
        }

        public Task<string> KillRequestAsync(KillFunction killFunction)
        {
             return ContractHandler.SendRequestAsync(killFunction);
        }

        public Task<string> KillRequestAsync()
        {
             return ContractHandler.SendRequestAsync<KillFunction>();
        }

        public Task<TransactionReceipt> KillRequestAndWaitForReceiptAsync(KillFunction killFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(killFunction, cancellationToken);
        }

        public Task<TransactionReceipt> KillRequestAndWaitForReceiptAsync(CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync<KillFunction>(null, cancellationToken);
        }

        public Task<string> OwnerQueryAsync(OwnerFunction ownerFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<OwnerFunction, string>(ownerFunction, blockParameter);
        }

        
        public Task<string> OwnerQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<OwnerFunction, string>(null, blockParameter);
        }

        public Task<string> StartGameRequestAsync(StartGameFunction startGameFunction)
        {
             return ContractHandler.SendRequestAsync(startGameFunction);
        }

        public Task<string> StartGameRequestAsync()
        {
             return ContractHandler.SendRequestAsync<StartGameFunction>();
        }

        public Task<TransactionReceipt> StartGameRequestAndWaitForReceiptAsync(StartGameFunction startGameFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(startGameFunction, cancellationToken);
        }

        public Task<TransactionReceipt> StartGameRequestAndWaitForReceiptAsync(CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync<StartGameFunction>(null, cancellationToken);
        }

        public Task<string> TransferOwnershipRequestAsync(TransferOwnershipFunction transferOwnershipFunction)
        {
             return ContractHandler.SendRequestAsync(transferOwnershipFunction);
        }

        public Task<TransactionReceipt> TransferOwnershipRequestAndWaitForReceiptAsync(TransferOwnershipFunction transferOwnershipFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(transferOwnershipFunction, cancellationToken);
        }

        public Task<string> TransferOwnershipRequestAsync(string newOwner)
        {
            var transferOwnershipFunction = new TransferOwnershipFunction();
                transferOwnershipFunction.NewOwner = newOwner;
            
             return ContractHandler.SendRequestAsync(transferOwnershipFunction);
        }

        public Task<TransactionReceipt> TransferOwnershipRequestAndWaitForReceiptAsync(string newOwner, CancellationTokenSource cancellationToken = null)
        {
            var transferOwnershipFunction = new TransferOwnershipFunction();
                transferOwnershipFunction.NewOwner = newOwner;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(transferOwnershipFunction, cancellationToken);
        }
    }
}
