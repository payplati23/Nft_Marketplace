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
using StarboardNFTLibrary.Contracts.StarboardNFT1155.ContractDefinition;

namespace StarboardNFTLibrary.Contracts.StarboardNFT1155
{
    public partial class StarboardNFT1155Service
    {
        public static Task<TransactionReceipt> DeployContractAndWaitForReceiptAsync(Nethereum.Web3.Web3 web3, StarboardNFT1155Deployment starboardNFT1155Deployment, CancellationTokenSource cancellationTokenSource = null)
        {
            return web3.Eth.GetContractDeploymentHandler<StarboardNFT1155Deployment>().SendRequestAndWaitForReceiptAsync(starboardNFT1155Deployment, cancellationTokenSource);
        }

        public static Task<string> DeployContractAsync(Nethereum.Web3.Web3 web3, StarboardNFT1155Deployment starboardNFT1155Deployment)
        {
            return web3.Eth.GetContractDeploymentHandler<StarboardNFT1155Deployment>().SendRequestAsync(starboardNFT1155Deployment);
        }

        public static async Task<StarboardNFT1155Service> DeployContractAndGetServiceAsync(Nethereum.Web3.Web3 web3, StarboardNFT1155Deployment starboardNFT1155Deployment, CancellationTokenSource cancellationTokenSource = null)
        {
            var receipt = await DeployContractAndWaitForReceiptAsync(web3, starboardNFT1155Deployment, cancellationTokenSource);
            return new StarboardNFT1155Service(web3, receipt.ContractAddress);
        }

        protected Nethereum.Web3.Web3 Web3{ get; }

        public ContractHandler ContractHandler { get; }

        public StarboardNFT1155Service(Nethereum.Web3.Web3 web3, string contractAddress)
        {
            Web3 = web3;
            ContractHandler = web3.Eth.GetContractHandler(contractAddress);
        }

        public Task<BigInteger> BalanceOfQueryAsync(BalanceOfFunction balanceOfFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<BalanceOfFunction, BigInteger>(balanceOfFunction, blockParameter);
        }

        
        public Task<BigInteger> BalanceOfQueryAsync(string account, BigInteger id, BlockParameter blockParameter = null)
        {
            var balanceOfFunction = new BalanceOfFunction();
                balanceOfFunction.Account = account;
                balanceOfFunction.Id = id;
            
            return ContractHandler.QueryAsync<BalanceOfFunction, BigInteger>(balanceOfFunction, blockParameter);
        }

        public Task<List<BigInteger>> BalanceOfBatchQueryAsync(BalanceOfBatchFunction balanceOfBatchFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<BalanceOfBatchFunction, List<BigInteger>>(balanceOfBatchFunction, blockParameter);
        }

        
        public Task<List<BigInteger>> BalanceOfBatchQueryAsync(List<string> accounts, List<BigInteger> ids, BlockParameter blockParameter = null)
        {
            var balanceOfBatchFunction = new BalanceOfBatchFunction();
                balanceOfBatchFunction.Accounts = accounts;
                balanceOfBatchFunction.Ids = ids;
            
            return ContractHandler.QueryAsync<BalanceOfBatchFunction, List<BigInteger>>(balanceOfBatchFunction, blockParameter);
        }

        public Task<string> CreateTokenRequestAsync(CreateTokenFunction createTokenFunction)
        {
             return ContractHandler.SendRequestAsync(createTokenFunction);
        }

        public Task<TransactionReceipt> CreateTokenRequestAndWaitForReceiptAsync(CreateTokenFunction createTokenFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(createTokenFunction, cancellationToken);
        }

        public Task<string> CreateTokenRequestAsync(BigInteger amount)
        {
            var createTokenFunction = new CreateTokenFunction();
                createTokenFunction.Amount = amount;
            
             return ContractHandler.SendRequestAsync(createTokenFunction);
        }

