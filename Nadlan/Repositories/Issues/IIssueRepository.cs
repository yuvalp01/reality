using System.Collections.Generic;
using System.Threading.Tasks;
using Nadlan.Models.Issues;

namespace Nadlan.Repositories.Issues
{
    public interface IIssueRepository
    {
        Task<List<Issue>> GetAllIssuesAsync(bool isOnlyOpen, int stakeholderId);
        Task<Issue> GetIssueByIdAsync(int issueId);
        Task UpdateIssueAsync(Issue issue);
        Task CreateIssueAsync(Issue issue);
        Task SoftDeleteIssueAsync(int id);

    }
}