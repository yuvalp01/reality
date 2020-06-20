using Nadlan.Models;
using Nadlan.Models.Renovation;
using System;
using System.Collections.Generic;

namespace Nadlan.MockData
{
    public class MockTransactions
    {

        public static List<Transaction> GetTransactions()
        {
            return new List<Transaction>
            {
                new Transaction
                    {
                        Id = 1000,
                        Amount = 0,
                        ApartmentId = 1,
                        Comments = "Money transfer",
                        Date = new DateTime(2019, 1, 3),
                        IsDeleted = false,
                     },
            };
        }





    }
}
