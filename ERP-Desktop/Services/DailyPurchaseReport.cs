using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_Desktop.Services
{
    public class DailyPurchaseReport
    {
        public DateOnly Date { get; set; }
        public int TotalQuantity { get; set; }
        public decimal TotalPurchaseAmount { get; set; }
    }

}
