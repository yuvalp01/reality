using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Nadlan.Models;
using Nadlan.Repositories;

namespace Nadlan.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonalTransactionsController : ControllerBase
    {
        private readonly NadlanConext _context;

        public PersonalTransactionsController(NadlanConext context)
        {
            _context = context;
        }

        // GET: api/PersonalTransactions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PersonalTransaction>>> GetPersonalTransactions()
        {
            return await _context.PersonalTransactions.ToListAsync();
        }

        [HttpGet("GetStakeholders")]
        public async Task<ActionResult<IEnumerable<Stakeholder>>> GetStakeholders()
        {
            return await _context.Stakeholders.ToListAsync();
        }

        // GET: api/PersonalTransactions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PersonalTransaction>> GetPersonalTransaction(int id)
        {
            var personalTransaction = await _context.PersonalTransactions.FindAsync(id);

            if (personalTransaction == null)
            {
                return NotFound();
            }

            return personalTransaction;
        }
        // GET: api/PersonalTransactions/5
        [HttpGet("GetPersonalTransactionByStakeholderId/{stakeholderId}")]
        public async Task<ActionResult<IEnumerable<PersonalTransaction>>> GetPersonalTransactionByStakeholderId(int stakeholderId)
        {
            var personalTransaction = await _context.PersonalTransactions.Where(a=>a.StakeholderId== stakeholderId).ToListAsync();

            if (personalTransaction == null)
            {
                return NotFound();
            }

            return personalTransaction;
        }

        // PUT: api/PersonalTransactions/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPersonalTransaction(int id, PersonalTransaction personalTransaction)
        {
            if (id != personalTransaction.Id)
            {
                return BadRequest();
            }

            _context.Entry(personalTransaction).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PersonalTransactionExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/PersonalTransactions
        [HttpPost]
        public async Task<ActionResult<PersonalTransaction>> PostPersonalTransaction(PersonalTransaction personalTransaction)
        {
            _context.PersonalTransactions.Add(personalTransaction);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPersonalTransaction", new { id = personalTransaction.Id }, personalTransaction);
        }

        // DELETE: api/PersonalTransactions/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<PersonalTransaction>> DeletePersonalTransaction(int id)
        {
            var personalTransaction = await _context.PersonalTransactions.FindAsync(id);
            if (personalTransaction == null)
            {
                return NotFound();
            }

            _context.PersonalTransactions.Remove(personalTransaction);
            await _context.SaveChangesAsync();

            return personalTransaction;
        }

        private bool PersonalTransactionExists(int id)
        {
            return _context.PersonalTransactions.Any(e => e.Id == id);
        }
    }
}
