using Microsoft.EntityFrameworkCore;
using Nadlan.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nadlan.Repositories
{
    public class NadlanConext: DbContext
    {
        public NadlanConext(DbContextOptions<NadlanConext> options) : base(options) 
        {

        }
        public DbSet<Apartment> Apartments { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
    }
}
