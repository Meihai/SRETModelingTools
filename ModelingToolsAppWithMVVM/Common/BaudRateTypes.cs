using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace ModelingToolsAppWithMVVM.Common
{
    /// <summary>
    /// 波特率类型
    /// </summary>
    public enum BaudRateTypes
    {
        [Description("2400")]
        b2400=1,
        [Description("4800")]
        b4800=2,
        [Description("9600")]
        b9600=3,
        [Description("14400")]
        b14400=4,
        [Description("19200")]
        b19200=5,
        [Description("38400")]
        b38400=6,
        [Description("56000")]
        b56000=7,
        [Description("57600")]
        b57600=8,
        [Description("115200")]
        b115200=9

    }
}
