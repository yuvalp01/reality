using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Nadlan.Models.Renovation;
using Nadlan.Repositories.Renovation;
using Nadlan.Repositories;
using Nadlan.ViewModels.Renovation;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nadlan.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class RenovationController : ControllerBase
    {
        //private readonly NadlanConext _context;
        private readonly RenovationRepositoryWrapper _repositoryWraper;
        private readonly IMapper _mapper;

        public RenovationController(NadlanConext context, IMapper mapper)
        {
            _repositoryWraper = new RenovationRepositoryWrapper(context);
            _mapper = mapper;
        }


        [HttpGet("payments/{renovationProjectId}")]
        public async Task<IActionResult> GetRenovationPaymentsByProjectId([FromRoute] int renovationProjectId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            //var mockRepo = new RenovationLineRepositoryMock();
            var renovationLines = await _repositoryWraper.RenovationPaymentRepository
                .GetPaymentsAsync(renovationProjectId);
            if (renovationLines == null)
            {
                return NotFound();
            }

            return Ok(renovationLines);
        }
        [HttpGet("payment/{id}")]
        public async Task<IActionResult> GetRenovationPaymentById([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            //var mockRepo = new RenovationLineRepositoryMock();
            var payment = await _repositoryWraper.RenovationPaymentRepository
                 .GetPaymentByIdAsync(id);
            if (payment == null)
            {
                return NotFound();
            }

            return Ok(payment);
        }

        [HttpPut("payment")]
        public async Task<IActionResult> UpdatePayment([FromBody] RenovationPayment payment)
        {


            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            //var mockRepo = new RenovationLineRepositoryMock();
            await _repositoryWraper.RenovationPaymentRepository.UpdateAsync(payment);
            //var payment = await mockRepo
            //.GetPaymentByIdAsync(id);
            if (payment == null)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpPut("makePayment")]
        public async Task<IActionResult> MakePayment([FromBody] RenovationPayment payment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            decimal newBalance =  await _repositoryWraper.RenovationPaymentRepository.MakePaymentAsync(payment);

            return Ok(newBalance);
        }


        [HttpPut("confirmPayment")]
        public async Task<IActionResult> ConfirmPayment([FromBody] int paymentId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            await _repositoryWraper.RenovationPaymentRepository.ConfirmAsync(paymentId);

            return NoContent();
        }

        [HttpPut("deletePayment")]
        public async Task<IActionResult> DeletePayment([FromBody] int paymentId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            await _repositoryWraper.RenovationPaymentRepository.SoftDeleteAsync(paymentId);

            return NoContent();
        }

        [HttpPost("payment")]
        public async Task<IActionResult> AddPayment([FromBody] RenovationPayment payment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            //var mockRepo = new RenovationLineRepositoryMock();
            await _repositoryWraper.RenovationPaymentRepository.CreateAsync(payment);

            //var payment = await mockRepo
            //.GetPaymentByIdAsync(id);
            if (payment == null)
            {
                return NotFound();
            }
            return CreatedAtAction("PaymentUpdated", new { id = payment.Id },payment);

        }

        [HttpGet("projects")]
        public async Task<IActionResult> GetRenovationProjects()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var mockRepo = new RenovationLineRepositoryMock();
            var renovationLines = await mockRepo.GetAllRenovationProjectsAsync();
            if (renovationLines == null)
            {
                return NotFound();
            }

            return Ok(renovationLines);
        }

        [HttpGet("lines/{renovationProjectId}")]
        public async Task<IActionResult> GetRenovationLines([FromRoute] int renovationProjectId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var mockRepo = new RenovationLineRepositoryMock();
            var renovationLines = await mockRepo
                 .GetLinesAsync(renovationProjectId);

            if (renovationLines == null)
            {
                return NotFound();
            }

            return Ok(renovationLines);
        }



    }
}


