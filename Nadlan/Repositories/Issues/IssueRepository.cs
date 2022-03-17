using Microsoft.EntityFrameworkCore;
using Nadlan.BusinessLogic;
using Nadlan.Models;
using Nadlan.Models.Enums;
using Nadlan.Models.Issues;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nadlan.Repositories.Issues
{
    public class IssueRepository : IIssueRepository
    {
        private IssueFilter _issueFilter;
        private NadlanConext _context;
        public IssueRepository(NadlanConext context)
        {
            _context = context;
            _issueFilter = new IssueFilter();

        }

        public async Task<List<Issue>> GetAllIssuesAsync(bool isOnlyOpen, int stakeholderId)
        {

            var queryableIssues = _context.Issues.Where(a => a.IsDeleted == false);
            if (isOnlyOpen)
            {
                queryableIssues = queryableIssues.Where(a => a.DateClose == null);
            }
            if (stakeholderId != (int)CreatedByEnum.Any)
            {
                queryableIssues = queryableIssues.Where(a => a.StakeholderId == stakeholderId);
            }

            queryableIssues = queryableIssues.OrderByDescending(a => a.DateOpen).ThenBy(a => a.Priority);

            var issues = await queryableIssues.ToListAsync();

            var messages = await
               _context.Messages
               .Where(a => a.TableName == "issues")
               .Where(a => a.IsDeleted == false)
               .ToListAsync();

            foreach (var issue in issues)
            {
                issue.Messages = messages.Where(a => a.ParentId == issue.Id).ToList();
            }

            return issues;
        }

        public Task<Issue> GetIssueByIdAsync(int id)
        {
            var issue = Task.Run(() =>
              _context.Issues.Find(id)
            );
            return issue;
        }


        public async Task CreateIssueAsync(Issue issue)
        {
            _context.Issues.Add(issue);
            await _context.SaveChangesAsync();
        }



        public async Task SoftDeleteIssueAsync(int id)
        {
            var issue = _context.Issues.Find(id);
            issue.IsDeleted = true;
            _context.Update(issue);
            await _context.SaveChangesAsync();

        }


        public async Task UpdateIssueAsync(Issue issue)
        {
            _context.Issues.Update(issue);
            await _context.SaveChangesAsync();
        }

    }
}