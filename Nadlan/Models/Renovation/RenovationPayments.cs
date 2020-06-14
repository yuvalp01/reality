using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Nadlan.Models.Renovation
{
    //[Table("renovationPayments")]
    [Table("payments", Schema = "renovation")]
    public class RenovationPayment
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime? DatePayment { get; set; }
        public decimal Amount { get; set; }
        public string Criteria { get; set; }
        public string Comments { get; set; }
        //public bool CheckIdWriten { get; set; }
        //public bool CheckInvoiceScanned { get; set; }
        public bool IsConfirmed { get; set; }
        public bool IsDeleted { get; set; }

        public RenovationProject RenovationProject { get; set; }
        public int RenovationProjectId { get; set; }
    }


}
