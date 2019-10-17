using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nadlan.Repositories
{
    public interface IRepositoryWrapper
    {
        TransactionRepository Transaction { get; }
        AccountRepository Account { get; }
        ReportRepository ApartmentReport { get; }
        InvestorReportRepository InvestorReport{ get; }
        PersonalTransactionRepository PersonalTransaction { get; }
        void Save();

    }
}
