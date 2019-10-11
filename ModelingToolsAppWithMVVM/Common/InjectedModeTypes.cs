using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace ModelingToolsAppWithMVVM.Common
{
    /// <summary>
    /// 注入方式类型
    /// </summary>
    public enum InjectedModeTypes
    {
        [Description("报文注入变量")]
        MessageInjectedToVariable=1,
        [Description("变量注入报文")]
        VariableInjectedToMessage=2,
        [Description("互注")]
        InjectedToEachOther=3
    }
}
