using LibraryAPI.DTO;
using LibraryAPI.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LibraryAPI.IServices
{
    public interface IBuyServices
    {
        Task Purchase(BuyRequestDTO buyRequestDto);
        IEnumerable<BuyRequest> GetPurchasesFromUsername(User user);
    }
}