        public Task<TransactionReceipt> CreateTokenRequestAndWaitForReceiptAsync(BigInteger amount, CancellationTokenSource cancellationToken = null)
        {
            var createTokenFunction = new CreateTokenFunction();
                createTokenFunction.Amount = amount;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(createTokenFunction, cancellationToken);
        }

        public Task<bool> IsApprovedForAllQueryAsync(IsApprovedForAllFunction isApprovedForAllFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<IsApprovedForAllFunction, bool>(isApprovedForAllFunction, blockParameter);
        }

        
        public Task<bool> IsApprovedForAllQueryAsync(string account, string @operator, BlockParameter blockParameter = null)
        {
            var isApprovedForAllFunction = new IsApprovedForAllFunction();
                isApprovedForAllFunction.Account = account;
                isApprovedForAllFunction.Operator = @operator;
            
            return ContractHandler.QueryAsync<IsApprovedForAllFunction, bool>(isApprovedForAllFunction, blockParameter);
        }

        public Task<string> OnERC1155BatchReceivedRequestAsync(OnERC1155BatchReceivedFunction onERC1155BatchReceivedFunction)
        {
             return ContractHandler.SendRequestAsync(onERC1155BatchReceivedFunction);
        }

        public Task<TransactionReceipt> OnERC1155BatchReceivedRequestAndWaitForReceiptAsync(OnERC1155BatchReceivedFunction onERC1155BatchReceivedFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(onERC1155BatchReceivedFunction, cancellationToken);
        }

        public Task<string> OnERC1155BatchReceivedRequestAsync(string returnValue1, string returnValue2, List<BigInteger> returnValue3, List<BigInteger> returnValue4, byte[] returnValue5)
        {
            var onERC1155BatchReceivedFunction = new OnERC1155BatchReceivedFunction();
                onERC1155BatchReceivedFunction.ReturnValue1 = returnValue1;
                onERC1155BatchReceivedFunction.ReturnValue2 = returnValue2;
                onERC1155BatchReceivedFunction.ReturnValue3 = returnValue3;
                onERC1155BatchReceivedFunction.ReturnValue4 = returnValue4;
                onERC1155BatchReceivedFunction.ReturnValue5 = returnValue5;
            
             return ContractHandler.SendRequestAsync(onERC1155BatchReceivedFunction);
        }

        public Task<TransactionReceipt> OnERC1155BatchReceivedRequestAndWaitForReceiptAsync(string returnValue1, string returnValue2, List<BigInteger> returnValue3, List<BigInteger> returnValue4, byte[] returnValue5, CancellationTokenSource cancellationToken = null)
        {
            var onERC1155BatchReceivedFunction = new OnERC1155BatchReceivedFunction();
                onERC1155BatchReceivedFunction.ReturnValue1 = returnValue1;
                onERC1155BatchReceivedFunction.ReturnValue2 = returnValue2;
                onERC1155BatchReceivedFunction.ReturnValue3 = returnValue3;
                onERC1155BatchReceivedFunction.ReturnValue4 = returnValue4;
                onERC1155BatchReceivedFunction.ReturnValue5 = returnValue5;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(onERC1155BatchReceivedFunction, cancellationToken);
        }

        public Task<string> OnERC1155ReceivedRequestAsync(OnERC1155ReceivedFunction onERC1155ReceivedFunction)
        {
             return ContractHandler.SendRequestAsync(onERC1155ReceivedFunction);
        }

        public Task<TransactionReceipt> OnERC1155ReceivedRequestAndWaitForReceiptAsync(OnERC1155ReceivedFunction onERC1155ReceivedFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(onERC1155ReceivedFunction, cancellationToken);
        }

        public Task<string> OnERC1155ReceivedRequestAsync(string returnValue1, string returnValue2, BigInteger returnValue3, BigInteger returnValue4, byte[] returnValue5)
        {
            var onERC1155ReceivedFunction = new OnERC1155ReceivedFunction();
                onERC1155ReceivedFunction.ReturnValue1 = returnValue1;
                onERC1155ReceivedFunction.ReturnValue2 = returnValue2;
                onERC1155ReceivedFunction.ReturnValue3 = returnValue3;
                onERC1155ReceivedFunction.ReturnValue4 = returnValue4;
                onERC1155ReceivedFunction.ReturnValue5 = returnValue5;
            
             return ContractHandler.SendRequestAsync(onERC1155ReceivedFunction);
        }

