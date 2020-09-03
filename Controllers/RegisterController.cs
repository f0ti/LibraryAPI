using LibraryAPI.DTO;
using LibraryAPI.IServices;
using LibraryAPI.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace LibraryAPI.Controllers
{
    public class RegisterController : ControllerBase
    {
        private readonly IUserServices _userService;

        public RegisterController(IUserServices userServices)
        {
            _userService = userServices;
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<ActionResult> Register([FromBody] UserRegisterRequestDTO userDto)
        {
            try
            {
                await _userService.Create(userDto);
                return Ok();
            }
            catch (ApplicationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [AllowAnonymous]
        [HttpPost("verify-account")]
        public async Task<ActionResult> VerifyAccount(string verificationCode)
        {
            try
            {
                await _userService.VerifyAccount(verificationCode);
                return Ok();
            }
            catch (ApplicationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [AllowAnonymous]
        [HttpGet("current")]
        public async Task<string> Current()
        {
            User user = await _userService.Current();
            if (user != null)
                return user.UserName;
            else
                return "No exist";
        }
    }

}
