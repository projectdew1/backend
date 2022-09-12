using System;
using System.Collections.Generic;

#nullable disable

namespace backend.Models
{
    public partial class Blog
    {
        public Blog()
        {
            Imageblogs = new HashSet<Imageblog>();
        }

        public string BlogId { get; set; }
        public DateTime? CreateDate { get; set; }
        public string CreateUser { get; set; }
        public DateTime? EditDate { get; set; }
        public string EditUser { get; set; }
        public string BlogSeo { get; set; }
        public string Content { get; set; }
        public string Title { get; set; }

        public virtual ICollection<Imageblog> Imageblogs { get; set; }
    }
}
