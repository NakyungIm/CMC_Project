   M S B A M L   `   `   `  ����F  BCMC_Project, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null5clr-namespace:CMC_Project.ViewsCMC_Project.Views  5      &     CMC_Project.Views.ConvertionPage   5      W SPresentationCore, Version=6.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=6.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      1localclr-namespace:CMC_Project.Views     5        N� Title$  ConvertionPage��5	      .��5      � $��170,10,168,0q�6    � ColumnDefinitions 5   
   �� 5      $��0* �6   �� 5      .+�5   
   �� -   5         ��Name$ BidOpenFile��5      $��
Open files��6;   $��1��6P   $	��100��6`   $��25��6l   $	��Top=�6x   $��Right=�6�   �� 5   
   $$��1. 공내역 BID 파일 추가��6   $��31��6+   $��15(�67   $��1��6E   $	��Top=�6U   $
��Left=�6m   �� 5   
   -   5        $��BIDList��5      $��1��6!   $��Center=�61   $��
0,31,0,313q�6N   $	��442��6b   �� 5   
   -   5        $ XlsOpenFile��5      $��
Open files��6;   $��1��6P   $	��100��6`   $��25��6l   $	��Top=�6x   $��Right=�6�   $��	0,129,0,0q�6�   �� 5   
   $,��&2. 조사내역서(.xls) 파일 추가��6   $��31��6/   $��15(�6;   $��1��6I   $	��Top=�6Y   $
��Left=�6q   $��	0,129,0,0q�6�   �� 5   
   -   5        $��XlsList��5      $��1��6!   $��Center=�61   $��0,160,0,184q�6N   $	��442��6c   �� 5   
   -   5        $ ConvertButton��5      $��단가 세팅��6C   $��1��6S   $	��100��6c   $��25��6o   $	��Top=�6{   $��Right=�6�   $��	0,267,0,0q�6�   �� 5   
   $��3. 단가세팅��6   $��31��6"   $��15(�6.   $��1��6<   $	��Top=�6L   $
��Left=�6d   $��	0,261,0,0q�6   �� 5   
   -   5        $ 
InitButton��5      $��	초기화��6=   $��1��6K   $	��100��6[   $��25��6g   $	��Top=�6s   $��Right=�6�   $��	0,317,0,0q�6�   �� 5   
   $��4. 저장 및 초기화��6   $��31��6&   $��15(�62   $��1��6@   $	��Top=�6P   $
��Left=�6h   $��	0,313,0,0q�6�   �� 5   
   -   5        $ AdjustButton��5      $��
Adjustment��6>   $��1��6S   $	��452��6c   $��25��6o   $	��Top=�6{   $��Right=�6�   $��	0,361,0,0q�6�   5       5!      