using System.Collections.Generic;
using System.Threading.Tasks;
using Nadlan.Models.Issues;

namespace Nadlan.Repositories.Issues
{
    public interface IIssueRepository
    {
        Task<List<Issue>> GetAllIssuesAsync(bool isOnlyOpen);
        Task<Issue> GetIssueById(int issueId);
        Task<List<IssueItem>> GetItemsByIssueId(int issueId);
    }
}