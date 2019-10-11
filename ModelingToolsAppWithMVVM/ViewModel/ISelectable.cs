using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelingToolsAppWithMVVM.ViewModel
{    
    /// <summary>
    /// 是否选中接口
    /// </summary>
    public interface ISelectable
    {
        bool IsSelected { get; set; }
    }
}
