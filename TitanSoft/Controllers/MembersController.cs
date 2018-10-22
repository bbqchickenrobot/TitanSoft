using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using TitanSoft.Models;
using TitanSoft.Services;

namespace TitanSoft.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/v1/[controller]")]
    [Produces("application/json")]
    public class MembersController : TitanControllerBase
    {
        readonly IMemberService userService;
        readonly UserManager<MemberModel> manager;
        readonly IUserStore<MemberModel> store;
        readonly ILogger log;
        readonly IConfiguration configuration;

        public MembersController(IMemberService service, UserManager<MemberModel> manager, 
                                 IUserStore<MemberModel> store, ILogger logger, IConfiguration configuration)
        {
            this.configuration = configuration;
            this.userService = service;
            this.manager = manager;
            this.store = store;
            this.log = logger;
        }

        [AllowAnonymous]
        [HttpPost("auth")]
        public async Task<IActionResult> Authenticate([FromBody]LoginModel model)
        {
            var authedUser = await userService.AuthenticateAsync(model.Username, model.Password, 
                                                                 configuration["appsettings:secret"]);

            if (authedUser == null)
            {
                log.LogError($"failed auth attempt for user {model.Username}");
                return Unauthorized();
            }

            return Ok(authedUser);
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegistrationModel user)
        {
            try
            {
                await userService.RegisterAsync(user);
                return Ok($"successfull registered user {user.Email}");
            }
            catch (Exception ex)
            {
                var msg = $"Error creating user {user.Email}";
                log.LogError(msg, ex);
                return BadRequest(msg);
            }
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllAsync() => Ok(await userService.GetAllAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var user = await userService.GetAsync(id);
            if (user == null)
                return BadRequest($"user not found with id {id}");
            return Ok(user);
        }

        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] UserViewModel user)
        {
            try
            {
                await userService.UpdateAsync(user);
                return Ok();
            }
            catch (Exception ex)
            {
                log.LogError(ex.Message, ex);
                return BadRequest($"unable to update user with id {user.Id}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                await userService.DeleteAsync(id);
                return Ok();
            }
            catch (Exception ex)
            {
                log.LogError(ex.Message, ex);
                return BadRequest($"there was an error deleting the user with id {id}");
            }
        }
    }
}
