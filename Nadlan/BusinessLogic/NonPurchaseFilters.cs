using Nadlan.Models;
using Nadlan.Models.Enums;
using System;

namespace Nadlan.BusinessLogic
{
    public class NonPurchaseFilters
    {
        /// <summary>
        /// This means that the distribution amount is reduced from the profit
        /// </summary>
        /// <returns></returns>
        public Func<Transaction, bool> GetProfitIncludingDistributionsFilter()
        {
            Func<Transaction, bool> basicPredicate = t =>
                        !t.IsDeleted &&
                        !t.IsPurchaseCost &&
                        !t.IsBusinessExpense &&
                        t.Account.AccountTypeId == 0;
            //t.AccountId != 198; //Not a deposit (this line is a not needed as we have the AccountTypeId condition)
            return basicPredicate;
        }

        public Func<Transaction, bool> GetAllBonusesFilter()
        {
            Func<Transaction, bool> bonusFilter = t =>
            !t.IsDeleted &&
            t.AccountId == (int)Accounts.Bonus;
            return bonusFilter;
        }


        public Func<Transaction, bool> GetGrossIncomeFilter()
        {
            Func<Transaction, bool> basic = GetProfitIncludingDistributionsFilter();
            Func<Transaction, bool> distributionPredicate = t =>
                        basic(t) &&
                        t.AccountId == (int)Accounts.Rent;//Rent
            return distributionPredicate;
        }
        public Func<Transaction, bool> GetAllExpensesFilter()
        {
            Func<Transaction, bool> basic = GetProfitIncludingDistributionsFilter();
            Func<Transaction, bool> expensesFilter = t =>
                        basic(t) &&
                        t.AccountId != (int)Accounts.Rent &&//Except for rent
                        t.AccountId != (int)Accounts.Distribution &&//Except for distribution
                        t.AccountId != (int)Accounts.Bonus;//Except for bonus
            return expensesFilter;
        }
        public Func<Transaction, bool> GetPendingExpensesFilter()
        {
            Func<Transaction, bool> basic = GetAllExpensesFilter();
            Func<Transaction, bool> expensesFilter = t =>
                        basic(t) &&
                        t.PersonalTransactionId == 0;//Not covered yet
            return expensesFilter;
        }
    }
}
