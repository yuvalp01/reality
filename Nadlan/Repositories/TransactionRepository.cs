using Nadlan.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Nadlan.Repositories
{
    public class TransactionRepository : Repository<Transaction>, ITransactionRepository
    {
        public TransactionRepository(NadlanConext context) : base(context)
        {
        }


        public override async Task<List<Transaction>> GetAllAsync()
        {
            return await Context.Transactions.OrderByDescending(a => a.Date).Include(a => a.Account).Include(a => a.Apartment).ToListAsync();
        }

        public async Task CreateTransactionAsync(Transaction transaction)
        {
            Account account = await Context.Accounts.FirstAsync(a => a.Id == transaction.AccountId);
            transaction.Amount = !account.IsIncome ? transaction.Amount * -1 : transaction.Amount;
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



        public async Task<SummaryReport> GetReport(int apartmentId, int year)
        {
            Func<Transaction, bool> predAll = t => t.IsPurchaseCost == false && t.ApartmentId == apartmentId;
            Func<Transaction, bool> predWithYear = t =>
               t.IsPurchaseCost == false
            && t.ApartmentId == apartmentId
            && t.Date.Year == year;


            Func<Transaction, bool> pred = year == 0 ? predAll : predWithYear;


            var grossIncome = Context.Transactions.Where(pred)
                .Where(a => a.AccountId == 1);


            var expenses = Context.Transactions.Where(pred)
                .Where(a => a.AccountId != 5
                         && a.Amount <= 0);

            var tax = Context.Transactions.Where(pred)
                .Where(a => a.AccountId == 5
                         && a.Amount <= 0);

            var netIncome = Context.Transactions.Where(pred);


            SummaryReport summaryReport = new SummaryReport
            {
                GrossIncome = await Task.FromResult(grossIncome.Sum(b => b.Amount)),
                Expenses = await Task.FromResult(expenses.Sum(b => b.Amount)),
                Tax = await Task.FromResult(tax.Sum(b => b.Amount)),
                NetIcome = await Task.FromResult(netIncome.Sum(b => b.Amount)),
                ForDistribution = await Task.FromResult(netIncome.Sum(b => b.Amount)) / 2
            };
            return summaryReport;
        }
    }

    //var grossIncome_old = Context.Transactions.Where(a =>
    //              a.IsPurchaseCost == false
    //           && a.Apartment.Id == apartmentId
    //           && a.Date.Year == year
    //           && a.AccountId == 1);//.Sum(b => b.Amount);

    //        var expenses_old = Context.Transactions.Where(a =>
    //               a.IsPurchaseCost == false
    //            && a.Apartment.Id == apartmentId
    //            && a.Date.Year == year
    //            && a.AccountId != 5
    //            && a.Amount <= 0);//.Sum(b => b.Amount);

    //        var tax_old = Context.Transactions.Where(a =>
    //               a.IsPurchaseCost == false
    //            && a.Apartment.Id == apartmentId
    //            && a.Date.Year == year
    //            && a.AccountId == 5
    //            && a.Amount <= 0);//.Sum(b => b.Amount);

    //        var netIncome_old = Context.Transactions.Where(a =>
    //               a.IsPurchaseCost == false
    //            && a.Apartment.Id == apartmentId
    //            && a.Date.Year == year);//.Sum(b => b.Amount);

    //        var forDistribution_old = Context.Transactions.Where(a =>
    //               a.IsPurchaseCost == false
    //            && a.Apartment.Id == apartmentId
    //            && a.Date.Year == year);//.Sum(b => b.Amount) / 2;

            //SummaryReport summaryReport_ = new SummaryReport
            //{
            //    GrossIncome = await grossIncome_old.SumAsync(b => b.Amount),
            //    Expenses = await expenses_old.SumAsync(b => b.Amount),
            //    Tax = await tax_old.SumAsync(b => b.Amount),
            //    NetIcome = await netIncome_old.SumAsync(b => b.Amount),
            //    ForDistribution = await netIncome.SumAsync(b => b.Amount) / 2,
            //};





    public class SummaryReport
    {
        public decimal GrossIncome { get; set; }
        public decimal Expenses { get; set; }
        public decimal Tax { get; set; }
        public decimal NetIcome { get; set; }
        public decimal ForDistribution { get; set; }
    }

}
