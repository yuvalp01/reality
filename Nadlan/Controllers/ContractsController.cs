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
        private readonly IMapper _mapper;

        public ContractsController(NadlanConext context, IMapper mapper)
        {
            _repositoryWraper = new RepositoryWrapper(context);
            _mapper = mapper;
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

        [HttpDelete]
        public async Task<IActionResult> DeleteMessage([FromBody] int id)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            await _repositoryWraper.ContractRepository.SoftDeleteAsync(id);
            return NoContent();
        }

    }
}
