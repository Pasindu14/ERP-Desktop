using System;
using System.Collections.Generic;

namespace ERP_Desktop.DBModels;

public partial class tblInvoiceLine
{
    public int line_id { get; set; }

    public int invoice_id { get; set; }

    public int prod_code { get; set; }

    public decimal old_price { get; set; }

    public decimal current_price { get; set; }

    public int quantity { get; set; }

    public decimal line_total { get; set; }

    public virtual tblInvoiceMaster invoice { get; set; } = null!;

    public virtual tblProductMaster prod_codeNavigation { get; set; } = null!;
}
