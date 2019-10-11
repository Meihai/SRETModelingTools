using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModelingToolsApp.UserControls
{
    /// <summary>
    /// 原始形状接口
    /// </summary>
    public interface IOShape
    {
        /// <summary>
        /// 所属流程图形状类型
        /// </summary>
        FlowChartTypes FlowChartType
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
