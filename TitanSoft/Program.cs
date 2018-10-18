﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using TitanSoft.DataAccess;
using TitanSoft.Models;

namespace TitanSoft
{
    public class Program
    {
        public static void Main(string[] args)
        {
            SeedDatabaseAsync().GetAwaiter().GetResult();
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseUrls("http://localhost:5000")
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

            var results = await Task.WhenAll(search("horror"), search("drama"), 
                                       search("comedy"), search("anime"));

            using (var db = RavenDocumentStore.Store.OpenAsyncSession())
            foreach (var model in results)
                foreach (var result in model.Search)
                {
                    var movie = await api.GetMovieAsync(result.ImdbId);
                    await db.StoreAsync(movie);
                }
            
        }
    }
}
