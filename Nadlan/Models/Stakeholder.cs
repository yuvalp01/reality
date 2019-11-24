using System.ComponentModel.DataAnnotations.Schema;

namespace Nadlan.Models
{
    [Table("stakeholders")]
    public class Stakeholder
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Type { get; set; }
    }
}