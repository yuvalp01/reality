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
    public class TransactionRepository : Repository<Transaction>, ITransactionRepository
    {
        public TransactionRepository(NadlanConext context) : base(context)
        {
        }


        public async Task<List<Transaction>> GetAllAsync(int monthsBack)
        {
            var transactions = Context.Transactions.OrderByDescending(a => a.Id)
                .Include(a => a.Account)
                .Include(a => a.Apartment)
                .Where(a => !a.IsDeleted);
            if (monthsBack>0)
            {
                return await transactions
                    .Where(a=>a.Date>DateTime.Today.AddMonths(-monthsBack))
                    .ToListAsync();
            }
            else
            {
                return await transactions.ToListAsync();
            }

            //if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development")
            //{
            //    transactions = transactions.Where(a=> a.Id>1000);
            //}



        }
        public async Task<List<TransactionDto>> GetAllExpensesAsync(int monthsBack)
        {
            Func<Transaction, bool> basicPredicate = t => !t.IsDeleted;
            Func<Transaction, bool> predicate;
            if (monthsBack!=0)
            {
                predicate = t =>
                basicPredicate(t) && t.Date > DateTime.Today.AddMonths(-monthsBack);
            }
            else
            {
                predicate = basicPredicate;
            }


            var expenses = Context.Expenses.Join(
                Context.Transactions
                .Where(a => predicate(a)),
                expense => expense.TransactionId,
                transaction => transaction.Id,
                (expense, transaction) => new TransactionDto
                {
                    AccountId = transaction.AccountId,
                    AccountName = transaction.Account.Name,
                    Amount = transaction.Amount * -1,
                    ApartmentId = transaction.ApartmentId,
                    ApartmentAddress = transaction.Apartment.Address,
                    Comments = transaction.Comments,
                    Date = transaction.Date,
                    Hours = expense.Hours,
                    Id = transaction.Id,
                    IsPurchaseCost = transaction.IsPurchaseCost,
                    IsConfirmed = transaction.IsConfirmed
                }
                ).OrderByDescending(a => a.Date).ThenByDescending(a => a.Id);//.Take(count);
            //IQueryable<TransactionDto> expensesList;
            //if (count==0)
            //{
            //    return await expenses.ToListAsync();
            //}
            //else
            //{
            //    return await expenses.Take(count).ToListAsync();
            //}

            return await expenses.ToListAsync();
        }

        public async Task<TransactionDto> GetExpenseByIdAsync(int transactionId)
        {
            return await Context.Expenses.Join(
                Context.Transactions.Where(a => a.Id == transactionId
                && !a.IsDeleted),
                expense => expense.TransactionId,
                transaction => transaction.Id,
                (expense, transaction) => new TransactionDto
                {
                    AccountId = transaction.AccountId,
                    AccountName = transaction.Account.Name,
                    Amount = transaction.Amount * -1,
                    ApartmentId = transaction.ApartmentId,
                    ApartmentAddress = transaction.Apartment.Address,
                    Comments = transaction.Comments,
                    Date = transaction.Date,
                    Hours = expense.Hours,
                    Id = transaction.Id,
                    IsPurchaseCost = transaction.IsPurchaseCost,
                    IsConfirmed = transaction.IsConfirmed
                }
                ).FirstOrDefaultAsync();
        }






        private Expense CreateCorrespondingExpense(Transaction originalTransaction)
        {
            Expense correspondingExpense = new Expense
            {
                Hours = originalTransaction.Hours,
                TransactionId = originalTransaction.Id,
            };
            return correspondingExpense;
        }

        public async Task UpdateExpenseAndTransactionAsync(Transaction transaction)
        {
            //Charge the original amount
            transaction.Amount = transaction.Amount * -1;
            var originalTransaction = Context.Transactions.Find(transaction.Id);
            SwitchIsBusinessExpense(transaction);

            //Update original transaction:
            Context.Entry(originalTransaction).CurrentValues.SetValues(transaction);
            //Update origial expense with hours:
            var originalExpense = Context.Expenses.Single(a => a.TransactionId == transaction.Id);
            originalExpense.Hours = transaction.Hours;

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
            // when it's hours for apartment maintances (so only apartments with tenants)
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
            SwitchIsBusinessExpense(transaction);
            //Charge the original amount
            transaction.Amount = transaction.Amount * -1;
            Create(transaction);
            Expense assiatantExpense = CreateCorrespondingExpense(transaction);
            //Create(assiatantTransaction);
            Context.Set<Expense>().Add(assiatantExpense);


            await SaveAsync();
        }

        public async Task CreateTransactionAsync(Transaction transaction)
        {
            Account account = await Context.Accounts.FirstAsync(a => a.Id == transaction.AccountId);
            //Only for normal accouts - depends on the account isIncome property
            if (account.AccountTypeId == 0)
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
        //[Obsolete]
        //public async Task DeleteTransactionAsync(Transaction transaction)
        //{
        //    throw new NotImplementedException("delete only direcly in db with boolean flag");
        //    Delete(transaction);
        //    await SaveAsync();
        //}

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

    }


}


