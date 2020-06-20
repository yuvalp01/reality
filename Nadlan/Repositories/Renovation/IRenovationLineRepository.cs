using Nadlan.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Nadlan.Models.Renovation;

namespace Nadlan.Repositories
{
    public interface IRenovationLineRepository
    {
        Task<List<RenovationProject>> GetAllRenovationProjectsAsync();
        Task<List<RenovationLine>> GetLinesAsync(int projectId);
        //Task<List<RenovationPayment>> GetPaymentsAsync(int projectId);
        //Task<RenovationPayment> GetPaymentByIdAsync(int id);

        Task CreateAsync(RenovationLine renovationLine);
        Task DeleteAsync(RenovationLine renovationLine);
        Task<RenovationLine> GetByIdAsync(int id);
        Task UpdateAsync(RenovationLine renovationLine);


    }
}
