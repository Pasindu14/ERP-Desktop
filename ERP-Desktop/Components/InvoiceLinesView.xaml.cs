using ERP_Desktop.DBModels;
using ERP_Desktop.Models;
using MahApps.Metro.Controls;
using System.Collections.Generic;
using System.Windows;

namespace ERP_Desktop.Components
{
    public partial class InvoiceLinesView : MetroWindow
    {
        public InvoiceLinesView(List<InvoiceLineItem> invoiceLines)
        {
            InitializeComponent();
            InvoiceLinesDataGrid.ItemsSource = invoiceLines;
        }
    }
}
