using MyCRM_Online.Models;
using Microsoft.EntityFrameworkCore;

namespace MyCRM_Online.Db
{
    public class DatabaseContext : DbContext
    {
        readonly static string dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "data.sqlite");

        public DbSet<Client> Clients { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<ShippingMethod> ShippingMethods { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Data Source={dbPath}");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Client>().Navigation(e => e.Country).AutoInclude();
            modelBuilder.Entity<Client>().Navigation(e => e.ShippingMethod).AutoInclude();
        }
    }
}
