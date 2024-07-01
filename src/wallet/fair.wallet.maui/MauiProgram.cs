using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;

namespace fair.wallet.maui
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            return builder
                .UseMauiApp<App>()
              .ConfigureFonts(fonts =>
              {
                  fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
              }).RunFairHost().Result;
        }
    }
}
