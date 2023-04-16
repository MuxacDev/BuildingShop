using BuildingShop.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BuildingShop.Data
{
    public class AppDbContext: IdentityDbContext /*DbContext*/
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        
        public DbSet<Category> Category { get; set; }
        public DbSet<AppType> AppType { get; set; }
        public DbSet<Product> Product { get; set; }
        public DbSet<AppUser> AppUser { get; set; }


    }
}
