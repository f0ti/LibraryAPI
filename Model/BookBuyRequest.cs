namespace LibraryAPI.Model
{
    public class BookBuyRequest
    {
        public int ID { get; set; }
        public Book Book { get; set; }
        public BuyRequest BuyRequest { get; set; }
    }
}