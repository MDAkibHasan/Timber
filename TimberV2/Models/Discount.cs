//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TimberV2.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Discount
    {
        public int DiscountID { get; set; }
        public int OfferID { get; set; }
        public int ItemID { get; set; }
        public int DiscountPercent { get; set; }
    
        public virtual Item Item { get; set; }
        public virtual Offer Offer { get; set; }
    }
}
