using Nadlan.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Nadlan.Repositories
{
    public class AccountRepository : Repository<Account>, IRepository<Account>
    {
        private readonly NadlanConext _context;
        public AccountRepository(NadlanConext context) : base(context)
        {
            _context = context;
        }
    }
}
