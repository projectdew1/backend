using System;
using System.Collections.Generic;

#nullable disable

namespace backend.Models
{
    public partial class Category
    {
        public Category()
        {
            Typemachines = new HashSet<Typemachine>();
        }

        public string CategoryId { get; set; }
        public string CategoryName { get; set; }
        public DateTime? CreateDate { get; set; }
        public string CreateUser { get; set; }
        public DateTime? EditDate { get; set; }
        public string EditUser { get; set; }
        public string FileImage { get; set; }
        public string LocalImage { get; set; }
        public string Seo { get; set; }

        public virtual ICollection<Typemachine> Typemachines { get; set; }
    }
}
