using System;
using System.Collections.Generic;

#nullable disable

namespace backend.Models
{
    public partial class Typenews
    {
        public Typenews()
        {
            News = new HashSet<News>();
        }

        public string TypeNewsId { get; set; }
        public string TypeNews1 { get; set; }

        public virtual ICollection<News> News { get; set; }
    }
}
