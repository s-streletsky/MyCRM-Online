using Microsoft.EntityFrameworkCore;
using System.Data.SQLite;
using System.Text;

namespace MyCRM_Online.Db
{
    public class RepoBase
    {
        readonly static string dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "data.sqlite");

        public static void InitializeDatabase()
        {
            if (File.Exists(dbPath)) { return; }

            using (var db = new DatabaseContext())
            {
                var dbInitScript = File.ReadAllText("./Db/db.init.sql", Encoding.UTF8);
                db.Database.ExecuteSqlRaw(dbInitScript);
            }
        }
    }
}
