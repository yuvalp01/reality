using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nadlan.Models;
using Nadlan.Models.Issues;
using Nadlan.Repositories;
using System.Threading.Tasks;

namespace Nadlan.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class IssuesController : ControllerBase
    {
        private RepositoryWrapper _repositoryWraper;
        private readonly IMapper _mapper;

        public IssuesController(NadlanConext context, IMapper mapper)
        {
            _repositoryWraper = new RepositoryWrapper(context);
            _mapper = mapper;
        }


        [HttpGet("{isOpenOnly}")]
        public async Task<IActionResult> GetIssues([FromRoute] bool isOpenOnly)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var issues = await _repositoryWraper.IssueRepository
                .GetAllIssuesAsync(isOpenOnly);
            if (issues == null) return NotFound();
            //var json = JsonConvert.SerializeObject(issues);
            return Ok(issues);
        }


        [HttpGet("issue/{id}")]
        public async Task<IActionResult> GetIssueById([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var payment = await _repositoryWraper.IssueRepository
                 .GetIssueByIdAsync(id);
            if (payment == null) return NotFound();

            return Ok(payment);
        }


        [HttpPut]
        public async Task<IActionResult> UpdateIssue([FromBody] Issue issue)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (issue == null) return NotFound();
            await _repositoryWraper.IssueRepository.UpdateIssueAsync(issue);
            return NoContent();
        }


        [HttpDelete("{issueId}")]
        public async Task<IActionResult> DeleteIssue([FromRoute] int issueId)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            await _repositoryWraper.IssueRepository.SoftDeleteIssueAsync(issueId);
            return NoContent();
        }

        [HttpPost]
        public async Task<IActionResult> AddIssue([FromBody] Issue issue)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            await _repositoryWraper.IssueRepository.CreateIssueAsync(issue);
            return Ok();
        }


    }
}


