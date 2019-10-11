using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace ModelingToolsAppWithMVVM.Common
{
    /// <summary>
    /// 数据校验位
    /// </summary>
    public enum DataParityTypes
    {
        [Description("无")]
        NONE=1,
        [Description("奇校验")]
        ODD=2,
        [Description("偶检验")]
        EVEN=3,
        [Description("Mark")]
        MARK=4,
        [Description("SPACE")]
        SPACE=5

    }
}
