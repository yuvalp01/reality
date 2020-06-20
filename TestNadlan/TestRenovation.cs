using Microsoft.VisualStudio.TestTools.UnitTesting;
using Nadlan.MockData;
using Nadlan.Models;
using Nadlan.Models.Renovation;
using Nadlan.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TestNadlan
{
    [TestClass]
    public class TestRenovation
    {
        private static NadlanConext _dbContext;
        private static TestContext _testContext;
        private static RenovationRepositoryWrapper _renovationRepositoryWrapper;

        [ClassInitialize()]
        public static void InitTestSuite(TestContext testContext)
        {
            _testContext = testContext;
            _dbContext = new InMemoryDbContextFactory().GetMockNadlanDbContext();
            _renovationRepositoryWrapper = new RenovationRepositoryWrapper(_dbContext);
            //arrange
            List<RenovationProject> projects = MockRenovation.GetRenovationProjects();
            _dbContext.RenovationProjects.AddRange(projects);
            //List<RenovationLine> lines = MockRenovation.GetRenovationLines();
            //_dbContext.RenovationLines.AddRange(lines);
            //List<RenovationPayment> payments = MockRenovation.GetPayments();
            //_dbContext.RenovationPayments.AddRange(payments);
            List<Transaction> transactions = MockTransactions.GetTransactions();
            _dbContext.Transactions.AddRange(transactions);

            _dbContext.SaveChanges();
        }


        [TestMethod]
        public void Test_MakePayment()
        {
            //arrange
            var transactionId = _dbContext.RenovationProjects.Find(1).TransactionId;
            RenovationPayment newPayment = new RenovationPayment { Amount = 100, RenovationProjectId = 1 };
            //act
            _renovationRepositoryWrapper.RenovationPaymentRepository
        .CreateAsync(newPayment);

            newPayment.DatePayment = DateTime.Today;
            var newBalance =  _renovationRepositoryWrapper.RenovationPaymentRepository
                 .MakePaymentAsync(newPayment);
            var payments = _renovationRepositoryWrapper
    .RenovationPaymentRepository.GetPaymentsAsync(1).Result;
            var paidSoFar = payments
                .Where(a => a.IsDeleted == false && a.DatePayment != null)
                .Sum(a => a.Amount);
            var transactionAmount = _dbContext.Transactions.Find(transactionId).Amount;

            Assert.AreEqual(paidSoFar*-1, transactionAmount);


        }


        [TestMethod]
        public void Test_CancePayment()
        {
            //arrange
            RenovationPayment payment1 = new RenovationPayment { Id=1, Amount = 50, DatePayment=DateTime.Today, RenovationProjectId = 1 };
            RenovationPayment payment2 = new RenovationPayment { Id=2, Amount = 100, DatePayment = DateTime.Today, RenovationProjectId = 1 };
            var transactionId = _dbContext.RenovationProjects.Find(1).TransactionId;
            var transaction = _dbContext.Transactions.Find(transactionId);
            transaction.Amount = -150;
            _dbContext.RenovationPayments.Add(payment1);
            _dbContext.RenovationPayments.Add(payment2);
            _dbContext.SaveChanges();

            var newBalance = _renovationRepositoryWrapper.RenovationPaymentRepository
                 .CancelPayment(2);
            var payments = _renovationRepositoryWrapper
    .RenovationPaymentRepository.GetPaymentsAsync(1).Result;
            var paidSoFar = payments
                .Where(a => a.IsDeleted == false && a.DatePayment != null)
                .Sum(a => a.Amount);
            var transactionAmount = _dbContext.Transactions.Find(transactionId).Amount;

            Assert.AreEqual(paidSoFar * -1, transactionAmount);


        }




        //[TestMethod]
        //public void CheckLineSumCorrect()
        //{
        //    //arrange
        //    var renovationLines = TestData.GetRenovationLines();
        //    //act
        //    decimal totalCost = renovationLines.Sum(a => a.Cost);
        //    var project = renovationLines[0].RenovationProject;
        //    var transaction = TestData.GetTransactions().Where(a=>a.Id == project.TransactionId).FirstOrDefault();
        //    decimal soFarTransactionPayment = transaction.Amount;

        //    //assert
        //    Assert.AreEqual(payments, soFarTransactionPayment);
        //}



    }
}
