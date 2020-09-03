using LibraryAPI.Model;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LibraryAPI.Data
{
    public class LibraryContext : IdentityDbContext<User>
    {
        public LibraryContext(DbContextOptions<LibraryContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Author>().
                HasIndex(a => new { a.FirstName, a.LastName }).
                IsUnique();
        }

        // Set DB Tables
        public DbSet<User> User { get; set; }
        public DbSet<Book> Book { get; set; }
        public DbSet<Author> Author { get; set; }
        public DbSet<BuyRequest> BuyRequest { get; set; }
        public DbSet<UnverifiedUser> UnverifiedUser { get; set; }
        public DbSet<BookBuyRequest> BookBuyRequests { get; set; }
    }
}