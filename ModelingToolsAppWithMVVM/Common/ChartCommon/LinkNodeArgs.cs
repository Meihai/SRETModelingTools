using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelingToolsAppWithMVVM.Common.ChartCommon
{
    public class LinkNodeArgs
    {

        public LinkNodeArgs()
        {

        }

        /// <summary>
        /// 在连接点停靠的参数
        /// </summary>
        /// <param name="dockedShape">停靠的流程图形状</param>
        /// <param name="linkNode">停靠的连接点</param>
        public LinkNodeArgs(IFlowChartBase dockedShape, LinkNode linkNode)
        {
            DockedShape = dockedShape;
            DocketLinkNode = linkNode;
        }

        private IFlowChartBase _shape;
        /// <summary>
        /// 鼠标停靠的形状
        /// </summary>
        public IFlowChartBase DockedShape
        {
            get { return _shape; }
            set { _shape = value; }
        }

        private LinkNode _linkNode;
        /// <summary>
        /// 鼠标停靠的联系点
        /// </summary>
        public LinkNode DocketLinkNode
        {
            get { return this._linkNode; }
            set { this._linkNode = value; }
        }
    }
}
