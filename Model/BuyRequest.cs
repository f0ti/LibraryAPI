using System;

namespace LibraryAPI.Model
{
    public class BuyRequest : Request
    {
        public int BuyRequestId { get; set; }

        public DateTime ExpireDate { get; set; }

        public string BuyRequestTokenIdentifier { get; set; }

        public string Status { get; set; }
    }
}