using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Raven.Client.Exceptions.Documents.Session;
using TitanSoft.Services;
using TitanSoft.Models;

namespace TitanSoft.Controllers
{
    [ApiController]
    [Authorize]
    [Produces("application/json")]
    [Route("api/v1/[controller]")]
    public class MoviesController : TitanControllerBase
    {
        ILogger log;
        IMovieService service;
        IMemoryCache cache;

        public MoviesController(IMovieService service, IMemoryCache cache, ILogger logger)
        {
            log = logger;
            this.service = service;
            this.cache = cache;
        }

        [HttpGet("all")]
        [AllowAnonymous]
        [ResponseCache(Duration = 120)]
        public async Task<IActionResult> GetAllAsync()
        {
            var movies = await cache.GetOrCreateAsync("all_movies", async (ICacheEntry e) =>
            {
                e.SetSlidingExpiration(TimeSpan.FromMinutes(2));
                return await service.GetAllAsync();
            });
            return Ok(movies);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        [ResponseCache(Duration = 120)]
        public async Task<ActionResult<string>> GetAsync(string id)
        {
            var results = await cache.GetOrCreateAsync(id, async (e) =>
                    {
                        e.SetSlidingExpiration(TimeSpan.FromMinutes(2));
                        return await service.GetAsync(id);
                    });
            return Ok(results);
        }

        [AllowAnonymous]
        [HttpGet("search/{term}")]
        public async Task<ActionResult<List<OmdbSearchResult>>> Search(string term)
        {
            var results = await cache.GetOrCreateAsync(term, async (e) =>
            {
                e.SetSlidingExpiration(TimeSpan.FromHours(12));
                var list = await service.SearchAsync(term);
                return list;
            });
            return Ok(results);
        }

        [AllowAnonymous]
        [HttpGet("search/{term}/{page}")]
        public async Task<IActionResult> PagedSearch(string term, int page)
        {
            var results = await cache.GetOrCreateAsync($"{term}-{page}", async (e) =>
            {
                e.SetSlidingExpiration(TimeSpan.FromHours(12));
                var list = await service.PagedSearchAsync(term, page);
                return list;
            });
            return Ok(results);
        }

        [Authorize]
        [HttpPost("save")]
        [HttpPut("update")]
        public async Task<ActionResult> Save(MovieModel movie) => await Upsert(movie);

        async Task<ActionResult> Upsert(MovieModel movie)
        {
            try
            {
                await service.UpdateAsync(movie);
            }
            catch (NonUniqueObjectException ex)
            {
                var msg = $"movie title with id - {movie.Id} - already exists";
                log.LogError(msg, ex);
                return BadRequest(msg);
            }
            catch(Exception ex)
            {
                var msg = $"error saving {movie.Title} - {movie.Id}";
                log.LogError(msg, ex);
                return BadRequest(msg);
            }
            return Ok(null);
        }
    }
}
