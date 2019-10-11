using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace ModelingToolsAppWithMVVM.Common.ChartCommon
{
    /// <summary>
    /// 构成连接线的点的类型
    /// </summary>
    public enum LinePointTypes
    {
        [Description("起点")]
        StartPoint,
        [Description("终点")]
        EndPoint,
        [Description("转折点")]
        BreakPoint,
        [Description("中点")]
        MiddlePoint
    }

}
