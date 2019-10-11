using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace ModelingToolsAppWithMVVM.Common.ChartCommon
{
   
    /// <summary>
    /// 线条类型
    /// </summary>
    public enum LineTypes
    {
        [Description("无")]
        No,
        [Description("实线")]
        Solid,
        [Description("长虚线")]
        LongDashes,
        [Description("短虚线")]
        ShortDashes
    }
}
