using System;
using System.Collections.Generic;

#nullable disable

namespace backend.Models
{
    public partial class Explaintype
    {
        public string TypeId { get; set; }
        public string ExplainTypeId { get; set; }
        public string ExplainDetail { get; set; }
        public DateTime? CreateDate { get; set; }
        public string CreateUser { get; set; }
        public DateTime? EditDate { get; set; }
        public string EditUser { get; set; }

        public virtual Typemachine Type { get; set; }
    }
}
