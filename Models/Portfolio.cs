using System;
using System.Collections.Generic;

#nullable disable

namespace backend.Models
{
    public partial class Portfolio
    {
        public string MachineId { get; set; }
        public string PortfolioId { get; set; }
        public string Seo { get; set; }
        public string Title { get; set; }
        public string FileImage { get; set; }
        public string LocalImage { get; set; }
        public string CreateUser { get; set; }
        public DateTime? CreateDate { get; set; }
        public string EditUser { get; set; }
        public DateTime? EditDate { get; set; }

        public virtual Machine Machine { get; set; }
    }
}
