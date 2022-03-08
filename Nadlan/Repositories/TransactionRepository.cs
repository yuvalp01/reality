using Microsoft.EntityFrameworkCore;
using Nadlan.Models;
using Nadlan.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Nadlan.Repositories
{
    public class Filter
    {
        public int? AccountId { get; set; }
        public int? ApartmentId { get; set; }
        public int? MonthsBack { get; set; }
        public bool? IsPurchaseCost { get; set; }
        public int? Year { get; set; }
        public bool? IsSoFar { get; set; }
        public int? PersonalTransactionId { get; set; }
        public bool? IsLiteObject { get; set; }
    }

    public class TransactionRepository : Repository<Transaction>, ITransactionRepository
    {
        public TransactionRepository(NadlanConext context) : base(context)
        {
        }


        public async Task<List<Transaction>> GetAllAsync(int monthsBack, CreatedByEnum createdBy)
        {
            var transactionList = Context.Transactions.OrderByDescending(a => a.Id)
                .Include(a => a.Account)
                .Include(a => a.Apartment)
                .Where(a => !a.IsDeleted);
            if (createdBy != CreatedByEnum.Any)
            {
                transactionList = transactionList.Where(a => a.CreatedBy == (int)createdBy);
            }
            if (monthsBack > 0)
            {
                var transactions = await transactionList
                    .Where(a => a.Date > DateTime.Today.AddMonths(-monthsBack))
                    .ToListAsync();
                var messages = await Context.Messages
                    .Where(a => a.IsDeleted == false)
                    .Where(a => a.TableName == "transactions")
                    .ToListAsync();
                foreach (var trans in transactions)
                {
                    trans.Messages = messages.Where(a => a.ParentId == trans.Id).ToList();
                }
                return transactions;
            }
            else
            {
                return await transactionList.ToListAsync();
            }
        }


        public async Task<List<TransactionDto>> GetAllTransactionDtoAsync(int monthsBack, CreatedByEnum createdBy)
        {
            var transactionList = Context.Transactions.OrderByDescending(a => a.Id)
                .Include(a => a.Account)
                .Include(a => a.Apartment)
                .Where(a => !a.IsDeleted);
            if (createdBy != CreatedByEnum.Any)
            {
                transactionList = transactionList.Where(a => a.CreatedBy == (int)createdBy);
            }

           var transactionListDto = transactionList.Select(transaction => new TransactionDto {
               AccountId = transaction.AccountId,
               AccountName = transaction.Account.Name,
               Amount = transaction.Amount * -1,
               ApartmentId = transaction.ApartmentId,
               ApartmentAddress = transaction.Apartment.Address,
               Comments = transaction.Comments,
               Date = transaction.Date,
               Hours = transaction.Hours,
               Id = transaction.Id,
               IsPurchaseCost = transaction.IsPurchaseCost,
               IsConfirmed = transaction.IsConfirmed,
               PersonalTransactionId = transaction.PersonalTransactionId,
               IsPettyCash = transaction.IsPettyCash,
               CreatedBy = transaction.CreatedBy,
               IsPending = transaction.IsPending
           });


            if (monthsBack > 0)
            {
                var transactions = await transactionListDto
                    .Where(a => a.Date > DateTime.Today.AddMonths(-monthsBack))
                    .ToListAsync();
                var messages = await Context.Messages
                    .Where(a => a.IsDeleted == false)
                    .Where(a => a.TableName == "transactions")
                    .ToListAsync();
                foreach (var trans in transactions)
                {
                    trans.Messages = messages.Where(a => a.ParentId == trans.Id).ToList();
                }
                return transactions;
            }
            else
            {
                return await transactionListDto.ToListAsync();
            }
        }



        public async Task UpdateExpenseAndTransactionAsync(Transaction transaction)
        {
            //Charge the original amount
            transaction.Amount = transaction.Amount * -1;
            var originalTransaction = Context.Transactions.Find(transaction.Id);
            SwitchIsBusinessExpense(transaction);

            //Update original transaction:
            Context.Entry(originalTransaction).CurrentValues.SetValues(transaction);
            //Removed - not using expense table anymore
            ////Update origial expense with hours:
            //var originalExpense = Context.Expenses.Single(a => a.TransactionId == transaction.Id);
            //originalExpense.Hours = transaction.Hours;

            await SaveAsync();
        }
        internal async Task Confirm(int transactionId)
        {
            var originalTransaction = Context.Transactions.FindAsync(transactionId);
            originalTransaction.Result.IsConfirmed = true;
            await SaveAsync();
        }

        public async Task SoftDeleteTransactionAsync(int transactionId)
        {
            var originalTransaction = Context.Transactions.FindAsync(transactionId);
            originalTransaction.Result.IsDeleted = true;
            await SaveAsync();
        }

        private void SwitchIsBusinessExpense(Transaction transaction)
        {
            // The expense is on the business when:
            //"Business/Geneal" transaction is a business expense and also
            // when its hours for apartment maintenance (so only apartments with tenants)
            if (transaction.AccountId == 200
                || (transaction.AccountId == 4 && transaction.Hours > 0))
            {
                transaction.IsBusinessExpense = true;
            }
            else
            {
                transaction.IsBusinessExpense = false;
            }
        }

        public async Task CreateExpenseAndTransactionAsync(Transaction transaction)
        {
            if (transaction.Hours > 0)
            {
                transaction.Comments = $"Hours: {transaction.Comments}";
            }
            if (!transaction.IsPettyCash)
            {
                transaction.IsPending = true;
            }
            SwitchIsBusinessExpense(transaction);
            //Charge the original amount
            transaction.Amount = transaction.Amount * -1;
            Create(transaction);
            //Removed - not using expense table anymore
            //Expense assiatantExpense = CreateCorrespondingExpense(transaction);
            //Context.Set<Expense>().Add(assiatantExpense);


            await SaveAsync();
        }

        public async Task CreateTransactionAsync(Transaction transaction)
        {
            Account account = await Context.Accounts.FirstAsync(a => a.Id == transaction.AccountId);
            //Only for normal accouts or bonus - depends on the account isIncome property
            if (account.AccountTypeId == 0 || account.AccountTypeId == 3)
            {
                transaction.Amount = !account.IsIncome ? transaction.Amount * -1 : transaction.Amount;
            }
            Create(transaction);
            await SaveAsync();
        }

        public async Task DistributeBalanceAsync(Transaction transaction)
        {
            if (transaction.AccountId != 100)
            {
                throw new ArgumentException("It is only possible to distribute from account 100");
            }
            List<Portfolio> portfolioLines = await Context.Portfolios.Where(a => a.ApartmentId == transaction.ApartmentId).ToListAsync();
            if (portfolioLines.Sum(a => a.Percentage) != 1)
            {
                throw new Exception($"Apartment Id {transaction.ApartmentId} ownership is not fully mapped in the portfolio table, make sure it sums up to 100%");
            }

            decimal absoluteAmount = transaction.Amount;
            //Change sign to reduction:
            transaction.Amount = transaction.Amount * -1;
            Create(transaction);
            foreach (var portfolioLine in portfolioLines)
            {
                PersonalTransaction distribution = new PersonalTransaction
                {
                    TransactionType = TransactionType.Distribution,
                    ApartmentId = transaction.ApartmentId,
                    StakeholderId = portfolioLine.StakeholderId,
                    Amount = absoluteAmount * portfolioLine.Percentage,
                    Date = transaction.Date,
                    Comments = $"{transaction.Comments} (Distribution of {absoluteAmount} based on {portfolioLine.Percentage * 100}% ownership)"
                };



                Context.PersonalTransactions.Add(distribution);
            }

            await SaveAsync();
        }

        internal async Task<bool> PayUnpay(int transactionId)
        {
            var originalTransaction = await Context.Transactions.FindAsync(transactionId);
            originalTransaction.IsPending = !originalTransaction.IsPending;
            await SaveAsync();
            return originalTransaction.IsPending;
        }

        public async Task<decimal> IncreaseTransactionAmountAsync(int transactionId, decimal additionalAmount)
        {
            var transaction = Context.Transactions.FirstOrDefault(a => a.Id == transactionId);
            transaction.Date = DateTime.Now;
            decimal currentAmount = transaction.Amount;
            transaction.Amount = currentAmount + additionalAmount;
            Update(transaction);
            await SaveAsync();
            return transaction.Amount;
        }

        public async Task UpdateTransactionAsync(Transaction dbTransaction, Transaction transaction)
        {
            Update(transaction);
            await SaveAsync();
        }

        public Task<Transaction> GetByIdAsync(int id)
        {
            return Context.Transactions.FindAsync(id);
        }


        public Task<List<Transaction>> GetByAcountAsync(int apartmentId, int accountId, bool isPurchaseCost, int year)
        {
            Expression<Func<Transaction, bool>> predAll = c =>
               !c.IsDeleted
              && c.ApartmentId == apartmentId
              && c.AccountId == accountId
              && c.IsPurchaseCost == isPurchaseCost;

            Expression<Func<Transaction, bool>> predWithYear = c =>
                 !c.IsDeleted
                && c.ApartmentId == apartmentId
                && c.AccountId == accountId
                && c.IsPurchaseCost == isPurchaseCost
                && c.Date.Year == year;
            Expression<Func<Transaction, bool>> predicate = year == 0 ? predAll : predWithYear;
            return FindByCondition(predicate).OrderByDescending(a => a.Date).ToListAsync();
        }

        public Task<List<Transaction>> GetPendingExpensesForInvestor(int stakeholderId)
        {
            List<int> apartmentsIds = Context.Portfolios
                .Where(a => a.StakeholderId == stakeholderId)
                .Select(a => a.ApartmentId)
                .ToList();

            return Context.Transactions
                         .Where(a => a.IsDeleted == false)
                         .Where(a => apartmentsIds.Contains(a.ApartmentId))
                         .Where(a => a.PersonalTransactionId == 0)
                         .ToListAsync();


        }

        public Task<List<Transaction>> GetPendingExpensesForApartment(int apartmentId, int year)
        {
            //If 0 show all up-to-date
            if (year == 0) year = DateTime.Today.Year;
            return Context.Transactions
                         .Where(a => a.IsDeleted == false)
                         .Where(a => a.ApartmentId == apartmentId)
                         .Where(a => a.PersonalTransactionId == 0)
                         .Where(a => a.Date.Year <= year)
                         .ToListAsync();


        }




        public Task<List<Transaction>> GetFilteredTransactions(Filter filter)
        {
            var query = Context.Transactions.OrderByDescending(a => a.Id)
                .Include(a => a.Account)
                .Include(a => a.Apartment)
                .Where(a => !a.IsDeleted);

            if (filter.IsLiteObject != null && filter.IsLiteObject.Value)
            {
                query = Context.Transactions.OrderByDescending(a => a.Id)
                .Where(a => !a.IsDeleted);
            }

            //Conditionaly filter accounts:
            query = query.Where(a => filter.AccountId == null ? true : a.AccountId == filter.AccountId);
            //Conditionaly filter apartments:
            query = query.Where(a => filter.ApartmentId == null ? true : a.ApartmentId == filter.ApartmentId);
            //Conditionaly filter months back:
            query = query.Where(a => filter.MonthsBack == null ? true : a.Date > DateTime.Today.AddMonths(-(int)filter.MonthsBack));

            //Conditionaly filter isPurchaseCost:
            query = query.Where(a => filter.IsPurchaseCost == null ? true : a.IsPurchaseCost == filter.IsPurchaseCost);
            //Conditionaly filter PersonalTransactionId:
            query = query.Where(a => filter.PersonalTransactionId == null ? true : a.PersonalTransactionId == filter.PersonalTransactionId);
            //Conditionaly filter year:
            if (filter.Year != null)
            {
                if (filter.Year > 0)
                {
                    if (filter.IsSoFar.Value)
                    {
                        query = query.Where(a => a.Date.Year <= filter.Year);
                    }
                    else
                    {
                        query = query.Where(a => a.Date.Year == filter.Year);
                    }
                }
            }
            return query.OrderBy(a => a.Date).ToListAsync();
        }
    }
}








