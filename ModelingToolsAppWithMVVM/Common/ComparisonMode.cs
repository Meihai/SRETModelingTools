using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace ModelingToolsAppWithMVVM.Common
{
    public enum ComparisonMode
    {   
        [Description("值相等")]
        EQUAL=1,
        [Description("值不相等")]
        NOTEQUAL=2,
        [Description("值包含")]
        CONTAIN=3,
        [Description("值不包含")]
        NOTCONTAIN=4
    }
}
