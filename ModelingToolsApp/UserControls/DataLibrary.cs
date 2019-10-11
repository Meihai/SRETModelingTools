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
using System.ComponentModel;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;


namespace ModelingToolsApp.UserControls
{ /// <summary>
    /// 线条类型
    /// </summary>
    public enum LineTypes
    {
        /// <summary>
        /// 无
        /// </summary>
        No,
        /// <summary>
        /// 实线
        /// </summary>
        Solid,
        /// <summary>
        /// 长虚线
        /// </summary>
        LongDashes,
        /// <summary>
        /// 短虚线
        /// </summary>
        ShortDashes
    }

    /// <summary>
    /// 端点类型
    /// </summary>
    public enum EndpointTypes
    {
        /// <summary>
        /// 无端点
        /// </summary>
        No,
        /// <summary>
        /// 实心箭头开始
        /// </summary>
        StartSolidArrow,
        /// <summary>
        /// 实心箭头结束
        /// </summary>
        EndSolidArrow,
        /// <summary>
        /// 两侧均为实心箭头
        /// </summary>
        BothSolidArrow,
        /// <summary>
        /// 线箭头开始
        /// </summary>
        StartLineArrow,
        /// <summary>
        /// 线箭头结束
        /// </summary>
        EndLineArrow,
        /// <summary>
        /// 两侧均为线箭头
        /// </summary>
        BothLineArrow,
        /// <summary>
        /// 圆点开始
        /// </summary>
        StartDot,
        /// <summary>
        /// 圆点结束
        /// </summary>
        EndDot,
        /// <summary>
        /// 两侧均为圆点
        /// </summary>
        BothDot
    }

    /// <summary>
    /// 联系点类型
    /// </summary>
    public enum LinkNodeTypes
    {
        LEFT = 1,
        TOP = 2,
        RIGHT = 3,
        BOTTOM = 4,
        CENTER = 5,
        START = 6,
        END = 7,
        NULL = 8
    }

    /// <summary>
    /// 控制点类型
    /// </summary>
    public enum CtrlNodeTypes
    {
        LEFT,
        TOP,
        RIGHT,
        BOTTOM,
        LEFT_TOP,
        RIGHT_TOP,
        RIGHT_BOTTOM,
        LEFT_BOTTOM,
        /// <summary>
        /// 所有方向均可移动
        /// </summary>
        ALL,
        /// <summary>
        /// 连接线的起点移动
        /// </summary>
        START,
        /// <summary>
        /// 连接线的终点移动
        /// </summary>
        END,
        /// <summary>
        /// 修改位置
        /// </summary>
        POSITION,
        /// <summary>
        /// 不作任何修改
        /// </summary>
        NO_CHANGE
    }

    /// <summary>
    /// 改变的属性
    /// </summary>
    public enum ChangedPropertys
    {
        /// <summary>
        /// 被选中
        /// </summary>
        SelectTrue,
        /// <summary>
        /// 取消选中
        /// </summary>
        SelectFalse,
        /// <summary>
        /// 尺寸改变
        /// </summary>
        Size,
        /// <summary>
        /// 位置改变
        /// </summary>
        Position
    }

    /// <summary>
    /// 连接线的线型
    /// </summary>
    public enum LinkLineTypes
    {
        /// <summary>
        /// 直线
        /// </summary>
        Straight,
        /// <summary>
        /// 折线
        /// </summary>
        Broken,
        /// <summary>
        /// 曲线
        /// </summary>
        Curve,
        /// <summary>
        /// 圆弧
        /// </summary>
        Arc
    }

    /// <summary>
    /// 流程图形状类型
    /// </summary>
    public enum FlowChartTypes
    {
        /// <summary>
        /// 流程
        /// </summary>
        ShapeProcess = 1,
        /// <summary>
        /// 判断
        /// </summary>
        ShapeJudge = 2,

        /// <summary>
        /// 顺序转移
        /// </summary>
        ShapeSeqTransfer=3,

        /// <summary>
        /// 概率转移
        /// </summary>
        ShapeProbTransfer=4,

        /// <summary>
        /// 建模入口
        /// </summary>
        ShapeEntrance=5,
        /// <summary>
        /// 建模出口
        /// </summary>
        ShapeExit=6,

        /// <summary>
        /// 建模出口
        /// </summary>
        ShapeTerminal=7,

        /// <summary>
        /// 建模操作
        /// </summary>
        ShapeOperation=8,

        /// <summary>
        /// 建模包
        /// </summary>
        ShapeReliableProfile=9,

        
        /// <summary>
        /// 折线连接线
        /// </summary>
        LinkBroken = 101,
        /// <summary>
        /// 直线连接线
        /// </summary>
        LinkStraight = 102,  

        /// <summary>
        /// 直线折线混合连接线,默认为直线,手动修改后为折线
        /// </summary>
        LinkSeqTransfer=103



    }

    /// <summary>
    /// 构成连接线的点的类型
    /// </summary>
    public enum LinePointTypes
    {
        /// <summary>
        /// 起点
        /// </summary>
        StartPoint,
        /// <summary>
        /// 终点
        /// </summary>
        EndPoint,
        /// <summary>
        /// 转折点
        /// </summary>
        BreakPoint,
        /// <summary>
        /// 中点
        /// </summary>
        MiddlePoint
    }

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

    /// <summary>
    /// 全局鼠标事件参数
    /// </summary>
    public class GlobalMouseArgs
    {
        /// <summary>
        /// 位置
        /// </summary>
        public Point Position
        {
            get;
            set;
        }

        /// <summary>
        /// 变化的键
        /// </summary>
        public MouseButton Button
        {
            get;
            set;
        }
    }

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
