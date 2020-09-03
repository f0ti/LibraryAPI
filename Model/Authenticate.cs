using System.ComponentModel.DataAnnotations;

namespace LibraryAPI.Model
{
    public class Authenticate
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
}