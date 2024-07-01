using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fair.extensions.wallet.entity
{
    public class TronCoin :BaseCoin
    {



        public bool? Used { get; set; }

        /// <summary>
        /// 总资产(TRX)
        /// </summary>
        public Int64 Amount { get; set; }
        /// <summary>
        /// 已质押的金额
        /// </summary>
        public Int64 FrozeBalance { get; set; }

        public Int64 USDT { get; set; }

 
        public int Level { get; set; }

        public decimal EnergyLimit { get; set; }

        public decimal NetLimit { get; set; }

        public long NetUsed { get; set; }
        public long EnergyUsed { get; set; }

        public long UnFrozeAmount {  get; set; }    

        public long TRX { get; set; }
        public List<string> Groups { get; set; }

        public override string ToString()
        {
            if (this.PubKey?.Length > 16)
            {
                return $"{this.PubKey.Substring(0, 6)}**{this.PubKey.Substring(this.PubKey.Length - 8)}{(this.PriKey?.Length >= 32 ? "(私" + this.PriKey.Length + ")" : "")}{(this.Remark?.Length > 0 ? $"({this.Remark})" : "")}";
            }
            return $"!!!{this.PubKey}{(this.PriKey?.Length == 32 ? "(私)" : "")}";
        }

    }
}
