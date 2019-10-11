using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelingToolsAppWithMVVM.Common.ChartCommon
{
    /// <summary>
    /// 控制点类型
    /// </summary>
    public enum CtrlNodeTypes
    {
        LEFT,
        TOP,
        RIGHT,
        BOTTOM,
        LEFT_TOP,
        RIGHT_TOP,
        RIGHT_BOTTOM,
        LEFT_BOTTOM,
        /// <summary>
        /// 所有方向均可移动
        /// </summary>
        ALL,
        /// <summary>
        /// 连接线的起点移动
        /// </summary>
        START,
        /// <summary>
        /// 连接线的终点移动
        /// </summary>
        END,
        /// <summary>
        /// 修改位置
        /// </summary>
        POSITION,
        /// <summary>
        /// 不作任何修改
        /// </summary>
        NO_CHANGE
    }

}
