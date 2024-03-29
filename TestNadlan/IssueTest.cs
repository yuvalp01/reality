﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using Nadlan.Repositories;
using Nadlan.Repositories.Issues;
using Nadlan.Repositories.Messages;

namespace TestNadlan
{
    [TestClass]
    public class IssueTest
    {

        private static NadlanConext _dbContext;
        private static TestContext _testContext;
        private static IIssueRepository _issueRepository;
        private static IMessageRepository _messageRepository;
        public IssueTest()
        {
        }

        [ClassInitialize()]
        public static void InitTestSuite(TestContext testContext)
        {
            //_testContext = testContext;
            //_dbContext = new InMemoryDbContextFactory().GetMockNadlanDbContext();
            //_issueRepository = new IssueRepository(_dbContext);
            //_messageRepository = new MessagesRepository(_dbContext);
            ////arrange
            //List<Issue> issues = MockIssues.GetAllIssues();
            //_dbContext.Issues.AddRange(issues);
            //List<Message> issueItems = MockIssues.GetAllMessages();
            //_dbContext.Messages.AddRange(issueItems);
            //_dbContext.SaveChanges();   
        }



        [TestMethod]
        public void GetAllIssues()
        {
            //act
            var openIssues = _issueRepository.GetAllIssuesAsync(false,0).Result;
            //assert       
            Assert.IsTrue(openIssues.Count == 4);
        }
        [TestMethod]
        public void GetOpenIssues()
        {

            //act
            var openIssues_ = _issueRepository.GetAllIssuesAsync(true,0).Result;
            //assert       
            Assert.IsTrue(openIssues_.Count == 3);
        }

        [TestMethod]
        public void GetMessagesOfDeletedIssue()
        {

            ////act
            //var openIssues_ = _messageRepository.GetMassagesByIssueIdAsync(5).Result;
            ////assert       
            //Assert.IsTrue(openIssues_.Count == 0);
        }

    }
}



//[ClassCleanup()]
//public static void CleanupTestSuite()
//{
//  //  removeTestDataFromDb();
//}






//[TestInitialize]
//public void SetUp()
//{
//    _dbContext = new InMemoryDbContextFactory().GetMockNadlanDbContext();
//    _issueRepository = new IssueRepository(_dbContext);
//    //arrange
//    List<Issue> issues = MockIssues.GetAllIssues();
//    _dbContext.Issues.AddRange(issues);
//    _dbContext.SaveChanges();
//}
//[TestCleanup]
//public void TearDown()
//{
//    _dbContext.Issues.emoveRange(MockIssues.GetAllIssues());
//    _dbContext.SaveChanges();
//}
