using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace ModelingToolsAppWithMVVM.Common.ChartCommon
{
    /// <summary>
    /// 改变的属性
    /// </summary>
    public enum ChangedPropertys
    {
        /// <summary>
        /// 被选中
        /// </summary>
        [Description("被选中")]
        SelectTrue,
        /// <summary>
        /// 取消选中
        /// </summary>
        [Description("取消选中")]
        SelectFalse,
        /// <summary>
        /// 尺寸改变
        /// </summary>
        [Description("尺寸改变")]
        Size,
        /// <summary>
        /// 位置改变
        /// </summary>
        [Description("位置改变")]
        Position
    }
}
