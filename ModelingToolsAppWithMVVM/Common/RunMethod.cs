using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace ModelingToolsAppWithMVVM.Common
{
    public enum RunMethod
    {
        [Description("仅运行一次")]
        RunByOnce=1,

        [Description("周期运行")]
        RunByPeriod=2
    }
}
