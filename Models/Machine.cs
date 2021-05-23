using System;
using System.Collections.Generic;

#nullable disable

namespace backend.Models
{
    public partial class Machine
    {
        public Machine()
        {
            Detailmachines = new HashSet<Detailmachine>();
            Explaimmachines = new HashSet<Explaimmachine>();
            Imagemachines = new HashSet<Imagemachine>();
            Manualmachines = new HashSet<Manualmachine>();
            Videomachines = new HashSet<Videomachine>();
        }

        public string MachineId { get; set; }
        public int? Price { get; set; }
        public int? Discount { get; set; }
        public sbyte? Soldout { get; set; }
        public string ItemsCode { get; set; }
        public string MachineName { get; set; }
        public DateTime? CreateDate { get; set; }
        public string CreateUser { get; set; }
        public DateTime? EditDate { get; set; }
        public string EditUser { get; set; }
        public string EditDiscountUser { get; set; }
        public DateTime? EditDiscountDate { get; set; }
        public string TypeId { get; set; }
        public string FileImage { get; set; }
        public string LocalImage { get; set; }
        public string MachineSeo { get; set; }
        public string Machinecol { get; set; }

        public virtual Typemachine Type { get; set; }
        public virtual ICollection<Detailmachine> Detailmachines { get; set; }
        public virtual ICollection<Explaimmachine> Explaimmachines { get; set; }
        public virtual ICollection<Imagemachine> Imagemachines { get; set; }
        public virtual ICollection<Manualmachine> Manualmachines { get; set; }
        public virtual ICollection<Videomachine> Videomachines { get; set; }
    }
}
