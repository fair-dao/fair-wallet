
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using fair.extensions.wallet.entity;
using fair.extensions.wallet.entity.tron;
using System.Net.Http.Json;
using fair.extensions.shared;
using Nethereum.Web3;
using BulmaRazor.Components;
using fair.extensions.wallet.entity.tron;
using Nethereum.BlockchainProcessing.BlockStorage.Entities.Mapping;
using System.Linq.Expressions;
using Nethereum.Contracts.Standards.ERC20;
using fair.extensions.wallet.entity;
using Nethereum.Model;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Util;
using Org.BouncyCastle.Cms;
using Nethereum.Contracts.Standards.ERC20.ContractDefinition;
using Nethereum.RPC.Eth.Mappers;
using Nethereum.RPC.TransactionManagers;
using System.Threading;
using Nethereum.Web3.Accounts;
using Nethereum.Hex.HexTypes;
using System.Numerics;

namespace fair.extensions.wallet.services.wallet.eth
{
    public class EthHttpClient: WalletProcesser
    {
        public EthHttpClient(Chain chain) : base(chain)
        {
        }


        /// <summary>
        ///  创建平台币交易对象
        /// </summary>
        /// <param name="owner_address"></param>
        /// <param name="to_address"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        public override async Task<TransactionData> CreateTransaction(string senderAddress, string receiveAddress, decimal amount)
        {
            var web3 = new Web3(rpcAddress);
            var nonce = await web3.Eth.Transactions.GetTransactionCount.SendRequestAsync(senderAddress);
            var gasPrice = await web3.Eth.GasPrice.SendRequestAsync();
            var val = Web3.Convert.ToWei(amount);
            var transactionInput = new TransactionInput
            {
                From = senderAddress,
                To = receiveAddress,
                Nonce = nonce,
                Gas = new HexBigInteger(900000),
                GasPrice = gasPrice,
                Value = new HexBigInteger(val)
            };

            TransactionData d = new TransactionData();
            d.Data = transactionInput;
            return d;


        }

        public override Task SignTransaction(TransactionData transaction, string privateKey)
        {
            var account = new Nethereum.Web3.Accounts.Account(privateKey, _Chain.ChainId);
            var web3 = new Web3(account, rpcAddress);
            TransactionInput input=transaction.Data as TransactionInput;
            // AccountOfflineTransactionSigner a = new AccountOfflineTransactionSigner();
            AccountOfflineTransactionSigner t = new AccountOfflineTransactionSigner();
            string sign = t.SignTransaction(account, input);
            transaction.SignCode=sign;

            return Task.CompletedTask;
        }


        public override async Task<string> BoastTransaction(TransactionData transaction)
        {
            var web3 = new Web3(rpcAddress);

            var transactionHash = await web3.Eth.Transactions.SendRawTransaction.SendRequestAsync(transaction.SignCode);

            Console.WriteLine($"hash: {transactionHash}");
            //请求交易回执
            var receipt = await web3.Eth.Transactions.GetTransactionReceipt.SendRequestAsync(transactionHash);
            while (receipt == null)
            {
                Thread.Sleep(1000);
                receipt = await web3.Eth.Transactions.GetTransactionReceipt.SendRequestAsync(transactionHash);
            }
            Console.WriteLine($"receipt: {receipt.BlockNumber}");
            return transactionHash;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="to"></param>
        /// <param name="coinType"></param>
        /// <param name="amount"></param>
        /// <param name="gas"></param>
        /// <returns></returns>
        public async Task<string> TransferAsync(string from, string to, string coinType, decimal amount, decimal gas)
        {
            //var web3=new Web3(apiAddress);

            var type = coinType.ToLower();
            if (type == "eth")
            {
                var gasPrice = (decimal)Web3.Convert.ToWei(amount, UnitConversion.EthUnit.Gwei);
                var val = (decimal)Web3.Convert.ToWei(amount, UnitConversion.EthUnit.Ether);

                var web3 = new Web3(_Chain.RPCAddress[0]);
                var txHash = await web3.Eth.GetEtherTransferService().TransferEtherAsync(to, val, gasPrice);
                return txHash;
            }
            return null;
        }


        public override async Task UpdateAssets(BaseCoin coin)
        {
            if (coin.Asets == null)
            {
                coin.Asets = new List<CoinAset>();
            }

            var baseAsset = coin.Asets.FirstOrDefault(m => m.Name == _Chain.Currency);
            if (baseAsset == null)
            {

                baseAsset = new CoinAset { Name = _Chain.Currency, Icon="eth" };
                coin.Asets.Insert(0, baseAsset);

            }
            baseAsset.Balance = await GetBalance(coin.PubKey);

            var tokens = coin.Asets.Where(m => m.Token?.Length > 5).Select(m => m.Token).ToArray();
            try
            {

                var web3 = new Web3(rpcAddress);
                var bakc = await web3.Eth.ERC20.GetAllTokenBalancesUsingMultiCallAsync(coin.PubKey, tokens);
                bakc.ForEach(m =>
                {
                    var aset = coin.Asets.FirstOrDefault(m2 => m2.Token == m.ContractAddress);
                    if (aset != null)
                    {
                        aset.Balance = (decimal)m.Balance / 1_000_000L;
                    }
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }


        /// <summary>
        /// 获取余额
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
         async  Task<decimal> GetBalance(string address)
        {


            var web3 = new Web3(rpcAddress);
            var amount = await web3.Eth.GetBalance.SendRequestAsync(address);
            return Web3.Convert.FromWei(amount);
        }




    }
}
