using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TimberV2.Models
{
    public class ItemVM
    {
        public int ItemID { get; set; }
        public string ItemName { get; set; }
        public string ItemArt { get; set; }
        public int ItemPrice { get; set; }
        public string ItemDesc { get; set; }
        public Nullable<int> CategoryID { get; set; }
        public Nullable<int> SupplierID { get; set; }
        public int ItemQuantity { get; set; }

        public virtual Category Category { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DraftCart> DraftCarts { get; set; }
        public virtual Supplier Supplier { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Review> Reviews { get; set; }
        public HttpPostedFileBase Picture { get; set; }
    }
}