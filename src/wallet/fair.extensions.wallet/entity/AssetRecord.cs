using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fair.extensions.wallet.entity
{
    public class AssetRecord
    {
       public string Id { get; set; }


        public decimal Amount { get; set; } 

        /// <summary>
        /// TRX总数，包括已质押的
        /// </summary>
        public decimal? TRX { get; set; }
        public decimal? USDT { get; set; }

        public decimal? TRX2USD { get; set; }

        public decimal? USDT2CNY { get; set; }

        public DateTime? HLUpdated { get; set; }

        /// <summary>
        /// 记录日期
        /// </summary>
        public DateTime RecordTime { get; set; }

        /// <summary>
        /// 附加金额（用于计算收益 ）
        /// </summary>
        public Int64 Supplementary { get; set; }


    }
}
