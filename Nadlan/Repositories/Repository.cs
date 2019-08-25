using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Nadlan.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {

        protected NadlanConext Context { get; set; }
        public Repository(NadlanConext conext)
        {
            Context = conext;
        }

        public void Create(T entity)
        {
            Context.Set<T>().Add(entity);
        }
        public void Update(T entity)
        {
            Context.Set<T>().Update(entity);
        }
        public void Delete(T entity)
        {
            Context.Set<T>().Remove(entity);
        }

        public virtual Task<List<T>> GetAllAsync()
        {
            return Context.Set<T>().ToListAsync();
        }

        public async Task SaveAsync()
        {
            await Context.SaveChangesAsync();
        }

        public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression)
        {
            return Context.Set<T>().Where(expression);
        }
    }
}
