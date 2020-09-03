using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LibraryAPI.DTO
{
    public class BuyRequestDTO
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public List<int> BookIds { get; set; }

        [Required]
        public string Address { get; set; }
    }
}