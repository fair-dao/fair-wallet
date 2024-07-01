using fair.extensions.shared;
using fair.extensions.shared.entity;
using fair.extensions.shared.services;
using fair.extensions.wallet.services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fair.extensions.wallet
{
    public class Extender : fair.extensions.shared.ServiceExtender
    {

        public override VCommpent[]? VCommpents => new[]
            {   new VCommpent
                {
                    Id = "wallet",
                    Parent=VCommpent.Page,
                    Icon="wallet-one",
                    Text="钱包",
                    ShowMode= MenuShowModes.CommpentMode,
                    Link="fair.extensions.wallet.Pages.Wallet,fair.extensions.wallet"
                },
                new VCommpent
                {
                    Id = "etherwallet",
                    Parent="home",
                    Icon="/_content/fair.extensions.wallet/images.chains/ether.svg",
                    Text="以太坊钱包",
                    ShowMode= MenuShowModes.IconMode,
                    Link="/wallet/ether"
                },
                new VCommpent
                {
                    Id = "tronwallet",
                    Parent="home",
                    Icon="/_content/fair.extensions.wallet/images.chains/tron.svg",
                    Text="波场钱包",
                    ShowMode= MenuShowModes.IconMode,
                    Link="/wallet/tron"
                }

            };
        public override void Config(IServiceCollection services)
        {


            base.Config(services);
            //添加账号服务
            services.AddSingleton<IAccountService, AccountService>();
            services.AddTransient<WalletService>();
     
        }

        public override void Use(IServiceProvider provider)
        {
            base.Use(provider);

        }
    }
}
