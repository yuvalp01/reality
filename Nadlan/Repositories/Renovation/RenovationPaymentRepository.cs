using Microsoft.EntityFrameworkCore;
using Nadlan.Models.Renovation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nadlan.Repositories.Renovation
{
    public class RenovationPaymentRepository : Repository<RenovationPayment>, IRenovationPaymentRepository
    {
        public RenovationPaymentRepository(NadlanConext nadlanConext) : base(nadlanConext)
        {
        }
        public Task<List<RenovationPayment>> GetPaymentsAsync(int projectId)
        {
            return Context.RenovationPayments
                .Include(a => a.RenovationProject)
                .Where(a => a.IsDeleted == false)
                .Where(a => a.RenovationProjectId == projectId)
                .OrderBy(a=>a.Id)
                //.OrderByDescending(a=>a.DatePayment==null)
                //.ThenByDescending(a=>a.DatePayment)
                .ToListAsync();
        }
        public Task<RenovationPayment> GetPaymentByIdAsync(int id)
        {
            return Context.RenovationPayments
                .Where(a => a.IsDeleted == false)
                .Include(a => a.RenovationProject)
                .FirstOrDefaultAsync(a => a.Id == id);
        }
        public async Task CreateAsync(RenovationPayment renovationPayment)
        {
            Create(renovationPayment);
            await SaveAsync();
        }
        public Task DeleteAsync(RenovationLine renovationLine)
        {
            throw new NotImplementedException();
        }
        public Task<RenovationPayment> GetByIdAsync(int id)
        {
            return FindByCondition(a => a.Id == id).FirstOrDefaultAsync();
        }
        public async Task UpdateAsync(RenovationPayment renovationPayment)
        {
            Update(renovationPayment);
            await SaveAsync();
        }






        public async Task ConfirmAsync(int paymentId)
        {
            RenovationPayment renovationPayment = Context.RenovationPayments
                .FirstOrDefault(a => a.Id == paymentId);
            renovationPayment.IsConfirmed = true;
            Update(renovationPayment);
            await SaveAsync();
        }

        public async Task<decimal?> MakePaymentAsync(RenovationPayment renovationPayment)
        {
            Update(renovationPayment);
            decimal? newBalance = null;
            //Payment is always with a date but we make a server side check anyway:
            if (renovationPayment.DatePayment != null)
            {
                TransactionRepository transactionRepository = new TransactionRepository(Context);
                //Increase the negativity of the transaction:
                newBalance = await transactionRepository
                    .IncreaseTransactionAmountAsync(renovationPayment.RenovationProject.TransactionId,
                    renovationPayment.Amount * -1);
                Update(renovationPayment);

            }
            await SaveAsync();
            return newBalance;
        }
        /// 1. Delete the milestone/payment altogether
        /// 2. Decrease the negativity of the transaction
        public async Task<decimal?> SoftDeleteAsync(int paymentId)
        {
            RenovationPayment renovationPayment = Context.RenovationPayments
                .Include(a => a.RenovationProject)
                .FirstOrDefault(a => a.Id == paymentId);
            decimal? newBalance = null;
            if (renovationPayment.DatePayment != null)
            {
                TransactionRepository transactionRepository = new TransactionRepository(Context);
                //Decrease the negativity of the transaction:
                newBalance = await transactionRepository
                    .IncreaseTransactionAmountAsync(renovationPayment.RenovationProject.TransactionId,
                    renovationPayment.Amount);
                Update(renovationPayment);

            }
            renovationPayment.IsDeleted = true;
            Update(renovationPayment);
            await SaveAsync();
            return newBalance;
        }
        /// <summary>
        /// 1. Remove the date from the payment
        /// 2. Decrease the negativity of the transaction
        /// 3. The milestone stays
        /// </summary>
        /// <param name="paymentId"></param>
        /// <returns></returns>
        public async Task<decimal> CancelPayment(int paymentId)
        {
            RenovationPayment renovationPayment = Context.RenovationPayments
                .Include(a => a.RenovationProject)
                .FirstOrDefault(a => a.Id == paymentId);
            renovationPayment.DatePayment = null;
            TransactionRepository transactionRepository = new TransactionRepository(Context);
            //Decrease the negativity of the transaction:
            decimal newBalance = await transactionRepository
                .IncreaseTransactionAmountAsync(renovationPayment.RenovationProject.TransactionId,
                renovationPayment.Amount);
            Update(renovationPayment);
            await SaveAsync();
            return newBalance;
        }

        public Task<List<RenovationProject>> GetAllRenovationProjectsAsync()
        {
            return Context.RenovationProjects.ToListAsync();
        }
        public Task<RenovationProject> GetRenovationProjectAsync(int projectId)
        {
            return  Context.RenovationProjects
                .Include(a=>a.Apartment)
                .FirstOrDefaultAsync(a=>a.Id==projectId);
        }
    }
}
