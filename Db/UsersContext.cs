using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MyCRM_Online.Models.Entities;

namespace MyCRM_Online.Db
{
    public class UsersContext : IdentityDbContext<UserEntity>
    {
        public UsersContext(DbContextOptions<UsersContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
