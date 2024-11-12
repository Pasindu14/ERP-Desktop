using System;
using System.Collections.Generic;

namespace ERP_Desktop.DBModels;

public partial class tblPurchaseOrderLine
{
    public int line_id { get; set; }

    public int purchase_order_id { get; set; }

    public int prod_code { get; set; }

    public decimal unit_price { get; set; }

    public int quantity { get; set; }

    public decimal line_total { get; set; }

    public virtual tblProductMaster prod_codeNavigation { get; set; } = null!;

    public virtual tblPurchaseOrderMaster purchase_order { get; set; } = null!;
}
