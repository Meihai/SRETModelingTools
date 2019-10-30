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
using System.Collections;
using System.Windows.Threading;
using ModelingToolsAppWithMVVM.Common.ChartCommon;

namespace ModelingToolsAppWithMVVM.Common.ChartCommon
{
    public delegate void delRefreshTmpLine(Geometry geometry);  

    /// <summary>
    /// LinkBase.xaml 的交互逻辑
    /// </summary>    
    public abstract partial class LinkBase : UserControl, IFlowChartBase
    {
        public LinkBase()
        {
            InitializeComponent();
            SetSerializeAttributes();
            InitializeShapePnt();
            this.CloneSourceId = Id;
            CreateShape();
        }


        public List<string> SerializeAttributes { get; internal set; }

        internal abstract void SetSerializeAttributes();

        /// <summary>
        /// 鼠标双击时触发
        /// </summary>
        public event delMouseDoubleClick evtMouseDoubleClick; 


       
        /// <summary>
        /// 刷新临时连接线
        /// </summary>
        public event delRefreshTmpLine evtRefreshTmpLine;
        protected void OnRefreshTmpLine(Geometry gemoretry)
        {
            if (null != evtRefreshTmpLine)
            {
                evtRefreshTmpLine(gemoretry);
            }
        }




        /// <summary>
        /// 是否手动设置过
        /// </summary>
        private bool _isManualSetted = false;

        public bool IsManualSetted
        {
            get { return this._isManualSetted; }
            set { this._isManualSetted = value; }
        }

        private LinkLineTypes _linkLineType = LinkLineTypes.Straight;
        private Brush _lineBrush = new SolidColorBrush(Colors.Black);

        public Brush LineBrush
        {
            get { return this._lineBrush; }
            set { this._lineBrush = value;
                  pathLink.Stroke = _lineBrush;
                  pathEnd.Fill = _lineBrush;
                 }
        }
      
        /// <summary>
        /// 连接线类型
        /// </summary>
        public LinkLineTypes LinkLineType
        {
            get { return this._linkLineType; }
            set { this._linkLineType = value; }
        }
        /// <summary>
        /// 线条类型
        /// </summary>
        private LineTypes _lineType = LineTypes.Solid;
        public LineTypes LineType
        {
            get { return this._lineType; }
            set { this._lineType = value; }
        }

        LineTerminalPoint _startPnt;
        /// <summary>
        /// 起点
        /// </summary>
        public LineTerminalPoint StartPnt
        {
            get { return this._startPnt; }
            set { this._startPnt = value; }
        }

        LineTerminalPoint _endPnt;
        /// <summary>
        /// 终点
        /// </summary>
        public LineTerminalPoint EndPnt
        {
            get { return this._endPnt; }
            set { this._endPnt = value; }
        }

        PointCollection _shapePnt = new PointCollection();
        /// <summary>
        /// 形状点
        /// </summary>
        public PointCollection ShapePnt
        {
            get { return this._shapePnt; }
            set { this._shapePnt = value; }
        }

        private Hashtable _properties = new Hashtable();
        /// <summary>
        /// 需要显示的属性集合
        /// </summary>
        public Hashtable Properties
        {
            get
            {
                _properties["desc"] = txtDescription.Text;
                return _properties;
            }
            set { _properties = value; }
        }

        private void pathRange_MouseEnter(object sender, MouseEventArgs e)
        {
            Cursor = Cursors.SizeAll;
        }

