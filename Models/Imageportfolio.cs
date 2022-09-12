using System;
using System.Collections.Generic;

#nullable disable

namespace backend.Models
{
    public partial class Imageportfolio
    {
        public string PortfolioId { get; set; }
        public string ImageId { get; set; }
        public string FileName { get; set; }
        public string Local { get; set; }
    }
}
