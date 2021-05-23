using System;
using System.Collections.Generic;

#nullable disable

namespace backend.Models
{
    public partial class Videomachine
    {
        public string MachineId { get; set; }
        public string VideoMachineId { get; set; }
        public string Link { get; set; }

        public virtual Machine Machine { get; set; }
    }
}
