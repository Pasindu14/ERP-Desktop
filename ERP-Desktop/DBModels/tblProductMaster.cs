using System;
using System.Collections.Generic;

namespace ERP_Desktop.DBModels;

public partial class tblProductMaster
{
    public int prod_code { get; set; }

    public string prod_code_usergen { get; set; } = null!;

    public string prod_name { get; set; } = null!;

    public string? prod_desc { get; set; }

    public decimal? prod_cost_price { get; set; }

    public decimal? prod_sales_price { get; set; }

    public string? prod_cat { get; set; }

    public decimal? stock { get; set; }

    public int? prod_status { get; set; }

    public virtual tblCategoryMaster? prod_catNavigation { get; set; }

    public virtual ICollection<tblInvoiceLine> tblInvoiceLine { get; set; } = new List<tblInvoiceLine>();
}
