using System;
using System.Collections.Generic;

#nullable disable

namespace backend.Models
{
    public partial class Contact
    {
        public int ContactId { get; set; }
        public string ContactName { get; set; }
        public string ContactMail { get; set; }
        public string ContactTel { get; set; }
        public string ContactTitle { get; set; }
        public string ContactDetail { get; set; }
        public int? Status { get; set; }
        public DateTime? CreateDate { get; set; }
    }
}
