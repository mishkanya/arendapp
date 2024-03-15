using ArendApp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace ArendApp.Api.Services
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Product> Products { get; set; }
        public DbSet<Image> Images { get; set; }

        public DbSet<User> UsersData { get; set; }
        public DbSet<SendedCode> SendedCodes { get; set; }

        public DbSet<UserInventory> UsersInventory { get; set; }
        public DbSet<UserBasket> UsersBasket { get; set; }
    }
}
