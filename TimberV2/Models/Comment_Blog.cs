using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TimberV2.Models
{
    public class Comment_Blog
    {
        public Blog BlogView { get; set; }
        public IEnumerable<Comment> CommentView { get; set; }
    }
}