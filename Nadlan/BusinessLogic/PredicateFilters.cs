using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Nadlan.Models;

namespace Nadlan.BusinessLogic
{
    public class PredicateFilters
    {

        public static Func<Transaction, bool> GetProfitAfterDistributionFilter()
        {
            Func<Transaction, bool> basicPredicate = t =>
                        !t.IsDeleted &&
                        !t.IsPurchaseCost &&
                        !t.IsBusinessExpense &&
                        t.Account.AccountTypeId == 0 &&
                        t.AccountId != 198; //Not a deposit (this line is a not needed as we have the AccountTypeId condition)

            return basicPredicate;
        }

        //Still not in use
        public static Func<Transaction, bool> GetProfitBeforeDistributionFilter()
        {
            Func<Transaction, bool> basic = GetProfitAfterDistributionFilter();
            int[] ExcludedAccounts = { 100 };//Distribution and deposit account account.

            Func<Transaction, bool> basicProfitPredicate = t =>
                        basic(t) &&
                        !ExcludedAccounts.Contains(t.AccountId);
            return basicProfitPredicate;
        }

        public static Func<Transaction, bool> GetBasicDistributionFilter()
        {
            Func<Transaction, bool> basic = GetProfitAfterDistributionFilter();
            Func<Transaction, bool> distributionPredicate = t => 
                        basic(t) &&
                        t.AccountId==100;//Distribution account
            return distributionPredicate;
        }

    }
}
