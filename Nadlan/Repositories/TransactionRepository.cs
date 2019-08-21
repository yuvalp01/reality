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
            return await Context.Transactions.OrderByDescending(a => a.Id).Include(a => a.Account).Include(a => a.Apartment).ToListAsync();
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

        public async Task<PurchaseReport> GetPurchaseReport(int apartmentId)
        {
            Func<Transaction, bool> basicPredicate = t => t.IsPurchaseCost && t.ApartmentId == apartmentId;
            var investment = Context.Transactions
                .Where(basicPredicate).Where(a => a.AccountId == 15);
            var totalCost = Context.Transactions.Where(basicPredicate)
                .Where(a => a.Amount <= 0);
            var renovationCost = Context.Transactions.Where(basicPredicate)
                .Where(a => a.AccountId == 6);
            var expensesNoRenovation = Context.Transactions.Where(basicPredicate)
                .Where(a => a.Amount <= 0 && a.AccountId != 6 && a.AccountId != 14);
            var accountSummary = Context.Transactions.Include(a=>a.Account).Where(basicPredicate).GroupBy(g => new { g.AccountId, g.Account.Name })
                .Select(a => new AccountSummary
                {
                    AccountId = a.Key.AccountId,
                    Name = a.Key.Name,
                    Total = a.Sum(s => s.Amount)
                }); ;


            PurchaseReport purchaseReport = new PurchaseReport
            {
                Investment = await Task.FromResult(investment.Sum(a => a.Amount)),
                TotalCost = await Task.FromResult(totalCost.Sum(a => a.Amount)),
                RenovationCost = await Task.FromResult(renovationCost.Sum(a => a.Amount)),
                ExpensesNoRenovation = await Task.FromResult(expensesNoRenovation.Sum(a => a.Amount)),
                AccountsSum = await Task.FromResult(accountSummary.ToList())
            };

            purchaseReport.Remainder = purchaseReport.Investment + purchaseReport.TotalCost;

            return purchaseReport;
        }

        public async Task<SummaryReport> GetSummaryReport(int apartmentId, int year)
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
                NetIncome = await Task.FromResult(netIncome.Sum(b => b.Amount)),
                ForDistribution = await Task.FromResult(netIncome.Sum(b => b.Amount)) / 2
            };
            return summaryReport;
        }
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