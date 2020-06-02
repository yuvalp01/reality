using Nadlan.Models;
using System;

namespace Nadlan.BusinessLogic
{
    public class PurchaseFilters
    {

        public static Func<Transaction, bool> GetTotalCostFilter()
        {
            Func<Transaction, bool> basicPredicate = t =>
                        !t.IsDeleted &&
                        !t.IsBusinessExpense &&
                        t.IsPurchaseCost &&
                        t.Account.AccountTypeId == 0 &&
                        !t.Account.IsIncome &&
                        t.AccountId != 13; //investment
                        

            return basicPredicate;
        }

        public static Func<Transaction, bool> GetRenovationFilter()
        {
            Func<Transaction, bool> basic = GetTotalCostFilter();
            bool predicate(Transaction t) =>
                          basic(t) &&
                         (t.AccountId == 6 || t.AccountId == 17);

            return predicate;
        }
        public static Func<Transaction, bool> GetCostNotRenovataionFilter()
        {
            Func<Transaction, bool> basic = GetTotalCostFilter();
            bool predicate(Transaction t) =>
                          basic(t) &&
                         !(t.AccountId == 6 || t.AccountId == 17);

            return predicate;
        }
        public static Func<Transaction, bool> GetInvestmentFilter()
        {
            bool predicate(Transaction t) =>
              !t.IsDeleted &&
              t.AccountId == 13;
            return predicate;
        }
    }
}
