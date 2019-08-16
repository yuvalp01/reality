using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nadlan.Repositories
{
    public interface IRepository<T>
    {
        Task<List<T>> GetAllAsync();
        Task Add(T entity);
        void Update(T entity);
        void Delete(T entity);
        Task<T> SaveAsync(T entity);
    }
}
