using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TitanSoft.Services;
using TitanSoft.Models;

namespace TitanSoft.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/v1/[controller]")]
    [Produces("application/json")]
    public class RentalsController : TitanControllerBase
    {
        readonly IRentalService service;
        readonly ILogger log;
        readonly IShippingService shippingService;
        readonly IPaymentService paymentService;

        public RentalsController(IRentalService service, IPaymentService paymentService, 
                                 IShippingService shippingService, ILogger logger)
        {
            this.paymentService = paymentService;
            this.shippingService = shippingService;
            this.service = service;
            log = logger;
        }

        [HttpPost]
        public async Task<ActionResult> RentMovie([FromBody] RentalModel model)
        {
            try
            {
                await service.RentAsync(model);
            }
            catch(Exception ex)
            {
                var msg = $"Error processing the rental";
                log.LogError(msg, ex);
                return BadRequest(msg);
            }
            return Ok($"Thank you for your purchase. Enjoy your movie. Your rental expires on {model.Expiring}");
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
