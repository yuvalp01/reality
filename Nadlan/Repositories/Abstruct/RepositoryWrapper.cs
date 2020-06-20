using Nadlan.Repositories.ApartmentReports;
using Nadlan.Repositories.Issues;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nadlan.Repositories
{
    public class RepositoryWrapper : IRepositoryWrapper
    {
        private NadlanConext _conext;
        private TransactionRepository _transaction;
        private PersonalTransactionRepository _personalTransaction;
        private AccountRepository _account;
        private ApartmentReportRepositoryNew _apartmentReport;
        private InvestorReportRepository _investorReport;
        private IssueRepository _issueRepository;


        public RepositoryWrapper(NadlanConext conext)
        {
            _conext = conext;
        }

        public TransactionRepository Transaction
        {
            get
            {
                if (_transaction == null)
                {
                    _transaction = new TransactionRepository(_conext);
                }
                return _transaction;
            }
        }
        public PersonalTransactionRepository PersonalTransaction
        {
            get
            {
                if (_personalTransaction == null)
                {
                    _personalTransaction = new PersonalTransactionRepository(_conext);
                }
                return _personalTransaction;
            }
        }
        public AccountRepository Account
        {
            get
            {
                if (_account == null)
                {
                    _account = new AccountRepository(_conext);
                }
                return _account;
            }
        }
        public ApartmentReportRepositoryNew ApartmentReport
        {
            get
            {
                if (_apartmentReport == null)
                {
                    _apartmentReport = new ApartmentReportRepositoryNew(_conext);
                }
                return _apartmentReport;
            }
        }
        public IssueRepository  IssueRepository
        {
            get
            {
                if (_issueRepository == null)
                {
                    _issueRepository = new IssueRepository(_conext);
                }
                return _issueRepository;
            }
        }



















        public InvestorReportRepository InvestorReport
        {
            get
            {
                if (_investorReport == null)
                {
                    _investorReport = new InvestorReportRepository(_conext);
                }
                return _investorReport;
            }
        }


        //public RenovationItemRepository RenovationItem
        //{
        //    get
        //    {
        //        if (_renovationItem == null)
        //        {
        //            _renovationItem = new RenovationItemRepository(_conext);
        //        }
        //        return _renovationItem;
        //    }
        //}

        public void Save()
        {
            _conext.SaveChanges();
            //throw new Exception("It is not possible to save reports");
        }
    }
}
