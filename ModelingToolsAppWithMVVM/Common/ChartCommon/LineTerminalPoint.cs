using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ModelingToolsAppWithMVVM.Common.ChartCommon
{
    /// <summary>
    /// 连接线端点
    /// </summary>
    public class LineTerminalPoint : ICloneable
    {
        public LineTerminalPoint(Point position)
        {
            this._position = position;
        }

        public LineTerminalPoint()
        { }


        private bool _dockedFlag = false;
        /// <summary>
        /// 是否停靠在联系点上
        /// </summary>
        public bool DockedFlag
        {
            get { return this._dockedFlag; }
            set { this._dockedFlag = value; }
        }


        Point _position = new Point(0, 0);
        /// <summary>
        /// 点坐标
        /// </summary>
        public Point Position
        {
            get { return this._position; }
            set { this._position = value; }
        }

        string _relatedShapeId = "";
        /// <summary>
        /// 关系点关联的形状编号
        /// </summary>
        public string RelatedShapeId
        {
            get { return this._relatedShapeId; }
            set { this._relatedShapeId = value; }
        }

        LinkNodeTypes _relatedType = LinkNodeTypes.NULL;
        /// <summary>
        /// 关系点与关联形状的关联方式
        /// </summary>
        public LinkNodeTypes RelatedType
        {
            get { return this._relatedType; }
            set { this._relatedType = value; }
        }

        #region ICloneable 成员

        public object Clone()
        {
            LineTerminalPoint ltp = new LineTerminalPoint();
            ltp._position = new Point(this._position.X, this._position.Y);
            ltp._dockedFlag = this._dockedFlag;
            ltp._relatedShapeId = this._relatedShapeId;
            ltp._relatedType = this._relatedType;
            return ltp;
        }

        #endregion
    }
}
