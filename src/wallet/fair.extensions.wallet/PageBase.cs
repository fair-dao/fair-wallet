using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Rendering;
using System.Net.Http;
using Microsoft.JSInterop;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Drawing;
using fair.extensions.shared;
using fair.extensions.wallet.entity;
using Org.BouncyCastle.Bcpg.OpenPgp;

namespace fair.extensions.wallet
{
    /// <summary>
    /// 页面基类
    /// </summary>
    public class PageBase : fair.extensions.shared.ComBase
    {

      
        public override string CurPlugId => "wallet";

    
     
        protected override async Task OnInitializedAsync()
        {
            base.OnInitializedAsync();
        }

        


        

    }

}
