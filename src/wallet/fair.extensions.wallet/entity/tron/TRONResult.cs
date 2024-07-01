using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fair.extensions.wallet.entity.tron
{
    public class TRONResult<T>
    {
        public T data { get; set; }

        public bool success {  get; set; }  

        public bool? Result { get; set; }    
        public string code { get; set; }
        
    }
}
