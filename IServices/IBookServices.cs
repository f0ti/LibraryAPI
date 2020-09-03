using LibraryAPI.Model;
using System.Threading.Tasks;

namespace LibraryAPI.IServices
{
    public interface IBookServices
    {
        Task AddBooks(Book book);
        Task<Book> GetBookById(int id);
        Task UpdateBook(Book updatedBook, int id);
    }
}
