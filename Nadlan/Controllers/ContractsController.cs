using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nadlan.Models;
using Nadlan.Models.Issues;
using Nadlan.Repositories;
using System;
using System.Threading.Tasks;

namespace Nadlan.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ContractsController : ControllerBase
    {
        private RepositoryWrapper _repositoryWraper;

        public ContractsController(NadlanConext context)
        {
            _repositoryWraper = new RepositoryWrapper(context);
        }

        [HttpGet]
        public async Task<IActionResult> GetContracs()
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var contracts = await _repositoryWraper.ContractRepository
                 .GetAllAsync();
            if (contracts == null) return NotFound();

            return Ok(contracts);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetContractById([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var contract = await _repositoryWraper.ContractRepository
                 .GetByIdAsync(id);
            if (contract == null) return NotFound();

            return Ok(contract);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] Contract  contract)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            await _repositoryWraper.ContractRepository.CreateAsync(contract);
            return Ok();
            //throw new NotImplementedException();
        }


        [HttpPut]
        public async Task<IActionResult> Update([FromBody] Contract contract)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (contract == null) return NotFound();
            await _repositoryWraper.ContractRepository.UpdateAsync(contract);
            return NoContent();
        }
        [HttpPut("cancelAll")]
        public async Task<IActionResult> CancelAllConfirmations()
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            await _repositoryWraper.ContractRepository.CancelAllConfirmations();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMessage([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            await _repositoryWraper.ContractRepository.SoftDeleteAsync(id);
            return NoContent();
        }

    }
}
