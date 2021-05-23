using System;
using System.Collections.Generic;

#nullable disable

namespace backend.Models
{
    public partial class Imagemachine
    {
        public string MachineId { get; set; }
        public string ImageMachineId { get; set; }
        public string FileName { get; set; }
        public string Local { get; set; }

        public virtual Machine Machine { get; set; }
    }
}
