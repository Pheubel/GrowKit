using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace GrowKitApi
{
    public class Program
    {
        /// <summary> The starting point of the program.</summary>
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        /// <summary> Sets up a webhost builder that will be used to run the web server.</summary>
        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseUrls("http://*:5000", "https://*:5001")
                .UseStartup<Startup>();
    }
}
