using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Nadlan.Models.Security
{

    [Table("userClaims",Schema = "secure")]
    public class AppUserClaim
    {

        [Required()]
        [Key()]
        public Guid ClaimId { get; set; }

        [Required()]
        public Guid  UserId { get; set; }

        [Required()]
        public string ClaimType { get; set; }

        [Required()]
        public string ClaimValue { get; set; }


    }


}
