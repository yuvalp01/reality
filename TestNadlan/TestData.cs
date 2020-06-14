using Nadlan.Models;
using Nadlan.Models.Renovation;
using System;
using System.Collections.Generic;

namespace TestNadlan
{
    public class TestData
    {


        public static List<Transaction> GetTransactions()
        {
            return new List<Transaction>
            {
                 new Transaction
                 {
                     Id = 1,
                     AccountId = 17,
                     ApartmentId =1,
                     Comments ="Different",
                     Amount = 0,
                     IsConfirmed = false,

                 },
                 new Transaction
                 {
                     Id = 100,
                     AccountId = 17,
                     ApartmentId =1,
                     Comments ="Payments to contractor",
                     Amount = 3180,
                     IsConfirmed = false,
                       
                 }
            };
        }

        public static List<RenovationProject> GetRenovationProjects()
        {
            return new List<RenovationProject>
            {
                 new RenovationProject
                 {
                     Id = 1,
                     Name = "Mavromoichli new renovation",
                     ApartmentId =1,
                     DateStart = new DateTime(2020,04,01),
                     DateEnd = new DateTime(2020,05,01),
                     PeneltyPerDay = 40,
                     Comments ="New bathroom*2 + wall + kitchen",
                     TransactionId = 100
                 }
            };
        }

        public static List<RenovationLine> GetRenovationLines()
        {
            var project = GetRenovationProjects()[0];
            return new List<RenovationLine>
            {
                 new RenovationLine
                 {
                     Id = 1,
                     Title = "Kitchen Furniture from IKEA",
                     RenovationProjectId =1,
                     Category = RenovationCategory.Kitchen,
                     Cost = 460,
                     Comments ="See link...",
                     RenovationProject = project
                 },
                new RenovationLine
                 {
                     Id = 2,
                     Title = "Dismantling tiles, tubes and sewage disposal of kitchen (rubble clearing included)",
                     RenovationProjectId =1,
                     Category = RenovationCategory.Kitchen,
                     Cost = 200,
                     Comments ="See link...",
                     RenovationProject = project

                 },
                 new RenovationLine
                 {
                     Id = 3,
                     Title = "New tubes & sewage disposal of kitchen",
                     RenovationProjectId =1,
                     Category = RenovationCategory.Kitchen,
                     Cost = 250,
                     Comments ="See link...",
                     RenovationProject = project
                 },
                 new RenovationLine
                 {
                     Id = 4,
                     Title = "Dismantling tiles, tubes and sewage disposal of bathroom (rubble clearing included)",
                     RenovationProjectId =1,
                     Category = RenovationCategory.Bathroom,
                     Cost = 200,
                     Comments ="See link...",
                     RenovationProject = project
                 },
                   new RenovationLine
                 {
                     Id = 5,
                     Title = "NEW tubes & sewage disposal of bathroom",
                     RenovationProjectId =1,
                     Category = RenovationCategory.Bathroom,
                     Cost = 500,
                     Comments ="See link...",
                     RenovationProject = project
                 },
                    new RenovationLine
                 {
                     Id = 6,
                     Title = "Mirror + cabinet",
                     RenovationProjectId =1,
                     Category = RenovationCategory.Bathroom,
                     Cost = 210,
                     Comments ="See: https://www.praktiker.gr/p/epiplo-mpaniou-bricola-leyko-55cm-me-niptira-kai-kathrepti-77025",
                     RenovationProject = project
                    },
                   new RenovationLine
                 {
                     Id = 7,
                     Title = "Dismantling tiles, tubes and sewage disposal of bathroom (rubble clearing included)",
                     RenovationProjectId =1,
                     Category = RenovationCategory.Bathroom,
                     Cost = 210,
                     Comments ="For second bathroom",
                     RenovationProject = project
                 },
                 new RenovationLine
                 {
                     Id = 8,
                     Title = "Electric Table Switch + replacing all cables",
                     RenovationProjectId =1,
                     Category = RenovationCategory.General,
                     Cost = 1000,
                     Comments ="Electric Table Switch + replacing all cables",
                     RenovationProject = project
                 },
                 new RenovationLine
                 {
                     Id = 8,
                     Title = "One door for the new room",
                     RenovationProjectId =1,
                     Category = RenovationCategory.Room,
                     Cost = 150,
                     Comments ="Electric Table Switch + replacing all cables",
                     RenovationProject = project
                 },
            };

        }


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
