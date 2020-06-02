using Microsoft.EntityFrameworkCore;
using Nadlan.BusinessLogic;
using Nadlan.Models;
using Nadlan.ViewModels.Reports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Nadlan.Repositories.ApartmentReports
{
    public class SummaryReportRepository : ApartmentReportRepositoryNew
    {
        public SummaryReportRepository(NadlanConext conext):base( conext)
        {

        }

        public async Task<SummaryReport> GetSummaryReport(int apartmentId)
        {

            var investment = GetInvestment(apartmentId);
            var netIncome = GetNetIncome(apartmentId,0);//all years
            var totalCost = GetTotalCost(apartmentId);
            var distributed = GetAllDistributions(apartmentId);

            SummaryReport summaryReport = new SummaryReport
            {
                Investment = await Task.FromResult(investment.Sum(a => a.Amount)),
                NetIncome = await Task.FromResult(netIncome.Sum(a => a.Amount)),
                Distributed = await Task.FromResult(distributed.Sum(a => a.Amount)),
            };
            //summaryReport.InitialRemainder = summaryReport.Investment + await Task.FromResult(totalCost.Sum(a => a.Amount));
            summaryReport.Balance =  summaryReport.NetIncome + summaryReport.Distributed;
            //summaryReport.Balance = summaryReport.InitialRemainder + await Task.FromResult(accumulated.Sum(a => a.Amount));
            //summaryReport.Balance = summaryReport.InitialRemainder + summaryReport.NetIncome;

            Apartment apartment = Context.Apartments.Where(a => a.Id == apartmentId).First();

            summaryReport.ROI = CalcROI(apartment, summaryReport);
            summaryReport.PredictedROI = CalcPredictedROI(apartmentId, summaryReport.Investment);


            return summaryReport;
        }

        private IEnumerable<Transaction> GetAllDistributions(int apartmentId)
        {
            var basic = NonPurchaseFilters.GetAllDistributionsFilter();
            return Context.Transactions
                  .Include(a => a.Account)
                  .Where(basic)
                  .Where(a => a.ApartmentId==apartmentId);
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


    }
}
