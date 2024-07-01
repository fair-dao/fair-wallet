using fair.extensions.wallet.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fair.extensions.wallet.entity
{

    /// <summary>
    /// 账号数据
    /// </summary>
    public class AccountData
    {

        public List<TronCoin>? TronAccounts { get; set; } = new ();
    }
}
