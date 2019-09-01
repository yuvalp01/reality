using Nadlan.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Nadlan.Repositories
{
    public class TransactionRepository : Repository<Transaction>, ITransactionRepository
    {
        public TransactionRepository(NadlanConext context) : base(context)
        {
        }


        public override async Task<List<Transaction>> GetAllAsync()
        {
            return await Context.Transactions.OrderByDescending(a => a.Id).Include(a => a.Account).Include(a => a.Apartment).ToListAsync();
        }



        public async Task CreateTransactionAsync(Transaction transaction)
        {
            Account account = await Context.Accounts.FirstAsync(a => a.Id == transaction.AccountId);
            //Only for normal accouts - depends on the account isIncome property
            if (account.AccountTypeId==0)
            {
                transaction.Amount = !account.IsIncome ? transaction.Amount * -1 : transaction.Amount;
            }
            Create(transaction);
            await SaveAsync();
        }

        public async Task UpdateTransactionAsync(Transaction dbTransaction, Transaction transaction)
        {
            Update(transaction);
            await SaveAsync();
        }
        public async Task DeleteTransactionAsync(Transaction transaction)
        {
            Delete(transaction);
            await SaveAsync();
        }

        public Task<Transaction> GetByIdAsync(int id)
        {
            return Context.Transactions.FindAsync(id);
        }

        public Task<List<Transaction>> GetByAcountAsync(int apartmentId, int accountId, bool isPurchaseCost)
        {
            return FindByCondition(c =>
            //c.Amount <= 0  &&
            c.ApartmentId == apartmentId 
            && c.AccountId == accountId 
            && c.IsPurchaseCost == isPurchaseCost).OrderByDescending( a =>a.Date).ToListAsync();
        }



    }


}




