using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JobPortal2.Models
{
    public class UserCredentialModel
    {
        [Display(Name = "User ID")]
        [Required]
        public int userId { get; set; }

        [Display(Name = "Password")]
        [Required]
        public string password { get; set; }

        //[Display(Name = "RoleName")]
        //public Nullable<int> RoleId { get; set; }
    }
}