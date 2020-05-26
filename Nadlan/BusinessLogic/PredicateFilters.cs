using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Nadlan.Models;

namespace Nadlan.BusinessLogic
{
    public class PredicateFilters
    {

        public static Func<Transaction, bool> GetBasicProfitFilter()
        {
            Func<Transaction, bool> basicPredicate = t =>
                        !t.IsDeleted &&
                        !t.IsPurchaseCost &&
                        !t.IsBusinessExpense &&
                        t.Account.AccountTypeId == 0;
            return basicPredicate;
        }

        public static Func<Transaction, bool> GetProfitFilter()
        {
            Func<Transaction, bool> basic = GetBasicProfitFilter();
            int[] ExcludedAccounts = { 100, 198 };//Distribution and deposit account account.

            Func<Transaction, bool> basicProfitPredicate = t =>
                        basic(t) &&
                        ExcludedAccounts.Contains(t.AccountId);
            return basicProfitPredicate;
        }

        public static Func<Transaction, bool> GetBasicDistributionFilter()
        {
            Func<Transaction, bool> basic = GetBasicProfitFilter();
            Func<Transaction, bool> distributionPredicate = t => 
                        basic(t) &&
                        t.AccountId==100;//Distribution account
            return distributionPredicate;
        }

    }
}
