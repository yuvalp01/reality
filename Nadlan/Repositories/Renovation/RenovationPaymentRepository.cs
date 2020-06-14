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
                .Where(a => a.IsDeleted==false)
                .Where(a => a.RenovationProjectId == projectId)
                .ToListAsync();
        }
        public Task<RenovationPayment> GetPaymentByIdAsync(int id)
        {
            return Context.RenovationPayments
                .Where(a => a.IsDeleted==false)
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

        public async Task<decimal> MakePaymentAsync(RenovationPayment renovationPayment)
        {
            Update(renovationPayment);
            TransactionRepository transactionRepository = new TransactionRepository(Context);
            decimal newBalance =   await transactionRepository
                .IncreaseTransactionAmountAsync(renovationPayment.RenovationProject.TransactionId,
                renovationPayment.Amount);
            await SaveAsync();
            return newBalance;
        }


        public async Task ConfirmAsync(int paymentId)
        {
            RenovationPayment renovationPayment = Context.RenovationPayments
                .FirstOrDefault(a => a.Id == paymentId);
            renovationPayment.IsConfirmed = true;
            Update(renovationPayment);
            await SaveAsync();
        }
        public async Task SoftDeleteAsync(int paymentId)
        {
            RenovationPayment renovationPayment = Context.RenovationPayments
                .FirstOrDefault(a => a.Id == paymentId);
            renovationPayment.IsDeleted = true;
            Update(renovationPayment);
            await SaveAsync();
        }

    }
}
