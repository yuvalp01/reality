using Microsoft.EntityFrameworkCore;
using Nadlan.BusinessLogic;
using Nadlan.Models;
using Nadlan.Models.Enums;
using Nadlan.ViewModels.Reports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nadlan.Repositories.ApartmentReports
{
    public class IncomeReportRepository : ApartmentReportRepository
    {
        private NonPurchaseFilters nonPurchaseFilters = new NonPurchaseFilters();
        private PurchaseFilters purchaseFilters = new PurchaseFilters();

        public IncomeReportRepository(NadlanConext conext) : base(conext)
        {
        }

            public async Task<IncomeReport> GetIncomeReport(int apartmentId, int year, DateTime currentDate)
        {

            IncomeReport incomeReport = new IncomeReport();
            incomeReport.AccountsSum = GetAccountSummaryNonPurchase(apartmentId, year);
            incomeReport.GrossIncome = GetAccountSum(apartmentId, 1, year);
            incomeReport.NetIncome = GetNetIncome(apartmentId, year);
            incomeReport.Expenses = GetAllExpenses(apartmentId, year);
            decimal investment = GetAccountSum(apartmentId, 13);
            DateTime purchaseDate = Context.Apartments.Where(a => a.Id == apartmentId).First().PurchaseDate;


            if (apartmentId != 20)
            {
                if (year == 0)
                {
                    incomeReport.Bonus = -1* await Task.FromResult(CalcBonus(investment, incomeReport.NetIncome, purchaseDate, currentDate));
                    incomeReport.NetForInvestor = incomeReport.NetIncome - incomeReport.Bonus;
                }
                //Do not calculate bonus so far if it's for specific year
                else
                {
                    decimal bonusPaid = GetAccountSum(apartmentId, 300, year);
                    incomeReport.NetForInvestor = incomeReport.NetIncome + bonusPaid;
                    incomeReport.Bonus = bonusPaid;
                }
            }



            return incomeReport;
        }





        protected decimal GetAllExpenses(int apartmentId, int year)
        {
            Func<Transaction, bool> basic = GetAllValidTransactionsForReports(apartmentId);
            Func<Transaction, bool> expensesFilter = t =>
                        basic(t) &&
                        t.IsPurchaseCost == false &&
                        t.AccountId != (int)Accounts.Rent &&//Except for rent
                        t.AccountId != (int)Accounts.Distribution &&//Except for distribution
                        t.AccountId != (int)Accounts.Bonus &&//Except for bonus

                        t.AccountId != (int)Accounts.SecurityDeposit &&//Except for Security Deposit
                        t.AccountId != (int)Accounts.Business &&//Except for Business
                        t.AccountId != (int)Accounts.Balance;//Except for Balance
                       // t.Account.AccountTypeId == 0;

            Func<Transaction, bool> filter;
            if (year != 0)
            {
                filter = b =>
                         expensesFilter(b) &&
                         b.Date.Year == year;
            }
            else
            {
                filter = expensesFilter;
            }

            var result = Context.Transactions
                //.Include(a => a.Account)
                .Where(filter)
                .Sum(a => a.Amount);
            // if (year != 0) return result.Where(a => a.Date.Year == year);
            return result;
        }




        protected List<AccountSummary> GetAccountSummaryNonPurchase(int apartmentId, int year)
        {
            var basic = GetRegularTransactionsFilter(apartmentId, year, false);
            var accountSummary = Context.Transactions
                .Include(a => a.Account)
                .Where(basic)
                .Where(a => a.AccountId != (int)Accounts.Rent)
                .GroupBy(g => new { g.AccountId, g.Account.Name })
                .OrderBy(a => a.Sum(s => s.Amount))
                .Select(a => new AccountSummary
                {
                    AccountId = a.Key.AccountId,
                    Name = a.Key.Name,
                    Total = a.Sum(s => s.Amount)
                });
            return accountSummary.ToList();
        }

    }
}

