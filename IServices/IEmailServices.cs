using System.Threading.Tasks;

namespace LibraryAPI.IServices
{
    public interface IEmailServices
    {
        Task Send(string to, string subject, string html, string from = null);
    }
}
