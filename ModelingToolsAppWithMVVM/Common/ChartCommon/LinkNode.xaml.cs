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
    /// 鼠标键按下
    /// </summary>
    public delegate void delMouseDownOnLinkNode(LinkNode linkNode);

    /// <summary>
    /// 鼠标进入连接点
    /// </summary>
    /// <param name="linkNode"></param>
    public delegate void delMouseEnterOnLinkNode(LinkNode linkNode);
    


    /// <summary>    ///  
    /// LinkNode.xaml 的交互逻辑
    /// </summary>
    [System.ComponentModel.DesignTimeVisible(false)]
    public partial class LinkNode : UserControl
    {
        LinkNodeTypes linkNodeType = LinkNodeTypes.NULL;
        /// <summary>
        /// 联系点类型
        /// </summary>
        public LinkNodeTypes LinkNodeType
        {
            get { return this.linkNodeType; }
            set { this.linkNodeType = value; }
        }


        /// <summary>
        /// 鼠标在连接点上键按下
        /// </summary>
        public event delMouseDownOnLinkNode evtMouseDownOnLinkNode;

        public event delMouseEnterOnLinkNode evtMouseEnterOnLinkNode;

        private Point _center = new Point(0, 0);
        /// <summary>
        /// 联系点中心坐标
        /// </summary>
        public Point Center
        {
            get { return _center; }
            set
            {
                _center = value;
                _minCorner = new Point(_center.X - this.Width / 2.0, _center.Y - Height / 2.0);
                _maxCorner = new Point(_center.X + this.Width / 2.0, _center.Y + Height / 2.0);
            }
        }
            

        //定义如下两个变量，为提高在判断鼠标停靠时的效率
        private Point _minCorner;
        /// <summary>
        /// 左上角坐标
        /// </summary>
        public Point MinCorner
        {
            get { return this._minCorner; }
        }
        private Point _maxCorner;
        /// <summary>
        /// 右下角坐标
        /// </summary>        
        public Point MaxCorner
        {
            get { return this._maxCorner; }
        }

        public LinkNode()
        {
            InitializeComponent();
        }

        private void Canvas_MouseEnter(object sender, MouseEventArgs e)
        {
            
            Cursor = Cursors.Cross;
            if (null != evtMouseEnterOnLinkNode)
            {
                evtMouseEnterOnLinkNode(this);
            }
           
        }

        private void Canvas_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (null != evtMouseDownOnLinkNode)
            {
                evtMouseDownOnLinkNode(this);
            }
        }


    }
}
