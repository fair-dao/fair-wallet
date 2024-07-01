using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fair.extensions.wallet.entity.tron
{
    public class AccountInfoV1
    {
        public Int64 balance {  get; set; }

        public Int64 create_time { get; set; }


        /// <summary>
        /// 已使用带宽
        /// </summary>
        public long net_usage { get; set; }


        public long net_window_size { get; set; }

        public Vote[] votes { get; set; }

        public ActivePermission[] active_permission { get; set; }


        /// <summary>
        /// 冻结的数据
        /// </summary>
        public FrozenV2[] frozenV2 { get; set; }

        /// <summary>
        /// 正在解冻的数据
        /// </summary>
        public UnFrozenV2[] unfrozenV2 { get; set; }
        /// <summary>
        /// TRC10资产列表
        /// </summary>
        public AssetV2[] assetV2 { get; set; }


        public Dictionary<string, string>[] trc20 { get; set; }


        public AccountResource account_resource { get; set; }


        /// <summary>
        /// 账户资源
        /// </summary>
        public struct AccountResource
        {
            /// <summary>
            /// 已使用能量
            /// </summary>
            public Int64 energy_usage { get; set; }

            public Int64 latest_consume_time_for_energy { get; set; }

            public Int64 energy_window_size { get; set; }
            /// <summary>
            /// 已代理的能量资产（TRX)
            /// </summary>
            public Int64 delegated_frozenV2_balance_for_energy { get; set; }

            public bool energy_window_optimized { get; set; }


        }
    }



    public struct Vote
    {
        public string vote_address { get; set; }
        public long vote_count { get; set; }
    }

    public struct FrozenV2
    {
        public string? type { get; set; }
        public long? amount { get; set; }
    }

    public struct UnFrozenV2
    {
        /// <summary>
        /// 解冻金额
        /// </summary>
        public long unfreeze_amount { get; set; }
        /// <summary>
        /// 解冻时间
        /// </summary>
        public long unfreeze_expire_time { get; set; }

    }

    public struct AssetV2
    {
        public string key { get; set; }

        public long value { get; set; }
    }

    public struct ActivePermission
    {
        public long id { get; set; }

        public string permission_name { get; set; }

        /// <summary>
        /// 操作权限
        /// </summary>
        public string operations { get; set; }

        public Key[] keys { get; set; }


        public struct Key
        {
            public string address { get; set; }
            public long weight { get; set; }
        }


    }
}
