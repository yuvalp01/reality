using Nadlan.Models.Renovation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nadlan.Repositories
{
    public class RenovationItemRepository : Repository<Item>, IRenovationItemRepository
    {
        public RenovationItemRepository(NadlanConext nadlanConext) : base(nadlanConext)
        {
        }

        public Task CreateAsync(Item item)
        {
            Create(item);          
            return Context.SaveChangesAsync();
        }

        public Task DeleteAsync(Item item)
        {
            throw new NotImplementedException();
        }

        public Task<Item> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(Item item)
        {
            throw new NotImplementedException();
        }
    }
}
