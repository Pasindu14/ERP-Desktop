using System;
using System.Collections.Generic;

namespace ERP_Desktop.DBModels;

public partial class tblPurchaseOrderMaster
{
    public int purchase_order_id { get; set; }

    public string purchase_order_number { get; set; } = null!;

    public DateOnly purchase_order_date { get; set; }

    public decimal total_amount { get; set; }

    public int status { get; set; }

    public virtual ICollection<tblPurchaseOrderLine> tblPurchaseOrderLine { get; set; } = new List<tblPurchaseOrderLine>();
}
