using LibraryAPI.DTO;
using LibraryAPI.IServices;
using LibraryAPI.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace LibraryAPI.Controllers
{
    [ApiController]
    [Route("admin-panel")]
    public class AdminController : ControllerBase
    {
        private readonly IAdminServices _adminServices;
        private readonly IBookServices _bookServices;

        public AdminController(IAdminServices adminServices, IBookServices bookServices)
        {
            _adminServices = adminServices;
            _bookServices = bookServices;
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("deleteAllUsers")]
        public async Task<ActionResult> DeleteAllUsers()
        {
            try
            {
                await _adminServices.DeleteAllUsers();
                return Ok();
            }
            catch (ApplicationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [AllowAnonymous]
        [HttpPost("makeAdmin")]
        public async Task<ActionResult> MakeAdmin(string email)
        {
            try
            {
                await _adminServices.MakeAdmin(email);
                return Ok();
            }
            catch (ApplicationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("getAllUsers")]
        public string GetAllUsers()
        {
            return JsonConvert.SerializeObject(_adminServices.GetAllUsers());
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("addRole")]
        public async Task<ActionResult> AddRole([FromBody] CreateRoleDTO roleDto)
        {
            try
            {
                await _adminServices.CreateRole(roleDto);
                return Ok();
            }
            catch (ApplicationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [Route("addBook")]
        public async Task AddBooks([FromBody] Book book)
        {
            await _bookServices.AddBooks(book);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        [Route("editBook/{id}")]
        public async Task EditBook(int id)
        {
            // Book book =
            await _bookServices.GetBookById(id);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [Route("editBook/{id}")]
        public async Task EditBook([FromBody] Book updatedBook, int id)
        {
            await _bookServices.UpdateBook(updatedBook, id);
        }
    }
}
