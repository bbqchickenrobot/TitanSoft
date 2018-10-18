using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TitanSoft.Api.Services;
using TitanSoft.Models;

namespace TitanSoft.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/v1/[controller]")]
    [Produces("application/json")]
    public class RentalsController : ControllerBase
    {
        readonly IRentalService service;
        readonly ILogger log;

        public RentalsController(IRentalService service, ILogger logger)
        {
            this.service = service;
            log = logger;
        }

        [HttpPost]
        public async Task RentMovie([FromBody] RentalModel model)
        {
            await service.RentAsync(model);
            Ok($"Thank you for your purchase. Enjoy your movie. Your rental expires on {model.Expiring}");
        }

        [HttpGet]
        public async Task<ActionResult<List<RentalModel>>> History()
        {
            var id = HttpContext.User.Identity.Name;
            var results = await service.GetHistoryAsync(id);

            return Ok(results);
        }
    }
}
