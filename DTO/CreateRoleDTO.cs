using System.ComponentModel.DataAnnotations;

namespace LibraryAPI.DTO
{
    public class CreateRoleDTO
    {
        [Required]
        public string RoleName { get; set; }
    }
}
