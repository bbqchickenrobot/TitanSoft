using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TitanSoft.Api.Services;

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

        public MoviesController(IMovieService service, ILogger logger)
        {
            log = logger;
            this.service = service;
        }


        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<string>>> AllAsync() => Ok(await service.GetAllAsync());

        [HttpGet("{id}")]
        public async Task<ActionResult<string>> GetAsync(string id) => Ok(await service.GetAsync(id));
    }
}
