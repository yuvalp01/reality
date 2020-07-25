﻿using Microsoft.EntityFrameworkCore;
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


        public Task<List<Issue>> GetAllIssuesAsync(bool isOnlyOpen)
        {
            Func<Issue, bool> filter = isOnlyOpen ?
                _issueFilter.GetOpenIssues() :
                _issueFilter.GetAllIssues();

            var issues = Task.Run(() =>
            _context.Issues
            .Include(a=>a.Messages)
            .OrderBy(a=>a.Priority).ThenBy(a=>a.DateOpen)
           .Where(filter)
           .ToList());
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