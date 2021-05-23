using System;
using System.Collections.Generic;

#nullable disable

namespace backend.Models
{
    public partial class Technically
    {
        public Technically()
        {
            Detailtechmachines = new HashSet<Detailtechmachine>();
        }

        public string TechnicallyId { get; set; }
        public string TechnicallyName { get; set; }

        public virtual ICollection<Detailtechmachine> Detailtechmachines { get; set; }
    }
}
