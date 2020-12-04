using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Nadlan.Models.Renovation;
using Nadlan.Repositories;
using Nadlan.Repositories.Renovation;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Nadlan.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class RenovationController : ControllerBase
    {
        //private readonly NadlanConext _context;
        private readonly RenovationRepositoryWrapper _repositoryWraper;
        private readonly IMapper _mapper;
        private readonly NadlanConext _context;

        public RenovationController(NadlanConext context, IMapper mapper)
        {
            _repositoryWraper = new RenovationRepositoryWrapper(context);
            _mapper = mapper;
            _context = context;
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
        }
        [HttpGet("products/{id}")]
        public async Task<IActionResult> GetRenovationProductsById([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var products = await _repositoryWraper.RenovationProductRepository.GetByIdAsync(id);
            if (products == null) return NotFound();
            return Ok(products);
        }

        [HttpGet("products/byType/{itemType}")]
        public async Task<IActionResult> GetRenovationProductsByStore([FromRoute] string itemType)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var products = await _repositoryWraper.RenovationProductRepository.GetByTypeAsync(itemType);
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


        [HttpPut("project")]
        public async Task<IActionResult> UpdateProject([FromBody] RenovationProject project)
        {

            if (!ModelState.IsValid) return BadRequest(ModelState);
            _context.RenovationProjects.Update(project);
            await _context.SaveChangesAsync();
            if (project == null) return NotFound();

            return NoContent();
        }

        [HttpDelete("products/{id}")]
        public async Task<IActionResult> DeleteProduct([FromRoute] int id)
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
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var renovationLines = await _repositoryWraper.RenovationLineRepository
                 .GetLinesAsyncOrderByCategory(renovationProjectId);

            if (renovationLines == null) return NotFound();

            return Ok(renovationLines);
        }

        [HttpPost("lines")]
        public async Task<IActionResult> AddLine([FromBody] RenovationLine line)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            await _repositoryWraper.RenovationLineRepository.CreateAsync(line);
            if (line.Id == 0) return NotFound();
            return Ok();

        }

        //[HttpGet("lines_/{renovationProjectId}")]
        //public async Task<IActionResult> GetRenovationLines_([FromRoute] int renovationProjectId)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }
        //    var renovationLines = await _repositoryWraper.RenovationLineRepository
        //         .GetLinesByProjectIdAsync(renovationProjectId);

        //    if (renovationLines == null)
        //    {
        //        return NotFound();
        //    }

        //    return Ok(renovationLines);
        //}
        [HttpDelete("lines/{lineId}")]
        public async Task<IActionResult> DeleteLine([FromRoute] int lineId)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            await _repositoryWraper.RenovationLineRepository.SoftDeleteAsync(lineId);
            return Ok();
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


