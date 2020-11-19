using Nadlan.Models;
using System;

namespace Nadlan.BusinessLogic
{
    public class SpecialFilters
    {

        public Func<Transaction, bool> GetValidTransactionsForReportsFilter()
        {
            Func<Transaction, bool> predicate = t =>
                  t.IsDeleted == false &&
                  t.IsBusinessExpense == false &&
                  t.Account.AccountTypeId == 0; 
            return predicate;
        }


        public Func<Transaction, bool> GetAllDepositFilter()
        {
            Func<Transaction, bool> basicPredicate = t =>
                        !t.IsDeleted &&
                        !t.IsPurchaseCost &&
                        !t.IsBusinessExpense &&
                        t.AccountId == 198;
            return basicPredicate;
        }

        public Func<Transaction, bool> GetAllBusinessFilter()
        {
            Func<Transaction, bool> basicPredicate = t =>
                        !t.IsDeleted &&
                        t.IsBusinessExpense;
            return basicPredicate;
        }
    }
}
