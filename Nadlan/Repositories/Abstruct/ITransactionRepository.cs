using Nadlan.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Nadlan.ViewModels;

namespace Nadlan.Repositories
{
    public interface ITransactionRepository
    {
        Task<List<Transaction>> GetAllAsync();
        Task<Transaction> GetByIdAsync(int id);
        Task<List<Transaction>> GetByAcountAsync(int apartmentId, int accountId, bool isPurchaseCost, int year);
        Task CreateTransactionAsync(Transaction transaction);
        Task UpdateTransactionAsync(Transaction dbTransaction, Transaction transaction);
        Task SoftDeleteTransactionAsync(int transactionId);


        Task<RentRelatedTransactionsResponse> CreateRentTransactionsAsync(Transaction rent);
        Task<RentRelatedTransactionsResponse> UpdateRentTransactionsAsync(Transaction transaction);
        Task SoftDeleteRentTransactionsAsync(int transactionId);
    }
}
