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

namespace ModelingToolsAppWithMVVM.Common.ChartCommon
{
    /// <summary>
    /// 请求开始规划路径
    /// </summary>
    /// <param name="start0">起点</param>
    /// <param name="end0">终点</param>
    /// <param name="start1">由于起点停靠在形状联系点上，引申出的连接点</param>
    /// <param name="end1">由于终点停靠在形状联系点上，引申出的连接点</param>
    public delegate void delStartSearchPath(Point start0, Point end0, Point start1, Point end1);

    /// <summary>
    /// 控制线的端点发生移动，参数point的坐标已经转换
    /// </summary>
    /// <param name="point"></param>
    /// <param name="seq">点序号:0起点，1终点</param>
    public delegate void delTerminalPointMoved(Point point, int seq);


    /// <summary>
    /// CtrlLine.xaml 的交互逻辑
    /// </summary>
    [System.ComponentModel.DesignTimeVisible(false)]
    public partial class CtrlLine : UserControl
    {
        /// <summary>
        /// 开始规划路径
        /// </summary>
        public event delStartSearchPath evtStartSearchPath;

        /// <summary>
        /// 控制线的端点发生移动，参数point的坐标已经转换.seq点序号:0起点，1终点
        /// </summary>
        public event delTerminalPointMoved evtTerminalPointMoved;
        private void OnTerminalPointMoved(Point point, int seq)
        {
            if (null != evtTerminalPointMoved)
            {
                evtTerminalPointMoved(point, seq);
            }
        }

        /// <summary>
        /// 和控制线关联的连接线
        /// </summary>
        private LinkBase RelatedLine;

        /// <summary>
        /// 修改尺寸的方式
        /// </summary>
        private CtrlNodeTypes ChangeSizeType = CtrlNodeTypes.NO_CHANGE;

        /// <summary>
        /// 当前选中的控制点
        /// </summary>
        private CtrlNode CurrentCtrlNode;

        private Point _offset = new Point(0, 0);
        /// <summary>
        /// 鼠标在控件上点击时的坐标
        /// </summary>
        public Point Offset
        {
            set { this._offset = value; }
        }

        LinkLineTypes LinkLineType;
        double HalfCtrlNodeSize = 13 / 2.0;
        LineTerminalPoint StartPoint;
        LineTerminalPoint EndPoint;
        Path pathTmp = new Path();

        public CtrlLine()
        {
            InitializeComponent();

            pathTmp.Stroke = new SolidColorBrush(Colors.Black);
            pathTmp.StrokeThickness = 1;
            pathTmp.StrokeDashArray.Add(3);
            pathTmp.StrokeDashArray.Add(3);
            pathTmp.Visibility = Visibility.Hidden;

            GlobalMouseHook.evtGlobalMouseUp += new delGlobalMouseUp(GlobalMouseHook_evtGlobalMouseUp);
        }


        /// <summary>
        /// 绑定连接线
        /// </summary>
        /// <param name="linkLine"></param>
        public void BindLinkLine(LinkBase linkLine)
        {
            this.Margin = linkLine.Margin;
            viewer.Children.Clear();
            viewer.Children.Add(pathTmp);

            RelatedLine = linkLine;
            StartPoint = RelatedLine.StartPnt;
            EndPoint = RelatedLine.EndPnt;
            LinkLineType = RelatedLine.LinkLineType;

            ChangeSizeType = CtrlNodeTypes.NO_CHANGE;

            //显示控制点
            LineCtrlPoint lcp = RelatedLine.GetLineCtrlPoints();
            LineCtrlPoint tmp = lcp;
            while (null != tmp)
            {
                if (null != tmp.Prev &&
                    tmp.PointTypes == LinePointTypes.MiddlePoint &&
                    Math.Abs(tmp.Position.X - tmp.Prev.Position.X) < 10 &&
                    Math.Abs(tmp.Position.Y - tmp.Prev.Position.Y) < 10)
                {

                }
                else
                {
                    CtrlNode cn = new CtrlNode(tmp.Id, tmp.PointTypes, tmp.CtrlNodeType);
                    cn.Margin = new Thickness(tmp.Position.X - HalfCtrlNodeSize, tmp.Position.Y - HalfCtrlNodeSize, 0, 0);
                    cn.evtMouseDownOnCtrlNode += new delMouseDownOnCtrlNode(cn_evtMouseDownOnCtrlNode);
                    viewer.Children.Add(cn);
                }

                tmp = tmp.Next;
            }
        }


