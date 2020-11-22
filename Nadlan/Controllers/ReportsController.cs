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
using Microsoft.AspNetCore.Authorization;

namespace Nadlan.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ReportsController : ControllerBase
    {
        private readonly ApartmentReportsRepositoryWrapper _repositoryWraper;
        private readonly InvestorReportRepository _investorReportWraper;
        private readonly IMapper _mapper;

        public ReportsController(IRepositoryWrapper repositoryWrapper, NadlanConext context, IMapper mapper)
        {
            _repositoryWraper = new ApartmentReportsRepositoryWrapper(context);
            _investorReportWraper = new InvestorReportRepository(context);
            _mapper = mapper;
        }

        [HttpGet("GetApartmentInfo/{apartmentId}")]
        public async Task<IActionResult> GetApartmentInfo([FromRoute]  int apartmentId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            ApartmentDto apartmentDto = await _repositoryWraper
                .GeneralView
                .GetApartmentInfo(apartmentId);
            return Ok(apartmentDto);
        }


        [HttpGet("GetInvestorReport/{accountId}")]
        public async Task<IActionResult> GetInvestorReport([FromRoute]  int accountId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            InvestorReportOverview investorReportOverview = await _investorReportWraper.GetInvestorReport(accountId);

            return Ok(investorReportOverview);
        }

        [HttpGet("GetPortfolio/{accountId}")]
        public async Task<IActionResult> GetPortfolio([FromRoute]  int accountId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            List<PortfolioReport> investorReportOverview =  _investorReportWraper.GetPortfolio(accountId);

            return Ok(investorReportOverview);
        }

        [HttpGet("GetPersonalBalance/{stakeholderId}")]
        public async Task<IActionResult> GetPersonalBalance([FromRoute]  int stakeholderId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            decimal balance = await _investorReportWraper.GetPersonalBalance(stakeholderId);

            return Ok(balance);
        }
        [HttpPost("GetDiagnosticReport")]
        public async Task<IActionResult> GetDiagnosticReport([FromBody] DiagnosticRequest diagnosticRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            DiagnosticReport diagnosticReport = await _repositoryWraper.Diagnostic.GetDiagnosticReport(diagnosticRequest);
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

            SummaryReport purchaseReport = await _repositoryWraper.Summary.GetSummaryReport(apartmentId);
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

            PurchaseReport purchaseReport = await _repositoryWraper.Purchase.GetPurchaseReport(apartmentId);
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

            IncomeReport summaryReport = await _repositoryWraper.Income.GetIncomeReport(apartmentId, year);

            if (summaryReport == null)
            {
                return NotFound();
            }

            return Ok(summaryReport);
        }

    }
}