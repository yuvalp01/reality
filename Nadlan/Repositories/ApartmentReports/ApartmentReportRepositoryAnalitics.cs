﻿using Microsoft.EntityFrameworkCore;
using Nadlan.BusinessLogic;
using Nadlan.Models;
using Nadlan.ViewModels;
using Nadlan.ViewModels.Reports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Nadlan.Repositories
{
    public class ApartmentReportRepositoryAnalitics
    {

        const decimal ANNUAL_COSTS = 100 + 350;
        protected NadlanConext Context { get; set; }
        public ApartmentReportRepositoryAnalitics(NadlanConext conext)
        {
            Context = conext;
        }



        public async Task<DiagnosticReport> GetDiagnosticReport(DiagnosticRequest diagnosticRequest)
        {
            DiagnosticReport diagnosticReport = new DiagnosticReport()
            {
                Accountency = 100,
                Research = 2 * 800,
                Registration = diagnosticRequest.Size * 6 + 35,
                Agency = diagnosticRequest.Price * 0.01m < 1000 ? 1400 : diagnosticRequest.Price * 0.01m + 400,
                Legal = diagnosticRequest.Price * 0.04m,
                PurchaseTax = diagnosticRequest.Price * 0.031m,
                Supervision = diagnosticRequest.Renovation * 0.005m + 200,
                Unpredicted = diagnosticRequest.Price * 0.01m,
                UnpredictedRenovation = diagnosticRequest.Renovation * 0.1m,
            };
            diagnosticReport.TotalCost = diagnosticRequest.Price +
               diagnosticReport.Accountency +
               diagnosticReport.Research +
               diagnosticReport.Registration +
               diagnosticReport.Agency +
               diagnosticReport.Legal +
               diagnosticReport.PurchaseTax +
               diagnosticReport.Supervision +
               diagnosticReport.Unpredicted +
               diagnosticReport.UnpredictedRenovation;

            decimal netRent = diagnosticRequest.PredictedRent * 0.85m - 40 - ANNUAL_COSTS / 12;
            diagnosticReport.ROI = netRent * 11 / diagnosticReport.TotalCost;
            return diagnosticReport;
        }



    }

}
