namespace Nadlan.Models.Enums
{
    public enum ApartmentStatus
    {
        InResearch = 10,
        OfferSubmitted = 20,
        CommittedToAgency = 30,
        CommittedToSeller = 35,
        WaitingForRenovation = 40,
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
    public enum CreatedByEnum
    {
        Any = 0,
        Yuval = 1,
        Stella = 2
    }

    public enum OwnershipType
    {
        Yuval = 10,
        YuvalPartners = 20,
        PrivateInvestor = 30,
        Company = 40,
        Investor = 50
    }

}

