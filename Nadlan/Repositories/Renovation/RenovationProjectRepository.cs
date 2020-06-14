using Microsoft.EntityFrameworkCore;
using Nadlan.Models;
using Nadlan.Models.Renovation;
using Nadlan.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Nadlan.Repositories.Renovation
{
    public class RenovationProjectRepository : Repository<RenovationProject>
    {
        public RenovationProjectRepository(NadlanConext context) : base(context)
        {
        }


        public override async Task<List<RenovationProject>> GetAllAsync()
        {
            throw new NotImplementedException();
            //return await Context.RenovationProjects
            //    .OrderByDescending(a => a.Id)
            //    //.Include(a => a.Stakeholder)
            //    //.Where(a=>!a.IsDeleted)
            //    .ToListAsync();
        }




       public async Task CreateAsync(RenovationProject  renovationProject)
        {
            Create(renovationProject);
            await SaveAsync();
        }
        public async Task UpdateAsync(RenovationProject renovationProject)
        {
            Update(renovationProject);
            await SaveAsync();
        }
        //public async Task UpdateTransactionAsync(PersonalTransaction dbTransaction, PersonalTransaction transaction)
        //{
        //    Update(transaction);
        //    await SaveAsync();
        //}
        [Obsolete]
        public async Task DeleteTransactionAsync(RenovationProject renovationProject)
        {
            throw new NotImplementedException("delete only direcly in db with boolean flag");
            //Delete(transaction);
            //await SaveAsync();
        }

        public Task<RenovationProject> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
           // return Context.RenovationProjects.FindAsync(id);
        }

    }


}




