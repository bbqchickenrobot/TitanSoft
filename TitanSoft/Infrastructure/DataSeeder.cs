using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Serilog;
using TitanSoft.DataAccess;
using TitanSoft.Models;

namespace TitanSoft.Api.Infrastructure
{
    public static class DataSeeder
    {
        /// <summary>
        /// Seeds the RavenDB database with movie data.
        /// </summary>
        /// <returns>The database async.</returns>
        /// <param name="overwrite">If set to <c>true</c> overwrite.</param>
        public static async Task SeedDatabaseAsync(bool overwrite = false)
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "Data") + Path.DirectorySeparatorChar
                           + "Databases" + Path.DirectorySeparatorChar + "TitanSoftStore";
            var exists = File.Exists(path + Path.DirectorySeparatorChar + "Raven.voron");
            if (overwrite && exists)
                Directory.Delete(path, true);
            else if (exists)
                return;

            var api = new OmdbApi(null, null);
            var results = await GetMovieResults(api);
            Log.Information($"found {results.Count} results");

            Parallel.ForEach(results, async (model) =>
            {
                using (var db = RavenDocumentStore.Store.OpenAsyncSession())
                {
                    foreach (var result in model.Search)
                    {
                        Log.Information($"saving {result.Title}");
                        var movie = await api.GetMovieAsync(result.ImdbId);
                        await db.StoreAsync(movie);
                    }
                    await db.SaveChangesAsync();
                }
            });
        }

        /// <summary>
        /// Gets the movie results from an online sourdce.
        /// </summary>
        /// <returns>The movie results.</returns>
        /// <param name="api">the OMDB API instantiated object.</param>
        private static async Task<List<OmdbSearchModel>> GetMovieResults(IOmdbApi api)
        {
            // anonymous method
            async Task<OmdbSearchModel> search(string term, int? page = null)
            {
                var task = await api.SearchAsync(term, page);
                return task;
            }

            var results = (await Task.WhenAll(search("horror"), search("drama"),
                    search("comedy"), search("anime"),
                    search("love"), search("wild"),
                    search("children"), search("thriller"),
                    search("kids"), search("cartoon"), search("troy"),
                    search("there"), search("mom"), search("dad"),
                    search("elle"), search("natalia"),
                    search("space"), search("rat"),
                    search("dog"), search("surf"), search("beauty"),
                                              search("mean", 1), search("test", 1), search("cat", 1),
                                              search("lies", 1), search("usa", 1), search("war", 1),
                    search("good"), search("bad"), search("ugly"))).ToList();

            results.AddRange(await Task.WhenAll(search("horror", 2), search("drama", 2),
                    search("comedy", 2), search("anime", 2),
                    search("love", 2), search("wild", 2),
                    search("children", 2), search("thriller", 2),
                    search("kids", 2), search("cartoon", 2), search("troy", 2),
                    search("there", 2), search("mom", 2), search("dad", 2),
                    search("elle", 2), search("natalia", 2),
                    search("space", 2), search("rat", 2),
                    search("dog", 2), search("surf", 2), search("beauty", 2),
                                                                                              search("mean", 1), search("test", 1), search("cat", 1),
                                              search("lies", 1), search("usa", 1), search("war", 1),
                    search("good", 2), search("bad", 2), search("ugly", 2)));

            results.AddRange(await Task.WhenAll(search("horror", 3), search("drama", 3),
                    search("comedy", 3), search("anime", 3),
                    search("love", 3), search("wild", 3),
                    search("children", 3), search("thriller", 3),
                    search("kids", 3), search("cartoon", 3), search("troy", 3),
                    search("there", 3), search("mom", 3), search("dad", 3),
                    search("elle", 3), search("natalia", 3),
                    search("space", 3), search("rat", 3), 
                    search("dog", 3), search("surf", 3), search("beauty", 3),
                                                                                              search("mean", 1), search("test", 1), search("cat", 1),
                                              search("lies", 1), search("usa", 1), search("war", 1),
                    search("good", 3), search("bad", 3), search("ugly", 3)));

            return results;
        }
    }
}
