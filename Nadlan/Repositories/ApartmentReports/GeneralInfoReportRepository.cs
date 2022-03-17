using Microsoft.EntityFrameworkCore;
using Nadlan.Models;
using Nadlan.Models.Enums;
using Nadlan.ViewModels;
using Nadlan.ViewModels.Reports;
using System;
using System.Linq;
using System.Threading.Tasks;


namespace Nadlan.Repositories.ApartmentReports
{
    public class GeneralInfoReportRepository : ApartmentReportRepository
    {
        public GeneralInfoReportRepository(NadlanConext conext):base( conext)
        {

        }

        public async Task<ApartmentDto> GetApartmentInfo(int apartmentId)
        {
            ExpectedTransaction expectedLastRent = await Context.ExpectedTransactions
                .OrderByDescending(a => a.Id)
                .FirstOrDefaultAsync(a => a.ApartmentId == apartmentId
                && a.AccountId == 1);
            decimal lastRent = 0;
            if (expectedLastRent != null)
            {
                lastRent = expectedLastRent.Amount;
            }
            var apartment = await Context.Apartments.FirstAsync(a => a.Id == apartmentId);

            ApartmentStatus apartmentStatus = (ApartmentStatus)apartment.Status;
            ApartmentDto apartmentDto = new ApartmentDto
            {
                Id = apartment.Id,
                Address = apartment.Address,
                Floor = apartment.Floor,
                Door = apartment.Door,
                Size = apartment.Size,
                PurchaseDate = apartment.PurchaseDate,
                CurrentRent = lastRent,
                Status = apartmentStatus.ToString()
            };
            return apartmentDto;
        }


    }
}
