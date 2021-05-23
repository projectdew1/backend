using System;
using System.Collections.Generic;

#nullable disable

namespace backend.Models
{
    public partial class Typemachine
    {
        public Typemachine()
        {
            Explaintypes = new HashSet<Explaintype>();
            Imagetypes = new HashSet<Imagetype>();
            Machines = new HashSet<Machine>();
        }

        public string TypeId { get; set; }
        public string TypeName { get; set; }
        public DateTime? CreateDate { get; set; }
        public string CreateUser { get; set; }
        public DateTime? EditDate { get; set; }
        public string EditUser { get; set; }
        public string FileImage { get; set; }
        public string LocalImage { get; set; }
        public string CategoryId { get; set; }
        public string TypeSeo { get; set; }

        public virtual Category Category { get; set; }
        public virtual ICollection<Explaintype> Explaintypes { get; set; }
        public virtual ICollection<Imagetype> Imagetypes { get; set; }
        public virtual ICollection<Machine> Machines { get; set; }
    }
}
