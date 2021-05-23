using System;
using System.Collections.Generic;

#nullable disable

namespace backend.Models
{
    public partial class Manualmachine
    {
        public string MachineId { get; set; }
        public string ManualMachineId { get; set; }
        public string Manual { get; set; }
        public DateTime? CreateDate { get; set; }
        public string CreateUser { get; set; }
        public DateTime? EditDate { get; set; }
        public string EditUser { get; set; }

        public virtual Machine Machine { get; set; }
    }
}
