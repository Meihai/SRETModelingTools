using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ModelingToolsAppWithMVVM.Common.ChartCommon
{
    /// <summary>
    /// 简单联系点，只包含坐标和连接点类型。为与LinkNode控件区别
    /// </summary>
    public class SimpleLinkNode
    {
        public SimpleLinkNode(LinkNodeTypes linkNodeType, Point position)
        {
            this._linkNodeType = linkNodeType;
            this._postion = position;
        }

        LinkNodeTypes _linkNodeType = LinkNodeTypes.NULL;
        /// <summary>
        /// 连接点类型
        /// </summary>
        public LinkNodeTypes LinkNodeType
        {
            get { return this._linkNodeType; }
            set { this._linkNodeType = value; }
        }

        Point _postion = new Point(0, 0);
        /// <summary>
        /// 连接点坐标
        /// </summary>
        public Point Position
        {
            get { return this._postion; }
            set { this._postion = value; }
        }
    }

}