//[Obsolete("Use GetAllExpensesAsync, exepenses table obsolete")]
//public async Task<List<TransactionDto>> GetAllExpensesAsync_(int monthsBack)
//{
//    Func<Transaction, bool> basicPredicate = t => !t.IsDeleted;
//    Func<Transaction, bool> predicate;
//    if (monthsBack != 0)
//    {
//        predicate = t =>
//        basicPredicate(t) && t.Date > DateTime.Today.AddMonths(-monthsBack);
//    }
//    else
//    {
//        predicate = basicPredicate;
//    }


//    var expensesList = Context.Expenses.Join(
//        Context.Transactions
//        .Where(a => predicate(a)),
//        expense => expense.TransactionId,
//        transaction => transaction.Id,
//        (expense, transaction) => new TransactionDto
//        {
//            AccountId = transaction.AccountId,
//            AccountName = transaction.Account.Name,
//            Amount = transaction.Amount * -1,
//            ApartmentId = transaction.ApartmentId,
//            ApartmentAddress = transaction.Apartment.Address,
//            Comments = transaction.Comments,
//            Date = transaction.Date,
//            Hours = expense.Hours,
//            Id = transaction.Id,
//            IsPurchaseCost = transaction.IsPurchaseCost,
//            IsConfirmed = transaction.IsConfirmed
//        }
//        ).OrderByDescending(a => a.Date).ThenByDescending(a => a.Id);


