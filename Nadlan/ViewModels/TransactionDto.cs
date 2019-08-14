﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nadlan.ViewModels
{
    public class TransactionDto
    {
        public int Id { get; set; }

        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public bool IsPurchaseCost { get; set; }
        public string Comments { get; set; }
        public int ApartmentId { get; set; }
        public string ApartmentAddress { get; set; }
        public int AccountId { get; set; }
        public string AccountName { get; set; }

    }
}