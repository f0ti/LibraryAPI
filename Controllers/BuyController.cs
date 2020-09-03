using LibraryAPI.DTO;
using LibraryAPI.IServices;
using LibraryAPI.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace LibraryAPI.Controllers
{
    public class BuyController : ControllerBase
    {
        private readonly IBuyServices _buyServices;
        private readonly UserManager<User> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public BuyController(IBuyServices buyServices, IHttpContextAccessor httpContextAccessor,
                            UserManager<User> userManager)
        {
            _buyServices = buyServices;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
        }

        [Authorize]
        [HttpPost("checkout")]
        public async Task<ActionResult> Checkout([FromBody] BuyRequestDTO buyRequestDTO)
        {
            try
            {
                await _buyServices.Purchase(buyRequestDTO);
                return Ok();
            }
            catch (ApplicationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [Authorize]
        [HttpGet("myPurchases")]
        public async Task<string> MyPurchases()
        {
            ClaimsPrincipal user = _httpContextAccessor.HttpContext.User;
            User currentUser = await _userManager.GetUserAsync(user);
            return JsonConvert.SerializeObject(_buyServices.GetPurchasesFromUsername(currentUser));
        }
    }
}
