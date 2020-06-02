using Microsoft.EntityFrameworkCore;
using Nadlan.BusinessLogic;
using Nadlan.Models;
using Nadlan.ViewModels;
using Nadlan.ViewModels.Reports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Nadlan.Repositories
{
    public class ApartmentReportRepository : Repository<Transaction>
    {

        const decimal ANNUAL_COSTS = 100 + 350;
        public ApartmentReportRepository(NadlanConext context) : base(context)
        {
        }


        private IEnumerable<Transaction> GetInvestment(int apartmetId)
        {
            var basic = PurchaseFilters.GetInvestmentFilter();
            return Context.Transactions
                 .Include(a => a.Account)
                 .Where(basic)
                 .Where(a=>a.ApartmentId ==apartmetId);
        }
        private IEnumerable<Transaction> GetTotalNetIncome(int apartmetId)
        {
            var basic = NonPurchaseFilters.GetProfitRemoveDistributionFilter();
            return Context.Transactions
                 .Include(a => a.Account)
                 .Where(basic)
                 .Where(a => a.ApartmentId == apartmetId);
        }
        private IEnumerable<Transaction> GetTotalCost(int apartmentId)
        {
            var basic = PurchaseFilters.GetTotalCostFilter();
            return Context.Transactions
                  .Include(a => a.Account)
                  .Where(basic)
                  .Where(a => !a.Account.IsIncome);
        }
        private IEnumerable<Transaction> GetAllDistributions(int apartmentId)
        {
            var basic = PurchaseFilters.GetTotalCostFilter();
            return Context.Transactions
                  .Include(a => a.Account)
                  .Where(basic)
                  .Where(a => !a.Account.IsIncome);
        }


        #region SummaryReport
        public async Task<SummaryReport> GetSummaryReport(int apartmentId)
        {

            var investment = GetInvestment(apartmentId);
            var netIncome = GetTotalNetIncome(apartmentId);
            var totalCost = GetTotalCost(apartmentId);
            var distributed = GetAllDistributions(apartmentId);

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
            summaryReport.PredictedROI = CalcPredictedROI(apartmentId, summaryReport.Investment);


            return summaryReport;
        }

        private decimal CalcPredictedROI(int apartmentId, decimal investment)
        {
            var expectedTransactions = Context.ExpectedTransactions.Where(a => a.ApartmentId == apartmentId).ToList();
            if (expectedTransactions.Count < 3)
            {
                return 0;
            }
            var income = expectedTransactions.Where(a => a.AccountId == 1).First();
            decimal anualRent = income.FrequencyPerYear * income.Amount;
            var allExpenses = expectedTransactions.Where(a => a.AccountId != 1);

            decimal annualExpenses = 0;
            foreach (var expense in allExpenses)
            {
                annualExpenses += expense.FrequencyPerYear * expense.Amount;
            }
            decimal annualNetIncome = anualRent - annualExpenses;
            decimal predictedRoi = 0;
            if (investment > 0)
            {
                predictedRoi = annualNetIncome / investment;
            }
            return predictedRoi;
        }

        private decimal CalcROI(Apartment apartment, SummaryReport summaryReport)
        {
            if (apartment.PurchaseDate > DateTime.Now)
            {
                return 0;
            }
            DateTime zeroTime = new DateTime(1, 1, 1);
            TimeSpan span = DateTime.Today - apartment.PurchaseDate;
            decimal years_dec = ((zeroTime + span).Year - 1) + ((zeroTime + span).Month - 1) / 12m;
            decimal roi = 0;
            if (summaryReport.Investment > 0 && years_dec > 0)
            {
                roi = (summaryReport.NetIncome / summaryReport.Investment) / years_dec;
            }
            return roi;
        }
        #endregion

        public async Task<IncomeReport> GetIncomeReport(int apartmentId, int year)
        {
            Func<Transaction, bool> predAll = t =>
               !t.IsDeleted
            && !t.IsPurchaseCost
            && !t.IsBusinessExpense
            && t.ApartmentId == apartmentId
            && t.Account.AccountTypeId == 0
            && t.AccountId != 100;

            Func<Transaction, bool> predWithYear = t =>
               !t.IsDeleted
            && !t.IsPurchaseCost
            && !t.IsBusinessExpense
            && t.ApartmentId == apartmentId
            && t.Account.AccountTypeId == 0
            && t.AccountId != 100
            && t.Date.Year == year;

            Func<Transaction, bool> basicPredicate = year == 0 ? predAll : predWithYear;

            var grossIncome = Context.Transactions.Include(a => a.Account).Where(basicPredicate)
                .Where(a => a.AccountId == 1);


            var expenses = Context.Transactions.Include(a => a.Account).Where(basicPredicate)
                .Where(a => !a.Account.IsIncome && !a.IsBusinessExpense);

            var tax = Context.Transactions.Include(a => a.Account).Where(basicPredicate)
                .Where(a => a.AccountId == 5);

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
                    Total = a.Where(x => !x.IsBusinessExpense).Sum(s => s.Amount)
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

        //public async Task<decimal> GetBalance(int accountId)
        //{
        //    var balance = Context.Transactions
        //        .Where(a => a.AccountId == accountId && !a.IsDeleted)
        //        .SumAsync(a => a.Amount);
        //    return await balance;
        //}

        public async Task<decimal> GetExpensesBalance()
        {
            var balance = Context.Expenses
                .Where(a => !a.Transaction.IsDeleted)
                .SumAsync(a => a.Transaction.Amount);
            return await balance * -1;
        }

        public async Task<PurchaseReport> GetPurchaseReport(int apartmentId)
        {
            var investment = GetInvestment(apartmentId);
            var totalCost = GetTotalCost(apartmentId);
            var renovationCost = GetRenovationCost(apartmentId);
            var expensesNoRenovation = GetExpensesNoRenovation(apartmentId);
            var accountSummary = GetAccountSummary(apartmentId);

            PurchaseReport purchaseReport = new PurchaseReport
            {
                Investment = await Task.FromResult(investment.Sum(a => a.Amount)),
                TotalCost = await Task.FromResult(totalCost.Sum(a => a.Amount)),
                RenovationCost = await Task.FromResult(renovationCost.Sum(a => a.Amount)),
                ExpensesNoRenovation = await Task.FromResult(expensesNoRenovation.Sum(a => a.Amount)),
                AccountsSum = await Task.FromResult(accountSummary.ToList())
            };

            purchaseReport.Remainder = purchaseReport.Investment + purchaseReport.TotalCost;

            purchaseReport.AccountsSum = KeepRenovationAccountsTogether(purchaseReport.AccountsSum);


            return purchaseReport;
        }

        private IEnumerable<Transaction> GetRenovationCost(int apartmetId)
        {
            var basic = PurchaseFilters.GetRenovationFilter();
            return Context.Transactions
                 .Include(a => a.Account)
                 .Where(basic)
                 .Where(a => a.ApartmentId == apartmetId);
        }

        private IEnumerable<Transaction> GetExpensesNoRenovation(int apartmetId)
        {
            var basic = PurchaseFilters.GetCostNotRenovataionFilter();
            return Context.Transactions
                 .Include(a => a.Account)
                 .Where(basic)
                 .Where(a => a.ApartmentId == apartmetId);
        }

        private IEnumerable<AccountSummary> GetAccountSummary(int apartmetId)
        {
            var basic = PurchaseFilters.GetTotalCostFilter();
            return Context.Transactions
                .Include(a => a.Account)
                .Where(basic)
                .Where(a => a.AccountId != 13)
                .GroupBy(g => new { g.AccountId, g.Account.Name, g.Account.DisplayOrder })
                .OrderBy(a => a.Sum(s => s.Amount))
                .Select(a => new AccountSummary
                {
                    AccountId = a.Key.AccountId,
                    Name = a.Key.Name,
                    Total = Math.Abs(a.Sum(s => s.Amount))
                });
        }

        private List<AccountSummary> KeepRenovationAccountsTogether(IEnumerable<AccountSummary> accnoutsList_)
        {
            var accnoutsList = accnoutsList_.ToList();
            var contractorIndex = accnoutsList.FindIndex(a => a.AccountId == 17);
            var miscellaneousIndex = accnoutsList.FindIndex(a => a.AccountId == 6);

            if (contractorIndex == -1 || miscellaneousIndex == -1)
            {
                return accnoutsList;
            }
            var miscellaneousRef = accnoutsList.Find(a => a.AccountId == 6);
            var contractorRef = accnoutsList.Find(a => a.AccountId == 17);
            var renovationContractor = new AccountSummary()
            {
                AccountId = accnoutsList[contractorIndex].AccountId,
                Name = accnoutsList[contractorIndex].Name,
                Total = accnoutsList[contractorIndex].Total
            };

            var renovationMiscellaneous = new AccountSummary()
            {
                AccountId = accnoutsList[miscellaneousIndex].AccountId,
                Name = accnoutsList[miscellaneousIndex].Name,
                Total = accnoutsList[miscellaneousIndex].Total
            };
            int min  = Math.Min(contractorIndex, miscellaneousIndex);
            accnoutsList.Remove(miscellaneousRef);
            accnoutsList.Remove(contractorRef);
            accnoutsList.Insert(min, renovationContractor);
            accnoutsList.Insert(min + 1, renovationMiscellaneous);
            return accnoutsList;
        }



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



    }

}
























//private List<AccountSummary> KeepRenovationAccountsTogether(IEnumerable<AccountSummary> accnoutsList_)
//{
//    var accnoutsList = accnoutsList_.ToList();
//    var renovationMiscellaneousIndex = accnoutsList.FindIndex(a => a.AccountId == 6);
//    var renovationContractorIndex = accnoutsList.FindIndex(a => a.AccountId == 17);
//    //var renovationContractor = accnoutsList.Find(a => a.AccountId == 17);
//    var renovationContractor = accnoutsList[renovationContractorIndex];
//    var temp = accnoutsList[renovationMiscellaneousIndex + 1];

//    //var indexToInsert = accnoutsList.FindIndex(a => a.AccountId == 6);
//    accnoutsList.Insert(indexToInsert + 1, renovationContractor);
//    accnoutsList.RemoveAt(renovationContractor);
//    return accnoutsList;
//}
//int indexToFollow = Math.Min(contractorIndex, miscellaneousIndex);
//var oldItem = accnoutsList[indexToFollow];
//var renovationMiscellaneous = new AccountSummary()
//{
//    AccountId = oldItem.AccountId,
//    Name = oldItem.Name,
//    Total = oldItem.Total
//};
//accnoutsList.Remove(oldItem);
//accnoutsList.Insert(indexToMove, renovationMiscellaneous);


////var renovationMiscellaneousIndex = accnoutsList.FindIndex(a => a.AccountId == 6);
//var indexOrigial = accnoutsList.FindIndex(a => a.AccountId == 17);
//var indexAfterRenovation = accnoutsList.FindIndex(a => a.AccountId == 6) + 1;
//var renovationContractor = accnoutsList.Find(a => a.AccountId == 17);


//////var indexToInsert = accnoutsList.FindIndex(a => a.AccountId == 6);
////accnoutsList.Insert(indexAfterRenovation, renovationContractor);
////accnoutsList.RemoveAt(indexOrigial+1);
//public async Task<SummaryReport> GetSummaryReport(int apartmentId)
//{
//    //Func<Transaction, bool> basicPredicatePurchase = NonPurchaseFilters.GetBasicPurchaseTransactionFilter(apartmentId);
//    //Func<Transaction, bool> basicPredicatePurchase_ = t =>
//    //!t.IsDeleted
//    //&& t.IsPurchaseCost
//    //&& !t.IsBusinessExpense
//    //&& t.ApartmentId == apartmentId
//    //&& t.Account.AccountTypeId == 0;

//    //Func<Transaction, bool> basicPredicateIncome = t =>
//    //!t.IsDeleted
//    //&& !t.IsPurchaseCost
//    //&& !t.IsBusinessExpense
//    //&& t.ApartmentId == apartmentId
//    //&& t.Account.AccountTypeId == 0;

//    var investment = GetInvestment(apartmentId);

//    //var investment = Context.Transactions
//    //    .Include(a => a.Account)
//    //    .Where(basicPredicatePurchase)
//    //    .Where(a => a.AccountId == 13);

//    var netIncome = GetTotalNetIncome(apartmentId);

//    //var netIncome = Context.Transactions
//    //    .Include(a => a.Account)
//    //    .Where(basicPredicateIncome)
//    //    .Where(a => a.AccountId != 100);

//    var totalCost = GetTotalCost(apartmentId);

//    //var totalCost = Context.Transactions
//    //    .Include(a => a.Account)
//    //    .Where(basicPredicatePurchase)
//    //    .Where(a => !a.Account.IsIncome);
//    //var accumulated = Context.Transactions.Include(a => a.Account).Where(basicPredicateIncome);
//    //var distributed = Context.Transactions
//    //    .Include(a => a.Account)
//    //    .Where(a => !a.IsDeleted && a.ApartmentId == apartmentId && a.AccountId == 100);
//    var distributed = GetAllDistributions(apartmentId);

//    SummaryReport summaryReport = new SummaryReport
//    {
//        Investment = await Task.FromResult(investment.Sum(a => a.Amount)),
//        NetIncome = await Task.FromResult(netIncome.Sum(a => a.Amount)),
//        Distributed = await Task.FromResult(distributed.Sum(a => a.Amount)),
//    };

//    summaryReport.InitialRemainder = summaryReport.Investment + await Task.FromResult(totalCost.Sum(a => a.Amount));
//    summaryReport.Balance = summaryReport.InitialRemainder + summaryReport.NetIncome + summaryReport.Distributed;
//    //summaryReport.Balance = summaryReport.InitialRemainder + await Task.FromResult(accumulated.Sum(a => a.Amount));
//    //summaryReport.Balance = summaryReport.InitialRemainder + summaryReport.NetIncome;

//    Apartment apartment = Context.Apartments.Where(a => a.Id == apartmentId).First();

//    summaryReport.ROI = CalcROI(apartment, summaryReport);
//    summaryReport.PredictedROI = CalcPredictedROI(apartmentId, summaryReport.Investment);


//    return summaryReport;
//}

//private IEnumerable<Transaction> GetInvestment(Func<Transaction, bool> basicPredicate)
//{
//    return Context.Transactions
//         .Include(a => a.Account)
//         .Where(basicPredicate)
//         .Where(a => a.AccountId == 13);
//}