
using fair.extensions.wallet.entity;
using fair.extensions.wallet.Pages.wallet;
using Microsoft.AspNetCore.Components.Web.Virtualization;
using Org.BouncyCastle.Utilities.Encoders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics.Arm;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using TronNet;
using TronNet.Crypto;

namespace fair.extensions.wallet
{
    public static class FairWalletHelper
    {


        public static string ToHexAddress(string trxAddress)
        {
            byte[] add = Base58Encoder.Decode(trxAddress);
            //去除校验码
            string aaa = Hex.ToHexString(add.Take(add.Length - 4).ToArray());
            return aaa;

        }

        public static string ToB58Address(string hexAddress)
        {

            if (string.IsNullOrEmpty(hexAddress)) return "";
            if (hexAddress.StartsWith("0x")) hexAddress = hexAddress.Substring(2);
            byte[] datas = Hex.Decode(hexAddress);
            byte[] hash1 = datas.ToSHA256Hash();
            byte[] hash2 = hash1.ToSHA256Hash();
            byte[] checkNum = new byte[4];
            for (int k = 0; k < 4; k++)
            {
                checkNum[k] = hash2[k];
            }
            byte[] ccc = datas.Concat(checkNum).ToArray();
            return Base58Encoder.Encode(ccc);
        }

        /// <summary>
        /// 给ETH地址加大小校验
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        static string GetCheckSum(string address)
        {
            string addr = address.ToLower();
            if (address.StartsWith("0x"))
            {
                address = address.Substring(2);
            }

            string add = address.ToKeccakHash();
            StringBuilder sb = new StringBuilder();
            sb.Append("0x");

            for (var i = 0; i < address.Length; i++)
            {
                if (add[i] >= '8')
                {
                    sb.Append(address[i].ToString().ToUpper());
                }
                else
                {
                    sb.Append(address[i]);
                }
            }

            return sb.ToString();

        }


        /// <summary>
        /// 给数据签名
        /// </summary>
        /// <param name="privateKey">私钥</param>
        /// <param name="data">数据</param>
        /// <returns></returns>
        public static string SignData(string privateKey, byte[] data)
        {
            byte[] hash = data.ToKeccakHash();
            return SignHash(hash, privateKey);

        }


        /// <summary>
        /// 给Hash签名
        /// </summary>
        /// <param name="hash"></param>
        /// <param name="privateKey"></param>
        /// <returns></returns>
        public static string SignHash(byte[] hash, string privateKey)
        {
            var ecKey = new ECKey(privateKey.HexToByteArray(), true);
            var sign = ecKey.Sign(hash).ToByteArray();
            return Hex.ToHexString(sign);
        }

        /// <summary>
        /// 验证签名
        /// </summary>
        /// <param name="wallet">钱包地址（公钥）</param>
        /// <param name="data">数据</param>
        /// <param name="sign">t</param>
        /// <returns></returns>
        public static bool Verfiy(string wallet, byte[] data, string sign)
        {
            string hexAddress = null;
            if (wallet.StartsWith("0x", StringComparison.OrdinalIgnoreCase))
            {
                hexAddress = wallet.Substring(2).ToLower();
            }
            else
            {
                if (wallet.StartsWith("T"))
                {
                    hexAddress = ToHexAddress(wallet).Substring(2);
                }
                else hexAddress = wallet;
            }
            if (sign.StartsWith("0x")) sign = sign.Substring(2);
            byte[] bSign = Hex.Decode(sign);
            if (bSign.Length == 65)
            {
                byte[] r = bSign.Take(32).ToArray();
                byte[] s = bSign.Skip(32).Take(32).ToArray();


                byte[] hash = data.ToKeccakHash();

                ECDSASignature signature = new ECDSASignature(new Org.BouncyCastle.Math.BigInteger(1, r), new Org.BouncyCastle.Math.BigInteger(1, s));
                int v = Int32.Parse(sign.Substring(sign.Length - 2), System.Globalization.NumberStyles.HexNumber);
                if (v < 27)
                {
                    if (v == 0 || v == 1)
                    {
                        v += 27;
                    }
                }
                v = 1 - v % 2;

                var e4 = ECKey.RecoverFromSignature(v, signature, hash, false);
                if (e4 != null)
                {
                    var bbb = e4.GetPubKeyNoPrefix();

                    var cc = bbb.ToKeccakHash();
                    var aa = cc.Skip(12).ToArray();
                    string vHexAddress = Hex.ToHexString(aa);

                    return hexAddress == vHexAddress;
                }
            }
            return false;

        }



