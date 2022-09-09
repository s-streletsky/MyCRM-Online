using MyCRM_Online.Models;
using System.Collections.ObjectModel;
using System.Data.Common;
using System.Data.SQLite;
using System.Diagnostics.Metrics;
using Microsoft.EntityFrameworkCore;

namespace MyCRM_Online.Db
{
    public class ClientsRepo : RepoBase
    {
        public IEnumerable<Client> GetAll()
        {
            using (var db = new DatabaseContext())
            {
                return db.Clients.ToList();
            }
        }

        public void Add(Client client)
        {
            using (var db = new DatabaseContext())
            {
                db.Clients.Add(client);
                db.SaveChanges();
            }
        }
    }
}
