using System;
using System.Collections.Generic;

#nullable disable

namespace backend.Models
{
    public partial class Imagenews
    {
        public string NewsId { get; set; }
        public string ImagenewsId { get; set; }
        public string FileName { get; set; }
        public string Local { get; set; }

        public virtual News News { get; set; }
    }
}
