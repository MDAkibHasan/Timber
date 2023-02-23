using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TimberV2.Models
{
    public class TempUser
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email")]
        public string UserEmail { get; set; }
        [Display(Name = "PassWord")]
        [DataType(DataType.Password)]
        public string UserPassword { get; set; }
     

    }
}