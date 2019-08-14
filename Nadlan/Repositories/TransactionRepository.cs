using Nadlan.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Nadlan.Repositories
{
    public class TransactionRepository : Repository<Transaction>, IRepository<Transaction>
    {
        private readonly NadlanConext _context;
        public TransactionRepository(NadlanConext context) : base(context)
        {
            _context = context;
        }

        public override async Task<List<Transaction>> GetAllAsync()
        {
            return await _context.Transactions.Include(a => a.Account).Include(a => a.Apartment).ToListAsync();
        }
    }
}
