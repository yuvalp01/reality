using Nadlan.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Nadlan.Models.Renovation;

namespace Nadlan.Repositories
{
    public interface IRenovationItemRepository 
    {
        Task<List<Item>> GetAllAsync();
        Task<Item> GetByIdAsync(int id);
        //Task<List<Transaction>> GetByAcountAsync(int apartmentId, int accountId, bool isPurchaseCost);
        Task CreateAsync(Item Item);
        Task UpdateAsync(Item Item);
        Task DeleteAsync(Item Item);

    }
}
