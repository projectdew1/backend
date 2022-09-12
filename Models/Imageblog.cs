using System;
using System.Collections.Generic;

#nullable disable

namespace backend.Models
{
    public partial class Imageblog
    {
        public string BlogId { get; set; }
        public string ImageId { get; set; }
        public string FileName { get; set; }
        public string Local { get; set; }

        public virtual Blog Blog { get; set; }
    }
}