        /// <summary>
        /// 鼠标点击计数器
        /// </summary>
        private int MouseClickCounter = 0;
        private void pathRange_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            Offset = e.GetPosition(pathRange);

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
                    evtMouseDoubleClick(this, txtDescription.Text);
                }
            }
            else
            {
                if (!IsMultiSelected)
                {
                    this.IsSelected = true;
                }

                if (null != evtMouseClick)
                {
                    evtMouseClick(this);
                }
            }
        }

        #region IFlowChartBase 成员

        private string _id = Guid.NewGuid().ToString();
        public string Id
        {
            get { return this._id; }
            set { this._id = value; }
        }

        private string _cloneSourceId="";
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
                this.txtDescription.Text = value;
                if (value.Trim().Length == 0)
                {
                    txtDescription.Visibility = Visibility.Hidden;
                }
                else
                {
                    txtDescription.Visibility = Visibility.Visible;
                    SetDescTextPosition();
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
                    pathLink.Stroke = new SolidColorBrush(Colors.Fuchsia);
                }
              //  else
              //  {
             //       pathLink.Stroke = new SolidColorBrush(Colors.Black);
             //   }
            }
        }

        public FlowChartTypes FlowChartType
        {
            get;
            internal set;
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
                pnts.Add(new SimpleLinkNode(LinkNodeTypes.START, StartPnt.Position));
                pnts.Add(new SimpleLinkNode(LinkNodeTypes.END, EndPnt.Position));
                return pnts;
            }
        }


        private Point _offset = new Point(0, 0);
        public Point Offset
        {
            get { return this._offset; }
            set { this._offset = value; }
        }

        public event delMouseClick evtMouseClick;

        /// <summary>
        /// 处理连接线终点所停靠的形状位置变化时，连接线端点位置作相应变化
        /// </summary>
        /// <param name="ifcb"></param>
        public virtual void MoveToDockedShape(IFlowChartBase ifcb)
        {
            if (ifcb is LinkBase)
            {
                return;
            }

            int seq = 0;
            foreach (SimpleLinkNode sln in this.LinkNodes)
            {
                //连接线端点已经停靠到形状，不再接受形状停靠
                if (0 == seq && StartPnt.DockedFlag)
                {
                    seq++;
                    continue;
                }

                if (1 == seq && EndPnt.DockedFlag)
                {
                    seq++;
                    continue;
                }

                Point p = new Point(sln.Position.X - ifcb.Margin.Left + Margin.Left, sln.Position.Y - ifcb.Margin.Top + Margin.Top);

                int pos = 0;
                double difx = 0;
                double dify = 0;
                bool flag = false;

                //判断点是否在联系点范围内
                if (p.X < 0 || p.Y < 0 || p.X > ifcb.Width || p.Y > ifcb.Height)
                {
                    if (IsMouseOnLinkNode[seq])
                    {
                        flag = false;
                    }
                }
                else
                {
                    for (int i = 0; i < ifcb.LinkNodes.Count; i++)
                    {
                        double size = 6.5;

                        if (p.X <= ifcb.LinkNodes[i].Position.X + size && p.X >= ifcb.LinkNodes[i].Position.X - size &&
                            p.Y <= ifcb.LinkNodes[i].Position.Y + size && p.Y >= ifcb.LinkNodes[i].Position.Y - size)
                        {
                            difx = p.X - ifcb.LinkNodes[i].Position.X;
                            dify = p.Y - ifcb.LinkNodes[i].Position.Y;
                            pos = i;
                            flag = true;
                            break;
                        }
                    }
                }

                //如果在联系点内，实现停靠
                if (flag)
                {
                    if (0 == seq)
                    {
                        this.StartPnt.DockedFlag = flag;
                        this.StartPnt.RelatedShapeId = ifcb.Id;
                        this.StartPnt.RelatedType = ifcb.LinkNodes[pos].LinkNodeType;
                    }
                    else if (1 == seq)
                    {
                        this.EndPnt.DockedFlag = flag;
                        this.EndPnt.RelatedShapeId = ifcb.Id;
                        this.EndPnt.RelatedType = ifcb.LinkNodes[pos].LinkNodeType;
                    }

                    if (difx != 0 && dify != 0)
                    {
                        ifcb.ChangePositionAndSize(difx, dify, 0, 0);
                    }

                }
                else
                {
                    //if (0 == seq)
                    //{
                    //    this.StartPnt.RelatedShapeId = "";
                    //    this.StartPnt.DockedFlag = flag;
                    //}
                    //else if (1 == seq)
                    //{
                    //    this.EndPnt.RelatedShapeId = "";
                    //    this.EndPnt.DockedFlag = flag;
                    //}
                }

                seq++;
            }
        }

        public void ChangePositionAndSize(double difLeft, double difTop, double difWidth, double difHeight)
        {
            this.Margin = new Thickness(Margin.Left + difLeft, Margin.Top + difTop, 0, 0);            
            CalcShapeRange();
            OnPropertyChanged(ChangedPropertys.Position);
        }

        public void AcceptPointMove(Point point, int seq)
        {
            //连接线不需要处理
        }

        public abstract void MoveToClone(IFlowChartBase cloneIfcb);

        //形状只接收连接线的移动，连接线最多两个端点，所以数组长度为2
        bool[] IsMouseOnLinkNode = new bool[] { false, false };
        public void AcceptShapeMove(IFlowChartBase ifcb)
        {
            if (ifcb is LinkBase)
            {
                return;
            }

            int seq = 0;
            foreach (SimpleLinkNode sln in this.LinkNodes)
            {
                //连接线端点已经停靠到形状，不再接受形状停靠
                if (0 == seq && StartPnt.DockedFlag)
                {
                    seq++;
                    continue;
                }

                if (1 == seq && EndPnt.DockedFlag)
                {
                    seq++;
                    continue;
                }

                Point p = new Point(sln.Position.X - ifcb.Margin.Left + Margin.Left, sln.Position.Y - ifcb.Margin.Top + Margin.Top);

                int pos = 0;
                double difx = 0;
                double dify = 0;
                bool flag = false;

                //判断点是否在联系点范围内
                if (p.X < 0 || p.Y < 0 || p.X > ifcb.Width || p.Y > ifcb.Height)
                {
                    if (IsMouseOnLinkNode[seq])
                    {
                        flag = false;
                    }
                }
                else
                {
                    for (int i = 0; i < ifcb.LinkNodes.Count; i++)
                    {
                        double size = 6.5;

                        if (p.X <= ifcb.LinkNodes[i].Position.X + size && p.X >= ifcb.LinkNodes[i].Position.X - size &&
                            p.Y <= ifcb.LinkNodes[i].Position.Y + size && p.Y >= ifcb.LinkNodes[i].Position.Y - size)
                        {
                            difx = p.X - ifcb.LinkNodes[i].Position.X;
                            dify = p.Y - ifcb.LinkNodes[i].Position.Y;
                            pos = i;
                            flag = true;
                            break;
                        }
                    }
                }

                //如果在联系点内，实现停靠
                if (flag)
                {
                    //ifcb.ChangePositionAndSize(difx, dify, 0, 0);
                }

                //停靠属性发生变化时，触发停靠事件
                if (IsMouseOnLinkNode[seq] != flag)
                {
                    IsMouseOnLinkNode[seq] = flag;
                    DockedLinkNodeArgs e = new DockedLinkNodeArgs(seq, IsMouseOnLinkNode[seq], this, this.LinkNodes[seq]);
                    ((ShapeBase)ifcb).OnDockedLinkNode(e);
                }

                seq++;
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
        /// 计算说明文字的所占的尺寸(像素)
        /// </summary>
        /// <returns></returns>
        protected void CalcDescTextSize(out double width, out double height)
        {
            width = 0;
            height = 0;
            //区分汉字和英文字母，统计宽度
            for (int i = 0; i < Description.Trim().Length; i++)
            {
                if (Description[i] <= 127)
                {
                    width += txtDescription.FontSize;
                }
                else
                {
                    width += txtDescription.FontSize * 2;
                }
            }
            height = txtDescription.FontSize;
            return;
        }

        /// <summary>
        /// 设置连接线的点
        /// </summary>
        /// <param name="startPnt">起点</param>
        /// <param name="endPnt">终点</param>
        /// <param name="linePnt">形状点</param>
        public void SetLinePnts(LineTerminalPoint startPnt, LineTerminalPoint endPnt, PointCollection linePnt)
        {
            this.StartPnt = startPnt;
            this.EndPnt = endPnt;
            this.ShapePnt = linePnt;
            CreateShape();
        }

        /// <summary>
        /// 设置说明文字位置
        /// </summary>
        public abstract void SetDescTextPosition();

        /// <summary>
        /// 创建形状
        /// </summary>
        public abstract void CreateShape();

        /// <summary>
        /// 计算形状的范围
        /// </summary>
        public abstract void CalcShapeRange();

        /// <summary>
        /// 初始化连接线的形状点
        /// </summary>
        public abstract void InitializeShapePnt();

        /// <summary>
        /// 创建的端点形状
        /// </summary>
        protected abstract void CreateTerminalShape();

        /// <summary>
        /// 获得连接线的控制点。 
        /// </summary>
        /// <returns></returns>
        public abstract LineCtrlPoint GetLineCtrlPoints();

        /// <summary>
        /// 创建临时连接线
        /// </summary>
        /// <param name="ctrlNode">变动的控制点</param>
        /// <param name="newPosition">鼠标移到的新位置</param>
        public abstract void CreateTmpLine(CtrlNode ctrlNode, Point position);

        /// <summary>
        /// 结束绘制临时连接线
        /// </summary>
        public abstract void FinishTmpLine(LineTerminalPoint start, LineTerminalPoint end);

        /// <summary>
        /// 获得说明文字的位置
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public abstract bool GetDescriptionPosition(out Point position);


        #region 为持久化添加的额外简单属性
        public double Left
        {
            get { return this.Margin.Left; }
            set{
                double top = this.Margin.Top;
                this.Margin = new Thickness(value, top, 0, 0);
            }
        }

        public double Top
        {
            get { return this.Margin.Top; }
            set {
                double left = this.Margin.Left;
                this.Margin = new Thickness(left, value, 0, 0);                
            }
        }

        public double StartPnt_Position_X
        {
            get { return this.StartPnt.Position.X; }
            set{
                double posy = this.StartPnt.Position.Y;
                this.StartPnt.Position = new Point(value, posy);
            }
        }


        public double StartPnt_Position_Y
        {
            get { return this.StartPnt.Position.Y; }
            set { 
                double posx=this.StartPnt.Position.X;
                this.StartPnt.Position = new Point(posx, value);
            }
        }

        public bool StartPnt_DockedFlag
        {
            get { return this.StartPnt.DockedFlag; }
            set { this.StartPnt.DockedFlag = value; }
        }


        public string StartPnt_RelatedShapeId
        {
            get {
                return this.StartPnt.RelatedShapeId;
            }
            set{
                this.StartPnt.RelatedShapeId=value;
            }
        }

        public LinkNodeTypes StartPnt_RelatedType{
            get{
                return this.StartPnt.RelatedType;
            }
            set{
                this.StartPnt.RelatedType=value;
            }
        }

        public double EndPnt_Position_X
        {
            get{
                return this.EndPnt.Position.X;
            }
            set{
                double posy = this.EndPnt.Position.Y;
                this.EndPnt.Position = new Point(value, posy);
            }
        }


        public double EndPnt_Position_Y
        {
            get
            {
                return this.EndPnt.Position.Y;
            }
            set
            {
                double posx = this.EndPnt.Position.X;
                this.EndPnt.Position = new Point(posx, value);
            }
        }

        public bool EndPnt_DockedFlag
        {
            get { return this.EndPnt.DockedFlag; }
            set { this.EndPnt.DockedFlag = value; }
        }

        public string EndPnt_RelatedShapeId
        {
            get { return this.EndPnt.RelatedShapeId; }
            set { this.EndPnt.RelatedShapeId = value; }
        }

        public LinkNodeTypes EndPnt_RelatedType
        {
            get { return this.EndPnt.RelatedType; }
            set { this.EndPnt.RelatedType = value; }
        }
             

        #endregion 为持久化添加的额外简单属性

    }
}
