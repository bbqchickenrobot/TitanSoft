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
    [AllowAnonymous]
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
        [ResponseCache(Duration = 120)]
        public async Task<ActionResult<string>> GetAsync(string id) =>
            Ok(await cache.GetOrCreateAsync(id, async (e) => 
                    {
                        e.SetSlidingExpiration(TimeSpan.FromMinutes(2));
                        return await service.GetAsync(id);
                    }));
    }
}
