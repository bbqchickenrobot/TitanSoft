using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TitanSoft.Api.Models;
using TitanSoft.Models;
using TitanSoft.Services;

namespace TitanSoft.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/v1/[controller]")]
    [Produces("application/json")]
    public class MembersController : ControllerBase
    {
        readonly IUserService userService;
        readonly UserManager<MemberModel> manager;
        readonly IUserStore<MemberModel> store;
        readonly ILogger log;

        public MembersController(IUserService service, UserManager<MemberModel> manager, IUserStore<MemberModel> store, ILogger logger)
        {
            this.userService = service;
            this.manager = manager;
            this.store = store;
            this.log = logger;
        }

        [AllowAnonymous]
        [HttpPost("auth")]
        public async Task<IActionResult> Authenticate([FromBody]LoginModel model)
        {
            var user = await userService.AuthenticateAsync(model.Username, model.Password);

            if (user == null)
            {
                log.LogError($"failed auth attempt for user {model.Username}");
                return Unauthorized();
            }
            user.PasswordHash = ""; // don't share sensitice information
            return Ok(user);
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegistrationModel user)
        {
            var result = await userService.RegisterAsync(user);
            result.PasswordHash = string.Empty;
            return Ok(result);
        }

        [HttpGet("all")]
        public IActionResult GetAll()
        {
            var users = userService.GetAll();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MemberModel>> Get(string id)
        {
            var user = await userService.GetAsync(id);
            if (user == null)
                return NotFound();
            return Ok(user);
        }

        [HttpPut()]
        public async Task<ActionResult> Update([FromBody] MemberModel user)
        {
            try{
                await userService.UpdateAsync(user);
                return Ok();
            }catch(Exception ex){
                log.LogError(ex.Message, ex);
            }
            return BadRequest();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(string id)
        {
            try{
                await userService.DeleteAsync(id);
                Ok();
            }catch(Exception ex){
                log.LogError(ex.Message, ex);
            }
            return BadRequest();
        }
    }
}
