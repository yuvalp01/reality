using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Nadlan.Models;

namespace Nadlan.BusinessLogic
{
    public class SpecialFilters
    {

        public static Func<Transaction, bool> GetAllDepositFilter()
        {
            Func<Transaction, bool> basicPredicate = t =>
                        !t.IsDeleted &&
                        !t.IsPurchaseCost &&
                        !t.IsBusinessExpense &&
                        t.AccountId == 198;
            return basicPredicate;
        }

        public static Func<Transaction, bool> GetAllBusinessFilter()
        {
            Func<Transaction, bool> basicPredicate = t =>
                        !t.IsDeleted &&
                        t.IsBusinessExpense;
            return basicPredicate;
        }
    }
}
