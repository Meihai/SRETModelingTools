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
    /// 尺寸发生变化
    /// </summary>
    /// <param name="difLeft">位置x的差值</param>
    /// <param name="difTop">位置y的差值</param>
    /// <param name="difWidth">宽度差值</param>
    /// <param name="difHeight">高度差值</param>
    public delegate void delSizeChanged(double difLeft, double difTop, double difWidth, double difHeight);


    public delegate void delPositionChanged(double left, double top);


    public delegate void delPrepareToChangePosition(bool flag);


    /// <summary>
    /// CtrlShape.xaml 的交互逻辑
    /// </summary>
    [System.ComponentModel.DesignTimeVisible(false)]
    public partial class CtrlShape : UserControl
    {

        /// <summary>
        /// 尺寸或位置改变时触发的事件
        /// </summary>
        public event delSizeChanged evtSizeChanged;

        public event delPositionChanged evtPositionChanged;

        public event delPrepareToChangePosition evtPrepareToChangePosition;

        /// <summary>
        /// 修改尺寸的方式
        /// </summary>
        private CtrlNodeTypes ChangeSizeType = CtrlNodeTypes.NO_CHANGE;


        private Point _offset = new Point(0, 0);
        /// <summary>
        /// 鼠标在控件上点击时的坐标
        /// </summary>
        public Point Offset
        {
            set { this._offset = value; }
        }

        public CtrlShape()
        {
            InitializeComponent();

            for (int i = 0; i < viewer.Children.Count; i++)
            {
                if (viewer.Children[i] is CtrlNode)
                {
                    ((CtrlNode)viewer.Children[i]).evtMouseDownOnCtrlNode += new delMouseDownOnCtrlNode(CtrlShape_evtMouseDownOnCtrlNode);
                }
            }

            GlobalMouseHook.evtGlobalMouseUp += new delGlobalMouseUp(GlobalMouseHook_evtGlobalMouseUp);
        }

        void CtrlShape_evtMouseDownOnCtrlNode(CtrlNode ctrlNode)
        {
            this.ChangeSizeType = ctrlNode.CtrlNodeType;

            if (null != evtPrepareToChangePosition)
            {
                evtPrepareToChangePosition(false);
            }
        }

        void GlobalMouseHook_evtGlobalMouseUp(GlobalMouseArgs e)
        {
            this.ChangeSizeType = CtrlNodeTypes.NO_CHANGE;
        }

        /// <summary>
        /// 接收鼠标移动事件
        /// </summary>
        /// <param name="e"></param>
        public void AcceptMouseMove(MouseEventArgs e)
        {
            if (this.Visibility == Visibility.Hidden || e.LeftButton == MouseButtonState.Released)
            {
                return;
            }

            double oldx, oldy, oldw, oldh;

            double w, h, x, y;
            oldw = w = Width;
            oldh = h = Height;
            oldx = x = Margin.Left;
            oldy = y = Margin.Top;
            //控制点移动放大缩小去掉
            //switch (this.ChangeSizeType)
            //{
            //    case CtrlNodeTypes.LEFT:
            //        w = w - e.GetPosition(this).X;
            //        x = x + e.GetPosition(this).X;
            //        break;
            //    case CtrlNodeTypes.TOP:
            //        h = h - e.GetPosition(this).Y;
            //        y = y + e.GetPosition(this).Y;
            //        break;
            //    case CtrlNodeTypes.RIGHT:
            //        w = e.GetPosition(this).X;
            //        break;
            //    case CtrlNodeTypes.BOTTOM:
            //        h = e.GetPosition(this).Y;
            //        break;
            //    case CtrlNodeTypes.LEFT_TOP:
            //        w = w - e.GetPosition(this).X;
            //        x = x + e.GetPosition(this).X;
            //        h = h - e.GetPosition(this).Y;
            //        y = y + e.GetPosition(this).Y;
            //        break;
            //    case CtrlNodeTypes.RIGHT_TOP:
            //        w = e.GetPosition(this).X;
            //        h = h - e.GetPosition(this).Y;
            //        y = y + e.GetPosition(this).Y;
            //        break;
            //    case CtrlNodeTypes.RIGHT_BOTTOM:
            //        w = e.GetPosition(this).X;
            //        h = e.GetPosition(this).Y;
            //        break;
            //    case CtrlNodeTypes.LEFT_BOTTOM:
            //        w = w - e.GetPosition(this).X;
            //        x = x + e.GetPosition(this).X;
            //        h = e.GetPosition(this).Y;
            //        break;
            //    case CtrlNodeTypes.ALL:
            //        break;
            //    case CtrlNodeTypes.START:
            //        break;
            //    case CtrlNodeTypes.END:
            //        break;
            //    case CtrlNodeTypes.POSITION:
            //        x = x + e.GetPosition(this).X - this._offset.X;
            //        y = y + e.GetPosition(this).Y - this._offset.Y;
            //        break;
            //    case CtrlNodeTypes.NO_CHANGE:
            //        break;
            //    default:
            //        break;
            //}

            Margin = new Thickness(x, y, 0, 0);
            w = w > 10 ? w : 10;
            h = h > 5 ? h : 5;
            Width = w;
            Height = h;

            if (ChangeSizeType == CtrlNodeTypes.POSITION && null != evtPositionChanged)
            {
                evtPositionChanged(x - oldx, y - oldy);
            }
            else if (null != evtSizeChanged)
            {
                evtSizeChanged(x - oldx, y - oldy, w - oldw, h - oldh);
            }
        }



        private void Rectangle_MouseEnter(object sender, MouseEventArgs e)
        {
            Cursor = Cursors.SizeAll;
        }

        private void Rectangle_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Offset = e.GetPosition(this);
            this.ChangeSizeType = CtrlNodeTypes.POSITION;

            if (null != evtPrepareToChangePosition)
            {
                evtPrepareToChangePosition(true);
            }
        }

    }
}
