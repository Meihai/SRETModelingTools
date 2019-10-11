using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace ModelingToolsAppWithMVVM.Common
{
    /// <summary>
    /// 变量值变化的触发方式
    /// </summary>
    public enum ValueTriggerMethod
    {
        /// <summary>
        /// 由内部设定的函数表达式根据时间更改值变量
        /// </summary>
        [Description("内触发")]
         InternalTrigger= 1,
        /// <summary>
        /// 由外部值更改值变量
        /// </summary>
        [Description("外触发")]
        ExternalTrigger = 2
    }
}
