using Microsoft.AspNetCore.Mvc;
using UserManager.Data.Interfaces.Services.Users;
using UserManager.Main.Contracts.Models;
using UserManager.Main.Contracts.Models.Users;

namespace UserManager.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<ActionResult<ICollection<User>>> GetUsers(CancellationToken cancellation = default)
        {
            var users = await _userService.GetUsers(cancellation);
            return Ok(users);
        }

        [HttpGet("{userId}")]
        public async Task<ActionResult<User>> GetUser(int userId, CancellationToken cancellation = default)
        {
            var users = await _userService.GetUser(userId, cancellation);
            return Ok(users);
        }

        [HttpPost]
        public async Task<ActionResult<User>> CreateUsers(UserNew model, CancellationToken cancellation = default)
        {
            var users = await _userService.CreateUser(model, cancellation);
            return Ok(users);
        }

        [HttpPut("{userId}")]
        public async Task<ActionResult<User>> SetUsers(int userId, UserUpdate model, CancellationToken cancellation = default)
        {
            var users = await _userService.SetUser(userId, model, cancellation);
            return Ok(users);
        }

        [HttpPut("{userId}/password")]
        public async Task<ActionResult<User>> SetPassword(int userId, PasswordUpdate model, CancellationToken cancellation = default)
        {
            var users = await _userService.SetPassword(userId, model, cancellation);
            return Ok(users);
        }

        [HttpDelete("{userId}")]
        public async Task<ActionResult<User>> DeleteUser(int userId, CancellationToken cancellation = default)
        {
            var users = await _userService.DeleteUser(userId, cancellation);
            return Ok(users);
        }
    }
}
