using fair.extensions.shared;
using fair.extensions.shared.entity;
using fair.extensions.shared.services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fair.extensions.wallet.services
{
    /// <summary>
    /// 账号服务
    /// </summary>
    public class AccountService : IAccountService
    {

        private SysHelper _Helper;
        public AccountService(SysHelper helper)
        {
            _Helper = helper;
          
        }

        private static WalletAccount? _account;
      

        public  async Task<WalletAccount> GetWalletAccountAsync()
        {
            //读取当前账号
            if (_account == null)
            {
                List<KeyValuePair<string, string>> accounts = await _Helper.GetCache<List<KeyValuePair<string,string>>>("fair-accounts");
                if (accounts?.Count() > 0)
                {
                    _account = await _Helper.GetCache<WalletAccount>($"fair-accounts-{accounts[0].Key}");
                }
            }
            return _account;
        }



        public string LoginUrl => "/wallet/login";

        public async Task ChangeAccountAsync(WalletAccount account)
        {
            string acc = account?.Account;
            if (!string.IsNullOrEmpty(acc))
            {
                List<KeyValuePair<string, string>> accounts = await _Helper.GetCache<List<KeyValuePair<string, string>>>("fair-accounts");
                if (accounts == null)
                {
                    accounts = new List<KeyValuePair<string, string>>();
                }
                acc = Encrypt.SHA1(acc);
                acc = acc.Substring(5, 6);
                KeyValuePair<string, string>? a = accounts.FirstOrDefault(m => m.Key == acc);
                if (a!=null)
                {
                    accounts.Remove(a.Value);
                }
                accounts.Insert(0, new KeyValuePair<string, string>(acc,account?.NickName??""));

                await _Helper.ReloadConfig();
                await _Helper.SetCache($"fair-accounts-{acc}", account);
                await _Helper.SetCache($"fair-accounts", accounts);
            }
        }

        public  async Task Logout()
        {
            _account = null;
            await _Helper.SetCache("mainAccount", null);
        }
    }
}
