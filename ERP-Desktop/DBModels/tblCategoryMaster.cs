using System;
using System.Collections.Generic;

namespace ERP_Desktop.DBModels;

public partial class tblCategoryMaster
{
    public string cat_code { get; set; } = null!;

    public string cat_name { get; set; } = null!;

    public int cat_status { get; set; }

    public virtual ICollection<tblProductMaster> tblProductMaster { get; set; } = new List<tblProductMaster>();
}
