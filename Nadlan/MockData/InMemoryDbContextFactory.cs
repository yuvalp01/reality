using Microsoft.EntityFrameworkCore;
using Nadlan.Repositories;

namespace Nadlan.MockData
{
    public class InMemoryDbContextFactory
    {
        public NadlanConext GetMockNadlanDbContext()
        {
            var options = new DbContextOptionsBuilder<NadlanConext>()
                            .UseInMemoryDatabase(databaseName: "InMemoryMockNadlanDatabase")
                            .Options;
            var dbContext = new NadlanConext(options);

            return dbContext;
        }
    }
}
