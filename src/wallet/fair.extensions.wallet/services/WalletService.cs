using fair.extensions.shared;
using fair.extensions.wallet.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using TronNet;

namespace fair.extensions.wallet.services
{
    public class WalletService
    {
        private SysHelper sysHelper;

        public WalletService(SysHelper sysHelper)
        {
            this.sysHelper = sysHelper;
        }




        internal string GetSafePriKey(string priKey,string password)
        {
            if (password?.Length <= 7)
            {
                throw new fair.extensions.shared.exs.CallErrorException("密码长度必须大于7位");
            }

            SHA256 s256 = SHA256.Create();

            byte[] pass = Encoding.UTF8.GetBytes(password);

            byte[] hsPassword = s256.ComputeHash(pass);
            byte[] btStoreData = new byte[34];
            string extData = $"{password}{sysHelper.ClientEnv.DeviceId ?? "none"}";


            byte[] firstHash = s256.ComputeHash(Encoding.UTF8.GetBytes(extData));

            //写入2位密码检验码
            for (int i1 = 0; i1 < 2; i1++) btStoreData[i1] = firstHash[i1];

            byte[] xorHash = s256.ComputeHash(Encoding.UTF8.GetBytes(firstHash.ToHex() + "wallet"));

            for (int i = 0; i < 8; i++)   //增加解密时间
            {
                string word = xorHash.ToHex() + password.Substring(i, 1);
                xorHash = s256.ComputeHash(Encoding.UTF8.GetBytes(word));
            }

            byte[] btPriKey = priKey.HexToByteArray();

            for (int i = 0; i < btPriKey.Length; i++)
            {
                btStoreData[i + 2] = (byte)(xorHash[i] ^ btPriKey[i]);
            }
            string priSaveKey = btStoreData.ToHex();
            return priSaveKey;

        }

        internal string GetSeedPriKey(string password,int seed)
        {
            string priKey=GetRealPriKey(password);
            if (seed == 0) return priKey;
            return FairWalletHelper.GenerateSeedKey(priKey, seed.ToString());

        }


        internal string GetRealPriKey(string password)
        {

            SHA256 s256 = SHA256.Create();
            string key=_LocalUser.Key;
            byte[] safeKey = key.HexToByteArray();
            byte[] pass = Encoding.UTF8.GetBytes(password);

            byte[] hsPassword = s256.ComputeHash(pass);
            string extData = $"{password}{sysHelper.ClientEnv.DeviceId ?? "none"}";

            byte[] firstHash = s256.ComputeHash(Encoding.UTF8.GetBytes(extData));

            //校验密码是否正确
            for (int i1 = 0; i1 < 2; i1++)
            {
                if (firstHash[i1] != safeKey[i1])
                {
                    throw new fair.extensions.shared.exs.PasswordException();
                }
            }
            byte[] xorHash = s256.ComputeHash(Encoding.UTF8.GetBytes(firstHash.ToHex() + "wallet"));

            for (int i = 0; i < 8; i++)   //增加解密时间
            {
                string word = xorHash.ToHex() + password.Substring(i, 1);
                xorHash = s256.ComputeHash(Encoding.UTF8.GetBytes(word));
            }

            byte[] priKey = new byte[32];
            for (int i = 0; i < priKey.Length; i++)
            {
                priKey[i] = (byte)(xorHash[i] ^ safeKey[i+2]);
            }
            return priKey.ToHex();

        }

        private static LocalUser _LocalUser;
        internal async Task SaveLocalUserAsync(LocalUser localUser)
        {
            //缓存本地用户信息
            await sysHelper.SetCache("localUser", localUser);
            _LocalUser = localUser;

        }

        internal async Task SaveLocalUserAsync()
        {
            //缓存本地用户信息
            await sysHelper.SetCache("localUser", _LocalUser);
        }

        internal async Task<LocalUser> GetLocalUserAsync()
        {
            if (_LocalUser == null)
            {
                var user = await sysHelper.GetCache<LocalUser>("localUser");
                if (user == null) { user = new LocalUser(); }
                _LocalUser = user;

            }
            return _LocalUser;
        }
    }
}
