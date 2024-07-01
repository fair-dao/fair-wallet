using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fair.extensions.wallet.services.wallet.eth
{
    public class Token
    {
        public string ABI { get; set; }

        public string Address { get; set; }

        public int SortId { get; set; }

        public string Name { get; set; }

        public static List<Token> Tokens = new(){
             new Token{ Name="USDT", Address="0xdac17f958d2ee523a2206206994597c13d831ec7",SortId=1 } ,
             new Token{ Name="BNB", Address="0xB8c77482e45F1F44dE1745F52C74426C631bDD52",SortId=2 } ,
             new Token{ Name="USDC", Address="0xa0b86991c6218b36c1d19d4a2e9eb0ce3606eb48",SortId=3 } ,
             new Token{ Name="stETH", Address="0xae7ab96520de3a18e5e111b5eaab095312d7fe84",SortId=4 } ,
             new Token{ Name="TRX", Address="0x50327c6c5a14dcade707abad2e27eb517df87ab5",SortId=5 } ,
             new Token{ Name="LINK", Address="0x514910771af9ca656af840dff83e8264ecf986ca",SortId=6 } ,
             new Token{ Name="WBTC", Address="0x2260fac5e5542a773aa44fbcfedf7c193bc2c599",SortId=7 } ,
             new Token{ Name="MATIC", Address="0x7d1afa7b718fb893db30a3abc0cfc608aacfebb0",SortId=8 } ,
             new Token{ Name="DAI", Address="0x6b175474e89094c44da98b954eedeac495271d0f",SortId=9 } ,
             new Token{ Name="SHIB", Address="0x95aD61b0a150d79219dCF64E1E6Cc01f0B64C4cE",SortId=10 } ,
             new Token{ Name="TONCOIN", Address="0x582d872a1b094fc48f5de31d3b73f2d9be47def1",SortId=11 } ,
             new Token{ Name="UNI", Address="0x1f9840a85d5af5bf1d1762f925bdaddc4201f984",SortId=12 } ,
             new Token{ Name="LEO", Address="0x2af5d2ad76741191d15dfe7bf6ac92d4bd912ca3",SortId=13 } ,
             new Token{ Name="OKB", Address="0x75231f58b43240c9718dd58b4967c5114342a86c",SortId=14 } ,
             new Token{ Name="TUSD", Address="0x0000000000085d4780B73119b644AE5ecd22b376",SortId=15 } ,
             new Token{ Name="CRO", Address="0xa0b73e1ff0b80914ab6fe0444e65848c4c34450b",SortId=16 } ,
             new Token{ Name="UIP", Address="0x4290563c2d7c255b5eec87f2d3bd10389f991d68",SortId=17 } ,
             new Token{ Name="LDO", Address="0x5a98fcbea516cf06857215779fd812ca3bef1b32",SortId=18 } ,
             new Token{ Name="NEAR", Address="0x85f17cf997934a597031b2e18a9ab6ebd4b9f6a4",SortId=19 } ,
             new Token{ Name="MNT", Address="0x3c3a81e81dc49a522a592e7622a7e711c06bf354",SortId=20 }
        };


    }


}
