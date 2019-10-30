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
using System.Windows.Threading;
using System.Xml;

namespace ModelingToolsAppWithMVVM.Common.ChartCommon
{
    /// <summary>
    /// 停靠点的逻辑交互
    /// </summary>
    /// <param name="e"></param>
    public delegate void delDockedLinkNode(DockedLinkNodeArgs e);

    public delegate void delOpenSubModelArea(ShapeBase modelItem);

    public delegate void delMouseDownOnShapeLinkNode(LinkNodeArgs e);

    public delegate void delMouseEnterOnShapeLinkNode(LinkNodeArgs e);

    /// <summary>
    /// ShapeBase.xaml 的交互逻辑
    /// </summary>
    public abstract partial class ShapeBase : UserControl, IFlowChartBase
    {
        //鼠标在对应形状上的连接点移动
        public event delMouseDownOnShapeLinkNode evtMouseDownOnShapeBaseLinkNode; 

        public event delMouseEnterOnShapeLinkNode evtMouseEnterOnShapeBaseLinkNode;

        public List<string> SerializeAttributes { get; internal set; }

        internal abstract void SetSerializeAttributes();

        public ShapeBase()
        {
            InitializeComponent();
            SetSerializeAttributes();
            CreateShape();

            //对于新建形状，克隆id就是本身id
            this.CloneSourceId = Id;
            this.ContextMenu = createContextMenu();
            this.lLinkNode.evtMouseDownOnLinkNode += MouseDownOnShapeBaseLinkNode;
            this.lLinkNode.Center = new Point(40, 50);
            this.lLinkNode.evtMouseEnterOnLinkNode += MouseEnterOnShapeBaseLinkNode;

            this.tLinkNode.evtMouseDownOnLinkNode += MouseDownOnShapeBaseLinkNode;
            this.tLinkNode.evtMouseEnterOnLinkNode+= MouseEnterOnShapeBaseLinkNode;

            this.rLinkNode.evtMouseDownOnLinkNode += MouseDownOnShapeBaseLinkNode;
            this.rLinkNode.evtMouseEnterOnLinkNode+=MouseEnterOnShapeBaseLinkNode;

            this.bLinkNode.evtMouseDownOnLinkNode += MouseDownOnShapeBaseLinkNode;
            this.bLinkNode.evtMouseEnterOnLinkNode+=MouseEnterOnShapeBaseLinkNode;
        }

        //处理单击连接点
        private void MouseDownOnShapeBaseLinkNode(LinkNode linkNode)
        {
            if (evtMouseDownOnShapeBaseLinkNode != null)
            {
                LinkNodeArgs linknodeArgs = new LinkNodeArgs(this, linkNode);
                evtMouseDownOnShapeBaseLinkNode(linknodeArgs);
            }
        }

        private void MouseEnterOnShapeBaseLinkNode(LinkNode linkNode){
            if(null!= evtMouseEnterOnShapeBaseLinkNode){
                LinkNodeArgs linknodeArgs=new LinkNodeArgs(this,linkNode);
                evtMouseEnterOnShapeBaseLinkNode(linknodeArgs);
            }
        }

        private ContextMenu createContextMenu(){
            ContextMenu cm = new ContextMenu();
            MenuItem mi = new MenuItem();
            mi.Header = "打开子系统";
            mi.Click += OpenSubModelArea;
            cm.Items.Add(mi);
            return cm;            
            
        }




        /// <summary>
        /// 建模子窗口打开事件
        /// </summary>
        public event delOpenSubModelArea evtOpenSubModelArea;
        private void OpenSubModelArea(object sender, RoutedEventArgs e)
        {
            if (null!=evtOpenSubModelArea) {
                evtOpenSubModelArea(this);
            }
        }

        /// <summary>
        /// 形状停靠到联系点时
        /// </summary>
        public event delDockedLinkNode evtDocketLinkNode;
        public void OnDockedLinkNode(DockedLinkNodeArgs e)
        {
            if (null != evtDocketLinkNode)
            {
                evtDocketLinkNode(e);
            }
        }

