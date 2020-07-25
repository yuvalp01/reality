using System.Collections.Generic;
using System.Threading.Tasks;
using Nadlan.Models;

namespace Nadlan.Repositories.Messages
{
    public interface IMessageRepository
    {
 
        Task<ICollection<Message>> GetMassagesByIssueIdAsync(int issueId);
        Task UpdateMessageAsync(Message message);
        Task CreateMessageAsync(Message message);
        Task SoftDeletMessageAsync(int id);

    }
}