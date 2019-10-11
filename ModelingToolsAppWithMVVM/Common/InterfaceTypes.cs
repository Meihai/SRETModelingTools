using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
namespace ModelingToolsAppWithMVVM.Common
{
    /// <summary>
    /// 接口连接类型
    /// </summary>
    public enum  InterfaceTypes
    {          
        [Description("RS485直连")]
        RS485DirectConnect=1,
        
        [Description("RS485透传")]
        RS485TransparentConnect=2,
      
        [Description("RS-232直连")]
        RS232DirectConnect=3,
    
        [Description("RS-232透传")]
        RS232TransparentConnect=4
    }
}
