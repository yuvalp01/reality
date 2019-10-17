using Nadlan.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Nadlan.Repositories
{
    public interface IPersonalTransactionRepository
    {
        Task<List<PersonalTransaction>> GetAllAsync();
        Task<PersonalTransaction> GetByIdAsync(int id);
        Task<List<PersonalTransaction>> GetByStakeholderAsync(int stakeholderId);
        Task CreateTransactionAsync(PersonalTransaction transaction);
        //Task UpdateTransactionAsync(PersonalTransaction dbTransaction, PersonalTransaction transaction);
        Task UpdateTransactionAsync(PersonalTransaction transaction);
        Task DeleteTransactionAsync(PersonalTransaction transaction);
    }
}
