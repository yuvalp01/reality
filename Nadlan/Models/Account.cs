﻿using System.ComponentModel.DataAnnotations.Schema;

namespace Nadlan.Models
{
    [Table("accounts")]
    public class Account
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsIncome { get; set; }
        public int AccountTypeId { get; set; }
        public AccountType AccountType { get; set; }

    }
    [Table("accountTypes", Schema = "nadlan")]
    public class AccountType
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
