using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using TitanSoft.Models;

namespace TitanSoft.DataAccess
{
    public class OmdbApi : IOmdbApi
    {
        string url = "";
        ILogger log;
        readonly HttpClient client = new HttpClient();

        public OmdbApi(IConfiguration config, ILogger logger)
        {
            url = config?["OmdbUrl"] ?? "http://www.omdbapi.com/?apikey=c4d85896&type=movie";
            log = logger;
        }

        public async Task<MovieModel> GetMovieAsync(string id) {
            try
            {
                var response = await GetJsonAsync($"{url}&i={id}");

                return JsonConvert.DeserializeObject<MovieModel>(response);
            }
            catch (HttpRequestException e)
            {
                log.LogError(e, e.Message);
            }
            return null;
        }

        public async Task<OmdbSearchModel> SearchAsync(string term, int? page = null){
            try
            {
                if (page.HasValue)
                    term += $"&page={page.Value}";
                var response = await GetJsonAsync($"{url}&s={term}");

                return JsonConvert.DeserializeObject<OmdbSearchModel>(response);
            }
            catch (HttpRequestException e)
            {
                log.LogError(e, e.Message);
            }
            return new OmdbSearchModel();
        }

        public MovieModel GetMovie(string id) => GetMovieAsync(id).GetAwaiter().GetResult();

        public OmdbSearchModel Search(string term, int? page) => SearchAsync(term, page).GetAwaiter().GetResult();

        protected async Task<string> GetJsonAsync(string uri) => await client.GetStringAsync(uri);
    }
}
