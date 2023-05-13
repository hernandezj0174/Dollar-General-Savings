using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebScraper.Classes
{
    public class DollarGeneral
    {
        public List<Category> Category { get; set; }

        public List<Brands> Brands{ get; set; }

        public List<Coupons> Coupons { get; set; }

        public PaginationInfo PaginationInfo { get; set; }
    }
}
