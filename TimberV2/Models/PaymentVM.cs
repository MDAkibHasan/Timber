using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TimberV2.Models
{
    public class PaymentVM
    {   [Required]
        [DataType(DataType.Text)]
        public string TrxID { get; set; }
        [Required]
        [DataType(DataType.Text)]
        public string Type { get; set; }
        [Required]
        [MinLength(11, ErrorMessage = "Minimum 11 charecters needed")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "PhoneNo must be numeric")]
        public string PhoneNo { get; set; }
    }
  


}