        /// <summary>
        /// 接收鼠标移动事件
        /// </summary>
        /// <param name="e"></param>
        public void AcceptMouseMove(DockedLinkNodeArgs e, Point containerPosi)
        {
            if (this.Visibility == Visibility.Hidden || ChangeSizeType == CtrlNodeTypes.NO_CHANGE)
            {
                return;
            }


            Cursor = Cursors.Cross;

            double oldx, oldy;
            double w, h, x, y;
            w = Width;
            h = Height;
            oldx = x = Margin.Left;
            oldy = y = Margin.Top;


            Point p = new Point(containerPosi.X - Margin.Left, containerPosi.Y - Margin.Top);
            //停靠在某个联系点上时，采用停靠到的坐标
            if (e.Flag && ((ChangeSizeType == CtrlNodeTypes.START && e.Id == 0) || (ChangeSizeType == CtrlNodeTypes.END && e.Id == 1)))
            {
                p = new Point(e.DocketLinkNode.Position.X + e.DockedShape.Margin.Left - Margin.Left,
                    e.DocketLinkNode.Position.Y + e.DockedShape.Margin.Top - Margin.Top);
            }


            switch (this.ChangeSizeType)
            {
                case CtrlNodeTypes.LEFT:
                    CreateTmpLine(p);
                    break;
                case CtrlNodeTypes.TOP:
                    CreateTmpLine(p);
                    break;
                case CtrlNodeTypes.ALL:
                    CreateTmpLine(p);
                    break;
                case CtrlNodeTypes.START:

                    OnTerminalPointMoved(containerPosi, 0);

                    StartPoint.Position = p;
                    if (e.Flag && e.Id == 0)
                    {
                        StartPoint.DockedFlag = true;
                        StartPoint.RelatedShapeId = e.DockedShape.Id;
                        StartPoint.RelatedType = e.DocketLinkNode.LinkNodeType;
                        CreateTmpLine(p);
                    }
                    else
                    {
                        StartPoint.DockedFlag = false;
                        StartPoint.RelatedShapeId = "";
                        StartPoint.RelatedType = LinkNodeTypes.NULL;
                        CreateTmpLine(p);
                    }

                    break;
                case CtrlNodeTypes.END:
                    OnTerminalPointMoved(containerPosi, 1);

                    EndPoint.Position = p;
                    if (e.Flag && e.Id == 1)
                    {
                        EndPoint.DockedFlag = true;
                        EndPoint.RelatedShapeId = e.DockedShape.Id;
                        EndPoint.RelatedType = e.DocketLinkNode.LinkNodeType;
                        CreateTmpLine(p);
                    }
                    else
                    {
                        EndPoint.DockedFlag = false;
                        EndPoint.RelatedShapeId = "";
                        EndPoint.RelatedType = LinkNodeTypes.NULL;
                        CreateTmpLine(p);
                    }
                    break;
                case CtrlNodeTypes.POSITION:
                    break;
                case CtrlNodeTypes.NO_CHANGE:
                    break;
                default:
                    break;
            }

            w = w > 10 ? w : 10;
            h = h > 5 ? h : 5;
            Width = w;
            Height = h;
        }


        /// <summary>
        /// 释放鼠标左键
        /// </summary>
        /// <param name="e"></param>
        void GlobalMouseHook_evtGlobalMouseUp(GlobalMouseArgs e)
        {
            ChangeSizeType = CtrlNodeTypes.NO_CHANGE;

            if (this.Visibility == Visibility.Visible && null != RelatedLine)
            {
                RelatedLine.FinishTmpLine(StartPoint, EndPoint);
            }
        }


        void cn_evtMouseDownOnCtrlNode(CtrlNode ctrlNode)
        {
            this.CurrentCtrlNode = ctrlNode;
            this.ChangeSizeType = ctrlNode.CtrlNodeType;
        }


        /// <summary>
        /// 创建临时连接线
        /// </summary>
        private void CreateTmpLine(Point position)
        {
            if (null != RelatedLine)
            {
                RelatedLine.CreateTmpLine(CurrentCtrlNode, position);
            }
        }


        private void CreateBrokeTmpLineEx()
        {
            if (null != evtStartSearchPath)
            {
                Point start1 = new Point(StartPoint.Position.X + Margin.Left,
                                             StartPoint.Position.Y + Margin.Top);
                Point end1 = new Point(EndPoint.Position.X + Margin.Left,
                                             EndPoint.Position.Y + Margin.Top);

                if (StartPoint.DockedFlag)
                {
                    switch (StartPoint.RelatedType)
                    {
                        case LinkNodeTypes.LEFT:
                            start1 = new Point(StartPoint.Position.X - 20 + Margin.Left, StartPoint.Position.Y + Margin.Top);
                            break;
                        case LinkNodeTypes.TOP:
                            start1 = new Point(StartPoint.Position.X + Margin.Left, StartPoint.Position.Y - 20 + Margin.Top);
                            break;
                        case LinkNodeTypes.RIGHT:
                            start1 = new Point(StartPoint.Position.X + 20 + Margin.Left, StartPoint.Position.Y + Margin.Top);
                            break;
                        case LinkNodeTypes.BOTTOM:
                            start1 = new Point(StartPoint.Position.X + Margin.Left, StartPoint.Position.Y + 20 + Margin.Top);
                            break;
                        case LinkNodeTypes.CENTER:
                            break;
                        case LinkNodeTypes.NULL:
                            break;
                        default:
                            break;
                    }
                }

                if (StartPoint.DockedFlag)
                {
                    switch (EndPoint.RelatedType)
                    {
                        case LinkNodeTypes.LEFT:
                            end1 = new Point(EndPoint.Position.X - 20 + Margin.Left, EndPoint.Position.Y + Margin.Top);
                            break;
                        case LinkNodeTypes.TOP:
                            end1 = new Point(EndPoint.Position.X + Margin.Left, EndPoint.Position.Y - 20 + Margin.Top);
                            break;
                        case LinkNodeTypes.RIGHT:
                            end1 = new Point(EndPoint.Position.X + 20 + Margin.Left, EndPoint.Position.Y + Margin.Top);
                            break;
                        case LinkNodeTypes.BOTTOM:
                            end1 = new Point(EndPoint.Position.X + Margin.Left, EndPoint.Position.Y + 20 + Margin.Top);
                            break;
                        case LinkNodeTypes.CENTER:
                            break;
                        case LinkNodeTypes.NULL:
                            break;
                        default:
                            break;
                    }
                }

                evtStartSearchPath(new Point(StartPoint.Position.X + Margin.Left,
                                             StartPoint.Position.Y + Margin.Top),
                                   new Point(EndPoint.Position.X + Margin.Left,
                                             EndPoint.Position.Y + Margin.Top),
                                             start1, end1);
            }
        }



    }

}
