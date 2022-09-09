using MyCRM_Online.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCRM_Online.Db
{
    public class CountriesRepo
    {
        public IEnumerable<Country> GetAll()
        {
            using (var db = new DatabaseContext())
            {
                return db.Countries.ToList();
            }
        }
    }
}
