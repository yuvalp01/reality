using Nadlan.Models.Issues;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nadlan.Repositories.Issues
{
    interface IIssueRepository
    {
        Task<List<Issue>> GetAllIssues(bool isOnlyOpen);
        Task<List<Issue>> GetOpenIssues(bool isOnlyOpen);
        Task<List<IssueItem>> GetItemsByIssueId(int issueId);

        Task<Issue> CreateIssue(Issue issue);
        Task<Issue> UpdateIssue(Issue issue);

    }
}
