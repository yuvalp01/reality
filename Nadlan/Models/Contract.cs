using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;


namespace Nadlan.Models
{
    [Table("contracts")]
    public class Contract
    {
        public int Id { get; set; }
        public int ApartmentId { get; set; }
        public Apartment Apartment { get; set; }
        public string Tenant { get; set; }
        public string TenantPhone { get; set; }
        public string TenantEmail { get; set; }
        public DateTime? DateStart { get; set; }
        public DateTime? DateEnd { get; set; }
        public int PaymentDay { get; set; }
        public decimal Price { get; set; }
        public decimal PenaltyPerDay { get; set; }
        public decimal Deposit { get; set; }
        public string Link { get; set; }
        public string Conditions { get; set; }
        public bool IsElectriciyChanged { get; set; }
        public bool IsPaymentConfirmed { get; set; }
        public int BankAccountId { get; set; }
        public bool IsDeleted { get; set; }
    }
}
