using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using TitanSoft.Api.Services;
using TitanSoft.Models;

namespace TitanSoft.Controllers
{
    [ApiController]
    [Authorize]
    [Produces("application/json")]
    [Route("api/v1/[controller]")]
    public class MoviesController : ControllerBase
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
        public async Task<ActionResult<List<MovieModel>>> AllAsync()
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
        public async Task<ActionResult<List<Search>>> Search(string term)
        {
            var results = await cache.GetOrCreateAsync(term, async (e) =>
            {
                e.SetSlidingExpiration(TimeSpan.FromHours(12));
                return await service.SearchAsync(term);
            });
            return Ok(results);
        }

        [Authorize]
        [HttpPost("save")]
        public async Task<ActionResult> Save(MovieModel movie)
        {
            await service.SaveAsync(movie);
            return Ok();
        }

        [Authorize]
        [HttpPut("update")]
        public async Task<ActionResult> Update(MovieModel movie){
            await service.UpdateAsync(movie);
            return Ok();
        }
    }
}
