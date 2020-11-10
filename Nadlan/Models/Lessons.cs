using Nadlan.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection.Emit;
using System.Threading.Tasks;

namespace Nadlan.Models
{
    [Table("lessons")]
    public class Lesson
    {
        [Key]
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public DateTime DateCreated { get; set; }
        public int ApartmentId { get; set; }
        public int CategoryId { get; set; }
        public Guid? ReferenceId { get; set; }

    }
}
