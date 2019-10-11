using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace ModelingToolsAppWithMVVM.Common
{
    public enum StopBitTypes
    {
        [Description("1bit")]
        BIT_1=1,
        [Description("1.5bit")]
        BIT_1P5=2,
        [Description("2bit")]
        BIT_2=3
    }
}
