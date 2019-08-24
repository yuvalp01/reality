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
        private AccountRepository _account;
        private ReportRepository _report;

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

        public ReportRepository Report
        {
            get
            {
                if (_report == null)
                {
                    _report = new ReportRepository(_conext);
                }
                return _report;
            }
        }
        public void Save()
        {
            throw new Exception("It is not possible to save reports");
        }
    }
}
