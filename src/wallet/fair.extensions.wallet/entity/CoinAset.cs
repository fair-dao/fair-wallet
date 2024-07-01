using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fair.extensions.wallet.entity
{
    public class CoinAset
    {
      
        public long Value { get; set; }

        public string Name { get; set; }

        public string Token { get; set; }
        public decimal Balance { get; set; }

        private string icon;

        public string Icon
        {
            get { 
                if(icon == null)
                {
                    icon = $"/_content/fair.extensions.wallet/images/tokens/{Name?.ToLower()}.svg";
                }
                
                return icon; }
            set { icon = value; }   
        }
    }
}
