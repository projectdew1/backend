using System;
using System.Collections.Generic;

#nullable disable

namespace backend.Models
{
    public partial class User
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Salt { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? LastLogin { get; set; }
        public string CreateUser { get; set; }
    }
}
