   M S B A M L   `   `   `  ����F  BERP-Desktop, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null?$clr-namespace:ERP_Desktop.ComponentsERP_Desktop.Components  5      T PMahApps.Metro, Version=2.0.0.0, Culture=neutral, PublicKeyToken=51482d6f650b2b3fV;clr-namespace:MahApps.Metro.Controls;assembly=MahApps.MetroMahApps.Metro.Controls 5	      5    /ERP_Desktop.Components.PurchaseOrderViewControl   5      W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      6local$clr-namespace:ERP_Desktop.Components     5      Kmah;clr-namespace:MahApps.Metro.Controls;assembly=MahApps.Metro     5	      .��5
      � .+�5   
   �� .+�5      ��   �� Orientation$  
Horizontal=�6   $��0,0,0,10q�62   .+�5      (  "System.Windows.Controls.DatePicker  -   5          Name$ 
dpFromDate��5      $	��200��61   $��0,0,10,0q�6=      SelectedDateFormat$ Short=�5      *  $MahApps.Metro.Controls.TextBoxHelper   	Watermark$ 	From Date��68     5      -   5        $ dpToDate��5      $	��200��6/   $��0,0,10,0q�6;   $ Short=�5      $ To Date��68   �� 5      -   5        $��Filter��5      $	��100��6*   66   5      �� 5      $  
Horizontal=�6   $��0,0,0,10q�62   .+�5      �� -   5         ��Name$ cmbPurchaseOrders��5      $	��200��66    �� DisplayMemberPath$ purchase_order_number��6B   $��0,0,10,0q�5      $ No data available��6-   �� 5      -   5        $��Generate��5      $	��100��6,   68   �� 5      -   5        $��Delete��5      $	��100��6*   $��20,0,0,0q�6R   
���  ��6d   5      &   System.Windows.Controls.DataGrid  5      -   5          Name$  PurchaseOrderLinesDataGrid��5         AutoGenerateColumns$ False��6;   $��Stretch=�5      $	��Top=�65   $	��400��6M   
���   �$��1q�6n      CanUserAddRows$ False��6�   	   Columns	 5       0  *System.Windows.Controls.DataGridTextColumn  5!      ,  &System.Windows.Controls.DataGridColumn
   Header$
 Product Code��6)      Width5  /System.Windows.Controls.DataGridLengthConverter$ * 6]   1  +System.Windows.Controls.DataGridBoundColumn   Binding 6?   �� *	prod_code+6]     5"      $
 
Unit Price��6)   $ * 6\    6=   �� *
unit_price+6\     5#      $
 Quantity��6)   $ * 6X    6;   �� *
quantity+6X     5$      $
 Total��6)   $ * 6W    68   �� *
line_total+6W   5&      �� 5(      $
��Left=�6   .+�5*      �� -   5         ��Name$ txtDate��5*      $��16(�6-   $
��Bold"�6;   $��Order Date:��6M   $
��Left=�6`   $y�Hidden=�6{    �� TextAlignment$
 Left=�6�   �� 5+      -	   5        $ txtTotalAmount��5+      $��16(�64   $
��Bold"�6B   $��Total: $0.00��6T   $
��Left=�6h   $y�Hidden=�6�   $
 Left=�6�   5,      5-      5.      5/      