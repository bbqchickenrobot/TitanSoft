using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Serilog;
using Serilog.Events;
using TitanSoft.DataAccess;
using TitanSoft.Models;

namespace TitanSoft
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .Enrich.FromLogContext()
                .WriteTo.ColoredConsole()
                .CreateLogger();
            SeedDatabaseAsync(true).GetAwaiter().GetResult();
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseUrls("http://localhost:5000")
                .UseSerilog()
                .UseKestrel()
                .UseStartup<Startup>();

        public static async Task SeedDatabaseAsync(bool overwrite = false)
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "Data") + Path.DirectorySeparatorChar 
                           + "Databases" + Path.DirectorySeparatorChar + "TitanSoftStore";
            if (!overwrite && File.Exists(path + Path.DirectorySeparatorChar + "Raven.voron")) 
                Directory.Delete(path);

            var api = new OmdbApi(null, null);

            // anonymous method
            async Task<SearchModel> search(string term){
                var task = await api.SearchAsync(term);
                return task;
            }

            var sb = new Stopwatch();
            sb.Start();
            var results = await Task.WhenAll(search("horror"), search("drama"), 
                                        search("comedy"), search("anime"),
                                        search("love"), search("wild"), 
                                        search("children"), search("thriller"));

            using (var db = RavenDocumentStore.Store.OpenAsyncSession())
            foreach (var model in results)
            foreach (var result in model.Search)
            {
                var movie = await api.GetMovieAsync(result.ImdbId);
                await db.StoreAsync(movie);
            }
            sb.Stop();
            Debug.WriteLine($"seeding the database took {sb.ElapsedMilliseconds}");
        }
    }
}
