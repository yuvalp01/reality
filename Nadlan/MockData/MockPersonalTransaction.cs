using Nadlan.Models;
using Nadlan.Models.Renovation;
using System;
using System.Collections.Generic;

namespace TestNadlan.MockData
{
    public class MockPersonalTransaction
    {

        public static List<PersonalTransaction> GetPersonalTransactions()
        {
            return new List<PersonalTransaction>
            {
                new PersonalTransaction
                    {
                        Id = 1,
                        Amount = 3000,
                        ApartmentId = 1,
                        Comments = "Money transfer",
                        Date = new DateTime(2019, 1, 3),
                        IsDeleted = false,
                        StakeholderId = 101,
                        TransactionType = TransactionType.MoneyTransfer

                    },
                new PersonalTransaction
                    {
                        Id = 2,
                        Amount = 500,
                        ApartmentId = 1,
                        Comments = "Cash withdrawal",
                        Date = new DateTime(2019, 2, 3),
                        IsDeleted = false,
                        StakeholderId = 101,
                        TransactionType = TransactionType.CashWithdrawal
                    },
                new PersonalTransaction
                    {
                        Id = 3,
                        Amount = 1000,
                        ApartmentId = 1,
                        Comments = "Cash withdrawal CANCELED!",
                        Date = new DateTime(2019, 2, 3),
                        IsDeleted = true,
                        StakeholderId = 101,
                        TransactionType = TransactionType.CashWithdrawal
                    },
            };
        }





    }
}
