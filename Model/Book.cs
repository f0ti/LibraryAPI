using System.ComponentModel.DataAnnotations;

namespace LibraryAPI.Model
{
    public class Book
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "A title is required")]
        public string Title { get; set; }

        public Author Author { get; set; }

        [Required(ErrorMessage = "ISBN is required")]
        [StringLength(13, ErrorMessage = "ISBN consists of 13 digits")]
        public string ISBN { get; set; }

        [Required]
        public int NrOfCopies { get; set; }

        public int PagesNumber { get; set; }

        [Required(ErrorMessage = "Rent Availability is required")]
        public bool RentAvailable { get; set; }

        [Required(ErrorMessage = "Buy Availability is required")]
        public bool BuyAvailable { get; set; }

        [Required(ErrorMessage = "Price is required")]
        public int Price { get; set; }

        public int Year { get; set; }

    }
}
