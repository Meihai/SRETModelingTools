using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModelingToolsApp.UserControls
{
    interface ISerializeAttributes
    {
        /// <summary>
        /// 需要进行序列化保存的属性
        /// </summary>
        List<string> Attributes { get; }
    }
}
