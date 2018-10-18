using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TitanSoft.Entities;
using TitanSoft.Models;
using TitanSoft.Services;

namespace TitanSoft.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class MembersController : ControllerBase
    {
        readonly IUserService userService;
        readonly ILogger log;

        public MembersController(IUserService service, ILogger logger)
        {
            userService = service;
            log = logger;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AppUser>> Get(string id)
        {
            var user = await userService.GetAsync(id);
            if (user == null)
                return NotFound();
            return Ok(user);
        }


        [HttpPost]
        public async Task<ActionResult<AppUser>> Login([FromBody] LoginModel model)
        {
            ActionResult result;
            try
            {
                var user = await userService.AuthenticateAsync(model.Username, model.Password);
                result = (user == null) ? Unauthorized() as ActionResult : Ok(user);
            }catch(Exception ex){
                log.LogError($"unable to find user {model.Username}");
            }
            result = Unauthorized();
            return result;
        }


        [HttpPut()]
        public async Task<ActionResult> Update([FromBody] AppUser user)
        {
            try{
                userService.Update(user);
            }catch(Exception ex){
                log.LogError(ex.Message, ex);
            }
            return BadRequest();
        }

        [HttpDelete("{id}")]
        public async Task Delete(string id)
        {
        }
    }
}
