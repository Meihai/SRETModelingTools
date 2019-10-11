using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace ModelingToolsAppWithMVVM.Common.ChartCommon
{
    /// <summary>
    /// 流程图形状类型
    /// </summary>
    public enum FlowChartTypes
    {
        #region 剖面建模相关图形
          /// <summary>
        /// 顺序转移
        /// </summary>
        [Description("顺序转移")]
        ShapeSeqTransfer = 1,

        /// <summary>
        /// 概率转移
        /// </summary>
        [Description("概率转移")]
        ShapeProbTransfer = 2,

        /// <summary>
        /// 建模入口
        /// </summary>
        [Description("建模入口")]
        ShapeEntrance = 3,

        /// <summary>
        /// 建模出口
        /// </summary>
        [Description("建模出口")]
        ShapeExit = 4,

        /// <summary>
        /// 建模终止
        /// </summary>
        [Description("建模终止")]
        ShapeTerminal = 5,

        /// <summary>
        /// 建模操作
        /// </summary>
        [Description("建模操作")]
        ShapeOperation = 6,

        /// <summary>
        /// 建模包
        /// </summary>
        [Description("建模包")]
        ShapeReliableProfile = 7,
        
        /// <summary>
        /// 条件分支
        /// </summary>
        [Description("条件分支")]
        ShapeConditionBranch=8,

        #endregion 剖面建模相关图形

        #region 软件接口相关枚举类
        [Description("接口交联关系线")]
        InterfaceInteractionLink=10,
       
        [Description("接口交联对象")]
        InterfaceInteractionObject=11,

        [Description("接口被测对象")]
        InterfaceTestedObject= 12
        #endregion 软件接口相关枚举类
    }
    
}
