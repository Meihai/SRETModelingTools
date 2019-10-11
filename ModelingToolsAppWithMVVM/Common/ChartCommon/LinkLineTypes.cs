using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace ModelingToolsAppWithMVVM.Common.ChartCommon
{
    /// <summary>
    /// 连接线的线型
    /// </summary>
    public enum LinkLineTypes
    {
        [Description("直线")]
        Straight,
       
        [Description("折线")]
        Broken,
       
        [Description("曲线")]
        Curve,
       
        [Description("圆弧")]
        Arc
    }
}