        /// <summary>
        /// 控制点的尺寸
        /// </summary>
        public double CtrlNodeSize = 6.5;

        #region IFlowChartBase 成员

        private string _id = Guid.NewGuid().ToString();
        public string Id
        {
            get { return this._id; }
            set { this._id = value; }
        }

        private string _cloneSourceId;
        public string CloneSourceId
        {
            get { return this._cloneSourceId; }
            set { this._cloneSourceId = value; }
        }

        private object _propertyModel;
        public virtual object PropertyModel
        {
            get { return this._propertyModel; }
            set { this._propertyModel = value; }
        }

        public string Description
        {
            get { return this.txtDescription.Text; }
            set
            {
                txtDescription.Text = value;
                if (value.Trim().Length == 0)
                {
                    txtDescription.Visibility = Visibility.Hidden;
                }
                else
                {
                    txtDescription.Visibility = Visibility.Visible;
                }
            }
        }


        private bool _isSetProperty = false;
        /// <summary>
        /// 是否设置了流程属性
        /// </summary>
        public bool IsSetProperty
        {
            get { return this._isSetProperty; }
            set { this._isSetProperty = value; }
        }

        private bool _isSelected = false;
        public bool IsSelected
        {
            get { return this._isSelected; }
            set
            {
                if (value)
                {
                    if (_isMultiSelected)
                    {
                        return;
                    }
                    else
                    {
                        this._isSelected = value;
                        OnPropertyChanged(value == true ? ChangedPropertys.SelectTrue : ChangedPropertys.SelectFalse);
                    }
                }
                else
                {
                    this._isSelected = value;
                }
            }
        }

        private bool _isMultiSelected = false;
        public bool IsMultiSelected
        {
            get { return this._isMultiSelected; }
            set
            {
                this._isMultiSelected = value;
                if (value)
                {
                    pathShape.Stroke = new SolidColorBrush(Colors.Fuchsia);
                }
                else
                {
                    pathShape.Stroke = new SolidColorBrush(Colors.Black);
                }
            }
        }

        private PointCollection _range = new PointCollection();
        public PointCollection Range
        {
            get { return this._range; }
        }

        public List<SimpleLinkNode> LinkNodes
        {
            get
            {
                List<SimpleLinkNode> pnts = new List<SimpleLinkNode>();
                //pnts.Add(new SimpleLinkNode(cLinkNode.LinkNodeType,cLinkNode.Center));
                
                pnts.Add(new SimpleLinkNode(lLinkNode.LinkNodeType, lLinkNode.Center));                
                pnts.Add(new SimpleLinkNode(tLinkNode.LinkNodeType, tLinkNode.Center));
                pnts.Add(new SimpleLinkNode(rLinkNode.LinkNodeType, rLinkNode.Center));
                pnts.Add(new SimpleLinkNode(bLinkNode.LinkNodeType, bLinkNode.Center));
                 
                return pnts;
            }
        }

        private Point _offset = new Point(0, 0);
        public Point Offset
        {
            get { return this._offset; }
            set { this._offset = value; }
        }

        public  FlowChartTypes FlowChartType
        {
            get;
            internal set;
        }

        public abstract void CreateShape();

        public void ChangePositionAndSize(double difLeft, double difTop, double difWidth, double difHeight)
        {
            if (this.Width + difWidth > 20 && this.Height + difHeight > 20)
            {
                this.Margin = new Thickness(Margin.Left + difLeft, Margin.Top + difTop, 0, 0);
                this.Width = this.Width + difWidth;
                this.Height = this.Height + difHeight;
                CreateShape();
                CalcShapeRange();

                OnPropertyChanged(ChangedPropertys.Position);
            }
        }

