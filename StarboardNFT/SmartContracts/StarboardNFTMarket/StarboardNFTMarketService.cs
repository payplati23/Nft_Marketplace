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
using StarboardNFTLibrary.Contracts.StarboardNFTMarket.ContractDefinition;

namespace StarboardNFTLibrary.Contracts.StarboardNFTMarket
{
    public partial class StarboardNFTMarketService
    {
        public static Task<TransactionReceipt> DeployContractAndWaitForReceiptAsync(Nethereum.Web3.Web3 web3, StarboardNFTMarketDeployment starboardNFTMarketDeployment, CancellationTokenSource cancellationTokenSource = null)
        {
            return web3.Eth.GetContractDeploymentHandler<StarboardNFTMarketDeployment>().SendRequestAndWaitForReceiptAsync(starboardNFTMarketDeployment, cancellationTokenSource);
        }

        public static Task<string> DeployContractAsync(Nethereum.Web3.Web3 web3, StarboardNFTMarketDeployment starboardNFTMarketDeployment)
        {
            return web3.Eth.GetContractDeploymentHandler<StarboardNFTMarketDeployment>().SendRequestAsync(starboardNFTMarketDeployment);
        }

        public static async Task<StarboardNFTMarketService> DeployContractAndGetServiceAsync(Nethereum.Web3.Web3 web3, StarboardNFTMarketDeployment starboardNFTMarketDeployment, CancellationTokenSource cancellationTokenSource = null)
        {
            var receipt = await DeployContractAndWaitForReceiptAsync(web3, starboardNFTMarketDeployment, cancellationTokenSource);
            return new StarboardNFTMarketService(web3, receipt.ContractAddress);
        }

        protected Nethereum.Web3.Web3 Web3{ get; }

        public ContractHandler ContractHandler { get; }

        public StarboardNFTMarketService(Nethereum.Web3.Web3 web3, string contractAddress)
        {
            Web3 = web3;
            ContractHandler = web3.Eth.GetContractHandler(contractAddress);
        }

        public Task<string> CreateMarketItemRequestAsync(CreateMarketItemFunction createMarketItemFunction)
        {
             return ContractHandler.SendRequestAsync(createMarketItemFunction);
        }

        public Task<TransactionReceipt> CreateMarketItemRequestAndWaitForReceiptAsync(CreateMarketItemFunction createMarketItemFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(createMarketItemFunction, cancellationToken);
        }

        public Task<string> CreateMarketItemRequestAsync(string nftContract, BigInteger tokenId, BigInteger price, BigInteger royalty)
        {
            var createMarketItemFunction = new CreateMarketItemFunction();
                createMarketItemFunction.NftContract = nftContract;
                createMarketItemFunction.TokenId = tokenId;
                createMarketItemFunction.Price = price;
                createMarketItemFunction.Royalty = royalty;
            
             return ContractHandler.SendRequestAsync(createMarketItemFunction);
        }

        public Task<TransactionReceipt> CreateMarketItemRequestAndWaitForReceiptAsync(string nftContract, BigInteger tokenId, BigInteger price, BigInteger royalty, CancellationTokenSource cancellationToken = null)
        {
            var createMarketItemFunction = new CreateMarketItemFunction();
                createMarketItemFunction.NftContract = nftContract;
                createMarketItemFunction.TokenId = tokenId;
                createMarketItemFunction.Price = price;
                createMarketItemFunction.Royalty = royalty;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(createMarketItemFunction, cancellationToken);
        }

        public Task<string> CreateMarketSaleRequestAsync(CreateMarketSaleFunction createMarketSaleFunction)
        {
             return ContractHandler.SendRequestAsync(createMarketSaleFunction);
        }

        public Task<TransactionReceipt> CreateMarketSaleRequestAndWaitForReceiptAsync(CreateMarketSaleFunction createMarketSaleFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(createMarketSaleFunction, cancellationToken);
        }

        public Task<string> CreateMarketSaleRequestAsync(string nftContract, BigInteger itemId)
        {
            var createMarketSaleFunction = new CreateMarketSaleFunction();
                createMarketSaleFunction.NftContract = nftContract;
                createMarketSaleFunction.ItemId = itemId;
            
             return ContractHandler.SendRequestAsync(createMarketSaleFunction);
        }

        public Task<TransactionReceipt> CreateMarketSaleRequestAndWaitForReceiptAsync(string nftContract, BigInteger itemId, CancellationTokenSource cancellationToken = null)
        {
            var createMarketSaleFunction = new CreateMarketSaleFunction();
                createMarketSaleFunction.NftContract = nftContract;
                createMarketSaleFunction.ItemId = itemId;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(createMarketSaleFunction, cancellationToken);
        }

