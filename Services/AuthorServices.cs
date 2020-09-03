using LibraryAPI.Data;
using LibraryAPI.IServices;
using LibraryAPI.Model;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace LibraryAPI.Services
{
    public class AuthorServices : IAuthorServices
    {
        private readonly LibraryContext _context;

        public AuthorServices(LibraryContext context)
        {
            _context = context;
        }

        public async Task AddAuthors(Author author)
        {
            await _context.Author.AddAsync(author);
            await _context.SaveChangesAsync();
        }

        public async Task<Author> GetAuthorById(int id)
        {
            return await _context.Author.SingleOrDefaultAsync(author => author.Id == id);
        }

        public async Task UpdateAuthor(Author updatedBook, int id)
        {
            Author author = await _context.Author.SingleOrDefaultAsync(author => author.Id == id);

            // Update values


            await _context.SaveChangesAsync();
        }

    }
}
