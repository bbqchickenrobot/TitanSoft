using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace TitanSoft.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class RentalsController : ControllerBase
    {
        ILogger log;

        public RentalsController(ILogger logger)
        {
            log = logger;
        }
    }
}
