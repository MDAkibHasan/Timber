using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TimberV2.Models
{
    public class BlogVM
    {
        public int BlogID { get; set; }
        public string Title { get; set; }
        public string Topic { get; set; }
        public string Body { get; set; }
       
        public string BlogArt { get; set; }
        public HttpPostedFileBase Picture { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Comment> Comments { get; set; }
    }
}