using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace fair.extensions.wallet.entity
{
    [JsonDerivedType(typeof(TronCoin),"tron")]
    [JsonDerivedType(typeof(EthCoin),"eth")]
    /// <summary>
    /// 币的基类
    /// </summary>
    public class BaseCoin
    {
        public Guid Id { get; set; }
        public string PubKey { get; set; }

        [JsonIgnore]
        public string PriKey { get; set; }

        public string ChainId {  get; set; }    

        public Int64 Balance { get; set; }

        /// <summary>
        /// 总估值（以USDT/1_000_000L为单位)
        /// </summary>
        public Int64 Value { get; set; }

        public List<string> Groups { get; set; }
        public DateTime CreateTime { get; set; }

        public DateTime UpdateTime { get; set; }
        = DateTime.Now;

        public int State { get; set; }

        /// <summary>
        /// 排序编号，越大越靠前
        /// </summary>
        public Int64 SortId { get; set; }

        /// <summary>
        /// 私钥生成种子
        /// </summary>
        public Int32 Seed { get; set; }
        public List<CoinAset> Asets { get; set; }

        public string Remark { get; set; }
    }


    /// <summary>
    /// 用户币
    /// </summary>
    /// <typeparam name="TCoin"></typeparam>
    public class UserCoin<TCoin> where TCoin : BaseCoin
    {
        public string Name { get; set; }

        public string Id { get; set; }

        public int LastSeed { get; set; }
        public TCoin DefaultCoin { get; set; }

        public List<TCoin> Coins { get; set; }=new List<TCoin>();
        


    }
}
