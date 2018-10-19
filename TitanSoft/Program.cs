using System.Diagnostics;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Serilog;
using Serilog.Events;
using TitanSoft.Api.Infrastructure;

namespace TitanSoft
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // configured the Serilog logger here to we can start logging right @ program startup
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .Enrich.FromLogContext()
                .WriteTo.ColoredConsole()
                .CreateLogger();

            var sb = new Stopwatch();
            sb.Start();
            // had to use the GetAwaiter() and GetResult() methods as my 
            // system wouldn't allow me to use the async Main() method (Mac's.... :/ )
            DataSeeder.SeedDatabaseAsync().GetAwaiter().GetResult();
            sb.Stop();
            Log.Information($"seeding the database took {sb.ElapsedMilliseconds} ms");

            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseUrls("http://localhost:5000;https://localhost:5001;")
                .UseSerilog()
                .UseKestrel()
                .UseStartup<Startup>();
    }
}
