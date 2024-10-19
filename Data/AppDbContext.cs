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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // تنظیم فیلتر برای Soft Delete
            modelBuilder.Entity<PhoneBook>().HasQueryFilter(p => !p.Deleted);

            base.OnModelCreating(modelBuilder);
        }
    }
}
