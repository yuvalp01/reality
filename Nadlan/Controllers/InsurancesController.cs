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
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class InsurancesController : ControllerBase
    {
        private readonly NadlanConext _context;

        public InsurancesController(NadlanConext context)
        {
            _context = context;
        }

        [HttpGet]
        public IEnumerable<Insurance> GetInsurances()
        {
            return _context.Insurances.Include(a => a.Apartment).OrderBy(a => a.Id);
        }

        // GET: api/Accounts/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetInsurance([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var account = await _context.Insurances.FindAsync(id);

            if (account == null)
            {
                return NotFound();
            }

            return Ok(account);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] Insurance insurance)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Add(insurance);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (InsuranceExists(insurance.Id))
                {
                    return new StatusCodeResult(StatusCodes.Status409Conflict);
                }
                else
                {
                    throw;
                }
            }
            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] Insurance insurance)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != insurance.Id)
            {
                return BadRequest();
            }

            _context.Entry(insurance).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InsuranceExists(id))
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
        private bool InsuranceExists(int id)
        {
            return _context.Insurances.Any(e => e.Id == id);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteInsurance([FromRoute] int id)
        {

            try
            {
                var insurance = _context.Insurances.Find(id);
                if (insurance != null)
                {
                    _context.Insurances.Remove(insurance);
                    await _context.SaveChangesAsync();                
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                return new StatusCodeResult(500);
            }
        }
    }
}