using Nadlan.Repositories.ApartmentReports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nadlan.Repositories
{
    public class ApartmentReportsRepositoryWrapper
    {
        NadlanConext _context;
        ApartmentReportRepository _genral;
        GeneralInfoReportRepository _genralView;
        SummaryReportRepository _summary;
        PurchaseReportRepository _purchase;
        IncomeReportRepository _income;
        DiagnosticReportRepository _diagnostic;

        public ApartmentReportsRepositoryWrapper(NadlanConext conext)
        {
            _context = conext;
        }
        public ApartmentReportRepository General_
        {
            get
            {
                if (_genral == null)
                {
                    _genral = new ApartmentReportRepository(_context);
                }
                return _genral;
            }
        }
        public GeneralInfoReportRepository GeneralView
        {
            get
            {
                if (_genralView == null)
                {
                    _genralView = new GeneralInfoReportRepository(_context);
                }
                return _genralView;
            }
        }
        public SummaryReportRepository Summary
        {
            get
            {
                if (_summary == null)
                {
                    _summary = new  SummaryReportRepository(_context);
                }
                return _summary;
            }
        }
        public PurchaseReportRepository Purchase
        {
            get
            {
                if (_purchase == null)
                {
                    _purchase = new PurchaseReportRepository(_context);
                }
                return _purchase;
            }
        }
        public IncomeReportRepository Income
        {
            get
            {
                if (_income == null)
                {
                    _income = new IncomeReportRepository(_context);
                }
                return _income;
            }
        }
        public DiagnosticReportRepository Diagnostic
        {
            get
            {
                if (_diagnostic == null)
                {
                    _diagnostic = new DiagnosticReportRepository(_context);
                }
                return _diagnostic;
            }
        }
    }
}
