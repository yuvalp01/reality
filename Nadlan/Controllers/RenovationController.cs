using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Nadlan.ViewModels;
using Nadlan.Models;
using Nadlan.Models.Renovation;
using Nadlan.Repositories;
using AutoMapper;

namespace Nadlan.Controllers
{
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
            //_context = context;
            _mapper = mapper;
        }

        // GET: api/Transactions
        [HttpGet("renovationItems/{apartmentId}")]
        public async Task<IEnumerable<Item>> GetRenovationItems([FromRoute] int apartmentId)
        {
            var renovationItems = await _repositoryWraper.RenovationItemRepository.GetAllAsync();
            //var transactionsDto = _mapper.Map<List<RenovationItem>, IEnumerable<TransactionDto>>(transactions);
            return renovationItems;
        }


        [HttpGet("renovationLines/{apartmentId}")]

        public async Task<IActionResult> GetRenovationLines([FromRoute] int apartmentId)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // SummaryReport purchaseReport = await _repositoryWraper.Report.GetSummaryReport(apartmentId);
            var renovationLines = await _repositoryWraper.RenovationLineRepository.GetByApartmentIdAsync(apartmentId);
            //var xxx = renovationLines.ForEach(a=>a.ItemsTotalPrice= a.Items.Sum(a=>a.Product.Price))
            renovationLines.ForEach(a =>
            {
                a.ItemsTotalPrice = a.Items.Sum(b => b.Product.Price);
                a.TotalPrice = a.ItemsTotalPrice + a.WorkCost;
            });
            if (renovationLines == null)
            {
                return NotFound();
            }

            return Ok(renovationLines);
        }

        //public async Task<IEnumerable<RenovationLine>> GetRenovationLines([FromRoute] int apartmentId)
        //{
        //    var renovationLines = await _repositoryWraper.RenovationLineRepository.GetAllAsync();
        //    return renovationLines;
        //}

        //[HttpGet("GetSummaryReport/{apartmentId}")]
        //public async Task<IActionResult> GetSummaryReport([FromRoute] int apartmentId)
        //{

        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    SummaryReport purchaseReport = await _repositoryWraper.Report.GetSummaryReport(apartmentId);
        //    if (purchaseReport == null)
        //    {
        //        return NotFound();
        //    }

        //    return Ok(purchaseReport);
        //}


        // POST: api/Transactions
        [HttpPost]
        public async Task<IActionResult> PostRenovationItem([FromBody] Item renovationItem)
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