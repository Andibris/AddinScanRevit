   M S B A M L   `   `   `  ����  ParameterScannerRevit    ParameterScannerRevit.UI   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      -   5        $	��350��5      $	��400��6     � WindowStartupLocation$  CenterScreen=�5       � Title$ Parameter Scanner��5
       � 
ResizeMode$ NoResize=�5      .��5      �  � RowDefinitions 5   
   �� 5      $	��100 �6   6+   �� 5      $��50 �6   6*   �� 5      $��50 �6   6*   �� 5      $��* �6   6)   .+�5   
   �� $��0��6   $��155,10,155,10q�6   $��Uniform=�64   $��images/icon64.png��6F   �� 5   
   $��1��6   $��Parameter Name:��6   $��Center=�67   $��Right=�6Y   
���   �$��
10,0,250,0q�6�   $��50��6�   $��Center=�6�   6�   �� 5   
   -   5        $��TxtName��5      $��1��6   $��Center=�6.   $��Center=�6P   
���   �$��155,10,30,10q�6�   6�   �� 5   
   $��2��6   $��Parameter Value:��6   $��Center=�68   $��Right=�6Z   
���   �$��
10,0,250,0q�6�   $��50��6�   $��Center=�6�   6�   �� 5   
   -   5        $��TxtValue��5      $��2��6   $��Center=�6/   $��Center=�6Q   
���   �$��155,10,30,10q�6�   6�   �� 5   
   -   5        $��
BtnIsolate��5      $��3��6   $��Isolate in View��60   $��Center=�6J   
���   �$��
30,0,230,0q�6�   $��50��6�   $��Center=�6�   6�   �� 5   
   -   5        $��	BtnSelect��5      $��3��6   $��Select��6/   $��Center=�6@   
���   �$��
230,0,30,0q�6w   $��50��6�   6�   5      5      