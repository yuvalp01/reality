using Microsoft.EntityFrameworkCore;
using Nadlan.Models.Renovation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nadlan.Repositories.Renovation
{
    public class RenovationLineRepository : Repository<RenovationLine>, IRenovationLineRepository
    {
        public RenovationLineRepository(NadlanConext nadlanConext) : base(nadlanConext)
        {
        }

        public Task<List<RenovationLine>> GetLinesAsync(int projectId)
        {
            var lines = Context.RenovationLines
                 .Where(a => a.RenovationProjectId == projectId)
                 .OrderBy(a=>a.Category)
                 .ToListAsync();

            return lines;
        }

        public Task<List<RenovationLine>> GetLinesByProjectIdAsync(int projectId)
        {

            var issues = Task.Run(() =>
            Context.RenovationLines
            //.Include(a => a.Messages)
            .OrderBy(a => a.Category)
            .Where(a=>a.RenovationProjectId == projectId)
            .ToList());
            return issues;
        }




        public async Task CreateAsync(RenovationLine renovationLine)
        {
            Create(renovationLine);
            await SaveAsync();
        }

        public Task DeleteAsync(RenovationLine renovationLine)
        {
            throw new NotImplementedException();
        }

        public Task<RenovationLine> GetByIdAsync(int id)
        {
            return FindByCondition(a => a.Id == id).FirstOrDefaultAsync();
        }

        public async Task UpdateAsync(RenovationLine renovationLine)
        {
            Update(renovationLine);
            await SaveAsync();
        }


    }
}
