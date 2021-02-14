using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nadlan.Repositories;
using Nadlan.ViewModels;
using Nadlan.ViewModels.Reports;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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

            DateTime currentDate = DateTime.Today;
            //if (currentYearEnd > 0) currentDate = new DateTime(currentYearEnd, 12, 31);

            InvestorReportOverview investorReportOverview = await _investorReportWraper.GetInvestorReport(accountId, DateTime.Today);

            return Ok(investorReportOverview);
        }

        [HttpGet("GetPortfolio/{accountId}")]
        public async Task<IActionResult> GetPortfolio([FromRoute]  int accountId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            DateTime currentDate = DateTime.Today;
            //if (currentYearEnd > 0) currentDate = new DateTime(currentYearEnd, 12, 31);

            List<PortfolioReport> investorReportOverview = _investorReportWraper.GetPortfolio(accountId, DateTime.Today);

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

        [HttpGet("GetSummaryReport/{apartmentId}/{currentYearEnd}")]
        public async Task<IActionResult> GetSummaryReport([FromRoute] int apartmentId, int currentYearEnd)
        {
            DateTime currentDate = DateTime.Today;
            //if (currentYearEnd > 0) currentDate = new DateTime(currentYearEnd, 12, 31);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            SummaryReport purchaseReport = await _repositoryWraper.Summary.GetSummaryReport(apartmentId, currentDate);
            if (purchaseReport == null)
            {
                return NotFound();
            }

            return Ok(purchaseReport);
        }

        [HttpGet("GetSoFarReport/{apartmentId}/{currentYearEnd}")]
        public async Task<IActionResult> GetSoFarReport([FromRoute] int apartmentId, int currentYearEnd)
        {
            DateTime currentDate = DateTime.Today;
            if (currentYearEnd > 0) currentDate = new DateTime(currentYearEnd, 12, 31, 23, 59, 59);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            SoFarReport purchaseReport = await _repositoryWraper.Summary.GetSoFarReport(apartmentId, currentDate);
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

            DateTime currentDate = DateTime.Today;
            //if (currentYearEnd > 0) currentDate = new DateTime(currentYearEnd, 12, 31);

            IncomeReport summaryReport = await _repositoryWraper.Income.GetIncomeReport(apartmentId, year, currentDate);

            if (summaryReport == null)
            {
                return NotFound();
            }

            return Ok(summaryReport);
        }

    }
}