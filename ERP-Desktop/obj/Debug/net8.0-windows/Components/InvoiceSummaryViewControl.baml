   M S B A M L   `   `   `  ����F  BERP-Desktop, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null?$clr-namespace:ERP_Desktop.ComponentsERP_Desktop.Components  5      T PMahApps.Metro, Version=2.0.0.0, Culture=neutral, PublicKeyToken=51482d6f650b2b3fV;clr-namespace:MahApps.Metro.Controls;assembly=MahApps.MetroMahApps.Metro.Controls 5      6    0ERP_Desktop.Components.InvoiceSummaryViewControl   5      W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      6local$clr-namespace:ERP_Desktop.Components     5      Kmah;clr-namespace:MahApps.Metro.Controls;assembly=MahApps.Metro     5      .��5
      � .+�5   
   �� .+�5      ��   �� Orientation$  
Horizontal=�6   $��0,0,0,10q�62   .+�5      (  "System.Windows.Controls.DatePicker  -   5          Name$ 
dpFromDate��5      $	��200��61   $��0,0,10,0q�6=      SelectedDateFormat$ Short=�5      *  $MahApps.Metro.Controls.TextBoxHelper   	Watermark$ 	From Date��68     5      -   5        $ dpToDate��5      $	��200��6/   $��0,0,10,0q�6;   $ Short=�5      $ To Date��68   �� 5      -   5        $��Filter��5      $	��100��6*   66   5      &   System.Windows.Controls.DataGrid  5      -   5          Name$ InvoicesDataGrid��5         AutoGenerateColumns$ False��61   $��Stretch=�5      $	��Top=�65   $	��400��6M   
���   �$��1q�6n      CanUserAddRows$ False��6�      Columns 5      0  *System.Windows.Controls.DataGridTextColumn  5      ,  &System.Windows.Controls.DataGridColumn   Header$
 Date��6)   	   Width5  /System.Windows.Controls.DataGridLengthConverter$	 * 6P   1  +System.Windows.Controls.DataGridBoundColumn
   Binding
 67   �� *Date+6P     5      $ Total Sales��6)   $	 * 6t   
 6>   �� *
TotalSales+ �� StringFormat$ {0:F2}��6t     5      $ Total Profit��6)   $	 * 6v   
 6?   �� *TotalProfit+$ {0:F2}��6v   5      �� 5       -   5         ��Name$ txtTotalSales��5       $��16(�6/   $
��Bold"�6=   $��
Total:0.00��6O   $
��Left=�6a   $y�Hidden=�6|    �� TextAlignment$
 Left=�6�   �� 5!      -   5        $ txtTotalProfit��5!      $��16(�60   $
��Bold"�6>   $��
Total:0.00��6P   $
��Left=�6b   $y�Hidden=�6}   $
 Left=�6�   5"      5#      5$      