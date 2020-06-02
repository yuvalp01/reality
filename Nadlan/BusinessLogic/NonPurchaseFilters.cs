using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Nadlan.Models;

namespace Nadlan.BusinessLogic
{
    public class NonPurchaseFilters
    {

        public static Func<Transaction, bool> GetProfitIncludingDistributionsFilter()
        {
            Func<Transaction, bool> basicPredicate = t =>
                        !t.IsDeleted &&
                        !t.IsPurchaseCost &&
                        !t.IsBusinessExpense &&
                        t.Account.AccountTypeId == 0 &&
                        t.AccountId != 198; //Not a deposit (this line is a not needed as we have the AccountTypeId condition)
            return basicPredicate;
        }

        public static Func<Transaction, bool> GetProfitRemoveDistributionFilter()
        {
            Func<Transaction, bool> basic = GetProfitIncludingDistributionsFilter();
            //int[] ExcludedAccounts = { 100,198 };//Distribution and deposit account account.

            Func<Transaction, bool> basicProfitPredicate = t =>
                        basic(t) &&
                        t.AccountId != 100;
            return basicProfitPredicate;
        }

        public static Func<Transaction, bool> GetAllDistributionsFilter()
        {
            Func<Transaction, bool> basic = GetProfitIncludingDistributionsFilter();
            Func<Transaction, bool> distributionPredicate = t => 
                        basic(t) &&
                        t.AccountId==100;//Distribution account
            return distributionPredicate;
        }

        public static Func<Transaction, bool> GetGrossIncomeFilter()
        {
            Func<Transaction, bool> basic = GetProfitRemoveDistributionFilter();
            Func<Transaction, bool> distributionPredicate = t =>
                        basic(t) &&
                        t.AccountId == 1;//Rent
            return distributionPredicate;
        }
        public static Func<Transaction, bool> GetAllExpensesFilter()
        {
            Func<Transaction, bool> basic = GetProfitRemoveDistributionFilter();
            Func<Transaction, bool> distributionPredicate = t =>
                        basic(t) &&
                        t.AccountId != 1;//Except for rent
            return distributionPredicate;
        }
    }
}