        /// <summary>
        /// 获取以太钱包地址
        /// </summary>
        /// <param name="priKey"></param>
        /// <returns></returns>
        public static string GetEtherWalletAddress(string priKey)
        {
            ECKey e1 = new ECKey(priKey.HexToByteArray(), true);
            var ethPubKey = e1.GetPubKeyNoPrefix().ToKeccakHash();
            string ethPubKey2 = ethPubKey.ToHex();
            string ethPubKey3 = ethPubKey2.Substring(ethPubKey2.Length - 40);
            string pub3 = GetCheckSum(ethPubKey3);
            return pub3;
        }


    

        /// <summary>
        /// 助记词转化成私钥
        /// </summary>
        /// <param name="preWord">助记词前缀，如fair,hard等</param>
        /// <param name="helpWord">助记词（长度不少于12）</param>
        /// <returns></returns>
        public static string FairHelpWordToPrikey(string preWord, string helpWord)
        {
            if (preWord == null) preWord = "";
            if (helpWord.Length < 12) throw new Exception("助记词长度不能少于12个字符");
            preWord = preWord.ToLower();
            string word = $"{(preWord.Length > 0 ? $"{preWord}:" : "")}{helpWord}";
            byte[] data = Encoding.UTF8.GetBytes(word);
            SHA256 s256 = SHA256.Create();
            byte[] hash2 = s256.ComputeHash(data);
            //再次Hash,24-02-27加
            if (preWord.Length > 0)
            {
                int times = helpWord.Length % 10;
                if (times < 4) times = 4;
                for (int i = 0; i < times; i++)
                {
                    word = hash2.ToHex() + preWord + helpWord.Substring(2, 5);
                    data = Encoding.UTF8.GetBytes(word);
                    hash2 = s256.ComputeHash(data);
                }
            }
            var _ecKey = new ECKey(hash2, true);
            return _ecKey.PrivateKey.D.ToByteArrayUnsigned().ToHex();
        }

        /// <summary>
        /// 派生私钥
        /// </summary>
        /// <param name="priKey">原私钥</param>
        /// <param name="seed">种子</param>
        /// <param name="complex">生成复杂的私钥</param>
        /// <returns></returns>
        public static string GenerateSeedKey(string priKey, string seed, bool complex = true)
        {
            SHA256 s256 = SHA256.Create();
            byte[] priKey1 = s256.ComputeHash(Encoding.UTF8.GetBytes($"{priKey}-{seed}"));
            string seed1PriKey = priKey1.ToHex();
            if (complex)
            {

                byte[] priKey2 = s256.ComputeHash(Encoding.UTF8.GetBytes($"{priKey}{seed}fair"));
                int times = priKey2[0];
                times = times % 8;
                if (times < 3) times = 3;

                for (int i = 0; i < times; i++)
                {
                    int count = priKey2[i];
                    count = count % 10;
                    if (count < 3) count = 3;
                    byte[] sub = s256.ComputeHash(Encoding.UTF8.GetBytes($"{seed1PriKey.Substring(count)}{seed}"));

                    string subKey = sub.ToHex();
                    int count2 = priKey2[priKey2.Length - i - 1];
                    count2 = count2 % 20;
                    if (count2 < 3) count2 = 3;
                    priKey1 = s256.ComputeHash(
                        Encoding.UTF8.GetBytes($"{seed1PriKey.Substring(count2)}{subKey.Substring(0, count)}{seed1PriKey.Substring(0, count2)}-{seed}"));
                    seed1PriKey = priKey1.ToHex();
                }
            }
            return seed1PriKey;
        }



        /// <summary>
        /// 获取波场钱包地址
        /// </summary>
        /// <param name="priKey"></param>
        /// <returns></returns>
        public static string GetTronWalletAddress(string priKey)
        {
            var ecKey = new ECKey(priKey.HexToByteArray(), isPrivate: true);
            byte[] array = ecKey.GetPubKeyNoPrefix().ToKeccakHash();
            byte[] array2 = new byte[array.Length - 11];
            Array.Copy(array, 12, array2, 1, 20);
            array2[0] = 65;
            byte[] sourceArray = Base58Encoder.TwiceHash(array2);
            byte[] array3 = new byte[4];
            Array.Copy(sourceArray, array3, 4);
            byte[] array4 = new byte[25];
            Array.Copy(array2, 0, array4, 0, 21);
            Array.Copy(array3, 0, array4, 21, 4);
            return Base58Encoder.Encode(array4);
        }
    }
}
