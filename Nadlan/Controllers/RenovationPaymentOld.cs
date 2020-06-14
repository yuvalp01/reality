using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Nadlan.Models.Renovation;
using Nadlan.Repositories;
using Nadlan.Repositories.Renovation;
using Nadlan.ViewModels.Renovation;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nadlan.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class RenovationPaymentOld : ControllerBase
    {
        //private readonly NadlanConext _context;
        private readonly RenovationRepositoryWrapper_old _repositoryWraper;
        private readonly IMapper _mapper;

        public RenovationPaymentOld(NadlanConext context, IMapper mapper)
        {
            _repositoryWraper = new RenovationRepositoryWrapper_old(context);
            _mapper = mapper;
        }


        [HttpGet("renovationProjects")]
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

        [HttpGet("renovationLines/{renovationProjectId}")]
        public async Task<IActionResult> GetRenovationLines([FromRoute] int renovationProjectId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var mockRepo = new RenovationLineRepositoryMock();
            var renovationLines = await mockRepo
                 .GetLinesAsync(renovationProjectId);
            //var renovationLines = await _repositoryWraper
            //    .RenovationLineRepository
            //    .GetByRenovationProjectIdAsync(renovationProjectId);
            if (renovationLines == null)
            {
                return NotFound();
            }

            return Ok(renovationLines);
        }

        [HttpGet("getRenovationPaymentsByProjectId/{renovationProjectId}")]
        public async Task<IActionResult> GetRenovationPaymentsByProjectId([FromRoute] int renovationProjectId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var mockRepo = new RenovationLineRepositoryMock();
            var renovationLines = await mockRepo
                 .GetPaymentsAsync(renovationProjectId);
            if (renovationLines == null)
            {
                return NotFound();
            }

            return Ok(renovationLines);
        }
        [HttpGet("getRenovationPaymentById/{id}")]
        public async Task<IActionResult> GetRenovationPaymentById([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var mockRepo = new RenovationLineRepositoryMock();
            var payment = await mockRepo
                 .GetPaymentByIdAsync(id);
            if (payment == null)
            {
                return NotFound();
            }

            return Ok(payment);
        }

        [HttpPut("renovationPayment")]
        public async Task<IActionResult> UpdatePayment([FromBody] RenovationPayment payment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var mockRepo = new RenovationLineRepositoryMock();
            await  mockRepo.UpdatePaymentAsync(payment);
            //var payment = await mockRepo
                 //.GetPaymentByIdAsync(id);
            if (payment == null)
            {
                return NotFound();
            }

            return Ok(payment);
        }

        [HttpPost("renovationPayment")]
        public async Task<IActionResult> AddPayment([FromBody] RenovationPayment payment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var mockRepo = new RenovationLineRepositoryMock();
            await mockRepo.InsertPaymentAsync(payment);
            //var payment = await mockRepo
            //.GetPaymentByIdAsync(id);
            if (payment == null)
            {
                return NotFound();
            }

            return Ok(payment);
        }






        //// GET: api/Transactions
        //[HttpGet("renovationItems/{apartmentId}")]
        //protected async Task<IEnumerable<ItemDto>> GetRenovationItems([FromRoute] int apartmentId)
        //{
        //    var renovationItems = await _repositoryWraper.RenovationItemRepository
        //        .GetItemsDtoAsync(apartmentId);
        //    //var transactionsDto = _mapper.Map<List<RenovationItem>, IEnumerable<TransactionDto>>(transactions);
        //    return renovationItems;
        //}




        //protected async Task<IActionResult> GetRenovationLines_([FromRoute] int apartmentId)
        //{

        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    // SummaryReport purchaseReport = await _repositoryWraper.Report.GetSummaryReport(apartmentId);
        //    var renovationLines = await _repositoryWraper.RenovationLineRepository_old.GetByApartmentIdAsync(apartmentId);
        //    renovationLines.ForEach(a =>
        //    {
        //        a.ItemsTotalPrice = a.Items.Sum(b => b.Product.Price);
        //        a.TotalPrice = a.ItemsTotalPrice + a.WorkCost;
        //    });
        //    if (renovationLines == null)
        //    {
        //        return NotFound();
        //    }

        //    return Ok(renovationLines);
        //}


        // POST: api/Transactions
        [HttpPost]
        protected async Task<IActionResult> PostRenovationItem([FromBody] Item renovationItem)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //var transaction = _mapper.Map<TransactionDto, Transaction>(transactionDto);
            await _repositoryWraper.RenovationItemRepository.CreateAsync(renovationItem);
            //await _repositoryWraper.Transaction.SaveAsync(transaction);
            //_context.Transactions.Add(transaction);
            //await _context.SaveChangesAsync();

            return CreatedAtAction("PostRenovationItem", new { id = renovationItem.Id }, renovationItem);
        }

    }
}