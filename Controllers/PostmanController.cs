using LibraryAPI.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace LibraryAPI.Controllers
{
    [ApiController]
    [Route("postie-panel")]
    public class PostManController : ControllerBase
    {

        private readonly IPostmanServices _postmanServices;

        public PostManController(IPostmanServices postmanServices)
        {
            _postmanServices = postmanServices;
        }

        [Authorize(Roles = "Admin, Postman")]
        [HttpGet("getAllRequests")]
        public string GetAllUsers()
        {
            return JsonConvert.SerializeObject(_postmanServices.GetAllBuyRequests());
        }

        [Authorize(Roles = "Admin, Postman")]
        [HttpPost("confirmDelivery")]
        public async Task<ActionResult> ConfirmDelivery(string filename)
        {
            try
            {
                await _postmanServices.ConfirmDeliveryStatus(filename);
                return Ok();
            }
            catch (ApplicationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
