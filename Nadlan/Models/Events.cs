using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nadlan.Models
{
    [Table("events")]
    public class Event
    {
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public DateTime DateCreated { get; set; }
        public int ApartmentId { get; set; }
        public int Severity { get; set; }
    }
}
