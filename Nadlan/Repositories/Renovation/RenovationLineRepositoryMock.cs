using Nadlan.Models.Renovation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestNadlan.MockData;

namespace Nadlan.Repositories.Renovation
{
    public class RenovationLineRepositoryMock : IRenovationLineRepository
    {
        static List<RenovationLine> _allLines;
        static List<RenovationPayment> _allPayments;
        public RenovationLineRepositoryMock()
        {
            _allLines = MockRenovation.GetRenovationLines().ToList();
            _allPayments = MockRenovation.GetPayments().ToList();
        }
        public Task<List<RenovationProject>> GetAllRenovationProjectsAsync()
        {
            var projects = Task.Run(() => MockRenovation.GetRenovationProjects()
            .ToList());
            return projects;
        }
        public Task<List<RenovationLine>> GetLinesAsync(int projectId)
        {
            var projectLines = Task.Run(() => _allLines
            .Where(a => a.RenovationProjectId == projectId)
            .OrderBy(a => a.Category)
            .ToList());
            return projectLines;
        }

        public Task<List<RenovationPayment>> GetPaymentsAsync(int projectId)
        {
            return Task.Run(() =>
            {
                //var allPayments = MockRenovation.GetPayments();
                return _allPayments.Where(a => a.RenovationProject.Id == projectId).ToList();
            });

        }

        public Task<RenovationPayment> GetPaymentByIdAsync(int id)
        {
            return Task.Run(() =>
            {
                //var allPayments = MockRenovation.GetPayments();
                return _allPayments.FirstOrDefault(a => a.Id == id);
            });

        }



        public Task CreateAsync(RenovationLine renovationLine)
        {
            var task = Task.Run(() =>
           {
               var newLine = new RenovationLine
               {
                   Id = _allLines.Max(a => a.Id) + 1,
                   Title = "Something important in the bathroom",
                   RenovationProjectId = 1,
                   Category = RenovationCategory.Bathroom,
                   Cost = 100,
                   Comments = "New line",
               };
               _allLines.Add(newLine);
           });
            return task;

        }

        public Task DeleteAsync(RenovationLine renovationLine)
        {
            throw new NotImplementedException();
        }

        public Task<RenovationLine> GetByIdAsync(int id)
        {
            return Task.Run(() => _allLines.Where(a => a.Id == id).FirstOrDefault());
        }

        public async Task UpdateAsync(RenovationLine renovationLine)
        {

            var line = _allLines.FirstOrDefault(a => a.Id == renovationLine.Id);
            line.Title = renovationLine.Title;
            line.Comments = renovationLine.Comments;
            line.Cost = renovationLine.Cost;
            line.Category = renovationLine.Category;

        }
        public async Task UpdatePaymentAsync(RenovationPayment renovationPayment)
        {

            var payment = _allPayments.FirstOrDefault(a => a.Id == renovationPayment.Id);
            payment.Title = renovationPayment.Title;
            payment.Comments = renovationPayment.Comments;
            payment.Amount = renovationPayment.Amount;
            //payment.CheckIdWriten = renovationPayment.CheckIdWriten;
            //payment.CheckInvoiceScanned = renovationPayment.CheckInvoiceScanned;
            payment.DatePayment = renovationPayment.DatePayment;

        }
        public async Task InsertPaymentAsync(RenovationPayment renovationPayment)
        {

            var payment = _allPayments.OrderByDescending(a => a.Id)
                .FirstOrDefault(a => a.Id == renovationPayment.Id);
            _allPayments.Add(payment);

        }
    }
}
