   M S B A M L   `   `   `  ���� F  BERP-Desktop, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null?$clr-namespace:ERP_Desktop.ComponentsERP_Desktop.Components  T PMahApps.Metro, Version=2.0.0.0, Culture=neutral, PublicKeyToken=51482d6f650b2b3fV;clr-namespace:MahApps.Metro.Controls;assembly=MahApps.MetroMahApps.Metro.Controls U QLiveCharts.Wpf, Version=0.9.7.0, Culture=neutral, PublicKeyToken=0bc1f845d1ebb8dfG4clr-namespace:LiveCharts.Wpf;assembly=LiveCharts.WpfLiveCharts.Wpf &     ERP_Desktop.Components.Dashboard   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     6local$clr-namespace:ERP_Desktop.Components     Kmah;clr-namespace:MahApps.Metro.Controls;assembly=MahApps.Metro     C	iconPacks-http://metro.mahapps.com/winfx/xaml/iconpacks     Dlvc4clr-namespace:LiveCharts.Wpf;assembly=LiveCharts.Wpf     $	��650��$
��1100��  � 	Resources     LargeTileStyle �� 
TargetType!  MahApps.Metro.Controls.Tile �� Property �� Value&   System.Windows.Media.TextOptions�� %�   &	        �� "  .���  � ��$	 125����  �@ TextFormattingMode$ Display=���  �@ TextRenderingMode$ 	ClearType=���  �@ TitleFontSize$ 14Q���  � ��$	 240��.���  � RowDefinitions �� $
��Auto ��� $��* �.+�� $��0��$��Center=�$	��Top=�$��	0,10,0,10q�.+�  -     Name$ DailySalesTile��$��3q� # MahApps.Brushes.ThemeForeground+  %MahApps.Metro.Controls.ControlsHelper   MouseOverBorderBrush# �  
����� �   HorizontalTitleAlignment$ Right=�#��[  .���� .+��� $��
Daily Sale��$��24(�$
��Bold"�
�������$��Center=�$��Center=��� $��0��$��24(�$
��Bold"�
�������$��Center=�$��Center=�  -   $ DailyProfitTile��$��3q�# �  
����� �#��[  .���� .+��� $��Daily Profit��$��24(�$
��Bold"�
�������$��Center=�$��Center=��� $��0��$��24(�$
��Bold"�
�������$��Center=�$��Center=�  -   $ MonthlySalesTile��$��3q�# �  $ Right=�#��[  .���� .+��� $��Monthly Sale��$��24(�$
��Bold"�
�������$��Center=�$��Center=��� $��0��$��24(�$
��Bold"�
�������$��Center=�$��Center=�  -   $ MonthlyProfitTile��$��3q�# �  #��[  .���� .+��� $��Monthly Profit��$��24(�$
��Bold"�
�������$��Center=�$��Center=��� $��0��$��24(�$
��Bold"�
�������$��Center=�$��Center=��� -   $	��⟳�� � ToolTip$ Refresh Data��$��Right=�$	��Top=�$��14(�$��0,0,10,0q�$��30��$��30��#  LiveCharts.Wpf.CartesianChart  $��1��&   LiveCharts.Wpf.Charts.Base.Chart	   LegendLocation$	 Right=�$��38,45,45,75q�
���   �$��1q�
   Series
 �� *SeriesCollection+   AxisY   LiveCharts.Wpf.Axis     Title$ Values��   LabelFormatter �� *
YFormatter+   AxisX   $ Month��   Labels �� *Labels+