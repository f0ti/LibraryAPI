using LibraryAPI.Data;
using LibraryAPI.Data.Exceptions;
using LibraryAPI.IServices;
using LibraryAPI.Model;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LibraryAPI.Services
{
    public class PostmanServices : IPostmanServices
    {
        private readonly LibraryContext _context;
        private readonly IQRServices _QRServices;
        public PostmanServices(LibraryContext context, IQRServices QRServices)
        {
            _context = context;
            _QRServices = QRServices;
        }

        public IEnumerable<BuyRequest> GetAllBuyRequests()
        {
            return _context.BuyRequest;
        }
        public async Task ConfirmDeliveryStatus(string filename)
        {
            string buyRequestToken = _QRServices.DecodeQRCode(filename);
            BuyRequest buyRequest = await _context.BuyRequest.SingleOrDefaultAsync(b => b.BuyRequestTokenIdentifier == buyRequestToken && b.Status != "DELIVERED");

            if (buyRequest == null)
            {
                throw new EntityInvalidationException("No Buy Request exists with this QR code");
            }

            buyRequest.Status = "DELIVERED";
            await _context.SaveChangesAsync();
        }

    }
}
