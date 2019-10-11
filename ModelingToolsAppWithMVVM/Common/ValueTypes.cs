using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace ModelingToolsAppWithMVVM.Common
{
    /// <summary>
    /// 值类型
    /// </summary>
    public enum ValueTypes
    {  
        [Description("整型")]
        Int=1,
        [Description("浮点型")]
        Float=2,
        [Description("布尔型")]
        Bool=3,
        [Description("字符串")]
        String=4,
        [Description("十六进制字符串")]
        HexString=5
    }
}
