using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Nadlan.Models.Renovation;
using Nadlan.Repositories;
using Nadlan.Repositories.Renovation;
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
            var renovationPayments = await _repositoryWraper.RenovationPaymentRepository
                .GetPaymentsAsync(renovationProjectId);
            if (renovationPayments == null)
            {
                return NotFound();
            }

            return Ok(renovationPayments);
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
            decimal? newBalance = await _repositoryWraper.RenovationPaymentRepository.MakePaymentAsync(payment);

            return Ok(newBalance);
        }

        //[HttpPut("revertPayment")]
        //public async Task<IActionResult> RevertPayment([FromBody] int paymentId)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }
        //    decimal newBalance = await _repositoryWraper.RenovationPaymentRepository.RevertPaymentAsync(paymentId);

        //    return Ok(newBalance);
        //}

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
            var newBalance = await _repositoryWraper.RenovationPaymentRepository.SoftDeleteAsync(paymentId);
            return Ok(newBalance);
        }
        [HttpPut("cancelPayment")]
        public async Task<IActionResult> CancelPayment([FromBody] int paymentId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var newBalance = await _repositoryWraper.RenovationPaymentRepository.CancelPayment(paymentId);
            return Ok(newBalance);
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
            return Ok();
            // return CreatedAtAction("PaymentUpdated", new { id = payment.Id },payment);

        }

        [HttpGet("projects")]
        public async Task<IActionResult> GetRenovationProjects()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var mockRepo = new RenovationLineRepositoryMock();
            var projects = await mockRepo.GetAllRenovationProjectsAsync();
            if (projects == null)
            {
                return NotFound();
            }

            return Ok(projects);
        }

        [HttpGet("lines/{renovationProjectId}")]
        public async Task<IActionResult> GetRenovationLines([FromRoute] int renovationProjectId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            // var mockRepo = new RenovationLineRepositoryMock();
            var renovationLines = await _repositoryWraper.RenovationLineRepository
                 .GetLinesAsync(renovationProjectId);

            if (renovationLines == null)
            {
                return NotFound();
            }

            return Ok(renovationLines);
        }

        [HttpPut("line")]
        public async Task<IActionResult> UpdateLine([FromBody] RenovationLine line)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            //var mockRepo = new RenovationLineRepositoryMock();
            await _repositoryWraper.RenovationLineRepository.UpdateAsync(line);
            if (line == null)
            {
                return NotFound();
            }

            return NoContent();
        }

    }
}


