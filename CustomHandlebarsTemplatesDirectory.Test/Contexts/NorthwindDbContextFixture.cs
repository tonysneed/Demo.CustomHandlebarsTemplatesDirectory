using System;
using Microsoft.EntityFrameworkCore;

namespace CustomHandlebarsTemplatesDirectory.Test.Contexts
{
    public class NorthwindDbContextFixture
    {
        private NorthwindDbContext _context;
        private DbContextOptions<NorthwindDbContext> _options;

        public void Initialize(Action seedData = null)
        {
            _options = new DbContextOptionsBuilder<NorthwindDbContext>()
                .UseSqlServer(Helpers.Connections.NorthwindTestConnection)
                .Options;
            _context = new NorthwindDbContext(_options);
            _context.Database.EnsureCreated(); // If login error, manually create NorthwindTestDb database
            seedData?.Invoke();
        }
    }
}
