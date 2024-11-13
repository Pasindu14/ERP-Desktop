using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_Desktop.Models
{
    public class InvoiceLineItem
    {
        public int ProdCode { get; set; }
        public int Quantity { get; set; }
        public decimal OldPrice { get; set; }
        public decimal CurrentPrice { get; set; }
        public decimal LineTotal { get; set; }
        public string? ProductName { get; set; }
    }

}
