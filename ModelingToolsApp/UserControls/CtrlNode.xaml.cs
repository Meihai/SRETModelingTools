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
    /// Interaction logic for CtrlNode.xaml
    /// </summary>
    public delegate void delMouseDownOnCtrlNode(CtrlNode ctrlNode);

    /// <summary>
    /// CtrlNode.xaml 的交互逻辑
    /// </summary>
    [System.ComponentModel.DesignTimeVisible(false)]
    public partial class CtrlNode : UserControl
    {
        /// <summary>
        /// 鼠标在控制点上移动
        /// </summary>
        public event delMouseDownOnCtrlNode evtMouseDownOnCtrlNode;

        private int _id = 0;
        /// <summary>
        /// 编号
        /// </summary>
        public int Id
        {
            get { return this._id; }
        }

        private LinePointTypes _linePointType;
        /// <summary>
        /// 控制点在连接线中位置
        /// </summary>
        public LinePointTypes LinePointType
        {
            get { return this._linePointType; }
        }


        private CtrlNodeTypes _ctrlNodeType;
        /// <summary>
        /// 控制类型
        /// </summary>
        public CtrlNodeTypes CtrlNodeType
        {
            get { return this._ctrlNodeType; }
            set { this._ctrlNodeType = value; }
        }


        public CtrlNode()
        {
            InitializeComponent();

            HorizontalAlignment = HorizontalAlignment.Left;
            VerticalAlignment = VerticalAlignment.Top;
        }


        public CtrlNode(int id, LinePointTypes lcnt, CtrlNodeTypes ctrlNodeType)
            : this()
        {
            this._id = id;
            this._ctrlNodeType = ctrlNodeType;
            this._linePointType = lcnt;

            switch (lcnt)
            {
                case LinePointTypes.StartPoint:
                    break;
                case LinePointTypes.EndPoint:
                    break;
                case LinePointTypes.BreakPoint:
                    p1.Visibility = Visibility.Hidden;
                    p2.Visibility = Visibility.Visible;
                    p3.Visibility = Visibility.Hidden;
                    break;
                case LinePointTypes.MiddlePoint:
                    p1.Visibility = Visibility.Hidden;
                    p2.Visibility = Visibility.Hidden;
                    p3.Visibility = Visibility.Visible;
                    break;
                default:
                    break;
            }
        }

        private void Canvas_MouseEnter(object sender, MouseEventArgs e)
        {
            switch (_ctrlNodeType)
            {
                case CtrlNodeTypes.LEFT:
                    Cursor = Cursors.SizeWE;//鼠标箭头从西到东
                    break;
                case CtrlNodeTypes.TOP:
                    Cursor = Cursors.SizeNS;//北南方向
                    break;
                case CtrlNodeTypes.RIGHT:
                    Cursor = Cursors.SizeWE;
                    break;
                case CtrlNodeTypes.BOTTOM:
                    Cursor = Cursors.SizeNS;
                    break;
                case CtrlNodeTypes.LEFT_TOP:
                    Cursor = Cursors.SizeNWSE;//西北东南
                    break;
                case CtrlNodeTypes.RIGHT_TOP:
                    Cursor = Cursors.SizeNESW;
                    break;
                case CtrlNodeTypes.RIGHT_BOTTOM:
                    Cursor = Cursors.SizeNWSE;
                    break;
                case CtrlNodeTypes.LEFT_BOTTOM:
                    Cursor = Cursors.SizeNESW;
                    break;
                case CtrlNodeTypes.NO_CHANGE:
                    break;
                case CtrlNodeTypes.ALL:
                    Cursor = Cursors.SizeAll;//全方向
                    break;
                case CtrlNodeTypes.START:
                    Cursor = Cursors.SizeAll;
                    break;
                case CtrlNodeTypes.END:
                    Cursor = Cursors.SizeAll;
                    break;
                default:
                    break;
            }
        }

        private void Canvas_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (null != evtMouseDownOnCtrlNode)
            {
                evtMouseDownOnCtrlNode(this);
            }
        }
    }
}
