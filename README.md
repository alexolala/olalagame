# olalagame

本游戏的诞生<br>
身为一个转专业只会php的小菜鸟来说，为了写这个游戏，我先学习用swift写围住神经猫，然后学会了用C++写俄罗斯方块和2048，但我依然是个C++的小菜鸟。写不了更复杂的游戏。因此，我尝试着将我会的融合起来，使用Unity3D做了这个游戏。
这是个很简单的游戏，但有时简单也许意味着有趣。<br>

很伤心，我在网上发现了类似的游戏，而且好像比我设计的更好玩~~~~<br>

一、游戏组成
-------
棋盘组成：4列*8排的格子；<br>
棋子组成：1枚落下的小方格，其值为1、2、4、8、16，均为2的次方；<br>
输入模块：屏幕纵向分为4块区域，对应着4列格子；<br>


二、游戏流程
-------
1、选择开始游戏；<br>
2、挑选难度（目前只完成level1）；<br>
3、进行游戏（详见游戏规则）；<br>
4、游戏结束。<br>

游戏规则：

1、上方随机生成一个值为（1、2、4、8、16）的小方格c；<br>
2、屏幕输入（触控）落下区域，小方格c落下；<br>
3、nghbr = 降落后周围的格子；<br>

   if(c的高度<6)<br>
   {<br>
>>      //level1<br>
>>      if(c + nghbr == 256)  <br>            
>>   {   <br>
>>>>    score +=1;<br>
>>  }<br>
>>  else if(c == nghbr)<br>
>>     {<br>
>>>>        nghbr +=c;<br>
>>>>        消失c;<br>
>>     }<br>
   }else{<br>
   game over;<br>
   }
