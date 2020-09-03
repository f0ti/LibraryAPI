using LibraryAPI.Data;
using LibraryAPI.Data.Exceptions;
using LibraryAPI.DTO;
using LibraryAPI.IServices;
using LibraryAPI.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryAPI.Services
{
    public class UserServices : IUserServices
    {
        private readonly ILogger<UserServices> _logger;
        private readonly LibraryContext _context;
        private readonly IEmailServices _emailServices;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserServices(LibraryContext context, ILogger<UserServices> logger, IEmailServices emailServices,
                            UserManager<User> userManager, SignInManager<User> signInManager, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _logger = logger;
            _emailServices = emailServices;
            _userManager = userManager;
            _signInManager = signInManager;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task Authenticate(UserLoginRequestDTO userDto)
        {
            User user = await _userManager.FindByNameAsync(userDto.Username);
            if (!await _userManager.IsEmailConfirmedAsync(user))
                throw new EntityInvalidationException("The account needs to be registered first with the received code in email");
            else
                await _signInManager.PasswordSignInAsync(userDto.Username, userDto.Password, userDto.RememberMe, lockoutOnFailure: false);
        }

        public async Task Create(UserRegisterRequestDTO userDto)
        {

            // Generate verification code
            string code = Guid.NewGuid().ToString("n").Substring(0, 8);
            while (_context.UnverifiedUser.Any(x => x.VerificationCode == code))
                code = Guid.NewGuid().ToString("n").Substring(0, 8);

            User user = new User
            {
                UserName = userDto.Username,
                Email = userDto.Email,
                FirstName = userDto.FirstName,
                LastName = userDto.LastName,
                PhoneNumber = userDto.PhoneNumber,
                NumberOfBooks = 0,
                Points = 0,
                EmailConfirmed = false,
                Role = "Member"
            };

            var resultCreate = await _userManager.CreateAsync(user, userDto.Password);
            var resultRole = await _userManager.AddToRoleAsync(user, user.Role);

            // Unverified user
            if (resultCreate.Succeeded && resultRole.Succeeded)
            {
                UnverifiedUser unverifiedUser = new UnverifiedUser
                {
                    UserId = user.Id,
                    VerificationCode = code
                };

                await _context.UnverifiedUser.AddAsync(unverifiedUser);
                await _context.SaveChangesAsync();

                _logger.LogInformation("User created a new account with password.");
                await _signInManager.SignInAsync(user, isPersistent: false);

                // Send email
                SendVerificationEmail(user, code);
            }
            else
                throw new EntityInvalidationException(string.Join(", ", resultCreate.Errors.Select(x => "Code " + x.Code + " Description" + x.Description)));
        }

        public async Task VerifyAccount(string verificationCode)
        {
            UnverifiedUser unverifiedUser =
                await _context.UnverifiedUser.SingleOrDefaultAsync(u => u.VerificationCode == verificationCode);

            if (unverifiedUser == null)
            {
                throw new EntityInvalidationException("The code entered is not correct");
            }

            User user = await _context.User.SingleOrDefaultAsync(u => u.Id == unverifiedUser.UserId);
            user.EmailConfirmed = true;

            // Remove unverified user
            _context.UnverifiedUser.Remove(unverifiedUser);
            await _context.SaveChangesAsync();
        }

        public void SendVerificationEmail(User user, string verificationCode)
        {
            string message;

            message = $@"<h3>Thank you for registering. Your verification code is: <br></h3>
                             <h1>{verificationCode}</h1>";

            _emailServices.Send(
                to: user.Email,
                subject: "Verify Your Email",
                html: message
            );
        }

        public async Task<User> Current()
        {
            var r = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);
            if (r != null)
                return r;
            else
                return null;
        }
    }
}
