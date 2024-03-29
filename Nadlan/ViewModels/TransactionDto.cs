﻿using Nadlan.Models;
using System;
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
        public bool IsBusinessExpense { get; set; }
        public bool IsConfirmed { get; set; }
        public string Comments { get; set; }
        public int ApartmentId { get; set; }
        public string ApartmentAddress { get; set; }
        public int ApartmentStatus { get; set; }
        public int AccountId { get; set; }
        public string AccountName { get; set; }
        public decimal Hours { get; set; }
        public int PersonalTransactionId { get; set; }
        public List<Message> Messages { get; set; }
        public int BankAccountId { get; set; }
        public bool IsPending { get; set; }
        public int CreatedBy { get; set; }

    }
}
