using LibraryAPI.Data;
using LibraryAPI.Data.Exceptions;
using LibraryAPI.DTO;
using LibraryAPI.IServices;
using LibraryAPI.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryAPI.Services
{
    public class BuyServices : IBuyServices
    {
        private readonly LibraryContext _context;
        private readonly IQRServices _QRServices;
        public BuyServices(LibraryContext context, IQRServices QRServices)
        {
            _context = context;
            _QRServices = QRServices;
        }

        public async Task Purchase(BuyRequestDTO buyRequestDto)
        {
            User user = await _context.User.FirstOrDefaultAsync(u => u.UserName == buyRequestDto.Username);
            List<Book> books = _context.Book.Where(b => buyRequestDto.BookIds.Contains(b.Id)).ToList();

            if (user == null || books == null)
            {
                throw new EntityInvalidationException("Gabim");
            }

            // Calculate the total price
            int totalPrice = 0;
            foreach (Book book in books)
            {
                totalPrice += book.Price;
            }

            // Generate the unique token identifier
            string code = Guid.NewGuid().ToString("n").Substring(0, 12);

            BuyRequest buyRequest = new BuyRequest
            {
                User = user,
                Address = buyRequestDto.Address,
                Date = DateTime.Now,
                ExpireDate = DateTime.Now.AddDays(5),
                Status = "PENDING",
                BuyRequestTokenIdentifier = code
            };

            await _context.BuyRequest.AddAsync(buyRequest);
            await _context.SaveChangesAsync();

            // Associating books to the BookBuyRequest table with the current Request
            foreach (Book book in books)
            {
                BookBuyRequest bookBuyRequest = new BookBuyRequest
                {
                    Book = book,
                    BuyRequest = buyRequest
                };
                await _context.BookBuyRequests.AddAsync(bookBuyRequest);
            }

            // Generate QR code for the request
            _QRServices.GenerateQRCode(code);

            // Add points to the user
            // More complex -> include a formula like this: nrOfBuys + (a parabola formula to avoid exponential growth) ## NEEDS RESTART
            user.Points = (int)(totalPrice * 0.1);

            await _context.SaveChangesAsync();
        }

        public IEnumerable<BuyRequest> GetPurchasesFromUsername(User user)
        {
            return _context.BuyRequest.Where(r => r.User == user);
        }
    }
}
