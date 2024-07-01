using fair.extensions.wallet.entity;
using fair.extensions.wallet.entity.tron;
using fair.extensions.wallet.services.wallet.eth;
using fair.extensions.wallet.services.wallet.tron;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace fair.extensions.wallet.services.wallet
{
    public class WalletProcesser
    {

        protected Chain _Chain;
        protected HttpClient client;
        protected string rpcAddress;
        public Chain CurChain { get { return _Chain; } }
        public WalletProcesser( Chain chain) { 
            _Chain = chain;
            string[] datas = chain.RPCAddress[0].Split(',');
            rpcAddress = datas[0];
            client = new HttpClient();
            if (datas.Length > 1)
            {
                for (int i = 1; i < datas.Length; i++)
                {
                    string data = datas[i];
                    string[] paras = data.Split("=");
                    client.DefaultRequestHeaders.TryAddWithoutValidation(paras[0], paras[1]);
                }
            }
        }
        public static WalletProcesser GetProcesser(Chain chain)
        {
            switch(chain.Type)
            {
                case "tron":
                    return new TronHttpClient(chain);
                case "ether":
                    return new EthHttpClient(chain);
                default: return null;
            }
        }


        /// <summary>
        /// 更新资产
        /// </summary>
        /// <param name="coin"></param>
        /// <returns></returns>
        public virtual async Task UpdateAssets(BaseCoin coin)
        {
            
        }


        /// <summary>
        ///  创建交易对象
        /// </summary>
        /// <param name="owner_address"></param>
        /// <param name="to_address"></param>
        /// <param name="amount"></param>
        /// <param name="visible">是否使用Base58地址</param>
        /// <returns></returns>
        public virtual Task<TransactionData> CreateTransaction(string owner_address, string to_address, decimal amount)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        ///  创建交易对象
        /// </summary>
        /// <returns></returns>
        public virtual Task<TransactionData> CreateTransaction<TInput>(TInput input) 
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// 获得签名
        /// </summary>
        /// <returns></returns>

        public virtual Task SignTransaction(TransactionData transaction,string password)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 广播交易 
        /// </summary>
        public virtual Task<string> BoastTransaction(TransactionData transaction)
        {

            throw new NotImplementedException();

        }
    }
}
