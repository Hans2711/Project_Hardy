   M S B A M L   `   `   `  ����H  DProject_Hardy, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullC&clr-namespace:Project_Hardy.PropertiesProject_Hardy.Properties  5   	   -clr-namespace:Project_HardyProject_Hardy  5   	       Project_Hardy.ProjectList   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   4p&clr-namespace:Project_Hardy.Properties     5   	   -localclr-namespace:Project_Hardy     5   	   -   5        (   "Project_Hardy.Properties.Resources    ProjectList � Title# Z  5	   	   $	��500��64   $	��850��6A   .��5
      �  � ColumnDefinitions 5   
   �� 5      $��1* �6   �� 5      $��1* �6   �� 5      $��1* �6   �� 5      $��1* �6    � RowDefinitions 5   
   �� 5      $��3* �6   �� 5      $��1* �6   .+�5   
   &   System.Windows.Controls.DataGrid  -   5          Name$ projectsDataGrid��5   P   $��0��6   $��4��6       AutoGenerateColumns .     
IsReadOnly . $	��Top=�6|   $��18(�6�      Columns 5      0  *System.Windows.Controls.DataGridTextColumn  5      	   Id,  &System.Windows.Controls.DataGridColumn	   Header#	 Z 6%   1  +System.Windows.Controls.DataGridBoundColumn
   Binding
 6H   �� *id+6a     5         Description#	 Z 6%   
 6Q   �� *description+6s     5         	Starttime#	 Z 6%   
 6O   �� *	starttime+6o     5         Endtime#	 Z 6%   
 6M   �� *	endtime+6k     5         ProjectManager#	 Z 6%   
 6T   �� *project_manager.fullname+6�     5         Steps#	 Z 6%   
 6K   �� *	stepCount+6k   5      �� 5!   
   -   5         ��Name$ 	newButton��5!         NoMouseOverButtonTemplate#��[  6$   $��1��6Z   ��5"      �� 5#       �� ImageSource$ /addButton.png��6    j� Stretch$ Uniform=�6:   5%      �� 5&   
   -   5        $ 
editButton��5&      #��[  6%   $��1��6[   $��1��6h   ��5'      �� 5(      $ /editButton.png��6   $ Uniform=�6;   5*      �� 5+   
   -   5        $ deleteButton��5+      #��[  6'   $��1��6]   $��2��6j   ��5,      �� 5-      $ /deleteButton.png��6   $ Uniform=�6=   5/      �� 50   
   -   5        $ ganttDiagrammButton��50      #��[  6.   $��1��6d   $��3��6q   ��51      �� 52      $ /gantt_diagramm_logo.png��6   $ Uniform=�6D   54      56      57      