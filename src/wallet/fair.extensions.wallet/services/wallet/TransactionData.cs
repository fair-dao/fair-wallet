using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fair.extensions.wallet.services.wallet
{
    /// <summary>
    /// 交易信息
    /// </summary>
    public class TransactionData
    {
        /// <summary>
        /// 交易id
        /// </summary>
        public string TxId { get; set; }

        /// <summary>
        /// 交易数据
        /// </summary>
        public string Raw_Data_Hex { get; set; }
             

        public object Data { get; set; }

        public string SignCode { get; set; }
    }
}
