using System;
using System.Collections.Generic;

#nullable disable

namespace backend.Models
{
    public partial class Imagetype
    {
        public string TypeId { get; set; }
        public string ImagetypeId { get; set; }
        public string FileName { get; set; }
        public string Local { get; set; }

        public virtual Typemachine Type { get; set; }
    }
}
