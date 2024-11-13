using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_Desktop.Models
{
    public class DailySalesReport
    {
        public DateOnly Date { get; set; }
        public int TotalQuantity { get; set; }
        public decimal TotalSales { get; set; }
        public decimal TotalProfit { get; set; }
    }

}
