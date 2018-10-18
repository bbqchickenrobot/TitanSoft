using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using TitanSoft.Services;
using TitanSoft.Entities;
using TitanSoft.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace TitanSoft.Controllers
{
    [Authorize]
    [ApiController]
    [Produces("application/json")]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService userService;
        readonly UserManager<AppUser> manager;
        readonly IUserStore<AppUser> store;
        readonly ILogger log;

        public UsersController(IUserService userService, UserManager<AppUser> manager, IUserStore<AppUser> store, ILogger logger)
        {
            this.userService = userService;
            this.manager = manager;
            this.store = store;
            this.log = logger;
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate([FromBody]LoginModel model)
        {
            var user = await userService.AuthenticateAsync(model.Username, model.Password);

            if (user == null)
            {
                log.LogError($"failed auth attempt for user {model.Username}");
                return BadRequest(new { message = "Username or password is incorrect" });
            }
            user.PasswordHash = ""; // don't share sensitice information
            return Ok(user);
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] AppUser user){
            var result = await userService.RegisterAsync(user.Email, user.FirstName, user.LastName, user.PasswordHash);
            result.PasswordHash = string.Empty;
            return Ok(result);
        }

        [HttpGet("all")]
        public IActionResult GetAll()
        {
            var users =  userService.GetAll();
            return Ok(users);
        }
    }
}
