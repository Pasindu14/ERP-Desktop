   M S B A M L   `   `   `  ���� F  BERP-Desktop, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null?$clr-namespace:ERP_Desktop.ComponentsERP_Desktop.Components  T PMahApps.Metro, Version=2.0.0.0, Culture=neutral, PublicKeyToken=51482d6f650b2b3fV;clr-namespace:MahApps.Metro.Controls;assembly=MahApps.MetroMahApps.Metro.Controls /    )ERP_Desktop.Components.InvoiceViewControl   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     6local$clr-namespace:ERP_Desktop.Components     Kmah;clr-namespace:MahApps.Metro.Controls;assembly=MahApps.Metro     .��� .+��� .+���   �� Orientation$  
Horizontal=�$��0,0,0,10q�.+�(  "System.Windows.Controls.DatePicker  -     Name$ 
dpFromDate��$	��200��$��0,0,10,0q�   SelectedDateFormat$ Short=�*  $MahApps.Metro.Controls.TextBoxHelper   	Watermark$ 	From Date��  -   $ dpToDate��$	��200��$��0,0,10,0q�$ Short=�$ To Date���� -   $��Filter��$	��100���� $  
Horizontal=�$��0,0,0,10q�.+��� -    ��Name$ cmbInvoices��$	��200�� �� DisplayMemberPath$ invoice_number��$��0,0,10,0q�$ No data available���� -   $��Generate��$	��100���� -   $��Delete��$	��100��$��20,0,0,0q�
���  ��&   System.Windows.Controls.DataGrid  -     Name$ InvoiceLinesDataGrid��   AutoGenerateColumns$ False��$��Stretch=�$	��Top=�$	��400��
���   �$��1q�   CanUserAddRows$ False��	   Columns	 0  *System.Windows.Controls.DataGridTextColumn  ,  &System.Windows.Controls.DataGridColumn
   Header$
 Product��   Width5  /System.Windows.Controls.DataGridLengthConverter$ * 1  +System.Windows.Controls.DataGridBoundColumn   Binding �� *ProductName+  $
 	Old Price��$ *  �� *
OldPrice+  $
 Current Price��$ *  �� *CurrentPrice+  $
 Quantity��$ *  �� *
Quantity+  $
 Total��$ *  �� *	LineTotal+�� $
��Left=�.+��� -    ��Name$ txtDate��$��16(�$
��Bold"�$��Invoice Date:��$
��Left=�$y�Hidden=� �� TextAlignment$
 Left=��� -	   $ txtTotalAmount��$��16(�$
��Bold"�$��Total: $0.00��$
��Left=�$y�Hidden=�$
 Left=