using ModelingToolsAppWithMVVM.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelingToolsAppWithMVVM.ViewModel
{
    /// <summary>
    /// 原始形状接口
    /// </summary>
   public  interface IOShape
    {
       /// <summary>
       /// 设定形状类型
       /// </summary>
        ReliabilityModelingTypes ModelType
       {
           get;
       }       
      

       /// <summary>
       /// 启动拖动事件
       /// </summary>
       /// <param name="sender"></param>
       /// <param name="e"></param>
       void OnDragDrop();
      
    }
}
