using Microsoft.EntityFrameworkCore;
using Nadlan.Models.Renovation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nadlan.Repositories
{
    public class RenovationLineRepository_old : Repository<Line>
    {
        public RenovationLineRepository_old(NadlanConext nadlanConext) : base(nadlanConext)
        {
        }

        //public Task<List<Line>> GetByApartmentIdAsync(int apartmentId)
        //{

        //    var lines = Context.Lines.Where(a => a.ApartmentId == apartmentId)
        //        .Include(a => a.Items)
        //        .ThenInclude(b => b.Product)
        //        //.Select(a=>a.ItemsTotalPrice= a.Items.Sum(b=>b.Product.Price))
        //        .ToListAsync();

        //    return lines;
        //}

        public Task CreateAsync(Line line)
        {
            Create(line);
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
