﻿using Microsoft.EntityFrameworkCore;
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
            return await Context.PersonalTransactions
                .OrderByDescending(a => a.Id)
                .Include(a => a.Stakeholder)
                .Where(a=>!a.IsDeleted)
                .ToListAsync();
        }

        public async Task<List<PersonalTransaction>> GetByStakeholderAsync(int stakeholderId)
        {
            return await Context.PersonalTransactions
                .Where(a => !a.IsDeleted)
                .Where(a => a.StakeholderId == stakeholderId)
                .OrderByDescending(a => a.Date)
                .Include(a=>a.Stakeholder)
                .Include(a=>a.Apartment)
                .ToListAsync();
        }



        internal async Task<List<PersonalTransaction>>  GetAllDistributions(int stakeholderId)
        {
            return await Context.PersonalTransactions
                .Where(a => !a.IsDeleted)
                .Where(a => a.StakeholderId == stakeholderId)
                .Where(a => a.TransactionType == TransactionType.Distribution)
                .OrderByDescending(a => a.Date)
                .Include(a => a.Stakeholder)
                .Include(a => a.Apartment)
                .ToListAsync();
        }

        public async Task<List<Stakeholder>> GetStakeholdersAsync()
        {
            return await Context.Stakeholders.ToListAsync();
                //.Where(a => a.StakeholderId == stakeholderId)
                //.OrderByDescending(a => a.Date)
                //.Include(a => a.Stakeholder)
                //.ToListAsync();
        }


        public async Task CreateTransactionAsync(PersonalTransaction transaction)
        {
            Create(transaction);
            await SaveAsync();
        }
        public async Task UpdateTransactionAsync(PersonalTransaction transaction)
        {
            Update(transaction);
            await SaveAsync();
        }
        //public async Task UpdateTransactionAsync(PersonalTransaction dbTransaction, PersonalTransaction transaction)
        //{
        //    Update(transaction);
        //    await SaveAsync();
        //}
        [Obsolete]
        public async Task DeleteTransactionAsync(PersonalTransaction transaction)
        {
            throw new NotImplementedException("delete only direcly in db with boolean flag");
            //Delete(transaction);
            //await SaveAsync();
        }

        public Task<PersonalTransaction> GetByIdAsync(int id)
        {
            return Context.PersonalTransactions.FindAsync(id);
        }

    }


}




