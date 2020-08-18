using Nadlan.BusinessLogic;
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

        public async  Task<List<Issue>> GetAllIssuesAsync(bool isOnlyOpen)
        {
            Func<Issue, bool> filter = isOnlyOpen ?
                _issueFilter.GetOpenIssues() :
                _issueFilter.GetAllIssues();

            var issues = await Task.Run(() =>
               _context.Issues
               .OrderByDescending(a => a.DateOpen).ThenBy(a => a.Priority)
               .Where(filter)
               .ToList());

            var messages = await Task.Run(() =>
               _context.Messages
               .Where(a => a.TableName == "issues")
               .Where(a => a.IsDeleted == false)
               .ToList());

              foreach (var issue in issues)
              {
                  issue.Messages = messages.Where(a => a.ParentId == issue.Id).ToList();
              }

            return  issues;
        }


        public List<Issue> GetAllIssuesAsync()
        {
            var messages = _context.Messages
                .Where(a => a.TableName == "issues")
                .Where(a => a.IsDeleted == false);
            var issues = _context.Issues
                .Where(a => a.IsDeleted == false);
            foreach (var issue in issues)
            {
                issue.Messages = messages.Where(a => a.ParentId == issue.Id).ToList();
            }
            //var messages = from i in _context.Issues
            //             join m in _context.Messages
            //             on i.Id equals m.ParentId
            //             select  new {Issue=i,Messages=m};
            //var xxx = messages.ToList();

            //List<Issue> issues = new List<Issue>();
            //foreach (var item in xxx)
            //{
            //    Issue issue = new Issue();
            //    issue = item.Issue;
            //    issue.Messages = xxx.FindAll(a=>a.Messages.ParentId==item.Issue.Id)
            //}
            return issues.ToList();

            //var issues = _context.Issues
            //    .Where(a => a.IsDeleted == false)
            //    .OrderByDescending(a => a.DateOpen)
            //    .ThenBy(a => a.Priority).ToListAsync();             
            //return issues;
        }


        //public Task<List<Issue>> GetAllIssuesAsync(bool isOnlyOpen)
        //{
        //    Func<Issue, bool> filter = isOnlyOpen ?
        //        _issueFilter.GetOpenIssues() :
        //        _issueFilter.GetAllIssues();

        //    var issues = Task.Run(() =>
        //    _context.Issues
        //    .Include(a => a.Messages)
        //    .OrderByDescending(a => a.DateOpen).ThenBy(a => a.Priority)
        //   .Where(filter)
        //   .ToList());
        //    return issues;
        //}


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