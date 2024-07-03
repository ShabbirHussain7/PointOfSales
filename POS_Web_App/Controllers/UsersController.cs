using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;
using POS.Models;
using POS.Services;

namespace POS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize()]
    [RequiredScope("API.Calls")]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;

        public UserController(UserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        public IActionResult AddUser(User user)
        {
            _userService.AddUser(user.Name, user.Email, user.Password, user.Role);
            return CreatedAtAction(nameof(AddUser), new { id = user.Id }, user);
        }

        [HttpPost("authenticate")]
        public IActionResult Authenticate(string email, string password)
        {
            var user = _userService.Authenticate(email, password);
            if (user != null)
            {
                return Ok(user);
            }
            return Unauthorized();
        }
    }
}