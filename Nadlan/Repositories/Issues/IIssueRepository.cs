using System.Collections.Generic;
using System.Threading.Tasks;
using Nadlan.Models.Issues;

namespace Nadlan.Repositories.Issues
{
    public interface IIssueRepository
    {
        Task<List<Issue>> GetAllIssuesAsync(bool isOnlyOpen);
        Task<Issue> GetIssueByIdAsync(int issueId);
        Task<List<Message>> GetMassagesByIssueIdAsync(int issueId);
        Task UpdateIssueAsync(Issue issue);
        Task UpdateMessageAsync(Message message);
        Task CreateIssueAsync(Issue issue);
        Task CreateMessageAsync(Message message);
        Task SoftDeleteIssueAsync(int id);
        Task SoftDeletMessageAsync(int id);

    }
}