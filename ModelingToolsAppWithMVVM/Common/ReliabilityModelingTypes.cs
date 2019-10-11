using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelingToolsAppWithMVVM.Common
{
    /// <summary>
    /// 可靠性建模类型
    /// </summary>
    public enum ReliabilityModelingTypes
    {       
        /// <summary>
        /// 顺序转移
        /// </summary>
        ShapeSeqTransfer = 1,

        /// <summary>
        /// 概率转移
        /// </summary>
        ShapeProbTransfer = 2,

        /// <summary>
        /// 建模入口
        /// </summary>
        ShapeEntrance = 3,

        /// <summary>
        /// 建模出口
        /// </summary>
        ShapeExit = 4,

        /// <summary>
        /// 建模终止
        /// </summary>
        ShapeTerminal = 5,

        /// <summary>
        /// 建模操作
        /// </summary>
        ShapeOperation = 6,

        /// <summary>
        /// 建模包
        /// </summary>
        ShapeReliableProfile = 7,
        
        /// <summary>
        /// 条件分支
        /// </summary>
        ShapeConditionBranch=8
     

    }
}
