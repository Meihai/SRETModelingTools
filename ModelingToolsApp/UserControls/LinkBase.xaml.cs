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

namespace ModelingToolsApp.UserControls
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
            InitializeShapePnt();
            CreateShape();
        }

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

        private string _cloneSourceId;
        public string CloneSourceId
        {
            get { return this._cloneSourceId; }
            set { this._cloneSourceId = value; }
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

        public abstract FlowChartTypes FlowChartType
        {
            get;
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

        public virtual byte[] ToBytes()
        {
            /*
             * 构成LinkBase的定义：
             * 每个LinkBase由如下几段构成：
             * Link的类型，8byte；每个Link的总长度，8byte；编号，36；左坐标，8；顶坐标，8； 宽度，8；高度，8；是否手动设置过，1；
             * 起点x，8；起点y，8；起点是否停靠，1；起点停靠形状id，36；起点停靠联系点类型，4；
             * 终点x，8；终点y，8；终点是否停靠，1；终点停靠形状id，36；终点停靠联系点类型，4；
             * 形状点数量，4；
             * 说明文字的字节数，4；
             * 形状点数据，不定长：形状点1 x，8；形状点1 y，8；形状点n x，8；形状点n y，8；             
             * 说明文字，不定长。
             * 每个Shape的总长度不包含shape类型和总长度这16个byte，只包含从编号到说明的总字节数。
             */


            int idx = 0;
            //36 + 36 + 8 + 8 + 8 + 8 + 1 + 8 + 8 + 1 + 36 + 4 + 8 + 8 + 1   + 4 + 4 + 4 = 191
            byte[] buf = new byte[191];

            byte[] tmp = Encoding.UTF8.GetBytes(Id);
            Array.Copy(tmp, 0, buf, idx, tmp.Length);
            idx += 36;


            tmp = BitConverter.GetBytes(Margin.Left);
            Array.Copy(tmp, 0, buf, idx, tmp.Length);
            idx += 8;

            tmp = BitConverter.GetBytes(Margin.Top);
            Array.Copy(tmp, 0, buf, idx, tmp.Length);
            idx += 8;

            tmp = BitConverter.GetBytes(Width);
            Array.Copy(tmp, 0, buf, idx, tmp.Length);
            idx += 8;

            tmp = BitConverter.GetBytes(Height);
            Array.Copy(tmp, 0, buf, idx, tmp.Length);
            idx += 8;

            tmp = BitConverter.GetBytes(IsManualSetted);
            Array.Copy(tmp, 0, buf, idx, tmp.Length);
            idx += 1;

            //起点数据
            tmp = BitConverter.GetBytes(StartPnt.Position.X);
            Array.Copy(tmp, 0, buf, idx, tmp.Length);
            idx += 8;

            tmp = BitConverter.GetBytes(StartPnt.Position.Y);
            Array.Copy(tmp, 0, buf, idx, tmp.Length);
            idx += 8;

            tmp = BitConverter.GetBytes(StartPnt.DockedFlag);
            Array.Copy(tmp, 0, buf, idx, tmp.Length);
            idx += 1;

            tmp = Encoding.UTF8.GetBytes(StartPnt.RelatedShapeId);
            Array.Copy(tmp, 0, buf, idx, tmp.Length);
            idx += 36;

            tmp = BitConverter.GetBytes((int)StartPnt.RelatedType);
            Array.Copy(tmp, 0, buf, idx, tmp.Length);
            idx += 4;

            //终点数据
            tmp = BitConverter.GetBytes(EndPnt.Position.X);
            Array.Copy(tmp, 0, buf, idx, tmp.Length);
            idx += 8;

            tmp = BitConverter.GetBytes(EndPnt.Position.Y);
            Array.Copy(tmp, 0, buf, idx, tmp.Length);
            idx += 8;

            tmp = BitConverter.GetBytes(EndPnt.DockedFlag);
            Array.Copy(tmp, 0, buf, idx, tmp.Length);
            idx += 1;

            tmp = Encoding.UTF8.GetBytes(EndPnt.RelatedShapeId);
            Array.Copy(tmp, 0, buf, idx, tmp.Length);
            idx += 36;

            tmp = BitConverter.GetBytes((int)EndPnt.RelatedType);
            Array.Copy(tmp, 0, buf, idx, tmp.Length);
            idx += 4;

            tmp = BitConverter.GetBytes(ShapePnt.Count);
            Array.Copy(tmp, 0, buf, idx, tmp.Length);
            idx += 4;

            byte[] desc = Encoding.Unicode.GetBytes(Description);

            tmp = BitConverter.GetBytes(desc.Length);
            Array.Copy(tmp, 0, buf, idx, tmp.Length);
            idx += 4;

            byte[] shapepnts = new byte[ShapePnt.Count * 16];
            for (int i = 0; i < ShapePnt.Count; i++)
            {
                tmp = BitConverter.GetBytes(ShapePnt[i].X);
                Array.Copy(tmp, 0, shapepnts, i * 16, 8);
                tmp = BitConverter.GetBytes(ShapePnt[i].Y);
                Array.Copy(tmp, 0, shapepnts, i * 16 + 8, 8);
            }

            int length = 191 + desc.Length + shapepnts.Length;
            byte[] data = new byte[length + 4 + 4];

            byte[] type = BitConverter.GetBytes((int)FlowChartType);
            byte[] len = BitConverter.GetBytes(length);

            Array.Copy(type, 0, data, 0, 4);
            Array.Copy(len, 0, data, 4, 4);
            Array.Copy(buf, 0, data, 8, buf.Length);
            Array.Copy(shapepnts, 0, data, 8 + buf.Length, shapepnts.Length);
            Array.Copy(desc, 0, data, 8 + buf.Length + shapepnts.Length, desc.Length);


            return data;
        }

        public IFlowChartBase FromBytes(byte[] data, int index, out int length)
        {
            IFlowChartBase ifcb = null;
            int idx = index;
            length = 0;

            int type = BitConverter.ToInt32(data, idx);
            if (type == (int)FlowChartTypes.LinkBroken)
            {
                ifcb = new LinkBroken();
            }
            else if (type == (int)FlowChartTypes.LinkStraight)
            {
                ifcb = new LinkStraight();
            }
            idx += 4;

            if (null != ifcb)
            {
                int len = BitConverter.ToInt32(data, idx);
                idx += 4;
                length = len + 8;

                string id = Encoding.UTF8.GetString(data, idx, 36).TrimEnd('\0');
                idx += 36;

                double left = BitConverter.ToDouble(data, idx);
                idx += 8;

                double top = BitConverter.ToDouble(data, idx);
                idx += 8;

                double width = BitConverter.ToDouble(data, idx);
                idx += 8;

                double height = BitConverter.ToDouble(data, idx);
                idx += 8;

                bool isSetted = BitConverter.ToBoolean(data, idx);
                idx += 1;

                double startx = BitConverter.ToDouble(data, idx);
                idx += 8;

                double starty = BitConverter.ToDouble(data, idx);
                idx += 8;

                bool isStartDocked = BitConverter.ToBoolean(data, idx);
                idx += 1;

                string startDockedShapeId = Encoding.UTF8.GetString(data, idx, 36).Trim('\0').TrimEnd('\0');
                idx += 36;

                int startDockedLinkNodeType = BitConverter.ToInt32(data, idx);
                idx += 4;

                double endx = BitConverter.ToDouble(data, idx);
                idx += 8;

                double endy = BitConverter.ToDouble(data, idx);
                idx += 8;

                bool isEndDocked = BitConverter.ToBoolean(data, idx);
                idx += 1;

                string endDockedShapeId = Encoding.UTF8.GetString(data, idx, 36).TrimEnd('\0');
                idx += 36;

                int endDockedLinkNodeType = BitConverter.ToInt32(data, idx);
                idx += 4;

                int shapePntsCnt = BitConverter.ToInt32(data, idx);
                idx += 4;

                int descLen = BitConverter.ToInt32(data, idx);
                idx += 4;

                PointCollection shapepnts = new PointCollection();
                for (int i = 0; i < shapePntsCnt; i++)
                {
                    double x = BitConverter.ToDouble(data, idx);
                    double y = BitConverter.ToDouble(data, idx + 8);
                    shapepnts.Add(new Point(x, y));
                    idx += 16;
                }

                string desc = Encoding.Unicode.GetString(data, idx, descLen).TrimEnd('\0');

                ifcb.Id = id;
                ifcb.Margin = new Thickness(left, top, 0, 0);
                ifcb.Width = width;
                ifcb.Height = height;
                ((LinkBase)ifcb).IsManualSetted = isSetted;

                ((LinkBase)ifcb).StartPnt = new LineTerminalPoint(new Point(startx, starty));
                ((LinkBase)ifcb).StartPnt.DockedFlag = isStartDocked;
                ((LinkBase)ifcb).StartPnt.RelatedShapeId = startDockedShapeId;
                ((LinkBase)ifcb).StartPnt.RelatedType = (LinkNodeTypes)startDockedLinkNodeType;

                ((LinkBase)ifcb).EndPnt = new LineTerminalPoint(new Point(endx, endy));
                ((LinkBase)ifcb).EndPnt.DockedFlag = isEndDocked;
                ((LinkBase)ifcb).EndPnt.RelatedShapeId = endDockedShapeId;
                ((LinkBase)ifcb).EndPnt.RelatedType = (LinkNodeTypes)endDockedLinkNodeType;

                ((LinkBase)ifcb).ShapePnt = shapepnts;
                ifcb.Description = desc;

                return ifcb;
            }
            else
            {
                return null;
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

  }
}
