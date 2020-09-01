using Microsoft.EntityFrameworkCore;
using Nadlan.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nadlan.Repositories.Messages
{
    public class ContractRepository : IContractRepository
    {
        private NadlanConext _context;
        public ContractRepository(NadlanConext context)
        {
            _context = context;
        }

        public Task<List<Contract>> GetAllAsync()
        {
            var contracts = _context.Contracts
                .Include(a => a.Apartment)
                .Where(a => a.IsDeleted == false)
                .OrderBy(a => a.Apartment.Id)
                .ToListAsync();
            return contracts;
        }

        public Task<Contract> GetByIdAsync(int id)
        {
            return _context.Contracts
                .Include(a => a.Apartment)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task CreateAsync(Contract contract)
        {
            _context.Contracts.Add(contract);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Contract contract)
        {
            _context.Contracts.Update(contract);
            await _context.SaveChangesAsync();

        }

        public Task CancelAllConfirmations()
        {
            return _context.Database.ExecuteSqlCommandAsync("update contracts set IsPaymentConfirmed=0");
        }
        public async Task SoftDeleteAsync(int id)
        {
            var contractToDelete = _context.Contracts.Find(id);
            contractToDelete.IsDeleted = true;
            //_context.Update(contractToDelete);
            await _context.SaveChangesAsync();

        }

    }
}

