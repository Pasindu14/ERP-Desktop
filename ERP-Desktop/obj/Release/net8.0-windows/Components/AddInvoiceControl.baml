   M S B A M L   `   `   `  ���� T  PMahApps.Metro, Version=2.0.0.0, Culture=neutral, PublicKeyToken=51482d6f650b2b3fV;clr-namespace:MahApps.Metro.Controls;assembly=MahApps.MetroMahApps.Metro.Controls   ERP-Desktop.   (ERP_Desktop.Components.AddInvoiceControl   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     Kmah;clr-namespace:MahApps.Metro.Controls;assembly=MahApps.Metro     .��� .+��� $
��Left=�.+���   �� Orientation$  
Horizontal=�.+��� -    ��Name$ txtInvoiceNumber��$	��200��$��10q�*   $MahApps.Metro.Controls.TextBoxHelper   	Watermark$ Enter Invoice Number��(  "System.Windows.Controls.DatePicker  -     Name$ dpInvoiceDate��$	��200��$��10q��� $  
Horizontal=�.+��� -    ��Name$ 
cmbProduct��$	��200��$��10q� �� DisplayMemberPath$ DisplayText��$ Select Product���� -   $ txtQuantity��$	��200��$��10q�$ Enter Quantity���� -   $��Add Product��$	��200��$��10q�&   System.Windows.Controls.DataGrid  -     Name$ InvoiceDataGrid��$��10q�   AutoGenerateColumns$ False��$��Stretch=�$	��Top=�$	��400��
���   �$��1q�   CanUserAddRows$ False��	   Columns	 0  *System.Windows.Controls.DataGridTextColumn  ,  &System.Windows.Controls.DataGridColumn
   Header$
 Product��   Width5  /System.Windows.Controls.DataGridLengthConverter$ * 1  +System.Windows.Controls.DataGridBoundColumn   Binding �� *ProductName+  $
 Quantity��$ *  �� *
Quantity+  $
 	Old Price��$ *  �� *
OldPrice+  $
 Current Price��$ *  �� *CurrentPrice+ �� Mode$ TwoWay=� �� UpdateSourceTrigger$ PropertyChanged=�  $
 Total��$ *  �� *Total+�� .+��� -   $��Create Invoice��$	��200��$��10q