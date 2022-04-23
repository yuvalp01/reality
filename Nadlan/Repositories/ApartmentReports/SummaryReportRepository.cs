using Nadlan.BusinessLogic;
using Nadlan.Models;
using Nadlan.Models.Enums;
using Nadlan.ViewModels.Reports;
using System;
using System.Linq;
using System.Threading.Tasks;


namespace Nadlan.Repositories.ApartmentReports
{
    public class SummaryReportRepository : ApartmentReportRepository
    {

        private SpecialFilters specialFilters = new SpecialFilters();

        public SummaryReportRepository(NadlanConext conext) : base(conext)
        {

        }

        public async Task<SoFarReport> GetSoFarReport(int apartmentId, DateTime currentDate)
        {
            SoFarReport soFarReport = new SoFarReport(apartmentId, currentDate);

            soFarReport.Investment = GetAccountSum(apartmentId, (int)Accounts.Investment);
            soFarReport.Distributed = GetAccountSum(apartmentId, (int)Accounts.Distribution, currentDate);
            soFarReport.BonusPaid = GetAccountSum(apartmentId, (int)Accounts.Bonus, currentDate);
            soFarReport.GrossIncome = GetAccountSum(apartmentId, (int)Accounts.Rent, currentDate);
            soFarReport.NetIncome = GetNetIncomeNew(apartmentId, currentDate, true);
            soFarReport.PendingExpenses = GetPendingExpenses(apartmentId, currentDate);

            Apartment apartment = Context.Apartments.Where(a => a.Id == apartmentId).First();
            soFarReport.RoiAccumulated = CalcAccumulatedRoi(apartment.PurchaseDate, currentDate, soFarReport.Investment, soFarReport.NetIncome);
            soFarReport.Years = GetAgeInYears(apartment.PurchaseDate, currentDate);
            //Make sure it's not a future purchase
            if (soFarReport.Years > 0)
            {
                soFarReport.ROI = soFarReport.RoiAccumulated / soFarReport.Years;
            }
            soFarReport.RoiForInvestor = soFarReport.ROI;

            //Leipzig - no bonus
            if (apartment.Id != 20)
            {
                const double THRESHOLD = 0.03;
                const decimal PERCENTAGE = (decimal)0.5;
                //Use compound interest
                soFarReport.ThresholdAccumulated = CalcAccumulatedThreshold(THRESHOLD, soFarReport.Years);

                //Bonus - only when roi is more than threshold         
                if (soFarReport.RoiAccumulated > soFarReport.ThresholdAccumulated)
                {
                    soFarReport.BonusPercentage = (soFarReport.RoiAccumulated - soFarReport.ThresholdAccumulated) * PERCENTAGE;
                    soFarReport.RoiForInvestor = (soFarReport.RoiAccumulated - soFarReport.BonusPercentage) / soFarReport.Years;
                    soFarReport.Bonus = soFarReport.BonusPercentage * soFarReport.Investment;
                    soFarReport.PendingBonus = soFarReport.Bonus + soFarReport.BonusPaid;
                }
            }

            soFarReport.PredictedROI = CalcPredictedROI(apartmentId, soFarReport.Investment);
            soFarReport.NetForInvestor = soFarReport.NetIncome - soFarReport.Bonus;
            soFarReport.PendingDistribution = soFarReport.NetForInvestor + soFarReport.Distributed;

            return soFarReport;
        }


        public async Task<SummaryReport> GetSummaryReport(int apartmentId, DateTime currentDate)
        {
            SummaryReport summaryReport = new SummaryReport();

            summaryReport.Investment = GetAccountSum(apartmentId, (int)Accounts.Investment);
            summaryReport.Distributed = GetAccountSum(apartmentId, (int)Accounts.Distribution);
            summaryReport.NetIncome = await Task.FromResult(GetNetIncome(apartmentId, 0));

            Apartment apartment = Context.Apartments.Where(a => a.Id == apartmentId).First();
            summaryReport.RoiAccumulated = CalcAccumulatedRoi(apartment.PurchaseDate, currentDate, summaryReport.Investment, summaryReport.NetIncome);
            summaryReport.Years = GetAgeInYears(apartment.PurchaseDate, currentDate);
            //Make sure it's not a future purchase
            if (summaryReport.Years > 0)
            {
                summaryReport.ROI = summaryReport.RoiAccumulated / summaryReport.Years;
            }
            summaryReport.RoiForInvestor = summaryReport.ROI;

            //Leipzig - no bonus
            if (apartment.Id != 20)
            {
                const double THRESHOLD = 0.03;
                //Use compound interest
                summaryReport.ThresholdAccumulated = CalcAccumulatedThreshold(THRESHOLD, summaryReport.Years);

                //less than threshold - no bonus         
                if (summaryReport.RoiAccumulated > summaryReport.ThresholdAccumulated)
                {
                    summaryReport.BonusPercentage = (summaryReport.RoiAccumulated - summaryReport.ThresholdAccumulated) / 2;
                    summaryReport.RoiForInvestor = (summaryReport.RoiAccumulated - summaryReport.BonusPercentage) / summaryReport.Years;
                    summaryReport.BonusSoFar = summaryReport.BonusPercentage * summaryReport.Investment;
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
            var income = expectedTransactions.Where(a => a.AccountId == (int)Accounts.Rent).First();
            decimal anualRent = income.FrequencyPerYear * income.Amount;
            var allExpenses = expectedTransactions.Where(a => a.AccountId != (int)Accounts.Rent);

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



        private decimal GetPendingExpenses(int apartmentId, DateTime currentDate)
        {

            Func<Transaction, bool> expensesFilter = t =>
           !t.IsDeleted &&
            t.PersonalTransactionId == 0;//Not covered yet
            var expenses = Context.Transactions
                .Where(a => a.IsDeleted == false)
                .Where(a => a.PersonalTransactionId == 0)
                .Where(a => a.ApartmentId == apartmentId)
                .Where(a => a.Date <= currentDate)
                .Sum(a => a.Amount);
            return expenses;
        }
    }

}

