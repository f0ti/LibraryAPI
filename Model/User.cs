using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace LibraryAPI.Model
{
    public class User : IdentityUser
    {
        [Required(ErrorMessage = "First Name is required")]
        public string FirstName { get; set; }


        [Required(ErrorMessage = "Last Name is required")]
        public string LastName { get; set; }

        public int NumberOfBooks { get; set; } = 0;
        public int Points { get; set; } = 0;

        public string Role { get; set; }

    }
}
