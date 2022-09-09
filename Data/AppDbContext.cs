using Dotnetrest.Model;
using Microsoft.EntityFrameworkCore;

namespace Dotnetrest.Data
{
    public class ApplicationDatabaseContext : DbContext
    {
        public ApplicationDatabaseContext(DbContextOptions options) : base(options) { }

        public DbSet<UserModel> User { get; set; }
    }
}