using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nadlan.Models
{
    [Table("apartments")]
    public class Apartment
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }
        public string Address { get; set; }
        public short Status { get; set; }
        public DateTime PurchaseDate { get; set; }
        public int Floor { get; set; }
        public int Size { get; set; }
        public string Door { get; set; }
        public decimal CurrentRent { get; set; }
        public decimal FixedMaintanance { get; set; }

    }


}
