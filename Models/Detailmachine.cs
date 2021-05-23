using System;
using System.Collections.Generic;

#nullable disable

namespace backend.Models
{
    public partial class Detailmachine
    {
        public string MachineId { get; set; }
        public string DetailMachineId { get; set; }
        public string Detail { get; set; }
        public DateTime? CreateDate { get; set; }
        public string CreateUser { get; set; }
        public DateTime? EditDate { get; set; }
        public string EditUser { get; set; }

        public virtual Machine Machine { get; set; }
    }
}
