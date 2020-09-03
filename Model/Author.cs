using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace LibraryAPI.Model
{
    public partial class Author
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "First Name is required")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last Name is required")]
        public string LastName { get; set; }

        public int NrBooks { get; set; }

        public IQueryable<Book> Books { get; set; }
    }
}
