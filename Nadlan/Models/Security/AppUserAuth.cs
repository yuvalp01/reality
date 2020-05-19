using System.Collections.Generic;

namespace Nadlan.Models.Security
{
    public class AppUserAuth
    {
        public AppUserAuth()
        {
            UserName = "Not authorized";
            BearerToken = string.Empty;
        }

        public string UserName { get; set; }
        public string BearerToken { get; set; }
        public bool IsAuthenticated { get; set; }
        public List<AppUserClaim> Claims { get; set; }
        //public bool CanViewReports { get; set; }
        //public bool CanViewExpenses { get; set; }
        //public bool CanConfirmExpenses { get; set; }
        //public bool CanViewTransactions { get; set; }
        //public bool CanAccessRest { get; set; }
    }
}
