using CustomHandlebarsTemplatesDirectory.Test.Models;
using Microsoft.EntityFrameworkCore;

namespace CustomHandlebarsTemplatesDirectory.Test.Contexts
{
    public class NorthwindDbContext : DbContext
    {
        public NorthwindDbContext() { }

        public NorthwindDbContext(DbContextOptions options) : base(options) { }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }
    }
}
