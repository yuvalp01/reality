using Nadlan.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Nadlan.Repositories.Messages
{
    public interface IContractRepository
    {

        Task<List<Contract>> GetAllAsync();
        Task<Contract> GetByIdAsync(int id);
        Task CreateAsync(Contract contract);
        Task UpdateAsync(Contract contract);
        Task SoftDeleteAsync(int id);

    }
}