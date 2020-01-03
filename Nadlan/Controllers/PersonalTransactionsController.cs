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
            // return await _context.PersonalTransactions.ToListAsync();
            return await _repositoryWrapper.PersonalTransaction.GetAllAsync();
        }

        [HttpGet("GetStakeholders")]
        public async Task<ActionResult<IEnumerable<Stakeholder>>> GetStakeholders()
        {
            //return await _context.Stakeholders.ToListAsync();
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
            //var personalTransaction = await _context.PersonalTransactions.Where(a => a.StakeholderId == stakeholderId).ToListAsync();
            var personalTransactions = await _repositoryWrapper.PersonalTransaction.GetByStakeholderAsync(stakeholderId);
            var personalTransactionsDto = _mapper.Map<List<PersonalTransaction>, IEnumerable<PersonalTransactionDto>>(personalTransactions);

            //if (personalTransactions == null)
            //{
            //    return NotFound();
            //}

            return  personalTransactionsDto;
        }

        [HttpGet("GetByType/{stakeholderId}/{transactionTypeId}")]
        public async Task<ActionResult<IEnumerable<PersonalTransaction>>> GetPersonalTransactionByType(int stakeholderId, int transactionTypeId)
        {
            //var personalTransaction = await _context.PersonalTransactions.Where(a => a.StakeholderId == stakeholderId).ToListAsync();
            var personalTransactions = _repositoryWrapper.PersonalTransaction.GetByStakeholderAsync(stakeholderId, transactionTypeId);

            if (personalTransactions == null)
            {
                return NotFound();
            }

            return await personalTransactions;
        }

        [HttpGet("GetAllDistributions/{stakeholderId}")]
        public async Task<ActionResult<IEnumerable<PersonalTransaction>>> GetAllDistributions(int stakeholderId)
        {
            //var personalTransaction = await _context.PersonalTransactions.Where(a => a.StakeholderId == stakeholderId).ToListAsync();
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
        public async Task<ActionResult<PersonalTransaction>> PostPersonalTransaction(PersonalTransaction personalTransaction)
        {
            await _repositoryWrapper.PersonalTransaction.CreateTransactionAsync(personalTransaction);
            //_context.PersonalTransactions.Add(personalTransaction);
            //await _context.SaveChangesAsync();

            return CreatedAtAction("GetPersonalTransaction", new { id = personalTransaction.Id }, personalTransaction);
        }



        // DELETE: api/PersonalTransactions/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<PersonalTransaction>> DeletePersonalTransaction(int id)
        {
            await _repositoryWrapper.PersonalTransaction.DeleteTransactionAsync(new PersonalTransaction { Id = id });

            //var personalTransaction = await _context.PersonalTransactions.FindAsync(id);
            //if (personalTransaction == null)
            //{
            //    return NotFound();
            //}

            //_context.PersonalTransactions.Remove(personalTransaction);
            //await _context.SaveChangesAsync();
            return CreatedAtAction("DeletePersonalTransaction", new { id });

        }

        //private bool PersonalTransactionExists(int id)
        //{
        //    return _context.PersonalTransactions.Any(e => e.Id == id);
        //}
    }
}
