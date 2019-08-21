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


        [HttpGet("GetPurchaseReport/{apartmentId}")]
        public async Task<IActionResult> GetPurchaseReport([FromRoute] int apartmentId)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            PurchaseReport purchaseReport = await _repositoryWraper.Transaction.GetPurchaseReport(apartmentId);
            if (purchaseReport == null)
            {
                return NotFound();
            }

            return Ok(purchaseReport);
        }


        [HttpGet("GetIncomeReports/{apartmentId}/{year=0}")]
        public async Task<IActionResult> GetIncomeReports([FromRoute] int apartmentId, int year)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            SummaryReport summaryReport = await _repositoryWraper.Transaction.GetSummaryReport(apartmentId, year);

            if (summaryReport == null)
            {
                return NotFound();
            }

            return Ok(summaryReport);
        }

    }
}