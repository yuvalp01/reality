using Microsoft.EntityFrameworkCore;
using Nadlan.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nadlan.Repositories.Messages
{
    public class MessagesRepository : IMessageRepository
    {
        //private IssueFilter _issueFilter;
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

        public Task<List<Message>> GetMassagesByParentIdAsync(string tableName, int parentId)
        {
            var messages = _context.Messages
                .Where(a => a.IsDeleted == false)
                .Where(a => a.ParentId == parentId)
                .Where(a => a.TableName == tableName)
                .ToListAsync();
            return messages;
        }



        public async Task CreateMessageAsync(Message message)
        {
            _context.Messages.Add(message);
            await _context.SaveChangesAsync();

        }

        public async Task MarkAsRead(Message message)
        {
            var messageToUpdate = await _context.Messages.FindAsync(message.Id);
            messageToUpdate.IsRead = true;
                 //.Where(a => a.ParentId == message.ParentId)
                 //.Where(a => a.TableName == message.TableName)
                 //.Where(a => a.UserName.ToLower() == userName.ToLower())
                 //.ForEachAsync(a => a.IsRead = true);

            //_context.Messages.Add(message);
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

