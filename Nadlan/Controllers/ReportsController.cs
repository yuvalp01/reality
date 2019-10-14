using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Nadlan.Repositories;
using AutoMapper;
using Nadlan.ViewModels.Reports;
using Nadlan.Models;
using Nadlan.ViewModels;

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

        [HttpGet("GetApartmentInfo/{apartmentId}")]
        public async Task<IActionResult> GetApartmentInfo([FromRoute]  int apartmentId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            ApartmentDto apartmentDto = await _repositoryWraper.ApartmentReport.GetApartmentInfo(apartmentId);
            return Ok(apartmentDto);
        }


        [HttpGet("GetInvestorReport/{accountId}")]
        public async Task<IActionResult> GetInvestorReport([FromRoute]  int accountId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            InvestorReportOverview investorReportOverview = await _repositoryWraper.InvestorReport.GetInvestorReport(accountId);

            return Ok(investorReportOverview);
        }
        //TODO: check if in use
        [HttpGet("GetBalance/{accountId}")]
        public async Task<IActionResult> GetBalance([FromRoute]  int accountId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            decimal balance = await _repositoryWraper.ApartmentReport.GetBalance(accountId);

            return Ok(balance);
        }

        [HttpGet("GetExpensesBalance")]
        public async Task<IActionResult> GetExpensesBalance([FromRoute]  int accountId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            decimal balance = await _repositoryWraper.ApartmentReport.GetExpensesBalance();

            return Ok(balance);
        }

        [HttpPost("GetDiagnosticReport")]
        public async Task<IActionResult> GetDiagnosticReport([FromBody] DiagnosticRequest diagnosticRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            DiagnosticReport diagnosticReport = await _repositoryWraper.ApartmentReport.GetDiagnosticReport(diagnosticRequest);
            if (diagnosticReport == null)
            {
                return NotFound();
            }

            return Ok(diagnosticReport);
        }

        [HttpGet("GetSummaryReport/{apartmentId}")]
        public async Task<IActionResult> GetSummaryReport([FromRoute] int apartmentId)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            SummaryReport purchaseReport = await _repositoryWraper.ApartmentReport.GetSummaryReport(apartmentId);
            if (purchaseReport == null)
            {
                return NotFound();
            }

            return Ok(purchaseReport);
        }

        [HttpGet("GetPurchaseReport/{apartmentId}")]
        public async Task<IActionResult> GetPurchaseReport([FromRoute] int apartmentId)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            PurchaseReport purchaseReport = await _repositoryWraper.ApartmentReport.GetPurchaseReport(apartmentId);
            if (purchaseReport == null)
            {
                return NotFound();
            }

            return Ok(purchaseReport);
        }


        [HttpGet("GetIncomeReport/{apartmentId}/{year=0}")]
        public async Task<IActionResult> GetIncomeReport([FromRoute] int apartmentId, int year)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            IncomeReport summaryReport = await _repositoryWraper.ApartmentReport.GetIncomeReport(apartmentId, year);

            if (summaryReport == null)
            {
                return NotFound();
            }

            return Ok(summaryReport);
        }

    }
}