        //形状只接收连接线的移动，连接线最多两个端点，所以数组长度为2
        bool[] IsOnLinkNode = new bool[] { false, false };
        public void AcceptShapeMove(IFlowChartBase ifcb)
        {
            if (ifcb is ShapeBase)
            {
                return;
            }

            int seq = 0;
            foreach (SimpleLinkNode sln in ifcb.LinkNodes)
            {
                Point p = new Point(sln.Position.X + ifcb.Margin.Left - Margin.Left, sln.Position.Y + ifcb.Margin.Top - Margin.Top);

                int pos = 0;
                double difx = 0;
                double dify = 0;
                bool flag = false;

                //判断点是否在联系点范围内
                if (p.X < 0 || p.Y < 0 || p.X > Width || p.Y > Height)
                {
                    if (IsOnLinkNode[seq])
                    {
                        flag = false;
                    }
                }
                else
                {
                    for (int i = 0; i < this.LinkNodes.Count; i++)
                    {
                        double size = 6.5;

                        if (p.X <= LinkNodes[i].Position.X + size && p.X >= LinkNodes[i].Position.X - size &&
                            p.Y <= LinkNodes[i].Position.Y + size && p.Y >= LinkNodes[i].Position.Y - size)
                        {
                            difx = LinkNodes[i].Position.X - p.X;
                            dify = LinkNodes[i].Position.Y - p.Y;
                            pos = i;
                            flag = true;
                            break;
                        }
                    }
                }

                //如果在联系点内，实现停靠
                if (flag)
                {
                    LineTerminalPoint ltp = new LineTerminalPoint();
                    ltp.Position = new Point(this.LinkNodes[pos].Position.X + Margin.Left - ifcb.Margin.Left,
                        this.LinkNodes[pos].Position.Y + Margin.Top - ifcb.Margin.Top);
                    ltp.DockedFlag = true;
                    ltp.RelatedShapeId = this.Id;
                    ltp.RelatedType = this.LinkNodes[pos].LinkNodeType;


                    if (0 == seq)
                    {
                        ((LinkBase)ifcb).StartPnt.RelatedShapeId = this.Id;
                        ((LinkBase)ifcb).StartPnt.DockedFlag = flag;
                        ((LinkBase)ifcb).StartPnt.RelatedType = this.LinkNodes[pos].LinkNodeType;
                    }
                    else if (1 == seq)
                    {
                        ((LinkBase)ifcb).EndPnt.RelatedShapeId = this.Id;
                        ((LinkBase)ifcb).EndPnt.DockedFlag = flag;
                        ((LinkBase)ifcb).EndPnt.RelatedType = this.LinkNodes[pos].LinkNodeType;
                    }

                    ifcb.ChangePositionAndSize(difx, dify, 0, 0);
                }
                else
                {
                    if (0 == seq && ((LinkBase)ifcb).StartPnt.DockedFlag && ((LinkBase)ifcb).StartPnt.RelatedShapeId == Id)
                    {
                        ((LinkBase)ifcb).StartPnt.RelatedShapeId = "";
                        ((LinkBase)ifcb).StartPnt.DockedFlag = flag;
                    }
                    else if (1 == seq && ((LinkBase)ifcb).EndPnt.DockedFlag && ((LinkBase)ifcb).EndPnt.RelatedShapeId == Id)
                    {
                        ((LinkBase)ifcb).EndPnt.RelatedShapeId = "";
                        ((LinkBase)ifcb).EndPnt.DockedFlag = flag;
                    }
                }


                //停靠属性发生变化时，触发停靠事件
                if (IsOnLinkNode[seq] != flag)
                {
                    IsOnLinkNode[seq] = flag;
                    DockedLinkNodeArgs e = new DockedLinkNodeArgs(seq, IsOnLinkNode[seq], this, this.LinkNodes[pos]);
                    OnDockedLinkNode(e);
                }

                seq++;
            }
        }

        public bool GetDescriptionPosition(out Point position)
        {
            position = new Point(0, 0);
            return false;
        }

        public void MoveToClone(IFlowChartBase cloneIfcb)
        {
            ChangePositionAndSize(cloneIfcb.Margin.Left - Margin.Left, cloneIfcb.Margin.Top - Margin.Top, 0, 0);
        }

        public event delMouseClick evtMouseClick;
        private void OnMouseClick()
        {
            if (null != evtMouseClick)
            {
                evtMouseClick(this);
            }
        }

        public event delMouseDoubleClick evtMouseDoubleClick;
        private void OnMouseDoubleClick()
        {
            if (null != evtMouseDoubleClick)
            {
                evtMouseDoubleClick(this, Description);
            }
        }


       

        #endregion

        #region ICloneable 成员

        public abstract object Clone();

        #endregion

        #region INotifyPropertyChanged 成员

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(ChangedPropertys changedProperty)
        {
            if (null != PropertyChanged)
            {
                PropertyChangedEventArgs e = new PropertyChangedEventArgs(changedProperty.ToString());
                PropertyChanged(this, e);
            }
        }

        #endregion


        /// <summary>
        /// 计算形状的范围
        /// </summary>
        private void CalcShapeRange()
        {
            //以下点，必须按点顺序构成一个多边形
            _range.Clear();
            _range.Add(new Point(Margin.Left, Margin.Top));
            _range.Add(new Point(Margin.Left + this.Width, Margin.Top));
            _range.Add(new Point(Margin.Left + this.Width, Margin.Top + this.Height));
            _range.Add(new Point(Margin.Left, Margin.Top + this.Height));
        }

        /// <summary>
        /// 重新计算联系点的中心位置
        /// </summary>
        public abstract void RepositionLinkNode();

        /// <summary>
        /// 鼠标点击计数器
        /// </summary>
        private int MouseClickCounter = 0;
        private void pathShapeEx_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.Offset = e.GetPosition(this);

            MouseClickCounter += 1;
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 0, 0, 300);
            timer.Tick += (s, e1) => { timer.IsEnabled = false; MouseClickCounter = 0; };
            timer.IsEnabled = true;

            if (MouseClickCounter % 2 == 0)
            {
                timer.IsEnabled = false;
                MouseClickCounter = 0;

                if (null != evtMouseDoubleClick)
                {
                    OnMouseDoubleClick();
                }
            }
            else
            {
                if (!IsMultiSelected)
                {
                    this.IsSelected = true;
                }

                OnMouseClick();
            }
        }

        private void pathShapeEx_MouseEnter(object sender, MouseEventArgs e)
        {
            Cursor = Cursors.SizeAll;
        }

        private bool IsMouseOnLinkNode = false;
        public void AcceptPointMove(Point point, int seq)
        {
            Point p = new Point(point.X - Margin.Left, point.Y - Margin.Top);
            bool flag = false;
            int pos = 0;

            if (p.X < 0 || p.Y < 0 || p.X > Width || p.Y > Height)
            {
                if (IsMouseOnLinkNode)
                {
                    flag = false;
                }
            }
            else
            {
                for (int i = 0; i < this.LinkNodes.Count; i++)
                {
                    double size = 6.5;
                    if (p.X <= LinkNodes[i].Position.X + size && p.X >= LinkNodes[i].Position.X - size &&
                        p.Y <= LinkNodes[i].Position.Y + size && p.Y >= LinkNodes[i].Position.Y - size)
                    {
                        //difx = LinkNodes[i].Position.X - p.X;
                        //dify = LinkNodes[i].Position.Y - p.Y;
                        pos = i;
                        flag = true;
                        break;
                    }
                }
            }

            //停靠属性发生变化时，触发停靠事件
            if (IsMouseOnLinkNode != flag)
            {
                IsMouseOnLinkNode = flag;
                DockedLinkNodeArgs e = new DockedLinkNodeArgs(seq, IsMouseOnLinkNode, this, this.LinkNodes[pos]);
                OnDockedLinkNode(e);
            }
        }

        /// <summary>
        /// 剖面下面的子剖面
        /// </summary>
        public BaseWorkModel ChildWorkModel { get; set; }


        #region 为持久化添加额外的属性
        public double Left
        {
            get
            {
                return this.Margin.Left;
            }
            set
            {
                double top = this.Margin.Top;
                this.Margin = new Thickness(value, top, 0, 0);
            }
        }

        public double Top
        {
            get { return this.Margin.Top; }
            set
            {
                double left = this.Margin.Left;
                this.Margin = new Thickness(left, value, 0, 0);
            }
        }

        public int ZIndex
        {
            get;
            set;
        }
        #endregion 为持久化添加额外的属性
     
                
        
    }
}