        public Task<TransactionReceipt> OnERC1155ReceivedRequestAndWaitForReceiptAsync(string returnValue1, string returnValue2, BigInteger returnValue3, BigInteger returnValue4, byte[] returnValue5, CancellationTokenSource cancellationToken = null)
        {
            var onERC1155ReceivedFunction = new OnERC1155ReceivedFunction();
                onERC1155ReceivedFunction.ReturnValue1 = returnValue1;
                onERC1155ReceivedFunction.ReturnValue2 = returnValue2;
                onERC1155ReceivedFunction.ReturnValue3 = returnValue3;
                onERC1155ReceivedFunction.ReturnValue4 = returnValue4;
                onERC1155ReceivedFunction.ReturnValue5 = returnValue5;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(onERC1155ReceivedFunction, cancellationToken);
        }

        public Task<string> SafeBatchTransferFromRequestAsync(SafeBatchTransferFromFunction safeBatchTransferFromFunction)
        {
             return ContractHandler.SendRequestAsync(safeBatchTransferFromFunction);
        }

        public Task<TransactionReceipt> SafeBatchTransferFromRequestAndWaitForReceiptAsync(SafeBatchTransferFromFunction safeBatchTransferFromFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(safeBatchTransferFromFunction, cancellationToken);
        }

        public Task<string> SafeBatchTransferFromRequestAsync(string from, string to, List<BigInteger> ids, List<BigInteger> amounts, byte[] data)
        {
            var safeBatchTransferFromFunction = new SafeBatchTransferFromFunction();
                safeBatchTransferFromFunction.From = from;
                safeBatchTransferFromFunction.To = to;
                safeBatchTransferFromFunction.Ids = ids;
                safeBatchTransferFromFunction.Amounts = amounts;
                safeBatchTransferFromFunction.Data = data;
            
             return ContractHandler.SendRequestAsync(safeBatchTransferFromFunction);
        }

        public Task<TransactionReceipt> SafeBatchTransferFromRequestAndWaitForReceiptAsync(string from, string to, List<BigInteger> ids, List<BigInteger> amounts, byte[] data, CancellationTokenSource cancellationToken = null)
        {
            var safeBatchTransferFromFunction = new SafeBatchTransferFromFunction();
                safeBatchTransferFromFunction.From = from;
                safeBatchTransferFromFunction.To = to;
                safeBatchTransferFromFunction.Ids = ids;
                safeBatchTransferFromFunction.Amounts = amounts;
                safeBatchTransferFromFunction.Data = data;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(safeBatchTransferFromFunction, cancellationToken);
        }

        public Task<string> SafeTransferFromRequestAsync(SafeTransferFromFunction safeTransferFromFunction)
        {
             return ContractHandler.SendRequestAsync(safeTransferFromFunction);
        }

        public Task<TransactionReceipt> SafeTransferFromRequestAndWaitForReceiptAsync(SafeTransferFromFunction safeTransferFromFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(safeTransferFromFunction, cancellationToken);
        }

        public Task<string> SafeTransferFromRequestAsync(string from, string to, BigInteger id, BigInteger amount, byte[] data)
        {
            var safeTransferFromFunction = new SafeTransferFromFunction();
                safeTransferFromFunction.From = from;
                safeTransferFromFunction.To = to;
                safeTransferFromFunction.Id = id;
                safeTransferFromFunction.Amount = amount;
                safeTransferFromFunction.Data = data;
            
             return ContractHandler.SendRequestAsync(safeTransferFromFunction);
        }

