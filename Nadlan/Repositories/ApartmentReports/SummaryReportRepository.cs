using Nadlan.BusinessLogic;
using Nadlan.Models;
using Nadlan.ViewModels.Reports;
using System;
using System.Linq;
using System.Threading.Tasks;


namespace Nadlan.Repositories.ApartmentReports
{
    public class SummaryReportRepository : ApartmentReportRepository
    {
        //private PurchaseFilters purchaseFilters = new PurchaseFilters();
        //private NonPurchaseFilters nonPurchaseFilters = new NonPurchaseFilters();
        private SpecialFilters specialFilters = new SpecialFilters();

        public SummaryReportRepository(NadlanConext conext) : base(conext)
        {

        }





        public async Task<SummaryReport> GetSummaryReport(int apartmentId)
        {

            var predicate = GetAllValidTransactionsForReports(apartmentId);

            SummaryReport summaryReport = new SummaryReport();

            summaryReport.Investment = GetAccountSum(apartmentId, 13);// Context.Transactions.Where(predicate).Where(a => a.AccountId == 13).Sum(a => a.Amount);
            summaryReport.Distributed = GetAccountSum(apartmentId, 100);
            summaryReport.BonusPaid = GetAccountSum(apartmentId, 300);
            summaryReport.NetIncome = await Task.FromResult(GetNetIncome(apartmentId, 0));

            Apartment apartment = Context.Apartments.Where(a => a.Id == apartmentId).First();
            summaryReport.RoiAccumulated = CalcAccumulatedRoi(apartment.PurchaseDate, DateTime.Now,  summaryReport.Investment, summaryReport.NetIncome);
            summaryReport.Years = GetAgeInYears(apartment.PurchaseDate);
            //Make sure it's not a future purchase
            if (summaryReport.Years > 0)
            {
                summaryReport.ROI = summaryReport.RoiAccumulated / summaryReport.Years;
            }
            //summaryReport.ROI = CalcROI(apartment, summaryReport);
            summaryReport.RoiForInvestor = summaryReport.ROI;



            //Leipzig - no bonus
            if (apartment.Id != 20)
            {
                //summaryReport.BonusPaid = await Task.FromResult(bonusPaidLine.Sum(a => a.Amount));
                const double THRESHOLD = 0.03;
                //Use compound interest
                summaryReport.ThresholdAccumulated = CalcAccumulatedThreshold(THRESHOLD, summaryReport.Years);


                //if (summaryReport.Years > 1)
                //{
                //    summaryReport.ThresholdAccumulated = (decimal)Math.Pow((1 + THRESHOLD), (double)summaryReport.Years) - 1;
                //}
                //else
                //{
                //    summaryReport.ThresholdAccumulated = (decimal)THRESHOLD * summaryReport.Years;
                //}
                //less than threshold - no bonus         
                if (summaryReport.RoiAccumulated > summaryReport.ThresholdAccumulated)
                {
                    // decimal bonusROI_ = (summaryReport.ROI - summaryReport.ThresholdAccumulated) / 2;
                    summaryReport.BonusPercentage = (summaryReport.RoiAccumulated - summaryReport.ThresholdAccumulated) / 2;

                    //summaryReport.RoiForInvestor = (summaryReport.ThresholdAccumulated + bonusROI)/summaryReport.Years;
                    summaryReport.RoiForInvestor = (summaryReport.RoiAccumulated - summaryReport.BonusPercentage) / summaryReport.Years;
                    summaryReport.BonusSoFar = summaryReport.BonusPercentage * summaryReport.Investment;
                    summaryReport.BonusExpected = -1 * (summaryReport.BonusSoFar + summaryReport.BonusPaid);
                }

            }

            summaryReport.PredictedROI = CalcPredictedROI(apartmentId, summaryReport.Investment);
            summaryReport.NetForInvestor = summaryReport.NetIncome - summaryReport.BonusSoFar;
            summaryReport.Balance = summaryReport.NetForInvestor + summaryReport.Distributed;

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




    }
}




////if (year != 0) basic = t => basic(t) && t.Date.Year == year;
//var result = Context.Transactions
//    .Where(predicate)
//    .Where(a => a.AccountId != 100)//remove distribution
//    .Where(a => a.AccountId != 300)//remove bonus
//    .Sum(a => a.Amount);




//summaryReport.NetIncome = await Task.FromResult(Context.Transactions
//.Include(a => a.Account)
////.Where(predicate)
//.Where(a => a.IsDeleted == false)
//.Where(a => a.IsBusinessExpense == false)
//.Where(a => a.AccountId != 100)//remove distribution
//.Where(a => a.AccountId != 300)//remove bonus
//.Where(a => a.ApartmentId == apartmentId)
//.Where(a => a.Account.AccountTypeId == 0)
//.Sum(a => a.Amount));

//private IEnumerable<Transaction> GetAllDistributions(IEnumerable<Transaction> transactions)
//{
//    var basic = nonPurchaseFilters.GetAllDistributionsFilter();
//    return transactions.Where(basic);
//}


//t.IsDeleted == false &&
//t.IsBusinessExpense == false &&
//t.ApartmentId == apartmetId;

//    // NetIncome = Context.Transactions.Where(predicate).Where(a => a.AccountId == 100).Sum(a => a.Amount),//all years

//    //Investment = await Task.FromResult(investment.Sum(a => a.Amount)),
//    //Distributed = await Task.FromResult(distributed.Sum(a => a.Amount)),

//};

//var investment = allTransactions.Where(a => a.AccountId == 13);
//var distributed = allTransactions.Where(a => a.AccountId == 100);
//var bonusPaidLine = allTransactions.Where(a => a.AccountId == 300);

// var allNonPurchase = GetAllNonPurchase(apartmentId);
//var allPurchase = GetAllPurchase(apartmentId);

//var investment = GetInvestment(allPurchase);
//var distributed = GetAllDistributions(allNonPurchase);
//// decimal bonus = GetBonus(netIncome,apartmentId);
////var totalCost = GetTotalCost(apartmentId);
//summaryReport.Distributed = await Task.FromResult(Context.Transactions
//    .Where(predicate)
//    .Where(a => a.AccountId == 100)
//    .Sum(a => a.Amount));//all years
//summaryReport.BonusPaid = await Task.FromResult(Context.Transactions
//    .Where(predicate)
//    .Where(a => a.AccountId == 300)
//    .Sum(a => a.Amount));

//var allTransactions = transactionRepository.GetAllValidTransactionsForReports(apartmentId);


//var investment = Context.Transactions.Where(predicate).Where(a => a.AccountId == 13).Sum(a => a.Amount);
//var distributed = Context.Transactions.Where(predicate).Where(a => a.AccountId == 100).Sum(a => a.Amount);
//var bonuspaidline = Context.Transactions.Where(predicate).Where(a => a.AccountId == 300).Sum(a => a.Amount);

//var basic = nonPurchaseFilters.GetProfitRemoveDistributionFilter();
//var basic = nonPurchaseFilters.GetProfitIncludingDistributionsFilter();