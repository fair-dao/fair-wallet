
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using fair.extensions.wallet.entity;
using fair.extensions.wallet.entity.tron;
using System.Net.Http.Json;
using fair.extensions.shared;
using BulmaRazor.Components;
using fair.extensions.wallet.entity.tron;
using fair.extensions.wallet.entity;
using TronNet;

namespace fair.extensions.wallet.services.wallet.tron
{
    public class TronHttpClient : WalletProcesser
    {

        public TronHttpClient(Chain chain) : base(chain)
        {

        }




        public async Task<T> GetAPI<T>(string api)
        {
            string d = null;
            try
            {
                d = await client.GetStringAsync($"{this.rpcAddress}/{api}");

                return System.Text.Json.JsonSerializer.Deserialize<T>(d);
            }
            catch (Exception e)
            {
                Console.WriteLine($"error:{d}:{e.Message}");
                throw new Exception("获取失败", e);
            }


        }


        public async Task<T> CallAPI<T>(string api, object data, HttpMethod? method = null)
        {
            StringContent content = null;
            if (data == null) data = "";
            if (data is string) { content = new StringContent((string)data); }
            else
            {
                // content.Headers.Add("TRON-PRO-API-KEY", "30cca307-6c9d-47df-9dd2-aab15fafa287");
                var str = System.Text.Json.JsonSerializer.Serialize(data);
                content = new StringContent(str);
            }
            if (method == null) { method = HttpMethod.Post; }
            string url = $"{this.rpcAddress}/{api}";


            var httpRequestMessage = new HttpRequestMessage
            {
                Method = method,
                RequestUri = new Uri(url),
                Content = content
                // Content = new StringContent(data.ToJson(), Encoding.UTF8, "application/json")
            };
            //Console.WriteLine($"httclient:{c},Form地址:{url}");
            //System.Threading.CancellationToken c = new System.Threading.CancellationToken();
            try
            {
                //c.Timeout = TimeSpan.FromSeconds(1);

                HttpResponseMessage r = await client.SendAsync(httpRequestMessage);
                string back = await r.Content.ReadAsStringAsync();
                if (r.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    Console.WriteLine($"{url}\r\nbackdata:   {back}");
                    T o = System.Text.Json.JsonSerializer.Deserialize<T>(back, new System.Text.Json.JsonSerializerOptions { PropertyNamingPolicy = null });
                    return o;
                }
                else
                {
                    switch (r.StatusCode)
                    {
                        case System.Net.HttpStatusCode.NotFound:
                            throw new fair.extensions.shared.exs.APICodeException(fair.extensions.shared.entity.StateCode.请求的资源不存在);

                        default:
                            throw new fair.extensions.shared.exs.APICodeException(fair.extensions.shared.entity.StateCode.操作失败, "出错：状态码: " + r.StatusCode);
                    }

                }
            }
            catch (Exception e)
            {
                Console.WriteLine("url:{0},ex:{1}", url, e);
                throw e;

            }

        }


        public async Task<AccountInfoV1> GetAccountV1Async(string account)
        {

            var back = await GetAPI<TRONResult<AccountInfoV1[]>>($"v1/accounts/{account}");
            return back?.data?.Length == 1 ? back.data[0] : null;


        }


        public async Task<string> ListExchangesAsync()
        {
            var data = await CallAPI<string>($"wallet/listexchanges", null);
            return data;
        }