        public Task<TransactionReceipt> SafeTransferFromRequestAndWaitForReceiptAsync(string from, string to, BigInteger id, BigInteger amount, byte[] data, CancellationTokenSource cancellationToken = null)
        {
            var safeTransferFromFunction = new SafeTransferFromFunction();
                safeTransferFromFunction.From = from;
                safeTransferFromFunction.To = to;
                safeTransferFromFunction.Id = id;
                safeTransferFromFunction.Amount = amount;
                safeTransferFromFunction.Data = data;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(safeTransferFromFunction, cancellationToken);
        }

        public Task<string> SetApprovalForAllRequestAsync(SetApprovalForAllFunction setApprovalForAllFunction)
        {
             return ContractHandler.SendRequestAsync(setApprovalForAllFunction);
        }

        public Task<TransactionReceipt> SetApprovalForAllRequestAndWaitForReceiptAsync(SetApprovalForAllFunction setApprovalForAllFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(setApprovalForAllFunction, cancellationToken);
        }

        public Task<string> SetApprovalForAllRequestAsync(string @operator, bool approved)
        {
            var setApprovalForAllFunction = new SetApprovalForAllFunction();
                setApprovalForAllFunction.Operator = @operator;
                setApprovalForAllFunction.Approved = approved;
            
             return ContractHandler.SendRequestAsync(setApprovalForAllFunction);
        }

        public Task<TransactionReceipt> SetApprovalForAllRequestAndWaitForReceiptAsync(string @operator, bool approved, CancellationTokenSource cancellationToken = null)
        {
            var setApprovalForAllFunction = new SetApprovalForAllFunction();
                setApprovalForAllFunction.Operator = @operator;
                setApprovalForAllFunction.Approved = approved;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(setApprovalForAllFunction, cancellationToken);
        }

        public Task<bool> SupportsInterfaceQueryAsync(SupportsInterfaceFunction supportsInterfaceFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<SupportsInterfaceFunction, bool>(supportsInterfaceFunction, blockParameter);
        }

        
        public Task<bool> SupportsInterfaceQueryAsync(byte[] interfaceId, BlockParameter blockParameter = null)
        {
            var supportsInterfaceFunction = new SupportsInterfaceFunction();
                supportsInterfaceFunction.InterfaceId = interfaceId;
            
            return ContractHandler.QueryAsync<SupportsInterfaceFunction, bool>(supportsInterfaceFunction, blockParameter);
        }

        public Task<string> TransferTokenRequestAsync(TransferTokenFunction transferTokenFunction)
        {
             return ContractHandler.SendRequestAsync(transferTokenFunction);
        }

        public Task<TransactionReceipt> TransferTokenRequestAndWaitForReceiptAsync(TransferTokenFunction transferTokenFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(transferTokenFunction, cancellationToken);
        }

        public Task<string> TransferTokenRequestAsync(string from, string to, BigInteger tokenId, BigInteger amount)
        {
            var transferTokenFunction = new TransferTokenFunction();
                transferTokenFunction.From = from;
                transferTokenFunction.To = to;
                transferTokenFunction.TokenId = tokenId;
                transferTokenFunction.Amount = amount;
            
             return ContractHandler.SendRequestAsync(transferTokenFunction);
        }

        public Task<TransactionReceipt> TransferTokenRequestAndWaitForReceiptAsync(string from, string to, BigInteger tokenId, BigInteger amount, CancellationTokenSource cancellationToken = null)
        {
            var transferTokenFunction = new TransferTokenFunction();
                transferTokenFunction.From = from;
                transferTokenFunction.To = to;
                transferTokenFunction.TokenId = tokenId;
                transferTokenFunction.Amount = amount;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(transferTokenFunction, cancellationToken);
        }

        public Task<string> UriQueryAsync(UriFunction uriFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<UriFunction, string>(uriFunction, blockParameter);
        }

        
        public Task<string> UriQueryAsync(BigInteger returnValue1, BlockParameter blockParameter = null)
        {
            var uriFunction = new UriFunction();
                uriFunction.ReturnValue1 = returnValue1;
            
            return ContractHandler.QueryAsync<UriFunction, string>(uriFunction, blockParameter);
        }
    }
}
