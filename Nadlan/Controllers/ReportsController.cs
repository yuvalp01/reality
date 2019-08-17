using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Nadlan.ViewModels;
using Nadlan.Models;
using Nadlan.Repositories;
using AutoMapper;

namespace Nadlan.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportsController : ControllerBase
    {
        private readonly IRepositoryWrapper _repositoryWraper;
        private readonly IMapper _mapper;

        public ReportsController(IRepositoryWrapper repositoryWrapper, NadlanConext context, IMapper mapper)
        {
            _repositoryWraper = repositoryWrapper;
            _mapper = mapper;
        }

        // GET: api/Transactions
        [HttpGet]
        public async Task<IEnumerable<SummaryReport>> GetReports()
        {

            Task<List<SummaryReport>> task = Task.Run(() => new List<SummaryReport>());

            return await task;



            //var transactions = await  _repositoryWraper.Transaction.GetAllAsync();
            //var transactionsDto =   _mapper.Map<List<Transaction>, IEnumerable<TransactionDto>>(transactions);
            //return transactionsDto;
        }

        // GET: api/Transactions/5
        //[HttpGet("{id}")]
        [HttpGet("{apartmentId}/{year=0}")]
        public async Task<IActionResult> GetReport([FromRoute] int apartmentId, int year)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var report = await _repositoryWraper.Transaction.GetReport(apartmentId, year);

            if (report == null)
            {
                return NotFound();
            }

            return Ok(report);
        }

    }
}