using System;
using System.Collections.Generic;

#nullable disable

namespace backend.Models
{
    public partial class News
    {
        public News()
        {
            Imagenews = new HashSet<Imagenews>();
        }

        public string NewsId { get; set; }
        public DateTime? CreateDate { get; set; }
        public string CreateUser { get; set; }
        public DateTime? EditDate { get; set; }
        public string EditUser { get; set; }
        public string NewsSeo { get; set; }
        public string Content { get; set; }
        public string Title { get; set; }
        public string FileImage { get; set; }
        public string LocalImage { get; set; }
        public string TypeNewsId { get; set; }

        public virtual Typenews TypeNews { get; set; }
        public virtual ICollection<Imagenews> Imagenews { get; set; }
    }
}
