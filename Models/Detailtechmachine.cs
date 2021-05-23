using System;
using System.Collections.Generic;

#nullable disable

namespace backend.Models
{
    public partial class Detailtechmachine
    {
        public string MachineId { get; set; }
        public string DetailTechMachineId { get; set; }
        public string DetailTech { get; set; }
        public DateTime? CreateDate { get; set; }
        public string CreateUser { get; set; }
        public DateTime? EditDate { get; set; }
        public string EditUser { get; set; }
        public string TechnicallyId { get; set; }

        public virtual Technically Technically { get; set; }
    }
}
