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


        //private decimal CalcPredictedROI(Apartment apartment, decimal investment)
        //{
        //    decimal yearlyCosts = 100 + 350;
        //    decimal netRent = apartment.CurrentRent * 0.85m - apartment.FixedMaintanance - yearlyCosts / 12;
        //    decimal anualNetIncome = netRent * 11;
        //    decimal predictedRoi = anualNetIncome / investment;
        //    return predictedRoi;
        //}

        //private decimal CalcROI(Apartment apartment, SummaryReport summaryReport)
        //{

        //    DateTime zeroTime = new DateTime(1, 1, 1);
        //    TimeSpan span = DateTime.Today - apartment.PurchaseDate;
        //    decimal years_dec = ((zeroTime + span).Year - 1) + ((zeroTime + span).Month - 1) / 12m;
        //    decimal roi = (summaryReport.NetIncome / summaryReport.Investment) / years_dec;
        //    return roi;
        //}



        //public async Task<SummaryReport> GetSummaryReport(int apartmentId)
        //{

        //    Func<Transaction, bool> basicPredicate = t => t.IsPurchaseCost && t.ApartmentId == apartmentId;
        //    var investment = Context.Transactions.Where(basicPredicate).Where(a => a.AccountId == 15);
        //    var netIncome = Context.Transactions.Where(basicPredicate);
        //    var totalCost = Context.Transactions.Where(basicPredicate).Where(a => a.Amount <= 0);
        //    SummaryReport summaryReport = new SummaryReport
        //    {
        //        Investment = await Task.FromResult(investment.Sum(a => a.Amount)),
        //        NetIncome = await Task.FromResult(netIncome.Sum(a => a.Amount)),
        //    };

        //    summaryReport.InitialRemainder = summaryReport.Investment + await Task.FromResult(totalCost.Sum(a => a.Amount));
        //    summaryReport.Balance = summaryReport.InitialRemainder + summaryReport.NetIncome;

        //    //Apartment apartment = Context.Apartments.Where(a => a.Id == apartmentId).First();     
        //    Apartment apartment = new Apartment { CurrentRent = 500, FixedMaintanance = 55, PurchaseDate = new DateTime(2017, 12, 20) };

        //    summaryReport.ORI = CalcROI(apartment, summaryReport);
        //    summaryReport.PredictedORI = CalcPredictedROI(apartment, summaryReport.Investment);


        //    return summaryReport;
        //}

        //public async Task<PurchaseReport> GetPurchaseReport(int apartmentId)
        //{
        //    Func<Transaction, bool> basicPredicate = t => t.IsPurchaseCost && t.ApartmentId == apartmentId;
        //    var investment = Context.Transactions.Where(basicPredicate).Where(a => a.AccountId == 15);

        //    var totalCost = Context.Transactions.Where(basicPredicate)
        //        .Where(a => a.Amount <= 0);
        //    var renovationCost = Context.Transactions.Where(basicPredicate)
        //        .Where(a => a.AccountId == 6);
        //    var expensesNoRenovation = Context.Transactions.Where(basicPredicate)
        //        .Where(a => a.Amount <= 0 && a.AccountId != 6 && a.AccountId != 14);
        //    var accountSummary = Context.Transactions.Include(a => a.Account).Where(basicPredicate).GroupBy(g => new { g.AccountId, g.Account.Name })
        //        .Select(a => new AccountSummary
        //        {
        //            AccountId = a.Key.AccountId,
        //            Name = a.Key.Name,
        //            Total = a.Sum(s => s.Amount)
        //        }); ;


        //    PurchaseReport purchaseReport = new PurchaseReport
        //    {
        //        Investment = await Task.FromResult(investment.Sum(a => a.Amount)),
        //        TotalCost = await Task.FromResult(totalCost.Sum(a => a.Amount)),
        //        RenovationCost = await Task.FromResult(renovationCost.Sum(a => a.Amount)),
        //        ExpensesNoRenovation = await Task.FromResult(expensesNoRenovation.Sum(a => a.Amount)),
        //        AccountsSum = await Task.FromResult(accountSummary.ToList())
        //    };

        //    purchaseReport.Remainder = purchaseReport.Investment + purchaseReport.TotalCost;

        //    return purchaseReport;
        //}

        //public async Task<IncomeReport> GetIncomeReport(int apartmentId, int year)
        //{
        //    Func<Transaction, bool> predAll = t => t.IsPurchaseCost == false && t.ApartmentId == apartmentId;
        //    Func<Transaction, bool> predWithYear = t =>
        //       t.IsPurchaseCost == false
        //    && t.ApartmentId == apartmentId
        //    && t.Date.Year == year;

        //    Func<Transaction, bool> basicPredicate = year == 0 ? predAll : predWithYear;

        //    var grossIncome = Context.Transactions.Where(basicPredicate)
        //        .Where(a => a.AccountId == 1);


        //    var expenses = Context.Transactions.Where(basicPredicate)
        //        .Where(a => a.AccountId != 5
        //                 && a.Amount <= 0);

        //    var tax = Context.Transactions.Where(basicPredicate)
        //        .Where(a => a.AccountId == 5
        //                 && a.Amount <= 0);

        //    var netIncome = Context.Transactions.Where(basicPredicate);


        //    IncomeReport summaryReport = new IncomeReport
        //    {
        //        GrossIncome = await Task.FromResult(grossIncome.Sum(b => b.Amount)),
        //        Expenses = await Task.FromResult(expenses.Sum(b => b.Amount)),
        //        Tax = await Task.FromResult(tax.Sum(b => b.Amount)),
        //        NetIncome = await Task.FromResult(netIncome.Sum(b => b.Amount)),
        //        ForDistribution = await Task.FromResult(netIncome.Sum(b => b.Amount)) / 2
        //    };
        //    return summaryReport;
        //}
    }




}



//DateTime zeroTime = new DateTime(1, 1, 1);
//TimeSpan span = DateTime.Today - apartment.PurchaseDate;

//int months = ((zeroTime + span).Year - 1) * 12 + (zeroTime + span).Month - 1;
//decimal month_dec = months / 12m;

//decimal years_dec = ((zeroTime + span).Year - 1) + ((zeroTime + span).Month - 1)/12m;


//decimal roi = (summaryReport.NetIncome / summaryReport.Investment) / month_dec;

////one year by 400/month
//int rentYear = 400 * 12;
//today = new DateTime(2018, 12, 31);
//span = today - purchaseDate;
//years_dec = ((zeroTime + span).Year - 1) + ((zeroTime + span).Month - 1) / 12m;
//roi = (rentYear / summaryReport.Investment) / years_dec;


//rentYear = 400 * 18;
//today = new DateTime(2019, 6, 1);
//span = today - purchaseDate;
//years_dec = ((zeroTime + span).Year - 1) + ((zeroTime + span).Month - 1) / 12m;
//roi = (rentYear / summaryReport.Investment) / years_dec;

//decimal netrent = 500 * 0.85m - 55 - (100+350)/12;
//decimal yearlyCosts = 100 + 350;
//decimal netrent_ = apartment.CurrentRent * 0.85m - apartment.FixedMaintanance - yearlyCosts / 12;
//roi = netrent / summaryReport.Investment;


