using Microsoft.VisualStudio.TestTools.UnitTesting;
using Nadlan.ViewModels.Reports;
using System.Linq.Expressions;
using System.Linq;

namespace TestNadlan
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void CheckLineSumCorrect()
        {
            //arrange
            var renovationLines = TestData.GetRenovationLines();
            //act
            decimal payments = renovationLines.Sum(a => a.Cost);
            var project = renovationLines[0].RenovationProject;
            var transaction = TestData.GetTransactions().Where(a=>a.Id == project.TransactionId).FirstOrDefault();
            decimal soFarTransactionPayment = transaction.Amount;

            //assert
            Assert.AreEqual(payments, soFarTransactionPayment);
        }



    }
}
