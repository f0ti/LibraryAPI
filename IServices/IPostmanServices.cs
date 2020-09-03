using LibraryAPI.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LibraryAPI.IServices
{
    public interface IPostmanServices
    {
        IEnumerable<BuyRequest> GetAllBuyRequests();
        Task ConfirmDeliveryStatus(string filename);
    }
}
