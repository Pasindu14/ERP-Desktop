using System;
using System.Collections.Generic;

namespace ERP_Desktop.DBModels;

public partial class tblInvoiceMaster
{
    public int invoice_id { get; set; }

    public string invoice_number { get; set; } = null!;

    public DateOnly invoice_date { get; set; }

    public decimal total_amount { get; set; }

    public int? status { get; set; }

    public virtual ICollection<tblInvoiceLine> tblInvoiceLine { get; set; } = new List<tblInvoiceLine>();
}
