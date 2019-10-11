using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ModelingToolsAppWithMVVM.Common.ChartCommon
{
    /// <summary>
    /// 全局鼠标事件参数
    /// </summary>
    public class GlobalMouseArgs
    {
        /// <summary>
        /// 位置
        /// </summary>
        public Point Position
        {
            get;
            set;
        }

        /// <summary>
        /// 变化的键
        /// </summary>
        public MouseButton Button
        {
            get;
            set;
        }
    }
}
