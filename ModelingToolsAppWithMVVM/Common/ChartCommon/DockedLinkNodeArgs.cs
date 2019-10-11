using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelingToolsAppWithMVVM.Common.ChartCommon
{
    /// <summary>
    /// 在联系点上停靠时的参数
    /// </summary>
    public class DockedLinkNodeArgs
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="flag">是否停靠在联系点上</param>
        /// <param name="shapeBase">停靠的流程图形状</param>
        /// <param name="linkNode">停靠的联系点</param>
        public DockedLinkNodeArgs(int id, bool flag, IFlowChartBase ifcb, SimpleLinkNode linkNode)
        {
            this._id = id;
            this._flag = flag;
            this._shape = ifcb;
            this._linkNode = linkNode;
        }

        public DockedLinkNodeArgs()
        {
            this._id = 0;
            this._flag = false;
        }


        private int _id;
        /// <summary>
        /// 产生停靠事件的点的序号
        /// </summary>
        public int Id
        {
            get { return this._id; }
            set { this._id = value; }
        }


        private bool _flag;
        /// <summary>
        /// 鼠标是否停靠在某个联系点上。鼠标离开某个联系点时=false;在某个联系点上时=true
        /// </summary>
        public bool Flag
        {
            get { return this._flag; }
            set { this._flag = value; }
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

        private SimpleLinkNode _linkNode;
        /// <summary>
        /// 鼠标停靠的联系点
        /// </summary>
        public SimpleLinkNode DocketLinkNode
        {
            get { return this._linkNode; }
            set { this._linkNode = value; }
        }
    }

}
