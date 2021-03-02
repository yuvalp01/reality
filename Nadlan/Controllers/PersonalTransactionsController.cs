using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Nadlan.Models;
using Nadlan.Repositories;
using Nadlan.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nadlan.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PersonalTransactionsController : ControllerBase
    {
        //private readonly NadlanConext _context;
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IMapper _mapper;
        public PersonalTransactionsController(NadlanConext context, IRepositoryWrapper repositoryWrapper, IMapper mapper)
        {
            _mapper = mapper;
            _repositoryWrapper = repositoryWrapper;
        }

        // GET: api/PersonalTransactions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PersonalTransaction>>> GetPersonalTransactions()
        {
            return await _repositoryWrapper.PersonalTransaction.GetAllAsync();
        }

        [HttpGet("GetStakeholders")]
        public async Task<ActionResult<IEnumerable<Stakeholder>>> GetStakeholders()
        {
            return await _repositoryWrapper.PersonalTransaction.GetStakeholdersAsync();
        }

        // GET: api/PersonalTransactions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PersonalTransaction>> GetPersonalTransaction(int id)
        {
            var personalTransaction = await _repositoryWrapper.PersonalTransaction.GetByIdAsync(id);

            if (personalTransaction == null)
            {
                return NotFound();
            }

            return personalTransaction;
        }

        [HttpGet("GetByStakeholderId/{stakeholderId}")]
        public async Task<IEnumerable<PersonalTransactionDto>> GetPersonalTransactionByStakeholderId(int stakeholderId)
        {
            var personalTransactions = await _repositoryWrapper.PersonalTransaction.GetByStakeholderAsync(stakeholderId);
            var personalTransactionsDto = _mapper.Map<List<PersonalTransaction>, IEnumerable<PersonalTransactionDto>>(personalTransactions);

            //if (personalTransactions == null)
            //{
            //    return NotFound();
            //}

            return  personalTransactionsDto;
        }


        [HttpGet("GetAllDistributions/{stakeholderId}")]
        public async Task<ActionResult<IEnumerable<PersonalTransaction>>> GetAllDistributions(int stakeholderId)
        {
            var personalTransactions = _repositoryWrapper.PersonalTransaction.GetAllDistributions(stakeholderId);

            if (personalTransactions == null)
            {
                return NotFound();
            }

            return await personalTransactions;
        }


        // PUT: api/PersonalTransactions/5
        [HttpPut()]
        public async Task<IActionResult> PutPersonalTransaction(PersonalTransaction personalTransaction)
        {

            await _repositoryWrapper.PersonalTransaction.UpdateTransactionAsync(personalTransaction);
            return NoContent();
        }

        // POST: api/PersonalTransactions
        [HttpPost]
        public async Task<ActionResult<PersonalTransaction>> PostPersonalTransaction(PersonalTransaction personalTransaction, [FromQuery] Filter filter)
        {
            await _repositoryWrapper.PersonalTransaction
                .CreateTransactionAsync(personalTransaction);
            return CreatedAtAction("GetPersonalTransaction", new { id = personalTransaction.Id }, personalTransaction);
        }


        [HttpPost("withFilter")]
        public ActionResult<object> PostWithFilter(PersonalTransWithFilter transWithFilter)
        {

            var affected =  _repositoryWrapper.PersonalTransaction
                .CreatePersonalTransAndUpdateTransactions(transWithFilter);
           // return CreatedAtAction("GetPersonalTransaction", new { id = transWithFilter.PersonalTransaction, affected });
            return Ok(affected); // CreatedAtAction("GetPersonalTransaction", new { id = transWithFilter.PersonalTransaction.Id, affected =  9 });


        }





        // DELETE: api/PersonalTransactions/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeletePersonalTransaction(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            await _repositoryWrapper.PersonalTransaction.SoftDeleteTransactionAsync(id);
            return Ok();

        }

    }
}
