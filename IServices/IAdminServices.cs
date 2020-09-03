using LibraryAPI.DTO;
using LibraryAPI.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LibraryAPI.IServices
{
    public interface IAdminServices
    {
        Task Authenticate(UserLoginRequestDTO clerkDto);
        Task DeleteAllUsers();
        IEnumerable<User> GetAllUsers();
        Task CreateRole(CreateRoleDTO createRoleDTO);
        Task MakeAdmin(string email);
    }
}
