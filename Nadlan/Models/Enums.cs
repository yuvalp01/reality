using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nadlan.Models
{
    public enum ApartmentStatus
    {
        InResearch = 10,
        OfferSubmitted = 20,
        Committed = 30,
        WaitingForRenovation =40,
        InRenovation = 50,
        SearchingForTenant = 60,
        Occupied = 70,
        Rented = 100

    }
}
