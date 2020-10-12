using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Nadlan.Models.Renovation;
using Nadlan.Repositories;
using Nadlan.Repositories.Renovation;
using System.Collections.Generic;
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
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var payment = await _repositoryWraper.RenovationPaymentRepository
                 .GetPaymentByIdAsync(id);
            if (payment == null) return NotFound();
            return Ok(payment);
        }
        [HttpGet("products")]
        public async Task<IActionResult> GetRenovationProducts()
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var products = await _repositoryWraper.RenovationProductRepository.GetAllAsync();
            if (products == null) return NotFound();
            return Ok(products);
            //     .GetPaymentByIdAsync(id);
            //var payment = new List<RenovationProduct>();
            //payment.Add(new RenovationProduct
            //{
            //    Id = 1,
            //    Description = "test description",
            //    Link = "https://www.google.com",
            //    Name = "test",
            //    PhotoUrl = "url",
            //    Price = 12,
            //    SerialNumber = "345sd345fdsf",
            //    Store = "IKEA"

            //});
            //if (payment == null) return NotFound();
            //return Ok(payment);
        }
        [HttpGet("products/{id}")]
        public async Task<IActionResult> GetRenovationProductsById([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var products = await _repositoryWraper.RenovationProductRepository.GetByIdAsync(id);
            if (products == null) return NotFound();
            return Ok(products);
        }
        [HttpPost("products")]
        public async Task<IActionResult> AddfProduct([FromBody] RenovationProduct product)
        { 
            if (!ModelState.IsValid) return BadRequest(ModelState);
            await _repositoryWraper.RenovationProductRepository.CreateAsync(product);
            if (product == null)return NotFound();
            return Ok();
        }
        [HttpPut("products")]
        public async Task<IActionResult> UpdateProduct([FromBody] RenovationProduct product)
        {

            if (!ModelState.IsValid) return BadRequest(ModelState);
            await _repositoryWraper.RenovationProductRepository.UpdateAsync(product);
            if (product == null) return NotFound();

            return NoContent();
        }

        [HttpDelete("products/{id}")]
        public async Task<IActionResult> DeleteProduct([FromBody] int id)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            await _repositoryWraper.RenovationProductRepository.SoftDeleteAsync(id);
            return NoContent();
        }


        [HttpPut("payment")]
        public async Task<IActionResult> UpdatePayment([FromBody] RenovationPayment payment)
        {

            if (!ModelState.IsValid) return BadRequest(ModelState);
            await _repositoryWraper.RenovationPaymentRepository.UpdateAsync(payment);
            if (payment == null) return NotFound();

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
            if (!ModelState.IsValid) return BadRequest(ModelState);
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
            await _repositoryWraper.RenovationPaymentRepository.CreateAsync(payment);
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
            var projects = await _repositoryWraper.RenovationPaymentRepository
                .GetAllRenovationProjectsAsync();
            if (projects == null)
            {
                return NotFound();
            }

            return Ok(projects);
        }
        [HttpGet("project/{projectId}")]
        public async Task<IActionResult> GetRenovationProject(int projectId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var projects = await _repositoryWraper.RenovationPaymentRepository
                .GetRenovationProjectAsync(projectId);
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

        [HttpGet("lines_/{renovationProjectId}")]
        public async Task<IActionResult> GetRenovationLines_([FromRoute] int renovationProjectId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var renovationLines = await _repositoryWraper.RenovationLineRepository
                 .GetLinesByProjectIdAsync(renovationProjectId);

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