        public Task<string> CreateMultiMarketItemRequestAsync(CreateMultiMarketItemFunction createMultiMarketItemFunction)
        {
             return ContractHandler.SendRequestAsync(createMultiMarketItemFunction);
        }

        public Task<TransactionReceipt> CreateMultiMarketItemRequestAndWaitForReceiptAsync(CreateMultiMarketItemFunction createMultiMarketItemFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(createMultiMarketItemFunction, cancellationToken);
        }

        public Task<string> CreateMultiMarketItemRequestAsync(string nft1155Contract, BigInteger tokenId, BigInteger price, BigInteger royalty, BigInteger amount)
        {
            var createMultiMarketItemFunction = new CreateMultiMarketItemFunction();
                createMultiMarketItemFunction.Nft1155Contract = nft1155Contract;
                createMultiMarketItemFunction.TokenId = tokenId;
                createMultiMarketItemFunction.Price = price;
                createMultiMarketItemFunction.Royalty = royalty;
                createMultiMarketItemFunction.Amount = amount;
            
             return ContractHandler.SendRequestAsync(createMultiMarketItemFunction);
        }

        public Task<TransactionReceipt> CreateMultiMarketItemRequestAndWaitForReceiptAsync(string nft1155Contract, BigInteger tokenId, BigInteger price, BigInteger royalty, BigInteger amount, CancellationTokenSource cancellationToken = null)
        {
            var createMultiMarketItemFunction = new CreateMultiMarketItemFunction();
                createMultiMarketItemFunction.Nft1155Contract = nft1155Contract;
                createMultiMarketItemFunction.TokenId = tokenId;
                createMultiMarketItemFunction.Price = price;
                createMultiMarketItemFunction.Royalty = royalty;
                createMultiMarketItemFunction.Amount = amount;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(createMultiMarketItemFunction, cancellationToken);
        }

        public Task<string> CreateMultiMarketSaleRequestAsync(CreateMultiMarketSaleFunction createMultiMarketSaleFunction)
        {
             return ContractHandler.SendRequestAsync(createMultiMarketSaleFunction);
        }

        public Task<TransactionReceipt> CreateMultiMarketSaleRequestAndWaitForReceiptAsync(CreateMultiMarketSaleFunction createMultiMarketSaleFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(createMultiMarketSaleFunction, cancellationToken);
        }

        public Task<string> CreateMultiMarketSaleRequestAsync(string nft1155Contract, List<BigInteger> itemIds, BigInteger tokenId, BigInteger amount)
        {
            var createMultiMarketSaleFunction = new CreateMultiMarketSaleFunction();
                createMultiMarketSaleFunction.Nft1155Contract = nft1155Contract;
                createMultiMarketSaleFunction.ItemIds = itemIds;
                createMultiMarketSaleFunction.TokenId = tokenId;
                createMultiMarketSaleFunction.Amount = amount;
            
             return ContractHandler.SendRequestAsync(createMultiMarketSaleFunction);
        }

        public Task<TransactionReceipt> CreateMultiMarketSaleRequestAndWaitForReceiptAsync(string nft1155Contract, List<BigInteger> itemIds, BigInteger tokenId, BigInteger amount, CancellationTokenSource cancellationToken = null)
        {
            var createMultiMarketSaleFunction = new CreateMultiMarketSaleFunction();
                createMultiMarketSaleFunction.Nft1155Contract = nft1155Contract;
                createMultiMarketSaleFunction.ItemIds = itemIds;
                createMultiMarketSaleFunction.TokenId = tokenId;
                createMultiMarketSaleFunction.Amount = amount;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(createMultiMarketSaleFunction, cancellationToken);
        }

        public Task<FetchItemsCreatedOutputDTO> FetchItemsCreatedQueryAsync(FetchItemsCreatedFunction fetchItemsCreatedFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryDeserializingToObjectAsync<FetchItemsCreatedFunction, FetchItemsCreatedOutputDTO>(fetchItemsCreatedFunction, blockParameter);
        }

        public Task<FetchItemsCreatedOutputDTO> FetchItemsCreatedQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryDeserializingToObjectAsync<FetchItemsCreatedFunction, FetchItemsCreatedOutputDTO>(null, blockParameter);
        }

