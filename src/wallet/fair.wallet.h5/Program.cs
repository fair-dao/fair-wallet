using fair.extensions.shared;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

namespace fairstar.H5
{
    public class Program
    {
        public static Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            return builder.RunFairHost(
                new fair.extensions.main.Extender(),
                new fair.extensions.wallet.Extender()
                );
        }
    }
}
