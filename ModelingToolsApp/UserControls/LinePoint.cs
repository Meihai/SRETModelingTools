using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ModelingToolsApp.UserControls
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


    /// <summary>
    /// 连接线控制点
    /// </summary>
    public class LineCtrlPoint
    {
        /// <summary>
        /// 插入新点
        /// </summary>
        /// <param name="lcp"></param>
        /// <param name="flag">0，插入到当前点之前；1，插入到当前点之后</param>
        public void Insert(LineCtrlPoint lcp, int flag)
        {
            if (0 == flag)
            {
                if (null != this.Prev)
                {
                    this.Prev.Next = lcp;
                }
                lcp.Prev = this.Prev;
                lcp.Next = this;
                this.Prev = lcp;

            }
            else if (1 == flag)
            {
                if (null != this.Next)
                {
                    this.Next.Prev = lcp;
                }
                lcp.Next = this.Next;
                lcp.Prev = this;
                this.Next = lcp;
            }
        }

        /// <summary>
        /// 删除一个节点
        /// </summary>
        /// <param name="flag">0，删除到当前点之前一个点；1，删除到当前点之后一个点</param>
        public void Delete(int flag)
        {
            if (0 == flag)
            {
                if (null != this.Prev)
                {
                    this.Prev = this.Prev.Prev; //删除前一个节点
                    this.Prev.Next=this;
                }          
            }
            else if (1 == flag)
            {
                if (null != this.Next)
                {
                    this.Next = this.Next.Next; //删除后一个节点
                    this.Next.Prev=this;
                }              
            }
        }

        /// <summary>
        /// 添加到最后
        /// </summary>
        /// <param name="position"></param>
        /// <param name="linePointType"></param>
        /// <param name="ctrlNodeType"></param>
        public void Append(Point position, LinePointTypes linePointType, CtrlNodeTypes ctrlNodeType)
        {
            LineCtrlPoint tmp = this;
            while (null != tmp.Next)
            {
                tmp = tmp.Next;
            }

            LineCtrlPoint lcp = new LineCtrlPoint(position, linePointType, ctrlNodeType);

            lcp.Id = tmp.Id + 1;
            lcp.Prev = tmp;
            tmp.Next = lcp;
        }

        /// <summary>
        /// 添加到最后
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="linePointType"></param>
        /// <param name="ctrlNodeType"></param>
        public void Append(double x, double y, LinePointTypes linePointType, CtrlNodeTypes ctrlNodeType)
        {
            Append(new Point(x, y), linePointType, ctrlNodeType);
        }


        public LineCtrlPoint(Point position, LinePointTypes linePointType, CtrlNodeTypes ctrlNodeType)
        {
            this._pointTypes = linePointType;
            this._position = position;
            this._ctrlNodeType = ctrlNodeType;
            this.Next = null;
            this.Prev = null;
        }

        public LineCtrlPoint(double x, double y, LinePointTypes linePointType, CtrlNodeTypes ctrlNodeType)
        {
            this._pointTypes = linePointType;
            this._position = new Point(x, y);
            this._ctrlNodeType = ctrlNodeType;
            this.Next = null;
            this.Prev = null;
        }

        private int _id = 0;
        /// <summary>
        /// 序号
        /// </summary>
        public int Id
        {
            get { return this._id; }
            private set { this._id = value; }
        }

        private Point _position = new Point(0, 0);
        /// <summary>
        /// 点的中心位置
        /// </summary>
        public Point Position
        {
            get { return this._position; }
            set { this._position = value; }
        }

        private LinePointTypes _pointTypes = LinePointTypes.BreakPoint;
        /// <summary>
        /// 点的类型
        /// </summary>
        public LinePointTypes PointTypes
        {
            get { return this._pointTypes; }
            set { this._pointTypes = value; }
        }

        private CtrlNodeTypes _ctrlNodeType = CtrlNodeTypes.ALL;
        /// <summary>
        /// 控制点类型
        /// </summary>
        public CtrlNodeTypes CtrlNodeType
        {
            get { return this._ctrlNodeType; }
            set { this._ctrlNodeType = value; }
        }

        LineCtrlPoint _prev;
        public LineCtrlPoint Prev
        {
            get { return this._prev; }
            set { this._prev = value; }
        }
        LineCtrlPoint _next;
        public LineCtrlPoint Next
        {
            get { return this._next; }
            set { this._next = value; }
        }
    }

}
