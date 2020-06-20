using Nadlan.Models;
using Nadlan.Models.Renovation;
using System;
using System.Collections.Generic;

namespace Nadlan.MockData
{
    public class MockRenovation
    {

        public static List<RenovationPayment> GetPayments()
        {
            var project = GetRenovationProjects().Find(a => a.Id == 1);
            return new List<RenovationPayment>
            {
                 new RenovationPayment {
                     Id =1,
                     Title ="First Payment",
                     Criteria = "Finish destruction and remoiving debris",
                     DatePayment = new DateTime(2020,05,01),
                     Amount = 1000,
                     Comments = "Paid at Makedonias in the afternoon",
                     RenovationProject = project,
                      //CheckIdWriten = true,
                      //CheckInvoiceScanned = true,
                      IsConfirmed = true,
                 },
               new RenovationPayment {
                     Id =2,
                     Title ="Second Payment",
                     Criteria = "Install kitchen and Aluminum",
                     DatePayment = new DateTime(2020,06,01),
                     Amount = 800,
                     Comments = "Aluminum delayed by safty door installed instead",
                     RenovationProject = project,
                      //CheckIdWriten = true,
                      //CheckInvoiceScanned = false

                 },
             new RenovationPayment {
                     Id =3,
                     Title ="Third Payment",
                     Criteria = "AC etc.",
                     DatePayment = null,
                     Amount = 200,
                     Comments = "Aluminum delayed by safty door installed instead",
                     RenovationProject = project,
                      //CheckIdWriten = false,
                      //CheckInvoiceScanned = false
                 }
            };
        }
        //public static List<Transaction> GetTransactions()
        //{
        //    return new List<Transaction>
        //    {
        //         new Transaction
        //         {
        //             Id = 1,
        //             AccountId = 17,
        //             ApartmentId =1,
        //             Comments ="Different",
        //             Amount = 0,
        //             IsConfirmed = false,

        //         },
        //         new Transaction
        //         {
        //             Id = 1224,
        //             AccountId = 17,
        //             ApartmentId =1,
        //             Comments ="Payments to contractor",
        //             Amount = 3180,
        //             IsConfirmed = false,

        //         }
        //    };
        //}

        public static List<RenovationProject> GetRenovationProjects()
        {
            //Transaction transaction = GetTransactions().Find(a => a.Id == 100);
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
                     TransactionId = 1000
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
                     Comments ="See link: https://www.ivory.co.il/catalog.php?id=28872",
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
                     RenovationProject = project,
                     IsCompleted= true


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
                     Id = 9,
                     Title = "One door for the new room",
                     RenovationProjectId =1,
                     Category = RenovationCategory.Room,
                     Cost = 150,
                     Comments ="Electric Table Switch + replacing all cables",
                     RenovationProject = project
                 },
                                  new RenovationLine
                 {
                     Id = 10,
                     Title = "Another thing for the kitchen",
                     RenovationProjectId =1,
                     Category = RenovationCategory.Kitchen,
                     Cost = 50,
                     Comments ="Shelf 15*80",
                     RenovationProject = project
                 },
            };

        }

    }
}
