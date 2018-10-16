using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TitanSoft.Models;

namespace TitanSoft.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class MembersController : ControllerBase
    {

        [HttpGet("{id}")]
        public async Task<ActionResult<object>> Get(string id)
        {
            return "value";
        }


        [HttpPost]
        public async Task Login([FromBody] LoginModel value)
        {
        }


        [HttpPut()]
        public async Task Update([FromBody] object user)
        {
        }

        [HttpDelete("{id}")]
        public async Task Delete(string id)
        {
        }
    }
}
