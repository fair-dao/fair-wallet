using fair.extensions.wallet.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fair.extensions.wallet.entity
{
    /// <summary>
    /// 本地用户信息
    /// </summary>
    public class LocalUser
    {
        /// <summary>
        /// 本地用户钱包
        /// </summary>
        public UserCoin<BaseCoin>  UserCoin { get; set; }=new ();


        public string Key { get; set; }

        /// <summary>
        /// 本地账号(用以太地址做为账号)
        /// </summary>
        public string Account { get; set; }

    }


    
}
