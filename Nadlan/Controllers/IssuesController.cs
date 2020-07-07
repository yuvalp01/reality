using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Nadlan.Models.Issues;
using Nadlan.Repositories;
using System.Threading.Tasks;

namespace Nadlan.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class IssuesController : ControllerBase
    {
        //private readonly NadlanConext _context;
        private RepositoryWrapper _repositoryWraper;
        private readonly IMapper _mapper;
        //private NadlanConext _dbContext;
        //private static IssueRepository _issueRepository;
        //static IssuesController()
        //{
        //    //_dbContext = new InMemoryDbContextFactory().GetMockNadlanDbContext();
        //    _repositoryWraper = new RepositoryWrapper(_dbContext);
        //    //_dbContext.Issues.AddRange(MockIssues.GetAllIssues());
        //    //_dbContext.SaveChanges();
        //}
        //private void LoadTestData(NadlanConext context)
        //{
        //        context.Issues.AddRange(MockIssues.GetAllIssues());
        //        context.Messages.AddRange(MockIssues.GetAllMessages());
        //        context.SaveChanges();

        //}
        public IssuesController(NadlanConext context, IMapper mapper)
        {

            //LoadTestData(context);
            //TODO: bring back
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

        //[HttpGet("getMessages/{isOpenOnly}")]
        //public async Task<IActionResult> GetMessages([FromRoute] bool isOpenOnly)
        //{
        //    if (!ModelState.IsValid) return BadRequest(ModelState);

        //    var issues = await _repositoryWraper.IssueRepository
        //        .GetAllmessagesAsync(isOpenOnly);
        //    if (issues == null) return NotFound();

        //    return Ok(issues);
        //}

        [HttpGet("issue/{id}")]
        public async Task<IActionResult> GetIssueById([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var payment = await _repositoryWraper.IssueRepository
                 .GetIssueByIdAsync(id);
            if (payment == null) return NotFound();

            return Ok(payment);
        }
        [HttpGet("message/{id}")]
        public async Task<IActionResult> GetMessageById([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var message = await _repositoryWraper.IssueRepository
                 .GetMessageByIdAsync(id);
            if (message == null) return NotFound();

            return Ok(message);
        }

        [HttpGet("messages/{id}")]
        public async Task<IActionResult> GetMessagesByIssueId([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var messages = await _repositoryWraper.IssueRepository
                 .GetMassagesByIssueIdAsync(id);
            if (messages == null) return NotFound();

            return Ok(messages);
        }


        [HttpPut("issue")]
        public async Task<IActionResult> UpdateIssue([FromBody] Issue issue)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (issue == null) return NotFound();
            await _repositoryWraper.IssueRepository.UpdateIssueAsync(issue);
            return NoContent();
        }

        [HttpPut("message")]
        public async Task<IActionResult> UpdateMessage([FromBody] Message message)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (message == null) return NotFound();
            await _repositoryWraper.IssueRepository.CreateMessageAsync(message);
            return NoContent();
        }


        [HttpDelete("issue")]
        public async Task<IActionResult> DeleteIssue([FromBody] int issueId)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            await _repositoryWraper.IssueRepository.SoftDeleteIssueAsync(issueId);
            return NoContent();
        }
        [HttpDelete("message")]
        public async Task<IActionResult> DeleteMessage([FromBody] int issueItemId)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            await _repositoryWraper.IssueRepository.SoftDeletMessageAsync(issueItemId);
            return NoContent();
        }


        [HttpPost("issue")]
        public async Task<IActionResult> AddIssue([FromBody] Issue issue)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            await _repositoryWraper.IssueRepository.CreateIssueAsync(issue);
            return Ok();
        }

        [HttpPost("message")]
        public async Task<IActionResult> AddMessage([FromBody] Message message)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            await _repositoryWraper.IssueRepository.CreateMessageAsync(message);
            return Ok();
        }

    }
}


