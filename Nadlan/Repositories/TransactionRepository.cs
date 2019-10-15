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


        public override async Task<List<Transaction>> GetAllAsync()
        {
            return await Context.Transactions.OrderByDescending(a => a.Id).Include(a => a.Account).Include(a => a.Apartment).ToListAsync();
        }
        public async Task<List<TransactionDto>> GetAllExpensesAsync()
        {
            return await Context.Expenses.Join(
                Context.Transactions,
                expense => expense.TransactionId,
                transaction => transaction.Id,
                (expense, transaction) => new TransactionDto
                {
                    AccountId = transaction.AccountId,
                    AccountName = transaction.Account.Name,
                    Amount = transaction.Amount*-1,
                    ApartmentId = transaction.ApartmentId,
                    ApartmentAddress = transaction.Apartment.Address,
                    Comments = transaction.Comments,
                    Date = transaction.Date,
                    Hours = expense.Hours,
                    Id = transaction.Id,
                    IsPurchaseCost = transaction.IsPurchaseCost
                }
                ).OrderByDescending(a=>a.Date).ThenByDescending(a=>a.Id).ToListAsync();
            //return await Context.Expenses.OrderByDescending(a => a.Id).Include(a => a.Transaction).ToListAsync();
        }

        public async Task<TransactionDto> GetExpenseByIdAsync(int transactionId)
        {
            return await Context.Expenses.Join(
                Context.Transactions.Where(a=>a.Id==transactionId),
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
                    IsPurchaseCost = transaction.IsPurchaseCost
                }
                ).FirstOrDefaultAsync();
        }




        //private Transaction CreateCorrespondingTransaction_old(Transaction originalTransaction, int accountId)
        //{
        //    Transaction correspondingTransaction = new Transaction
        //    {
        //        ApartmentId = originalTransaction.ApartmentId,
        //        AccountId = accountId,
        //        Amount = originalTransaction.Amount,
        //        Date = originalTransaction.Date,
        //        Comments = $"{originalTransaction.Comments}"
        //    };
        //    return correspondingTransaction;
        //}

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
            
            Context.Entry(originalTransaction).CurrentValues.SetValues(transaction);

            //Expense updatedExpense = new Expense { TransactionId = transaction.Id, Hours = transaction.Hours };
            var originalExpense = Context.Expenses.Single(a=>a.TransactionId ==transaction.Id);
            originalExpense.Hours = transaction.Hours;
            //Context.Entry(originalExpense).CurrentValues.SetValues(updatedExpense);
            //if (transaction.Hours > 0)
            //{
            //    transaction.Comments = $"Hours: {transaction.Comments}";
            //}


            //Expense assiatantExpense = CreateCorrespondingExpense(transaction);
            ////Create(assiatantTransaction);
            //Context.Set<Expense>().Add(assiatantExpense);

            await SaveAsync();
        }

        public async Task CreateExpenseAndTransactionAsync(Transaction transaction)
        {
            if (transaction.Hours>0)
            {
                transaction.Comments = $"Hours: {transaction.Comments}";
            }
            //if (isHourCharge)
            //{
            //    transaction.Comments = $"Hours: {transaction.Comments}";
            //}
            //Charge the original amount
            transaction.Amount = transaction.Amount * -1;
            Create(transaction);
            Expense assiatantExpense = CreateCorrespondingExpense(transaction);
            //Create(assiatantTransaction);
            Context.Set<Expense>().Add(assiatantExpense);

            ////hours for existing apartment maintances - the the expense of the business
            //if (isHourCharge && transaction.AccountId == 4)
            //{
            //    transaction.AccountId = 200;
            //}

            await SaveAsync();
        }

        //public async Task CreateDoubleTransactionAsync_old(Transaction transaction, bool isHourCharge)
        //{
        //    if (isHourCharge)
        //    {
        //        transaction.Comments = $"Hours: {transaction.Comments}";
        //    }
        //    Transaction assiatantTransaction = CreateCorrespondingTransaction(transaction, 107);
        //    Create(assiatantTransaction);

        //    //hours for existing apartment maintances - the the expense of the business
        //    if (isHourCharge && transaction.AccountId == 4)
        //    {
        //        transaction.AccountId = 200;
        //    }

        //    //Charge the original account
        //    transaction.Amount = transaction.Amount * -1;
        //    Create(transaction);
        //    await SaveAsync();
        //}








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
                Transaction distribution = new Transaction
                {
                    ApartmentId = transaction.ApartmentId,
                    AccountId = portfolioLine.AccountId,
                    Amount = absoluteAmount * portfolioLine.Percentage,
                    Date = transaction.Date,
                    Comments = $"{transaction.Comments} (Distribution of {absoluteAmount} based on {portfolioLine.Percentage * 100}% ownership)"
                };
                Create(distribution);
            }

            await SaveAsync();
        }



        public async Task UpdateTransactionAsync(Transaction dbTransaction, Transaction transaction)
        {
            Update(transaction);
            await SaveAsync();
        }
        public async Task DeleteTransactionAsync(Transaction transaction)
        {
            Delete(transaction);
            await SaveAsync();
        }

        public Task<Transaction> GetByIdAsync(int id)
        {
            return Context.Transactions.FindAsync(id);
        }


        public Task<List<Transaction>> GetByAcountAsync(int apartmentId, int accountId, bool isPurchaseCost, int year)
        {
            Expression<Func<Transaction, bool>> predAll = c =>
                c.ApartmentId == apartmentId
              && c.AccountId == accountId
              && c.IsPurchaseCost == isPurchaseCost;

            Expression<Func<Transaction, bool>> predWithYear = c =>
                c.ApartmentId == apartmentId
                && c.AccountId == accountId
                && c.IsPurchaseCost == isPurchaseCost
                && c.Date.Year == year;
            Expression<Func<Transaction, bool>> predicate = year == 0 ? predAll : predWithYear;
            return FindByCondition(predicate).OrderByDescending(a => a.Date).ToListAsync();

            //return FindByCondition(c =>
            ////c.Amount <= 0  &&
            //c.ApartmentId == apartmentId
            //&& c.AccountId == accountId
            //&& c.IsPurchaseCost == isPurchaseCost).OrderByDescending(a => a.Date).ToListAsync();
        }



    }


}



//public async Task CreateDoubleTransactionToForHoursAsync_old(Transaction transaction)
//{
//    Transaction assiatantTransaction = CreateCorrespondingTransaction(transaction, 107);
//    //Transaction assiatantTransaction = new Transaction
//    //{
//    //    ApartmentId = transaction.ApartmentId,
//    //    AccountId = 107,
//    //    Amount = transaction.Amount,
//    //    Date = transaction.Date,
//    //    Comments = $"{transaction.Comments}"
//    //};
//    Create(assiatantTransaction);

//    if (transaction.AccountId == 4)
//    {
//        transaction.AccountId = 200;
//    }

//    //Charge the original account
//    transaction.Amount = transaction.Amount * -1;

//    Create(transaction);

//    //Account account = await Context.Accounts.FirstAsync(a => a.Id == transaction.AccountId);
//    //Only for normal accouts - depends on the account isIncome property
//    //if (account.AccountTypeId == 0)
//    //{
//    //    transaction.Amount = !account.IsIncome ? transaction.Amount * -1 : transaction.Amount;
//    //}
//    await SaveAsync();
//}
