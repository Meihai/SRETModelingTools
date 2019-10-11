using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace ModelingToolsAppWithMVVM.Common
{   
    /// <summary>
    /// 接口对应的协议类型
    /// </summary>
    public enum ProtocolTypes
    {  
        [Description("UDP")]
        UDP=1,
        [Description("TCP")]
        TCP=2
    }
}