//    if (monthsBack != 0)
//    {
//        var expenses = await expensesList
//            .Where(a => a.Date > DateTime.Today.AddMonths(-monthsBack))
//            .ToListAsync();

//        var messages = await Context.Messages
//             .Where(a => a.IsDeleted == false)
//             .Where(a => a.TableName == "transactions")
//             .ToListAsync();
//        foreach (var expense in expenses)
//        {
//            expense.Messages = messages.Where(a => a.ParentId == expense.Id).ToList();
//        }
//        return expenses;
//    }
//    else
//    {
//        return await expensesList.ToListAsync();
//    }


//}

//[Obsolete("Not needed anymore, used directly in the Expenses controller")]
//public async Task<TransactionDto> GetExpenseByIdAsync(int transactionId)
//{
//    return await Context.Expenses.Join(
//        Context.Transactions.Where(a => a.Id == transactionId
//        && !a.IsDeleted),
//        expense => expense.TransactionId,
//        transaction => transaction.Id,
//        (expense, transaction) => new TransactionDto
//        {
//            AccountId = transaction.AccountId,
//            AccountName = transaction.Account.Name,
//            Amount = transaction.Amount * -1,
//            ApartmentId = transaction.ApartmentId,
//            ApartmentAddress = transaction.Apartment.Address,
//            Comments = transaction.Comments,
//            Date = transaction.Date,
//            Hours = expense.Hours,
//            Id = transaction.Id,
//            IsPurchaseCost = transaction.IsPurchaseCost,
//            IsConfirmed = transaction.IsConfirmed
//        }
//        ).FirstOrDefaultAsync();
//}

//[Obsolete("Expenses table is not in use anymore")]
//private Expense CreateCorrespondingExpense(Transaction originalTransaction)
//{
//    Expense correspondingExpense = new Expense
//    {
//        Hours = originalTransaction.Hours,
//        TransactionId = originalTransaction.Id,
//    };
//    return correspondingExpense;
//}