using Microsoft.EntityFrameworkCore;
using Nadlan.Models;
using Nadlan.Models.Enums;
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
        static readonly List<int> apartmentsWithMortgage = new List<int> {14, 16, 17, 18 };
        //private readonly IMapper _mapper;
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

            var transactionListDto = transactionList.Select(transaction => new TransactionDto
            {
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
                BankAccountId = transaction.BankAccountId,
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
            if (transaction.AccountId == (int)Accounts.Business
                || (transaction.AccountId == (int)Accounts.MaintenanceMiscellaneous && transaction.Hours > 0))
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
            if (transaction.BankAccountId!=0)
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
            if (transaction.AccountId != (int)Accounts.Distribution)
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
            return query.OrderByDescending(a => a.Date).ToListAsync();
        }


        public async Task<RentRelatedTransactionsResponse> CreateRentTransactionsAsync(Transaction rentTrans)
        {

            //Classic transactions (not expenses) will get userAccount 1 
            rentTrans.CreatedBy = (int)CreatedByEnum.Yuval;
            //decimal rent = rentTrans.Amount;
            await CreateTransactionAsync(rentTrans);

            //Cloe management transaction based on the rent
            var managementTrans = Context.Transactions.AsNoTracking()
                             .FirstOrDefault(e => e.Id == rentTrans.Id);
            //reset id
            managementTrans.Id = 0;
            //Update new values
            SetManagementValues(rentTrans, managementTrans);

            await CreateTransactionAsync(managementTrans);

            //Clone TaxTrans
            var taxTrans = Context.Transactions.AsNoTracking()
                             .FirstOrDefault(e => e.Id == rentTrans.Id);
            //reset id
            taxTrans.Id = 0;
            //Update new values
            SetTaxValues(rentTrans, taxTrans);

            await CreateTransactionAsync(taxTrans);

            Transaction interestTrans = null;
            if (apartmentsWithMortgage.Contains(rentTrans.ApartmentId))
            {
                //Clone Interest trans
                interestTrans = Context.Transactions.AsNoTracking()
                                 .FirstOrDefault(e => e.Id == rentTrans.Id);
                //reset id
                interestTrans.Id = 0;
                //Update new values
                SetMortgageInterestValues(interestTrans, rentTrans);

                await CreateTransactionAsync(interestTrans);
            }

            return new RentRelatedTransactionsResponse
            {
                Rent = rentTrans,
                Management = managementTrans,
                Tax = taxTrans,
                Interest = interestTrans

            };
        }

        private void SetManagementValues(Transaction rentTrans, Transaction managementTrans)
        {
            List<int> partnershipApartments = new List<int> { 1, 3, 4, 20 };

            //Update new values
            managementTrans.AccountId = (int)Accounts.Management;
            managementTrans.PersonalTransactionId = 0; //most cases - not cover yet
            if (partnershipApartments.Contains(rentTrans.ApartmentId))
            {
                managementTrans.PersonalTransactionId = -2; //for partnership use project funds
            }
            managementTrans.Amount = rentTrans.Amount * 0.1m;
            managementTrans.Comments = $"Created automatically based on 10% of ${rentTrans.Amount} rent. (transactionId: {rentTrans.Id})";
        }

        private void SetTaxValues(Transaction rentTrans, Transaction taxTrans)
        {
            //Update new values
            taxTrans.AccountId = (int)Accounts.TaxEstimation; ;
            taxTrans.PersonalTransactionId = -4;//Future payment
            taxTrans.Amount = rentTrans.Amount * 0.15m;
            taxTrans.Comments = $"Created automatically based on 15% of ${rentTrans.Amount} rent. (transactionId: {rentTrans.Id})";
        }

        private void SetMortgageInterestValues(Transaction interestTrans, Transaction rentTrans)
        {
            //Update new values
            decimal monthlyInerestCost = GetMonthyInterestCost(interestTrans.ApartmentId);
            interestTrans.AccountId = (int)Accounts.Mortgage_Interest;
            interestTrans.PersonalTransactionId = (int)NonPersonalTrasactionId.CoveredWithCreditCard;//Covered as part of the mortgage payments
            interestTrans.Amount = monthlyInerestCost;
            interestTrans.Comments = $"Created automatically based on the mortgage interest rate. (transactionId: {rentTrans.Id})";
        }


        private decimal GetMonthyInterestCost(int apartmentId)
        {
            switch (apartmentId)
            {

                case 14:
                    return 0;
                case 16:
                    return 0;
                case 17:
                    return 0;
                case 18:
                    return 0;
                default:
                    return 0;

            }
        }



        public async Task<RentRelatedTransactionsResponse> UpdateRentTransactionsAsync(Transaction rentTrans)
        {

            var originalRent = Context.Transactions.AsNoTracking()
                 .FirstOrDefault(e => e.Id == rentTrans.Id);

            //Update rent
            rentTrans.IsConfirmed = false;
            Context.Entry(rentTrans).State = EntityState.Modified;

            //update Management
            var mngTrans = FindTransByAccount(originalRent, (int)Accounts.Management);
            //reset confirm
            mngTrans.IsConfirmed = false;
            //Update new values
            mngTrans.Amount = rentTrans.Amount * -0.1m;
            mngTrans.Date = rentTrans.Date;
            mngTrans.Comments = $"Created automatically based on 10% of ${rentTrans.Amount} rent. (transactionId: {rentTrans.Id}";
            Context.Entry(mngTrans).State = EntityState.Modified;

            //update Tax
            var taxTrans = FindTransByAccount(originalRent, (int)Accounts.TaxEstimation);
            //reset confirm
            taxTrans.IsConfirmed = false;
            taxTrans.Amount = rentTrans.Amount * -0.15m;
            taxTrans.Date = rentTrans.Date;
            taxTrans.Comments = $"Created automatically based on 15% of ${rentTrans.Amount} rent. (transactionId: {rentTrans.Id}";
            Context.Entry(taxTrans).State = EntityState.Modified;

            Transaction interestTrans = FindTransByAccount(originalRent, (int)Accounts.Mortgage_Interest);
            if (interestTrans != null)
            {
                //update Interest           
                decimal monthlyInerestCost = GetMonthyInterestCost(interestTrans.ApartmentId);
                //reset confirm
                interestTrans.IsConfirmed = false;
                interestTrans.Amount = monthlyInerestCost;
                interestTrans.Date = rentTrans.Date;
                interestTrans.Comments = $"Created automatically based on the mortgage interest rate. (transactionId: {rentTrans.Id})";
                Context.Entry(interestTrans).State = EntityState.Modified;
            }

            await Context.SaveChangesAsync();

            return new RentRelatedTransactionsResponse
            {
                Rent = rentTrans,
                Management = mngTrans,
                Tax = taxTrans,
                Interest = interestTrans
            };

        }

        public async Task SoftDeleteRentTransactionsAsync(int transactionId)
        {
            var originalTransaction = await Context.Transactions.FindAsync(transactionId);
            originalTransaction.IsDeleted = true;

            var mngTrans = FindTransByAccount(originalTransaction, (int)Accounts.Management);
            if (mngTrans != null)
            {
                mngTrans.IsDeleted = true;
            }
            var taxTrans = FindTransByAccount(originalTransaction, (int)Accounts.TaxEstimation);
            if (taxTrans != null)
            {
                taxTrans.IsDeleted = true;
            }
            var interestTrans = FindTransByAccount(originalTransaction, (int)Accounts.Mortgage_Interest);
            if (interestTrans != null)
            {
                interestTrans.IsDeleted = true;
            }



            await SaveAsync();

        }

        /// <summary>
        /// Get all Stella's expenses
        /// </summary>
        /// <returns></returns>
        public async Task<decimal> GetExpensesBalance()
        {
            var balance = Context.Transactions
                .Where(a => !a.IsDeleted)
                .Where(a => a.CreatedBy == (int)CreatedByEnum.Stella)
                .Where(a => a.BankAccountId == 0)
                .SumAsync(a => a.Amount);
            return await balance * -1;
        }

        private Transaction FindTransByAccount(Transaction originalRent, int accountId)
        {
            var transaction = Context.Transactions
            .Where(a => !a.IsDeleted
             && a.Date == originalRent.Date
             && a.ApartmentId == originalRent.ApartmentId
             && a.AccountId == accountId).FirstOrDefault();
            return transaction;
        }





    }


    public class RentRelatedTransactionsResponse
    {
        public Transaction Rent { get; set; }
        public Transaction Management { get; set; }
        public Transaction Tax { get; set; }
        public Transaction Interest { get; set; }

    }


}

