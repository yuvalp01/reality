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
    public class ApartmentsController : ControllerBase
    {
        private readonly NadlanConext _context;

        public ApartmentsController(NadlanConext context)
        {
            _context = context;
        }

        // GET: api/Apartments
        [HttpGet]
        public IEnumerable<Apartment> GetApartment()
        {
            return _context.Apartments;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetApartment([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var apartment = await _context.Apartments.FindAsync(id);

            if (apartment == null)
            {
                return NotFound();
            }

            return Ok(apartment);
        }




        // PUT: api/Apartments/5
        [Authorize]
        [HttpPut("{id}")]
        protected async Task<IActionResult> PutApartment([FromRoute] int id, [FromBody] Apartment apartment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != apartment.Id)
            {
                return BadRequest();
            }

            _context.Entry(apartment).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ApartmentExists(id))
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

        // POST: api/Apartments
        [Authorize]
        [HttpPost]
        protected async Task<IActionResult> PostApartment([FromBody] Apartment apartment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Add(apartment);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (ApartmentExists(apartment.Id))
                {
                    return new StatusCodeResult(StatusCodes.Status409Conflict);
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetApartment", new { id = apartment.Id }, apartment);
        }

        // DELETE: api/Apartments/5
        [Authorize]
        [HttpDelete("{id}")]
        protected async Task<IActionResult> DeleteApartment([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var apartment = await _context.Apartments.FindAsync(id);
            if (apartment == null)
            {
                return NotFound();
            }

            _context.Remove(apartment);
            await _context.SaveChangesAsync();

            return Ok(apartment);
        }

        private bool ApartmentExists(int id)
        {
            return _context.Apartments.Any(e => e.Id == id);
        }
    }
}