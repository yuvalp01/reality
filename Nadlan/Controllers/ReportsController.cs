﻿using System;
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

        [HttpPost("GetDiagnosticReport")]
        public async Task<IActionResult> GetDiagnosticReport([FromBody] DiagnosticRequest diagnosticRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            DiagnosticReport diagnosticReport = await _repositoryWraper.Report.GetDiagnosticReport(diagnosticRequest);
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

            SummaryReport purchaseReport = await _repositoryWraper.Report.GetSummaryReport(apartmentId);
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

            PurchaseReport purchaseReport = await _repositoryWraper.Report.GetPurchaseReport(apartmentId);
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

            IncomeReport summaryReport = await _repositoryWraper.Report.GetIncomeReport(apartmentId, year);

            if (summaryReport == null)
            {
                return NotFound();
            }

            return Ok(summaryReport);
        }

    }
}