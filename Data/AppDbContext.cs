using Microsoft.EntityFrameworkCore;
using Web_Api.Class;

namespace Web_Api.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        
        }
        public DbSet<PhoneBook> PhoneBooks { get; set; }
    }
}
