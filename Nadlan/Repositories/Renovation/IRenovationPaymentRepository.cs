using System.Collections.Generic;
using System.Threading.Tasks;
using Nadlan.Models.Renovation;

namespace Nadlan.Repositories.Renovation
{
    public interface IRenovationPaymentRepository
    {
        Task CreateAsync(RenovationPayment renovationPayment);
        Task DeleteAsync(RenovationLine renovationLine);
        Task<RenovationPayment> GetByIdAsync(int id);
        Task<RenovationPayment> GetPaymentByIdAsync(int id);
        Task<List<RenovationPayment>> GetPaymentsAsync(int projectId);
        Task UpdateAsync(RenovationPayment renovationPayment);
    }
}