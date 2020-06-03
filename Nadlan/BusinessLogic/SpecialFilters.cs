using Nadlan.Models;
using System;

namespace Nadlan.BusinessLogic
{
    public class SpecialFilters
    {

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
