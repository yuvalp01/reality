using Microsoft.EntityFrameworkCore;
using Nadlan.Models.Renovation;
using Nadlan.ViewModels;
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

        public Task<List<ItemDto>> GetItemsDtoAsync(int apartmentId)
        {

            var items = Context.Lines.Where(a => a.ApartmentId == apartmentId).OrderBy(a=>a.Category).SelectMany(
                a => a.Items, (line, item) => new ItemDto
                {
                    LineId = line.Id,
                    LineCategory = line.Category,
                    LineTitle = line.Title,
                    ItemId = item.Id,
                    ItemDescription = item.Description,
                    Quantity = item.Quantity,
                    ProductId = item.Product.Id,
                    ProductName = item.Product.Name,
                    Price = item.Product.Price,
                    Link = item.Product.Link,
                    Reference = item.Product.Reference,
                    TotalPrice = item.Product.Price * item.Quantity
                });
            return items.ToListAsync();
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
