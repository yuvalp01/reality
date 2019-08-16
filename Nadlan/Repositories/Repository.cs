using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nadlan.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly NadlanConext _context;
        public Repository(NadlanConext conext)
        {
            _context = conext;
        }
        public virtual async Task Add(T entity)
        {
           await _context.Set<T>().AddAsync(entity);
        }
        public void Update(T entity)
        {
            _context.Set<T>().Update(entity);
        }
        public void Delete(T entity)
        {
            _context.Set<T>().Remove(entity);
        }

        public async virtual Task<List<T>> GetAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public async Task<T> SaveAsync(T entity)
        {
            await _context.SaveChangesAsync();
            return entity;
        }
       

    }
}
