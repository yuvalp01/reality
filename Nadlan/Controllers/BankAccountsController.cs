using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Nadlan.Models;
using Nadlan.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Nadlan.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BankAccountsController : ControllerBase
    {
        private readonly NadlanConext _context;

        public BankAccountsController(NadlanConext context)
        {
            _context = context;
        }

        // GET: api/Accounts
        [HttpGet]
        public IEnumerable<BankAccount> GetAccounts()
        {
            return _context.BankAccounts.OrderBy(a => a.Id);
        }

        // GET: api/Accounts/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAccount([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var account = await _context.BankAccounts.FindAsync(id);

            if (account == null)
            {
                return NotFound();
            }

            return Ok(account);
        }

        // PUT: api/Accounts/5
        [Authorize]
        [HttpPut("{id}")]
        protected async Task<IActionResult> PutAccount([FromRoute] int id, [FromBody] BankAccount bankAccount)
        {
            throw new NotImplementedException("It is not possible to change bank account from the API");
        }

        // POST: api/Accounts
        [Authorize]
        [HttpPost]
        protected async Task<IActionResult> PostAccount([FromBody] Account account)
        {
            throw new NotImplementedException("It is not possible to add a new bank account from the API");

        }

        // DELETE: api/Accounts/5
        [Authorize]
        [HttpDelete("{id}")]
        protected async Task<IActionResult> DeleteAccount([FromRoute] int id)
        {
            throw new NotImplementedException("It is not possible to delete bank account from the API");
        }

        private bool BankAccountExists(int id)
        {
            return _context.BankAccounts.Any(e => e.Id == id);
        }
    }
}