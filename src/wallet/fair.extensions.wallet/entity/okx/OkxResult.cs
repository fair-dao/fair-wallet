using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fair.extensions.wallet.entity.okx
{
    internal class OkxResult<T>
    {
        public int code { get; set; }
        public T data { get; set; } 
    }
}
