using LibraryAPI.DTO;
using LibraryAPI.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace LibraryAPI.Controllers
{
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IUserServices _userService;

        public LoginController(IUserServices userServices)
        {
            _userService = userServices;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult> Authenticate([FromBody] UserLoginRequestDTO userDto)
        {
            try
            {
                await _userService.Authenticate(userDto);
                return Ok();
            }

            catch (ApplicationException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
