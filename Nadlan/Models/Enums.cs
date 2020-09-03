using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nadlan.Models
{
    public enum ApartmentStatus
    {
        InResearch = 10,
        OfferSubmitted = 20,
        CommittedToAgency = 30,
        CommittedToSeller = 35,
        WaitingForRenovation =40,
        InRenovation = 50,
        SearchingForTenant = 60,
        Occupied = 70,
        Rented = 100
    }
    public enum TransactionType
    {
        MoneyTransfer = 5,
        PaidOnBefalf = 10,
        CashWithdrawal = 13,
        //ReminderDistribution = 15,
        Distribution = 20,
        Moneyback = 30
    }

    public enum NonPersonalTrasactionId
    {
        NotCoveredYet = 0,
        CoveredWithCreditCard = -1,
        CoveredByFunds = -2,
        NotRelevant = -3,
        ExpectedPayment = -4
    }

}

