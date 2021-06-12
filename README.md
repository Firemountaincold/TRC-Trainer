# TRC Trainer
 用DlibDotNet给的example写成的用于训练Dlib目标识别模型的小程序。
[下载官方数据集](http://dlib.net/files/data/dlib_rear_end_vehicles_v1.tar)  
因为数据集太大，我还没试过，仅供参考。
# 窗口：
![image](https://github.com/Firemountaincold/TRC-Trainer/blob/main/Image.png)
# 更新文档：
## 2021.4.8：
### 1.0：
完成基本功能。  
## 2021.5.25:
### 1.1:
完善功能。   
添加查看是否支持CUDA。   
添加调用python训练功能。
## 2021.5.30：
### 1.2：
改善UI。   
添加了调整迭代次数的功能。
## 2021.6.1：
### 1.3：
修改了UI界面。  
现在可以显示轮次和学习率。  
现在可以中途停止训练并保存模型。  
## 2021.6.2:
### 1.4:
加入了不停止训练并保存的功能（待测试）。  
加入了关闭上采样评估的功能。  
加入了Loss足够小时自动停止训练的功能。  
删除了无用的python调用功能。  
加入预估时间功能（不太准）。  


