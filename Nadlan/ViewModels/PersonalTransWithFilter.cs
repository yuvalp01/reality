using Nadlan.Models;
using Nadlan.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nadlan.ViewModels
{
    public class PersonalTransWithFilter
    {
        public PersonalTransaction PersonalTransaction { get; set; }
        public Filter Filter { get; set; }
    }
}
