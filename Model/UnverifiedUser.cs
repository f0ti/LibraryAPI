using System.ComponentModel.DataAnnotations;

namespace LibraryAPI.Model
{
    public class UnverifiedUser
    {
        [Required]
        public int UnverifiedUserId { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        public string VerificationCode { get; set; }
    }
}
