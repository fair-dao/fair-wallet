using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fair.extensions.wallet.entity
{
    public struct DelegatedResourceInfo
    {

        public DelegatedResource[] delegatedResource {  get; set; } 
        

        public struct DelegatedResource
        {
            public string from {  get; set; }

            public string to { get; set; }
            /// <summary>
            /// 质押trx数量 
            /// </summary>
            public Int64 frozen_balance_for_energy { get; set; } 

            /// <summary>
            /// 过期时间
            /// </summary>
            public Int64 expire_time_for_energy { get; set; }    

        }

    }
}
