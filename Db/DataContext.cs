using Microsoft.EntityFrameworkCore;
using System.Text;
using MyCRM_Online.Models.Entities;

namespace MyCRM_Online.Db
{
    public class DataContext : DbContext
    {
        readonly static string dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "data.sqlite");

        public DbSet<UserEntity> Users { get; set; }
        public DbSet<ClientEntity> Clients { get; set; }
        public DbSet<CountryEntity> Countries { get; set; }
        public DbSet<ShippingMethodEntity> ShippingMethods { get; set; }

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
            modelBuilder.Entity<ClientEntity>().Navigation(e => e.Country).AutoInclude();
            modelBuilder.Entity<ClientEntity>().Navigation(e => e.ShippingMethod).AutoInclude();
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
