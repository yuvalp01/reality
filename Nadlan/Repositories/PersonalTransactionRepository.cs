using Microsoft.EntityFrameworkCore;
using Nadlan.Models;
using Nadlan.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Nadlan.Repositories
{
    public class PersonalTransactionRepository : Repository<PersonalTransaction>, IPersonalTransactionRepository
    {
        public PersonalTransactionRepository(NadlanConext context) : base(context)
        {
        }


        public override async Task<List<PersonalTransaction>> GetAllAsync()
        {
            return await Context.PersonalTransactions.OrderByDescending(a => a.Id).Include(a => a.Stakeholder).ToListAsync();
        }

        public async Task<List<PersonalTransaction>> GetByStakeholderAsync(int stakeholderId)
        {
            return await Context.PersonalTransactions
                .Where(a => a.StakeholderId == stakeholderId)
                .OrderByDescending(a => a.Date)
                .Include(a=>a.Stakeholder)
                .ToListAsync();
        }

        public async Task CreateTransactionAsync(PersonalTransaction transaction)
        {
            Create(transaction);
            await SaveAsync();
        }

        public async Task UpdateTransactionAsync(PersonalTransaction dbTransaction, PersonalTransaction transaction)
        {
            Update(transaction);
            await SaveAsync();
        }
        public async Task DeleteTransactionAsync(PersonalTransaction transaction)
        {
            Delete(transaction);
            await SaveAsync();
        }

        public Task<PersonalTransaction> GetByIdAsync(int id)
        {
            return Context.PersonalTransactions.FindAsync(id);
        }


 
    }


}




