using Microsoft.EntityFrameworkCore;
using Practic.Domain.Entities;

namespace Practic.Data.DbContexts
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }
        public DbSet<User> Users { get; set; }
    }
}
