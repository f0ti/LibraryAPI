using LibraryAPI.Data;
using LibraryAPI.Data.Exceptions;
using LibraryAPI.DTO;
using LibraryAPI.IServices;
using LibraryAPI.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LibraryAPI.Services
{
    public class AdminServices : IAdminServices
    {
        private readonly LibraryContext _context;
        private readonly ILogger<AdminServices> _logger;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<User> _signInManager;

        public AdminServices(LibraryContext context, ILogger<AdminServices> logger,
                RoleManager<IdentityRole> roleManager, SignInManager<User> signInManager,
                UserManager<User> userManager)
        {
            _context = context;
            _logger = logger;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _userManager = userManager;
        }
        public async Task Authenticate(UserLoginRequestDTO adminDto)
        {
            // check if it's really admin
            await _signInManager.PasswordSignInAsync(adminDto.Username, adminDto.Password, adminDto.RememberMe, lockoutOnFailure: false);
        }

        public async Task CreateRole(CreateRoleDTO roleDto)
        {
            IdentityRole identityRole = new IdentityRole
            {
                Name = roleDto.RoleName
            };

            IdentityResult result = await _roleManager.CreateAsync(identityRole);

            if (result.Succeeded)
            {
                _logger.LogInformation("Role added successfully");
            }
        }

        public async Task DeleteAllUsers()
        {
            _context.User.RemoveRange(_context.User);
            await _context.SaveChangesAsync();
        }

        public async Task MakeAdmin(string email)
        {
            User user = await _context.User.FirstOrDefaultAsync(u => u.Email == email);

            if (user == null)
            {
                throw new EntityInvalidationException("User with the provided email was not found");
            }

            if (await _userManager.IsInRoleAsync(user, "Admin"))
            {
                throw new EntityInvalidationException("User is already an admin");
            }

            await _userManager.AddToRoleAsync(user, "Admin");
            await _userManager.RemoveFromRoleAsync(user, "Member");

            user.Role = "Admin";

            await _context.SaveChangesAsync();
        }

        public IEnumerable<User> GetAllUsers()
        {
            return _context.User;
        }
    }
}
