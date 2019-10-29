using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nadlan.ViewModels
{
    public class PersonalTransactionDto
    {
    public int Id { get; set; }
    public decimal Amount { get; set; }
    public DateTime Date { get; set; }
    public string Comments { get; set; }

    public int StakeholderId { get; set; }
    public string StakeholderName { get; set; }
    public int ApartmentId { get; set; }
    public string ApartmentAddress { get; set; }
    }
}
