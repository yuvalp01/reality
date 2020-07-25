using Nadlan.BusinessLogic;
using Nadlan.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nadlan.Repositories.Messages
{
    public class MessagesRepository : IMessageRepository
    {
        private IssueFilter _issueFilter;
        private NadlanConext _context;
        public MessagesRepository(NadlanConext context)
        {
            _context = context;
            //_issueFilter = new IssueFilter();

        }

        public Task<Message> GetMessageByIdAsync(int id)
        {
            var messages = Task.Run(() =>
              _context.Messages.Find(id)
            );
            return messages;
        }

        public Task<ICollection<Message>> GetMassagesByIssueIdAsync(int issueId)
        {
            var filter = _issueFilter.GetAllIssues();
            var issues = Task.Run(() =>
             _context.Issues
             //.Where(filter)
             //.Where(a => a.IssueId == issueId)             
             .Where(a => a.Id == issueId)   
             //.Select(a => a.Messages.Where(m=>m.TableName=="issues"))
             .Select(a => a.Messages)
             .FirstOrDefault());

            return issues;
        }

        public Task<ICollection<Message>> GetMassagesByRenovationLineIdAsync(int renovationLineId)
        {
            var messages = Task.Run(() =>
             _context.RenovationLines          
             .Where(a => a.Id == renovationLineId)
             .Select(a => a.Messages)
             .FirstOrDefault());
            return messages;
        }




        public async Task CreateMessageAsync(Message message)
        {
            _context.Messages.Add(message);
            await _context.SaveChangesAsync();
        }


        public async Task SoftDeletMessageAsync(int id)
        {
            var message = _context.Messages.Find(id);
            message.IsDeleted = true;
            _context.Update(message);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateMessageAsync(Message message)
        {
            _context.Messages.Update(message);
            await _context.SaveChangesAsync();
        }

    }
}