using System.Collections.Generic;
using System.Threading.Tasks;
using Nadlan.Models;

namespace Nadlan.Repositories.Messages
{
    public interface IMessageRepository
    {
 
        Task<List<Message>> GetMassagesByParentIdAsync(string tableName, int parentId);
        Task UpdateMessageAsync(Message message);
        Task CreateMessageAsync(Message message);
        Task SoftDeletMessageAsync(int id);

    }
}