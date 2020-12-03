using Microsoft.EntityFrameworkCore;
using Nadlan.Models;
using Nadlan.Models.Renovation;
using Nadlan.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Nadlan.Repositories.Renovation
{
    public class RenovationProductRepository : Repository<RenovationProduct>
    {
        public RenovationProductRepository(NadlanConext context) : base(context)
        {
        }


        public override async Task<List<RenovationProduct>> GetAllAsync()
        {
            return await Context.RenovationProducts
                .OrderByDescending(a => a.Id)
                .Where(a=>!a.IsDeleted)
                .ToListAsync();
        }

        internal async Task<List<RenovationProduct>> GetByTypeAsync(string itemType)
        {
            return await Context.RenovationProducts
                .OrderByDescending(a => a.Id)
                .Where(a => !a.IsDeleted)
                .Where(a => a.ItemType== itemType)
                .ToListAsync();
        }


        public Task<RenovationProduct> GetByIdAsync(int id)
        {
            return Context.RenovationProducts
                .FirstOrDefaultAsync(a => a.Id == id);
        }


        public async Task CreateAsync(RenovationProduct  renovationProduct)
        {
            Create(renovationProduct);
            await SaveAsync();
        }
        public async Task UpdateAsync(RenovationProduct renovationProduct)
        {
            Update(renovationProduct);
            await SaveAsync();
        }

        public async Task SoftDeleteAsync(int renovationProductId)
        {
            var renovationProduct = await GetByIdAsync(renovationProductId);
            renovationProduct.IsDeleted = true;
            await UpdateAsync(renovationProduct);

        }


    }


}




