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

        public void Save()
        {
            _conext.SaveChanges();
        }
    }
}
