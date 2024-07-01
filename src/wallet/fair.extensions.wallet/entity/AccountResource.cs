using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace fair.extensions.wallet.entity
{
    public class AccountResource
    {
        /// <summary>
        /// 免费带宽
        /// </summary>
        public long freeNetLimit { get; set; }


        public Int64 freeNetUsed { get; set; }

        /// <summary>
        /// 已使用带宽
        /// </summary>
        public long NetUsed { get; set; }

        /// <summary>
        /// 带宽总量（不含免费的带宽）
        /// </summary>
        public long NetLimit { get; set; }

        public long TotalNetLimit { get; set; }

        public long TotalNetWeight { get; set; }

        public long tronPowerUsed { get; set; }

        public long tronPowerLimit { get; set; }


        /// <summary>
        /// 已使用能量
        /// </summary>
        public long EnergyUsed { get; set; }

        /// <summary>
        /// 能量总量
        /// </summary>
        public long EnergyLimit { get; set; }

        public long TotalEnergyLimit { get; set; }

        public long TotalEnergyWeight { get; set; }
    }
}
