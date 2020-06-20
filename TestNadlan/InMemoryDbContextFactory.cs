using Microsoft.EntityFrameworkCore;
using Nadlan.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace TestNadlan
{
    class InMemoryDbContextFactory
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
