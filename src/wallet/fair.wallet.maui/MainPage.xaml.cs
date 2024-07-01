
using Microsoft.AspNetCore.Components.WebView;
using Microsoft.AspNetCore.Components.WebView.Maui;
using Microsoft.Maui.Platform;

#if ANDROID
using AndroidX.Activity;
#endif

namespace fair.wallet.maui
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            this.blazorWebView.InitBlazorWebView();
        }

    }
}
