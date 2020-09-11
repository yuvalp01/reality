using Nadlan.BusinessLogic;
using Nadlan.Models;
using Nadlan.ViewModels.Reports;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Nadlan.Repositories.ApartmentReports
{
    public class SummaryReportRepository : ApartmentReportRepository
    {
        private PurchaseFilters purchaseFilters = new PurchaseFilters();
        private NonPurchaseFilters nonPurchaseFilters = new NonPurchaseFilters();

        public SummaryReportRepository(NadlanConext conext) : base(conext)
        {

        }

        public async Task<SummaryReport> GetSummaryReport(int apartmentId)
        {
            var allNonPurchase = GetAllNonPurchase(apartmentId);
            var allPurchase = GetAllPurchase(apartmentId);

            var investment = GetInvestment(allPurchase);
            // decimal netIncome = GetNetIncome(allNonPurchase, 0).Sum(a => a.Amount);//all years
            var distributed = GetAllDistributions(allNonPurchase);
            // decimal bonus = GetBonus(netIncome,apartmentId);
            //var totalCost = GetTotalCost(apartmentId);

            SummaryReport summaryReport = new SummaryReport
            {
                Investment = await Task.FromResult(investment.Sum(a => a.Amount)),
                NetIncome = GetNetIncome(allNonPurchase, 0).Sum(a => a.Amount),//all years
                Distributed = await Task.FromResult(distributed.Sum(a => a.Amount)),
                // Bonus = -bonus,
            };
            Apartment apartment = Context.Apartments.Where(a => a.Id == apartmentId).First();
            summaryReport.ROI = CalcROI(apartment, summaryReport);
            summaryReport.RoiForInvestor = summaryReport.ROI;
            //Leipzig - no bonus
            if (apartment.Id != 20)
            {
                const decimal threshold = (decimal)0.03;
                //less than threshold - no bonus
                if (summaryReport.ROI > threshold)
                {
                    decimal bonusROI = (summaryReport.ROI - threshold)/2;
                    summaryReport.RoiForInvestor = threshold+ bonusROI;
                    summaryReport.Bonus = bonusROI * summaryReport.Investment;
                }
            }

            summaryReport.PredictedROI = CalcPredictedROI(apartmentId, summaryReport.Investment);
            summaryReport.NetForInvestor = summaryReport.NetIncome - summaryReport.Bonus;
            summaryReport.Balance = summaryReport.NetIncome - summaryReport.Bonus + summaryReport.Distributed;

            return summaryReport;
        }

        private IEnumerable<Transaction> GetAllDistributions(IEnumerable<Transaction> transactions)
        {
            var basic = nonPurchaseFilters.GetAllDistributionsFilter();
            return transactions.Where(basic);
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
