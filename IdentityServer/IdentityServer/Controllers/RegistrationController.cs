using IdentityServer.Data;
using IdentityServer.Models;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdentityServer.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class RegistrationController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;

        public RegistrationController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpPost]
        public async Task<IActionResult> RegistrationAsync([FromBody] UserDto userDto)
        {
            var user = await _userManager.FindByNameAsync(userDto.UserName);
            if (user != null)
            {
                return StatusCode(StatusCodes.Status409Conflict);
            }
            user = new AppUser
            {
                UserName = userDto.UserName,
                Id = userDto.Id,
            };

            var result = await _userManager.CreateAsync(user, password: userDto.Password);
            if (result.Succeeded)
            {
                return StatusCode(StatusCodes.Status201Created);
            } 
            return BadRequest();
        }
    }
}
