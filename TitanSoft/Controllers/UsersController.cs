using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using TitanSoft.Services;
using TitanSoft.Entities;
using TitanSoft.Models;

namespace WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService userService;

        public UsersController(IUserService userService) => this.userService = userService;

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody]LoginModel model)
        {
            var user = userService.Authenticate(model.Username, model.Password);

            if (user == null)
                return BadRequest(new { message = "Username or password is incorrect" });
            user.PasswordHash = ""; // don't share sensitice information
            return Ok(user);
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var users =  userService.GetAll();
            return Ok(users);
        }
    }
}
