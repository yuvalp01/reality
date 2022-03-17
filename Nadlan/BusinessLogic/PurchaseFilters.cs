using Nadlan.Models;
using Nadlan.Models.Enums;
using System;

namespace Nadlan.BusinessLogic
{
    public class PurchaseFilters
    {

        public Func<Transaction, bool> GetAllPurchaseFilter()
        {
            Func<Transaction, bool> basicPredicate = t =>
                        !t.IsDeleted &&
                        !t.IsBusinessExpense &&
                        t.IsPurchaseCost &&
                        t.Account.AccountTypeId == 0;
                        //!t.Account.IsIncome;

            return basicPredicate;
        }


        public Func<Transaction, bool> GetTotalCostFilter()
        {
            Func<Transaction, bool> basicPredicate = t =>
                        !t.IsDeleted &&
                        !t.IsBusinessExpense &&
                        t.IsPurchaseCost &&
                        //t.Account.AccountTypeId == 0 &&
                        t.AccountId != (int)Accounts.SecurityDeposit &&//Except for Security Deposit
                        t.AccountId != (int)Accounts.Business &&//Except for Business
                        t.AccountId != (int)Accounts.Balance &&//Except for Balance
                        //  !t.Account.IsIncome &&
                        t.AccountId != (int)Accounts.Investment && //not investment
                        t.AccountId != (int)Accounts.Rent; // not rent





            return basicPredicate;
        }

        public Func<Transaction, bool> GetRenovationFilter()
        {
            Func<Transaction, bool> basic = GetTotalCostFilter();
            bool predicate(Transaction t) =>
                          basic(t) &&
                         (t.AccountId == (int)Accounts.RenovationMiscellaneous 
                         || t.AccountId == (int)Accounts.RenovationContractor);

            return predicate;
        }
        public Func<Transaction, bool> GetCostNotRenovataionFilter()
        {
            Func<Transaction, bool> basic = GetTotalCostFilter();
            bool predicate(Transaction t) =>
                          basic(t) &&
                         !(t.AccountId == (int)Accounts.RenovationMiscellaneous || t.AccountId == (int)Accounts.RenovationContractor) &&
                         t.AccountId!= (int)Accounts.ApartmentCost;

            return predicate;
        }
        public Func<Transaction, bool> GetInvestmentFilter()
        {
            bool predicate(Transaction t) =>
              !t.IsDeleted &&
              t.AccountId == (int)Accounts.Investment;
            return predicate;
        }
    }
}
