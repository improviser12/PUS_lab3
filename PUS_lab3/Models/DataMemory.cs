using Microsoft.EntityFrameworkCore;

namespace PUS_lab3.Models
{
    public class DataMemory : DbContext
    {
        public DataMemory(DbContextOptions<DataMemory> options)
            : base(options)
        {

        }

        public DbSet<Person> People { get; set; }
        public DbSet<Car> Cars { get; set; }
        public DbSet<UserInfo> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserInfo>().HasData
            (
                new UserInfo { UserId = 1, UserName = "admin", Password = "123", UserRole = "Admin" }
            );
        }
    }
}