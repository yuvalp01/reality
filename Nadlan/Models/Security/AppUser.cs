using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Nadlan.Models.Security
{

    [Table("users",Schema ="secure")]
    public class AppUser
    {

        [Required()]
        [Key()]
        public Guid UserId { get; set; }

        [Required()]
        [StringLength(255)]
        public string  UserName { get; set; }


        [Required()]
        [StringLength(255)]
        public string Password { get; set; }


    }


}
