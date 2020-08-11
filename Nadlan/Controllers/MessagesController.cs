using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Nadlan.Models;
using Nadlan.Models.Issues;
using Nadlan.Repositories;
using System.Threading.Tasks;

namespace Nadlan.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class MessagesController : ControllerBase
    {
        private RepositoryWrapper _repositoryWraper;
        private readonly IMapper _mapper;

        public MessagesController(NadlanConext context, IMapper mapper)
        {

            _repositoryWraper = new RepositoryWrapper(context);
            _mapper = mapper;


        }

        [HttpGet("message/{id}")]
        public async Task<IActionResult> GetMessageById([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var message = await _repositoryWraper.MessagesRepository
                 .GetMessageByIdAsync(id);
            if (message == null) return NotFound();

            return Ok(message);
        }

        [HttpGet("messages/{id}")]
        public async Task<IActionResult> GetMessagesByIssueId([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var messages = await _repositoryWraper.MessagesRepository
                 .GetMassagesByIssueIdAsync(id);
            if (messages == null) return NotFound();

            return Ok(messages);
        }


        [HttpPut("message")]
        public async Task<IActionResult> UpdateMessage([FromBody] Message message)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (message == null) return NotFound();
            await _repositoryWraper.MessagesRepository.CreateMessageAsync(message);
            return NoContent();
        }

        [HttpDelete("message")]
        public async Task<IActionResult> DeleteMessage([FromBody] int issueItemId)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            await _repositoryWraper.MessagesRepository.SoftDeletMessageAsync(issueItemId);
            return NoContent();
        }


        [HttpPost("message")]
        public async Task<IActionResult> AddMessage([FromBody] Message message)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            await _repositoryWraper.MessagesRepository.CreateMessageAsync(message);
            return Ok();
        }

    }
}
