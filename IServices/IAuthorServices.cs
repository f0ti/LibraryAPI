using LibraryAPI.Model;
using System.Threading.Tasks;

namespace LibraryAPI.IServices
{
    public interface IAuthorServices
    {
        Task AddAuthors(Author author);
        Task<Author> GetAuthorById(int id);
        Task UpdateAuthor(Author updatedBook, int id);
    }
}
