   M S B A M L   `   `   `  ????C  ?WaterIot, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null#clr-namespace:WaterIotWaterIot  5   	   -clr-namespace:WaterIot.BaseWaterIot.Base  5   	       WaterIot.MainWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   V RMaterialDesignThemes.Wpf, Version=3.1.3.1513, Culture=neutral, PublicKeyToken=nullFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5   	   "localclr-namespace:WaterIot  5   	   &Baseclr-namespace:WaterIot.Base  5   	      MaterialDesignBody#???   5
   	   $??Regular"?5   	   $??16(?5   	   &   System.Windows.Media.TextOptions    TextFormattingMode$  Ideal=?5   	      TextRenderingMode$
 Auto=?5   	     MaterialDesignPaper#???  5   	    ? Title$ 
MainWindow??5   	   $	??800??6   $
??1280??6)   ??5   	   :  4MaterialDesignThemes.Wpf.MaterialDesignFontExtension   ? 	Resources 5          WaterIot.Base.BindingProxy  vm   Data'  !MaterialDesignThemes.Wpf.PackIcon ?? 
TargetType ?? Property ?? Value??%?   &	       '         5   
    6'   ?? ?? 5   
   "  6   .?5      ??  ? ??$ 25??6&   ?? 5       ? ??$ 25??6'   ?? 5       ? ??$ 0q?6'   5      5       ? DataContext 5      "   WaterIot.MainWindowViewModel  5   
   .??5      )  #MaterialDesignThemes.Wpf.DialogHost  .??5   
   )  #MaterialDesignThemes.Wpf.DrawerHost  .??5      ]? .+?5       (  "MaterialDesignThemes.Wpf.ColorZone  $	??Top=?6+   	   Mode$	 
PrimaryMid=?6@   $??16q?6R   .??5!      ]? .+?5"      '	  !MaterialDesignThemes.Wpf.PopupBox	  $??Right=?62   
 	  PlacementMode$
 BottomAndAlignRightEdges=?6I    	  PopupContent. 5#      ?? .@?5$   "   ?? $
??Exit??6)   ??68   ?? *Exit+5%      5&      ?? 5'      $
??Left=?6$   $??Center=?6:   $??Center=?6W   $??22(?6r   Title6?   5(      5)        5*      $??Bottom=?6+   +
  %MaterialDesignThemes.Wpf.ShadowAssist 
  ShadowDepth$ Depth2=?6C   $	 Accent=?6t   $??5q?6?   .??5+      ?? $??Right=?6!    ?? Orientation$ 
Horizontal=?6=   .+?5,      ?? $??14(?6J   $
??10,0q?6X   ??6$   ?? *	NowTime+ ?? Mode$ OneWay=?6X   5-      5.      ? 5/      $??3q?6    ? ColumnDefinitions 50      ?? 51      $??4* ?6+   ?? 52      $??1* ?6+   .+?54      &   System.Windows.Controls.DataGrid  $??16(?6      CanUserAddRows .     AutoGenerateColumns .     
IsReadOnly .    RowDetailsVisibilityMode$ Visible=?6?   ??6-   ?? *	Devices+   Columns 55      0  *System.Windows.Controls.DataGridTextColumn  56      ,  &System.Windows.Controls.DataGridColumn   Header$ device??61   1  +System.Windows.Controls.DataGridBoundColumn   Binding 6A   ?? *
DeviceID+  57      $
 name??61    6?   ?? *Name+  58      $ C__name??61    6B   ?? *CName+  59      $ 	longitude??61    6D   ?? *	Longitude+ ?? StringFormat$ {0:F4}??  5:      $ latitude??61    6C   ?? *
Latitude+$ {0:F4}??  5;      $ create??61    6A   ?? *
CreateTime+   RowDetailsTemplate 5E      ?? 5F      .R?5G   "   ? $	??0,3q?6'   .+?5H   &   ?? $
??10,0q?6S   ??63   ?? *	Records+??5I   *   ?? 5J   .   .R?5K   2   ]?  5L   6    % !MaterialDesignFloatingHintTextBox ?? BasedOn)  #MaterialDesignThemes.Wpf.HintAssist  SecondaryAccentBrush.  (MaterialDesignThemes.Wpf.TextFieldAssist & "MaterialDesignFloatingHintComboBox " MaterialDesignFlatAccentButton??%?  '??       '?? q     7  7  7  '?? A    7  7  7  '?? ?    7  ?? 5M   :   " ??6@   .?5N   >   ??  ? ??$	 8,3q?6W   ?? 5O   >    ? ??$ Center=?6b   5P   ;   ?? 5Q   :   " ??6@   8   .?5R   >   ??  ? ??$	 8,0q?6W   ?? 5S   >    ? ??$ Center=?6b   ?? 5T   >    ?@ 
Foreground8  6u   ?? 5U   >    ?@ UnderlineBrush8  6~   5V   ;   ?? 5W   :   " ??6@   8   .?5X   >   ??  ? ??$	 8,0q?6W   ?? 5Y   >    ?@ 
Foreground8  6u   ?? 5Z   >    ?@ UnderlineBrush8  6~   5[   ;   ?? 5\   :   " ??6@   8   .?5]   >   ??  ? ??$ 30??6W   ?? 5^   >    ? ??$ 30??6V   ?? 5_   >    ? ??$ 3q?6W   ?? 5`   >    ? ??$ 0q?6X   5a   ;   5b   7   .+?5c   6   ?? $??Right=?6A   $ 
Horizontal=?6X   .+?5d   :   ?? ??6A   ?? *Data.UploadRecord+ ?? Source# [ ??6?   ?? .??5e   >        Kind$ Upload=?6V   5f   ;   ?? 5j   :   ??6A   ?? *Data.RefreshRecord+# [ ??6?   ?? .??5k   >     $ Download=?6V   5l   ;   5m   7   ?? 5n   6   $
??Left=?6A   $ 
Horizontal=?6W   .+?5o   :   ??    MaterialDesignAccentCheckBox#??[ 6C   $	??No.??6?    d? 	IsChecked 6y   ?? *Upload+6?   ?? 5p   :      Hint$
 UUID??6B   $	??330??6~   ??6h   ?? *Guid+.??6?    5q   >   ??%"   '        ?? 5r   B   "  6H   5s   ?   .??6Q      HasClearButton . .??5t   ?   5u   ;   ?? 5v   :   $ Value??6B   $??70??6?    ?? 
