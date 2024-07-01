using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fair.extensions.wallet.entity
{
    public class Chain
    {
        public string  Id { get; set; }

        /// <summary>
        /// 链Id
        /// </summary>
        public long ChainId { get; set; }

        /// <summary>
        /// 网络类型 (ether,tron,eos)
        /// </summary>
        public string Type { get; set; } = "ether";

        /// <summary>
        /// 网络名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 货币符号
        /// </summary>
        public string Currency { get; set; }


        public string[] RPCAddress { get; set; }

        /// <summary>
        /// 区块浏览器地址
        /// </summary>
        public string InfoUrl { get; set; }

        public string Logo { get; set; }



        public static List<Chain> CoinNets = new List<Chain>()
        {
            new Chain{ Id="tron", Name="Tron 网络", Type="tron", ChainId=22,Currency="TRX"
                ,RPCAddress=new string[]{ "https://api.trongrid.io" } }
            ,new Chain{  Id="ether", Name="以太坊主网", Type="ether", ChainId=1, Currency="ETH"
                ,RPCAddress=new string[]{ "https://eth-mainnet.g.alchemy.com/v2/8TCz6_L75bsBM7WqI6IU_SL1t0pnuwoP", "https://mainnet.infura.io/v3/" }
                ,InfoUrl="https://etherscan.io"}

            ,new Chain{  Id="matic", Name="Polygon Mainnet", Type="ether"
                ,ChainId=137, Currency="MATIC"
                ,RPCAddress=new string[]{ "https://polygon-mainnet.infura.io" }
                ,InfoUrl="https://polygonscan.com/"}
            ,new Chain{  Id="bnb", Name="SmartChain", Type="ether"
                ,ChainId=56, Currency="BNB"
                ,RPCAddress=new string[]{ "https://bsc-dataseed.binance.org/" }
                ,InfoUrl="https://bscscan.com/"}
            ,new Chain{  Id="okt", Name="OKExChain", Type="ether"
                ,ChainId=66, Currency="OKT"
                ,RPCAddress=new string[]{ "https://exchainrpc.okex.org/" }
                ,InfoUrl=""}

            ,new Chain{  Id="btt", Name="BitTorrent Chain Mainnet", Type="ether"
                ,ChainId=199, Currency="BTT"
                ,RPCAddress=new string[]{ "https://rpc.bittorrentchain.io/" }
                ,InfoUrl="https://polygonscan.com/"}

            ,new Chain{ Id="tron-shasta", Name="波场shasta测试网络", Type="tron", ChainId=23,Currency="TRX"
                ,RPCAddress=new string[]{ "https://api.shasta.trongrid.io" } }
            ,new Chain{ Id="gor", Name="Ethereum TestNet Goerli",Type="ether", ChainId=5, Currency="GOR"
                ,RPCAddress=new string[]{"https://goerli.infura.io/v3/9aa3d95b3bc440fa88ea12eaa4456161"} }
            ,new Chain{ Id="tmatic", Name="Matic Testnet Mumbai",Type="ether", ChainId=80001, Currency="MATIC"
                ,RPCAddress=new string[]{ "https://polygon-mumbai.blockpi.network/v1/rpc/public" },InfoUrl="https://mumbai.polygonscan.com" }
            

        };
    }

   

}
