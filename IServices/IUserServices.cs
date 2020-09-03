using LibraryAPI.DTO;
using LibraryAPI.Model;
using System.Threading.Tasks;

namespace LibraryAPI.IServices
{
    public interface IUserServices
    {
        Task Authenticate(UserLoginRequestDTO userDto);
        Task Create(UserRegisterRequestDTO userDto);
        Task VerifyAccount(string verificationCode);
        Task<User> Current();
    }
}