IsReadOnly . ??6i   ?? *Value+6?   ?? 5w   :   $ DateTime??6B    . ??6l   ?? *LastUploadTime+6?   5x   7   5y   3   5z   /   5|   '   5}   #   5~      5?      ? 5?      $??1??6   $??10q?6+    5?        MaterialDesignRaisedButton??%u  '??       '?? o     7  7  7  '??     7  7  7  '?? ?    7  ?? 5?      " ??6$   .?5?   "   ??  ? ??$ 3q?6;   ?? 5?   "    ? ??$ Center=?6F   5?      ?? 5?      " ??6$   8   .?5?   "   ??  ? ??$
 3,10q?6;   ?? 5?   "    ?@ 
Foreground8  6Y   ?? 5?   "    ?@ UnderlineBrush8  6b   5?      ?? 5?      " ??6$   8   .?5?   "   ??  ? ??$
 3,10q?6;   ?? 5?   "    ?@ 
Foreground8  6Y   ?? 5?   "    ?@ UnderlineBrush8  6b   5?      ?? 5?      " ??6$   8   .?5?   "   ??  ? ??$ 40??6;   ?? 5?   "    ? ??$ 3q?6;   5?      5?      .+?5?      ?? .+?5?      ?? #??[ 6'   $??On‑Line State??6]    6u   ?? *IsConnected+?? 5?      #??[ 6'   $??Auto Upload??6]    6s   ?? *AutoEnabled+?? 5?      #??[ 6'   $!??Upload the Alternative Data??6]    6?   ?? *FakeDataEnabled+?? 5?      $ Auto Interval (Sec.)??6&   ??6\   ?? *AutoInterval+?? 5?      $# Flooding Auto Interval (Sec.)??6&   ??6e   ?? *FloodingAutoInterval+?? 5?      ??6%   ?? *
SaveConfig+.??5?   "   ?? $ 
Horizontal=?6-   .+?5?   &     $ ContentSave=?6>   ?? 5?   &   ?? 61   $?
SaveConfig??65   6J   5?   #   5?      ?? 5?      ??6%   ?? *	UploadAll+.??5?   "   ?? $ 
Horizontal=?6-   .+?5?   &     $ UploadMultiple=?6>   ?? 5?   &   ?? 61   $?
All Upload??65   6J   5?   #   5?      ?? 5?      ??6%   ?? *
RefreshAll+.??5?   "   ?? $ 
Horizontal=?6-   .+?5?   &     $ DownloadMultiple=?6>   ?? 5?   &   ?? 61   $?All Download??65   6L   5?   #   5?      ?? 5?      ??6%   ?? *TestConnect+.??5?   "   ?? $ 
Horizontal=?6-   .+?5?   &     $ DownloadMultiple=?6>   ?? 5?   &   ?? 61   $?On-Line??65   6G   5?   #   5?      ?? 5?      ??6%   ?? *TestDisconnect+.??5?   "   ?? $ 
Horizontal=?6-   .+?5?   &     $ DownloadMultiple=?6>   ?? 5?   &   ?? 61   $?Off-Line??65   6H   5?   #   5?      5?      5?      5?      5?      5?      5?      5?      