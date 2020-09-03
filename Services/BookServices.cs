using LibraryAPI.Data;
using LibraryAPI.IServices;
using LibraryAPI.Model;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryAPI.Services
{
    public class BookServices : IBookServices
    {
        private readonly LibraryContext _context;

        public BookServices(LibraryContext context)
        {
            _context = context;
        }

        public async Task AddBooks(Book book)
        {
            // Check if the author already exists
            Author oldAuthor = _context.Author.FirstOrDefault(a =>
                (a.FirstName == book.Author.FirstName && a.LastName == book.Author.LastName));

            // If old author exists, then assign its info to the book we're adding
            if (oldAuthor != null)
            {
                book.Author = oldAuthor;
            }

            book.Author.NrBooks++;

            await _context.Book.AddAsync(book);
            await _context.SaveChangesAsync();
        }

        public async Task<Book> GetBookById(int id)
        {
            return await _context.Book.SingleOrDefaultAsync(book => book.Id == id);
        }

        public async Task UpdateBook(Book updatedBook, int id)
        {
            Book book = await _context.Book.SingleOrDefaultAsync(book => book.Id == id);

            // Update values
            book.Title = updatedBook.Title;
            book.Author = updatedBook.Author;
            book.ISBN = updatedBook.ISBN;
            book.PagesNumber = updatedBook.PagesNumber;
            book.RentAvailable = updatedBook.RentAvailable;
            book.Year = updatedBook.Year;

            await _context.SaveChangesAsync();
        }

    }
}
