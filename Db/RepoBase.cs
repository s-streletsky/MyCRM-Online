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

            //using (var connection = GetConnection())
            //{
            //    var sql = connection.CreateCommand();
            //    sql.CommandText = File.ReadAllText("./Db/db.init.sql", Encoding.UTF8);
                
            //    sql.ExecuteNonQuery();
            //}
        }

        //internal static SQLiteConnection GetConnection(bool open = true)
        //{
        //    var builder = new SQLiteConnectionStringBuilder();
        //    builder.DataSource = dbPath;

        //    var connection = new SQLiteConnection(builder.ToString());
        //    if (open)
        //    {
        //        connection.Open();
        //    }

        //    return connection;
        //}
    }
}
