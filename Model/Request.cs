using System;
using System.ComponentModel.DataAnnotations;

namespace LibraryAPI.Model
{
    public class Request
    {
        public int RequestId { get; set; }

        [Required]
        public User User { get; set; }

        [Required]
        public double TotalPrice { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        public DateTime Date { get; set; }
    }
}
