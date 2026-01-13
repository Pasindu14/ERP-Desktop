using ERP_Desktop.DBModels;
using System.ComponentModel;

namespace ERP_Desktop
{
    public static class Wrapper
    {
        // Wrapper for Category display information
        public class CategoryDisplayWrapper
        {
            private readonly tblCategoryMaster _category;

            public CategoryDisplayWrapper(tblCategoryMaster category)
            {
                _category = category;
            }

            public string CatCode => _category.cat_code;
            public string DisplayText => $"{_category.cat_code} - {_category.cat_name}";
        }

        // Wrapper for Product display information
        public class ProductDisplayWrapper
        {
            private readonly tblProductMaster _product;

            public ProductDisplayWrapper(tblProductMaster product)
            {
                _product = product;
            }

            public int ProdCode => _product.prod_code;
            public string ProductName => _product.prod_name;
            public string DisplayText => $"{_product.prod_code_usergen} - {_product.prod_name}";
        }

        // Wrapper for Invoice line item display information
        public class InvoiceLineDisplayWrapper : INotifyPropertyChanged
        {
            private tblInvoiceLine _lineItem;

            public InvoiceLineDisplayWrapper(tblInvoiceLine lineItem, tblProductMaster product)
            {
                _lineItem = lineItem;
                ProductCode = product.prod_code;
                ProductName = product.prod_name;
                Quantity = lineItem.quantity;
                OldPrice = lineItem.old_price;
                CurrentPrice = lineItem.current_price;
                Total = lineItem.line_total;
            }

            public int ProductCode { get; set; }
            public string ProductName { get; }
            public int Quantity { get; set; }
            public decimal OldPrice { get; }

            private decimal _currentPrice;
            public decimal CurrentPrice
            {
                get => _currentPrice;
                set
                {
                    if (_currentPrice != value)
                    {
                        _currentPrice = value;
                        Total = _currentPrice * Quantity;
                        _lineItem.current_price = _currentPrice;
                        _lineItem.line_total = Total;
                        OnPropertyChanged(nameof(CurrentPrice));
                        OnPropertyChanged(nameof(Total));
                    }
                }
            }

            private decimal _total;
            public decimal Total
            {
                get => _total;
                private set
                {
                    _total = value;
                    OnPropertyChanged(nameof(Total));
                }
            }

            public event PropertyChangedEventHandler? PropertyChanged;

            protected void OnPropertyChanged(string propertyName)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        // Wrapper for Purchase Order line item display information
        // Wrapper for Purchase Order line item display information
        public class PurchaseOrderLineDisplayWrapper : INotifyPropertyChanged
        {
            private readonly tblPurchaseOrderLine _lineItem;

            public PurchaseOrderLineDisplayWrapper(tblPurchaseOrderLine lineItem, tblProductMaster product)
            {
                _lineItem = lineItem;
                ProductName = product.prod_name;
                Quantity = lineItem.quantity;
                ProductCode = product.prod_code;
                _unitPrice = lineItem.unit_price;
                _lineTotal = lineItem.line_total;
            }

            public string ProductName { get; }
            public int Quantity { get; set; }

            public int ProductCode { get; set; }

            private decimal _unitPrice;
            public decimal UnitPrice
            {
                get => _unitPrice;
                set
                {
                    if (_unitPrice != value)
                    {
                        _unitPrice = value;
                        _lineItem.unit_price = _unitPrice;

                        LineTotal = _unitPrice * Quantity;
                        _lineItem.line_total = LineTotal;

                        OnPropertyChanged(nameof(UnitPrice));
                        OnPropertyChanged(nameof(LineTotal));
                    }
                }
            }

            private decimal _lineTotal;
            public decimal LineTotal
            {
                get => _lineTotal;
                private set
                {
                    if (_lineTotal != value)
                    {
                        _lineTotal = value;
                        _lineItem.line_total = _lineTotal;
                        OnPropertyChanged(nameof(LineTotal));
                    }
                }
            }

            public event PropertyChangedEventHandler? PropertyChanged;

            protected void OnPropertyChanged(string propertyName)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }

    }
}
