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
    public class InsuranceController : ControllerBase
    {
        private readonly NadlanConext _context;

        public InsuranceController(NadlanConext context)
        {
            _context = context;
        }

        [HttpGet]
        public IEnumerable<Insurance> GetInsurances()
        {
            return _context.Insurances.OrderBy(a => a.Id);
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


        [Authorize]
        [HttpPut("{id}")]
        protected async Task<IActionResult> PutInsurance([FromRoute] int id, [FromBody] Insurance insurance)
        {
            throw new NotImplementedException("It is not possible to change insurance from the API");
        }

        [Authorize]
        [HttpPost]
        protected async Task<IActionResult> PostInsurances([FromBody] Insurance insurance)
        {
            throw new NotImplementedException("It is not possible to add a new insurance from the API");

        }

        [Authorize]
        [HttpDelete("{id}")]
        protected async Task<IActionResult> DeleteInsurance([FromRoute] int id)
        {
            throw new NotImplementedException("It is not possible to delete insurance from the API");
        }

        //private bool InsuranceExists(int id)
        //{
        //    return _context.Insurances.Any(e => e.Id == id);
        //}
    }
}