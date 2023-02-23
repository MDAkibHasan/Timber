using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TimberV2.Models
{
    public class cat_item
    {
       public IEnumerable<Item> ItemView { get; set; }
        public IEnumerable<Category> CategoryView { get; set; }
    }
}