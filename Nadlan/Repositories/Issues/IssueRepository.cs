using Microsoft.EntityFrameworkCore;
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


        public Task<List<Message>> GetAllmessagesAsync(bool isOnlyOpen)
        {
            Func<Message, bool> filter = isOnlyOpen ?
                _issueFilter.GetItemsOfOpenIssues() :
                _issueFilter.GetItemsOfIssues();

            var messages = Task.Run(() =>
            _context.Messages
           .Where(filter)
           .ToList());
            return messages;
        }


        public Task<List<Issue>> GetAllIssuesAsync(bool isOnlyOpen)
        {
            Func<Issue, bool> filter = isOnlyOpen ?
                _issueFilter.GetOpenIssues() :
                _issueFilter.GetAllIssues();

            var issues = Task.Run(() =>
            _context.Issues
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

        public Task<Message> GetMessageByIdAsync(int id)
        {
            var messages = Task.Run(() =>
              _context.Messages.Find(id)
            );
            return messages;
        }

        public Task<List<Message>> GetMassagesByIssueIdAsync(int issueId)
        {
            var filter = _issueFilter.GetAllMessages();
            var issues = Task.Run(() =>
             _context.Messages
             .Include(a => a.Issue)
             .Where(filter)
             .Where(a => a.IssueId == issueId)
             .ToList()
            );
            return issues;
        }

        public async Task CreateIssueAsync(Issue issue)
        {
            _context.Issues.Add(issue);
            await _context.SaveChangesAsync();
        }

        public async Task CreateMessageAsync(Message message)
        {
            _context.Messages.Add(message);
            await _context.SaveChangesAsync();
        }


        public async Task SoftDeleteIssueAsync(int id)
        {
            var issue = _context.Issues.Find(id);
            issue.IsDeleted = true;
            _context.Update(issue);
            await _context.SaveChangesAsync();

        }


        public async Task SoftDeletMessageAsync(int id)
        {
            var message = _context.Messages.Find(id);
            message.IsDeleted = true;
            _context.Update(message);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateIssueAsync(Issue issue)
        {
            _context.Issues.Update(issue);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateMessageAsync(Message message)
        {
            _context.Messages.Update(message);
            await _context.SaveChangesAsync();
        }


    }
}