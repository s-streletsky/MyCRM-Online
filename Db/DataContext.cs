using MyCRM_Online.Models;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace MyCRM_Online.Db
{
    public class DataContext : DbContext
    {
        readonly static string dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "data.sqlite");

        public DbSet<User> Users { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<ShippingMethod> ShippingMethods { get; set; }

        static DataContext()
        {
            InitializeDatabase();
        }

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

        public static void InitializeDatabase()
        {
            if (File.Exists(dbPath)) { return; }

            using (var databaseContext = new DataContext())
            {
                var dbInitScript = File.ReadAllText("./Db/db.init.sql", Encoding.UTF8);
                databaseContext.Database.ExecuteSqlRaw(dbInitScript);

                //var dbDefaultData = File.ReadAllText("./Db/db.data.sql", Encoding.UTF8);
                //databaseContext.Database.ExecuteSqlRaw(dbDefaultData);
            }
        }
    }
}
