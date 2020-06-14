using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Nadlan.Models;
using Nadlan.Repositories;


namespace Nadlan.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AccountsController : ControllerBase
    {
        private readonly NadlanConext _context;

        public AccountsController(NadlanConext context)
        {
            _context = context;
        }

        // GET: api/Accounts
        [HttpGet]
        public IEnumerable<Account> GetAccount()
        {
            return _context.Accounts.OrderBy(a=>a.DisplayOrder);
        }

        // GET: api/Accounts/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAccount([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var account = await _context.Accounts.FindAsync(id);

            if (account == null)
            {
                return NotFound();
            }

            return Ok(account);
        }

        // PUT: api/Accounts/5
        [Authorize]
        [HttpPut("{id}")]
        protected async Task<IActionResult> PutAccount([FromRoute] int id, [FromBody] Apartment account)
        {
            throw new NotImplementedException("It is not possible to change account from the API");
            //if (!ModelState.IsValid)
            //{
            //    return BadRequest(ModelState);
            //}

            //if (id != account.Id)
            //{
            //    return BadRequest();
            //}

            //_context.Entry(account).State = EntityState.Modified;

            //try
            //{
            //    await _context.SaveChangesAsync();
            //}
            //catch (DbUpdateConcurrencyException)
            //{
            //    if (!AccountExists(id))
            //    {
            //        return NotFound();
            //    }
            //    else
            //    {
            //        throw;
            //    }
            //}

            //return NoContent();
        }

        // POST: api/Accounts
        [Authorize]
        [HttpPost]
        protected async Task<IActionResult> PostAccount([FromBody] Account account)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Accounts.Add(account);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (AccountExists(account.Id))
                {
                    return new StatusCodeResult(StatusCodes.Status409Conflict);
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetAccount", new { id = account.Id }, account);
        }

        // DELETE: api/Accounts/5
        [Authorize]
        [HttpDelete("{id}")]
        protected async Task<IActionResult> DeleteAccount([FromRoute] int id)
        {
            throw new NotImplementedException("It is not possible to delete account from the API");
            //if (!ModelState.IsValid)
            //{
            //    return BadRequest(ModelState);
            //}

            //var account = await _context.Accounts.FindAsync(id);
            //if (account == null)
            //{
            //    return NotFound();
            //}

            //_context.Accounts.Remove(account);
            //await _context.SaveChangesAsync();

            //return Ok(account);
        }

        private bool AccountExists(int id)
        {
            return _context.Accounts.Any(e => e.Id == id);
        }
    }
}