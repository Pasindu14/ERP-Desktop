   M S B A M L   `   `   `  ����F  BERP-Desktop, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null?$clr-namespace:ERP_Desktop.ComponentsERP_Desktop.Components  5      T PMahApps.Metro, Version=2.0.0.0, Culture=neutral, PublicKeyToken=51482d6f650b2b3fV;clr-namespace:MahApps.Metro.Controls;assembly=MahApps.MetroMahApps.Metro.Controls 5      U QLiveCharts.Wpf, Version=0.9.7.0, Culture=neutral, PublicKeyToken=0bc1f845d1ebb8dfG4clr-namespace:LiveCharts.Wpf;assembly=LiveCharts.WpfLiveCharts.Wpf 5
      &     ERP_Desktop.Components.Dashboard   5      W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      6local$clr-namespace:ERP_Desktop.Components     5      Kmah;clr-namespace:MahApps.Metro.Controls;assembly=MahApps.Metro     5      C	iconPacks-http://metro.mahapps.com/winfx/xaml/iconpacks     5	      Dlvc4clr-namespace:LiveCharts.Wpf;assembly=LiveCharts.Wpf     5
      $	��650��5      $
��1100��6     � 	Resources  5         LargeTileStyle �� 
TargetType!  MahApps.Metro.Controls.Tile �� Property �� Value&   System.Windows.Media.TextOptions�� 5   
   %#  &	        �� 5      "  6+   .�5      ��  � ��$	 125��6+   �� 5       �@ TextFormattingMode$ Display=�6C   �� 5       �@ TextRenderingMode$ 	ClearType=�6B   �� 5       �@ TitleFontSize$ 14Q�62   �� 5       � ��$	 240��6*   5      5      .��5      �  � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $��* �6   .+�5    
   � $��0��6   $��Center=�5!      $	��Top=�5"      $��	0,10,0,10q�5#      .+�5$        -   5          Name$ DailySalesTile��5$      $��3q�5%       # MahApps.Brushes.ThemeForeground+  %MahApps.Metro.Controls.ControlsHelper   MouseOverBorderBrush# �  5&      
����� �   HorizontalTitleAlignment$ Right=�5(      #��[  5)      .��5*      �� .+�5+      �� $��
Daily Sale��6    $��24(�5,       $
��Bold"�5-       
�������$��Center=�5/       $��Center=�50       �� 51      $��0��6    $��24(�52       $
��Bold"�53       
�������$��Center=�55       $��Center=�56       57      58        5:      -   5        $ DailyProfitTile��5:      $��3q�5;      # �  5<      
����� �#��[  5=      .��5>      �� .+�5?      �� $��Daily Profit��6    $��24(�5@       $
��Bold"�5A       
�������$��Center=�5C       $��Center=�5D       �� 5E      $��0��6    $��24(�5F       $
��Bold"�5G       
�������$��Center=�5I       $��Center=�5J       5K      5L        5N      -   5        $ MonthlySalesTile��5N      $��3q�5O      # �  5P      $ Right=�5Q      #��[  5R      .��5S      �� .+�5T      �� $��Monthly Sale��6    $��24(�5U       $
��Bold"�5V       
�������$��Center=�5X       $��Center=�5Y       �� 5Z      $��0��6    $��24(�5[       $
��Bold"�5\       
�������$��Center=�5^       $��Center=�5_       5`      5a        5c      -   5        $ MonthlyProfitTile��5c      $��3q�5d      # �  5e      #��[  5f      .��5g      �� .+�5h      �� $��Monthly Profit��6    $��24(�5i       $
��Bold"�5j       
�������$��Center=�5l       $��Center=�5m       �� 5n      $��0��6    $��24(�5o       $
��Bold"�5p       
�������$��Center=�5r       $��Center=�5s       5t      5u      �� 5w      -   5        $	��⟳��5w       � ToolTip$ Refresh Data��6!   $��Right=�5x      $	��Top=�61   $��14(�6I   $��0,0,10,0q�5y      $��30��6'   $��30��62   5z      5|      #  LiveCharts.Wpf.CartesianChart  5   
   $��1��6   &   LiveCharts.Wpf.Charts.Base.Chart	   LegendLocation$	 Right=�5�      $��38,45,45,75q�5�      
���   �$��1q�6F   
   Series
 5�      �� *SeriesCollection+   AxisY 5�        LiveCharts.Wpf.Axis     Title$ Values��6      LabelFormatter 6*   �� *
YFormatter+   AxisX 5�        $ Month��6      Labels 6)   �� *Labels+5�      5�      5�      