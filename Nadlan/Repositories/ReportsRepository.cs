using Microsoft.EntityFrameworkCore;
using Nadlan.Models;
using Nadlan.ViewModels;
using Nadlan.ViewModels.Reports;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Nadlan.Repositories
{
    public class ReportRepository : Repository<Transaction>
    {

        const decimal ANNUAL_COSTS = 100 + 350;
        public ReportRepository(NadlanConext context) : base(context)
        {
        }


        public async Task<ApartmentDto> GetApartmentInfo(int apartmentId)
        {
            ExpectedTransaction expectedLastRent = await Context.ExpectedTransactions.OrderByDescending(a => a.Id)
                .FirstOrDefaultAsync(a => a.ApartmentId == apartmentId
                && a.AccountId == 1);
            decimal lastRent = 0;
            if (expectedLastRent != null)
            {
                lastRent = expectedLastRent.Amount;
            }
            var apartment = await Context.Apartments.FirstAsync(a => a.Id == apartmentId);

            ApartmentStatus apartmentStatus = (ApartmentStatus)apartment.Status;
            //string status = string.Empty;
            ApartmentDto apartmentDto = new ApartmentDto
            {
                Id = apartment.Id,
                Address = apartment.Address,
                Floor = apartment.Floor,
                Door = apartment.Door,
                Size = apartment.Size,
                PurchaseDate = apartment.PurchaseDate,
                CurrentRent = lastRent,
                Status = apartmentStatus.ToString()
            };
            return apartmentDto;
        }

        public async Task<decimal> GetBalance(int accountId)
        {
            var balance = Context.Transactions.Where(a => a.AccountId == accountId && !a.IsDeleted).SumAsync(a => a.Amount);
            return await balance;
        }

        public async Task<decimal> GetExpensesBalance()
        {
            var balance = Context.Expenses.SumAsync(a => a.Transaction.Amount);
            return await balance*-1;
        }

        //public async Task<decimal> GetPersonalBalance(int stakeholderId)
        //{
        //    var balance = Context.PersonalTransactions.Where(a => a.StakeholderId == stakeholderId).SumAsync(a => a.Amount);
        //    return await balance;
        //}


        public async Task<DiagnosticReport> GetDiagnosticReport(DiagnosticRequest diagnosticRequest)
        {
            DiagnosticReport diagnosticReport = new DiagnosticReport()
            {
                Accountency = 100,
                Research = 2 * 800,
                Registration = diagnosticRequest.Size * 6 + 35,
                Agency = diagnosticRequest.Price * 0.01m < 1000 ? 1400 : diagnosticRequest.Price * 0.01m + 400,
                Legal = diagnosticRequest.Price * 0.04m,
                PurchaseTax = diagnosticRequest.Price * 0.031m,
                Supervision = diagnosticRequest.Renovation * 0.005m + 200,
                Unpredicted = diagnosticRequest.Price * 0.01m,
                UnpredictedRenovation = diagnosticRequest.Renovation * 0.1m,
            };
            diagnosticReport.TotalCost = diagnosticRequest.Price +
               diagnosticReport.Accountency +
               diagnosticReport.Research +
               diagnosticReport.Registration +
               diagnosticReport.Agency +
               diagnosticReport.Legal +
               diagnosticReport.PurchaseTax +
               diagnosticReport.Supervision +
               diagnosticReport.Unpredicted +
               diagnosticReport.UnpredictedRenovation;

            decimal netRent = diagnosticRequest.PredictedRent * 0.85m - 40 - ANNUAL_COSTS / 12;
            diagnosticReport.ROI = netRent * 11 / diagnosticReport.TotalCost;
            return diagnosticReport;
        }



        public async Task<SummaryReport> GetSummaryReport(int apartmentId)
        {

            Func<Transaction, bool> basicPredicatePurchase = t =>
            !t.IsDeleted
            && t.IsPurchaseCost
            && t.ApartmentId == apartmentId
            && t.Account.AccountTypeId == 0;

            Func<Transaction, bool> basicPredicateIncome = t =>
            !t.IsDeleted
            && !t.IsPurchaseCost
            && t.ApartmentId == apartmentId
            && t.Account.AccountTypeId == 0;

            var investment = Context.Transactions.Include(a => a.Account).Where(basicPredicatePurchase).Where(a => a.AccountId == 13);
            var netIncome = Context.Transactions.Include(a => a.Account).Where(basicPredicateIncome).Where(a => a.AccountId != 100);
            var totalCost = Context.Transactions.Include(a => a.Account).Where(basicPredicatePurchase).Where(a => !a.Account.IsIncome);//.Where(a => a.Amount <= 0);
            var accumulated = Context.Transactions.Include(a => a.Account).Where(basicPredicateIncome);
            var distributed = Context.Transactions.Include(a => a.Account).Where(a => a.ApartmentId == apartmentId && a.AccountId == 100);
            SummaryReport summaryReport = new SummaryReport
            {
                Investment = await Task.FromResult(investment.Sum(a => a.Amount)),
                NetIncome = await Task.FromResult(netIncome.Sum(a => a.Amount)),
                Distributed = await Task.FromResult(distributed.Sum(a => a.Amount)),
            };

            summaryReport.InitialRemainder = summaryReport.Investment + await Task.FromResult(totalCost.Sum(a => a.Amount));
            summaryReport.Balance = summaryReport.InitialRemainder + summaryReport.NetIncome + summaryReport.Distributed;
            //summaryReport.Balance = summaryReport.InitialRemainder + await Task.FromResult(accumulated.Sum(a => a.Amount));
            //summaryReport.Balance = summaryReport.InitialRemainder + summaryReport.NetIncome;

            Apartment apartment = Context.Apartments.Where(a => a.Id == apartmentId).First();

            summaryReport.ROI = CalcROI(apartment, summaryReport);
            summaryReport.PredictedROI = CalcPredictedROI(apartment, summaryReport.Investment);


            return summaryReport;
        }


        private decimal CalcPredictedROI(Apartment apartment, decimal investment)
        {
            // decimal yearlyCosts = 100 + 350;
            //TODO - find formula for enfia
            decimal netRent = apartment.CurrentRent * 0.85m - apartment.FixedMaintanance - ANNUAL_COSTS / 12;
            decimal anualNetIncome = netRent * 11;
            decimal predictedRoi = 0;
            if (investment > 0)
            {
                predictedRoi = anualNetIncome / investment;
            }
            return predictedRoi;
        }

        private decimal CalcROI(Apartment apartment, SummaryReport summaryReport)
        {

            DateTime zeroTime = new DateTime(1, 1, 1);
            TimeSpan span = DateTime.Today - apartment.PurchaseDate;
            decimal years_dec = ((zeroTime + span).Year - 1) + ((zeroTime + span).Month - 1) / 12m;
            decimal roi = 0;
            if (summaryReport.Investment > 0 && years_dec>0)
            {
                roi = (summaryReport.NetIncome / summaryReport.Investment) / years_dec;
            }
            return roi;
        }


        public async Task<PurchaseReport> GetPurchaseReport(int apartmentId)
        {
            Func<Transaction, bool> basicPredicate = t => t.IsPurchaseCost && t.ApartmentId == apartmentId && t.Account.AccountTypeId == 0;
            var investment = Context.Transactions.Include(a => a.Account).Where(basicPredicate).Where(a => a.AccountId == 13);

            var totalCost = Context.Transactions.Include(a => a.Account).Where(basicPredicate)
                .Where(a => !a.Account.IsIncome);
            //.Where(a => a.Amount <= 0);
            var renovationCost = Context.Transactions.Include(a => a.Account).Where(basicPredicate)
                .Where(a => !a.IsDeleted && a.AccountId == 6);
            var expensesNoRenovation = Context.Transactions.Include(a => a.Account).Where(basicPredicate)
                .Where(a => !a.IsDeleted && !a.Account.IsIncome && a.AccountId != 6 && a.AccountId != 12);
            // .Where(a => a.Amount <= 0 && a.AccountId != 6 && a.AccountId != 12);
            var accountSummary = Context.Transactions.Include(a => a.Account)
                .Where(basicPredicate)
                .Where(a => a.AccountId != 13).GroupBy(g => new { g.AccountId, g.Account.Name })
                .OrderBy(a => a.Sum(s => s.Amount))
                .Select(a => new AccountSummary
                {
                    AccountId = a.Key.AccountId,
                    Name = a.Key.Name,
                    Total = Math.Abs(a.Sum(s => s.Amount))
                });


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

        public async Task<IncomeReport> GetIncomeReport(int apartmentId, int year)
        {
            Func<Transaction, bool> predAll = t =>
               !t.IsDeleted 
            && !t.IsPurchaseCost
            && t.ApartmentId == apartmentId
            && t.Account.AccountTypeId == 0
            && t.AccountId != 100;

            Func<Transaction, bool> predWithYear = t =>
               !t.IsDeleted
            && !t.IsPurchaseCost
            && t.ApartmentId == apartmentId
            && t.Account.AccountTypeId == 0
            && t.AccountId != 100
            && t.Date.Year == year;

            Func<Transaction, bool> basicPredicate = year == 0 ? predAll : predWithYear;

            var grossIncome = Context.Transactions.Include(a => a.Account).Where(basicPredicate)
                .Where(a => a.AccountId == 1);


            var expenses = Context.Transactions.Include(a => a.Account).Where(basicPredicate)
                .Where(a => !a.Account.IsIncome);

            //&& a.AccountId != 5);
            //&& a.Amount <= 0);

            var tax = Context.Transactions.Include(a => a.Account).Where(basicPredicate)
                .Where(a => a.AccountId == 5);
            //&& a.Amount <= 0);

            //Net income (distribution is not an expence)
            var netIncome = Context.Transactions.Include(a => a.Account).Where(basicPredicate).Where(a => a.AccountId != 100);

            var accountSummary = Context.Transactions.Include(a => a.Account)
                .Where(basicPredicate)
                .Where(a => a.AccountId != 1 && a.AccountId != 100)
                .GroupBy(g => new { g.AccountId, g.Account.Name })
                .OrderBy(a => a.Sum(s => s.Amount))
                .Select(a => new AccountSummary
                {
                    AccountId = a.Key.AccountId,
                    Name = a.Key.Name,
                    Total = a.Sum(s => s.Amount)
                });


            IncomeReport summaryReport = new IncomeReport
            {
                GrossIncome = await Task.FromResult(grossIncome.Sum(b => b.Amount)),
                Expenses = await Task.FromResult(expenses.Sum(b => b.Amount)),
                Tax = await Task.FromResult(tax.Sum(b => b.Amount)),
                NetIncome = await Task.FromResult(netIncome.Sum(b => b.Amount)),
                //ForDistribution = await Task.FromResult(netIncome.Sum(b => b.Amount)) / 2,
                AccountsSum = await Task.FromResult(accountSummary.ToList())

            };
            return summaryReport;
        }



    }
}
