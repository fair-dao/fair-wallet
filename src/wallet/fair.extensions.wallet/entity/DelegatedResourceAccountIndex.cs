using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fair.extensions.wallet.entity
{
    public struct DelegatedResourceAccountIndex
    {
        public string account {  get; set; }

        public string[] fromAccounts { get; set; }  

       public string[] toAccounts { get; set;}
    }
}