        void UpdateCoinAccount(TronCoin coin, AccountInfoV1 infoV1, AccountResource res)
        {
            if (infoV1?.active_permission != null)
            {
                // StringBuilder sb = new StringBuilder();

                coin.Used = true;
                coin.FrozeBalance = infoV1.account_resource.delegated_frozenV2_balance_for_energy;
                var a = infoV1.frozenV2?.FirstOrDefault(m => m.type == "ENERGY");
                if (a?.amount != null)
                {
                    coin.FrozeBalance += a?.amount.Value ?? 0;
                }
                a = infoV1.frozenV2?.FirstOrDefault(m => m.type == null);
                if (a?.amount != null)
                {
                    coin.FrozeBalance += a?.amount.Value ?? 0;
                }
                coin.UnFrozeAmount = 0;
                if (infoV1.unfrozenV2?.Length > 0)
                {
                    foreach (var a1 in infoV1.unfrozenV2)
                    {
                        coin.UnFrozeAmount += a1.unfreeze_amount;
                        DateTime d = DateTime.UnixEpoch.AddMilliseconds(a1.unfreeze_expire_time);
                        //      sb.AppendLine($"{d.ToLocalTime().ToString("MM-dd HH:mm")} 解押 {a1.unfreeze_amount / 1_000_000L} TRX ");
                    }
                }
                coin.Balance = infoV1.balance;
                coin.EnergyUsed = res.EnergyUsed;
                coin.NetUsed = res.NetUsed + res.freeNetUsed;
                coin.EnergyLimit = res.EnergyLimit;
                coin.NetLimit = res.NetLimit + res.freeNetLimit;
                coin.TRX = coin.Balance + coin.FrozeBalance + coin.UnFrozeAmount;
                coin.Amount = coin.TRX;

                coin.UpdateTime = DateTime.UtcNow;
                if (infoV1.trc20.Length == 0)
                {
                    coin.USDT = 0;
                }
                else
                {
                    bool found = false;
                    foreach (var item0 in infoV1.trc20)
                    {
                        foreach (var item in item0)
                        {
                            if (item.Key == C_USDT)
                            {
                                coin.USDT = long.Parse(item.Value ?? "0");
                                found = true;
                                break;
                            }
                        }
                        if (found)
                        {
                            break;
                        }
                    }
                }
                coin.Asets = new List<CoinAset>();
                coin.Asets.Add(new CoinAset
                {
                    Name = "TRX",
                    Balance = coin.Balance

                });
                coin.Asets.Add(new CoinAset

                {
                    Balance = coin.USDT,
                    Name = "USDT"

                });
                // decimal? trtousd = Helper.Current.GetRate(RateNames.TRON_USD);
                // if (trtousd > 0)
                // {
                //     coin.Amount += (Int64)(coin.USDT / trtousd.Value);
                // }
                //  coin.Description = sb.ToString();
            }

        }



        public override async Task UpdateAssets(BaseCoin coin)
        {
            string key = coin.PubKey;
            var data2 = await GetAccountV1Async(key);
            if (data2?.active_permission != null)
            {
                try
                {
                    var res = await GetAccountResource(key);
                    var a = coin as TronCoin;
                    UpdateCoinAccount(a, data2, res);
                }
                catch (Exception e)
                {
                    Console.Write(e);
                }

            }
        }

        public async Task<AccountResource> GetAccountResource(string account)
        {
            return await CallAPI<AccountResource>("wallet/getaccountresource", new { address = account, visible = true });
        }

        public static string C_USDT = "TR7NHqjeKQxGTCi8q8ZY4pL8otSzgjLj6t";



        /// <summary>
        ///  创建平台币交易对象
        /// </summary>
        /// <param name="owner_address"></param>
        /// <param name="to_address"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        public override async Task<TransactionData> CreateTransaction(string owner_address, string to_address, decimal amount)
        {
            var t = await CallAPI<TronTransaction>("wallet/createtransaction", new { owner_address, to_address, amount, visible = true });
            return new TransactionData() { Data = t };
        }


        public override Task SignTransaction(TransactionData transaction, string priKey)
        {
            TronTransaction tronTransaction = transaction.Data as TronTransaction;
            string sign = FairWalletHelper.SignHash(tronTransaction.txID.HexToByteArray(), priKey);
            tronTransaction.signature = new string[] { sign };
            transaction.SignCode = sign;
            return Task.CompletedTask;

        }


        public override async Task<string> BoastTransaction(TransactionData trans)
        {
            TronTransaction transaction = trans.Data as TronTransaction;
            var data = await CallAPI<TRONResult<string>>("wallet/broadcasttransaction", transaction);
            if (data?.Result != true)
            {
                throw new fair.extensions.shared.exs.CallErrorException(data?.data ?? "出错");
            }
            return "";
        }

        /// <summary>
        /// 获取代理资源列表
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public async Task<DelegatedResourceAccountIndex> GetDelegatedResourceAccountIndexV2(string account)
        {
            return await CallAPI<DelegatedResourceAccountIndex>("wallet/getdelegatedresourceaccountindexv2", new { value = account, visible = true });


        }


        public async Task<DelegatedResourceInfo> GetDelegatedResourceV2(string fromAddress, string toAddress)
        {
            return await CallAPI<DelegatedResourceInfo>("wallet/getdelegatedresourcev2", new { fromAddress, toAddress, visible = true });


        }


    }
}
