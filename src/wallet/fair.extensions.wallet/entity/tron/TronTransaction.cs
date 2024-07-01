using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fair.extensions.wallet.entity.tron
{
    public class TronTransaction
    {
       public bool visible { get; set; }

        public string txID { get; set; }

        public RawData raw_data { get; set; }     
        public string raw_data_hex { get; set; }

        public string[] signature { get; set; }

    }

    public class RawData
    {
        public object[] contract { get; set;}

        public string ref_block_bytes { get; set; }

        public string ref_block_hash { get; set; }

        public long expiration { get; set; }

        public long timestamp { get; set; }

    }
}