        public Task<FetchMarketItemsOutputDTO> FetchMarketItemsQueryAsync(FetchMarketItemsFunction fetchMarketItemsFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryDeserializingToObjectAsync<FetchMarketItemsFunction, FetchMarketItemsOutputDTO>(fetchMarketItemsFunction, blockParameter);
        }

        public Task<FetchMarketItemsOutputDTO> FetchMarketItemsQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryDeserializingToObjectAsync<FetchMarketItemsFunction, FetchMarketItemsOutputDTO>(null, blockParameter);
        }

        public Task<FetchMyNFTsOutputDTO> FetchMyNFTsQueryAsync(FetchMyNFTsFunction fetchMyNFTsFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryDeserializingToObjectAsync<FetchMyNFTsFunction, FetchMyNFTsOutputDTO>(fetchMyNFTsFunction, blockParameter);
        }

        public Task<FetchMyNFTsOutputDTO> FetchMyNFTsQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryDeserializingToObjectAsync<FetchMyNFTsFunction, FetchMyNFTsOutputDTO>(null, blockParameter);
        }

        public Task<BigInteger> GetListingPriceQueryAsync(GetListingPriceFunction getListingPriceFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<GetListingPriceFunction, BigInteger>(getListingPriceFunction, blockParameter);
        }

        
        public Task<BigInteger> GetListingPriceQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<GetListingPriceFunction, BigInteger>(null, blockParameter);
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

        public Task<string> PutItemToResellRequestAsync(PutItemToResellFunction putItemToResellFunction)
        {
             return ContractHandler.SendRequestAsync(putItemToResellFunction);
        }

        public Task<TransactionReceipt> PutItemToResellRequestAndWaitForReceiptAsync(PutItemToResellFunction putItemToResellFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(putItemToResellFunction, cancellationToken);
        }

        public Task<string> PutItemToResellRequestAsync(string nftContract, BigInteger itemId, BigInteger newPrice)
        {
            var putItemToResellFunction = new PutItemToResellFunction();
                putItemToResellFunction.NftContract = nftContract;
                putItemToResellFunction.ItemId = itemId;
                putItemToResellFunction.NewPrice = newPrice;
            
             return ContractHandler.SendRequestAsync(putItemToResellFunction);
        }

        public Task<TransactionReceipt> PutItemToResellRequestAndWaitForReceiptAsync(string nftContract, BigInteger itemId, BigInteger newPrice, CancellationTokenSource cancellationToken = null)
        {
            var putItemToResellFunction = new PutItemToResellFunction();
                putItemToResellFunction.NftContract = nftContract;
                putItemToResellFunction.ItemId = itemId;
                putItemToResellFunction.NewPrice = newPrice;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(putItemToResellFunction, cancellationToken);
        }

        public Task<string> PutMultiItemsToResellRequestAsync(PutMultiItemsToResellFunction putMultiItemsToResellFunction)
        {
             return ContractHandler.SendRequestAsync(putMultiItemsToResellFunction);
        }

        public Task<TransactionReceipt> PutMultiItemsToResellRequestAndWaitForReceiptAsync(PutMultiItemsToResellFunction putMultiItemsToResellFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(putMultiItemsToResellFunction, cancellationToken);
        }

        public Task<string> PutMultiItemsToResellRequestAsync(string nft1155Contract, List<BigInteger> itemIds, BigInteger amount, BigInteger newPrice)
        {
            var putMultiItemsToResellFunction = new PutMultiItemsToResellFunction();
                putMultiItemsToResellFunction.Nft1155Contract = nft1155Contract;
                putMultiItemsToResellFunction.ItemIds = itemIds;
                putMultiItemsToResellFunction.Amount = amount;
                putMultiItemsToResellFunction.NewPrice = newPrice;
            
             return ContractHandler.SendRequestAsync(putMultiItemsToResellFunction);
        }

        public Task<TransactionReceipt> PutMultiItemsToResellRequestAndWaitForReceiptAsync(string nft1155Contract, List<BigInteger> itemIds, BigInteger amount, BigInteger newPrice, CancellationTokenSource cancellationToken = null)
        {
            var putMultiItemsToResellFunction = new PutMultiItemsToResellFunction();
                putMultiItemsToResellFunction.Nft1155Contract = nft1155Contract;
                putMultiItemsToResellFunction.ItemIds = itemIds;
                putMultiItemsToResellFunction.Amount = amount;
                putMultiItemsToResellFunction.NewPrice = newPrice;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(putMultiItemsToResellFunction, cancellationToken);
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
    }
}
