using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using Microsoft.Win32;
using System.Xml.Linq;
using System.IO;
using System.ComponentModel;
using System.Xml;
using System.Reflection;


namespace ModelingToolsAppWithMVVM.Common.ChartCommon
{
    public class BaseWorkModel:Canvas
    {
        /// <summary>
        /// 全局鼠标事件监听
        /// </summary>
        protected GlobalMouseHook GlobalMouseListener = new GlobalMouseHook();
        /// <summary>
        /// 判断鼠标是否开始移动
        /// </summary>
        protected  bool IsStartMove = false;
        /// <summary>
        /// 鼠标在viewer上按下左键时的位置
        /// </summary>
        protected Point Offset = new Point(0, 0);

        /// <summary>
        /// 鼠标在连接点按下按键
        /// </summary>
        protected bool IsLinkNodeDown = false;

        /// <summary>
        /// 鼠标停靠在某个形状上的联系点
        /// </summary>
        protected DockedLinkNodeArgs MouseDockedLinkNode = new DockedLinkNodeArgs();
        /// <summary>
        /// 当前工作区中所有已经添加的形状
        /// </summary>
        protected  ObservableCollection<IFlowChartBase> AllShapes = new ObservableCollection<IFlowChartBase>();
        /// <summary>
        /// 当前工作区所拥有的建模元素
        /// </summary>
        public ObservableCollection<IFlowChartBase> ModelItems
        {
            get { return AllShapes; }
            set
            {
                AllShapes = value;
                //接收到数据后的进一步处理                
            }
        }

        /// <summary>
        /// 当前工作区所属的父级建模工作区
        /// </summary>
        public BaseWorkModel ParentWorkModel
        {
            get;
            set;
        }

        /// <summary>
        /// 当前工作区所属的父级建模元素
        /// </summary>
        public ShapeBase BelongToModelItem
        {
            get;
            set;
        }

        /// <summary>
        /// 设置流程详细参数,"properties"必须包含key为desc的项，其值为字符串，在流程图中显示
        /// </summary>
        public event delSetShapeDetail evtSetShapeDetail;       

        /// <summary>
        /// 流程被选中
        /// </summary>
        public event delShapeSelected evtShapeSelected;

        #region  连接线相关参数
        // 连接线起始点
        protected Point? dragStartPoint = null;
        //连接线终点
        protected Point? dragEndPoint=null;
        //连接线起点关联形状ID
        protected string linkStartRelatedShapeId = "";
        //连接线终点关联形状ID
        protected string linkEndRelatedShapeId = "";
        protected LinkNodeTypes startLinkNodeType=LinkNodeTypes.NULL;
        protected LinkNodeTypes endLinkNodeType=LinkNodeTypes.NULL;

        #endregion 连接线相关参数

        protected List<IFlowChartBase> CloneShapes = new List<IFlowChartBase>();

        protected CtrlLine GCtrlLine;
        protected CtrlShape GCtrlShape;
        protected Rectangle GMoveShape;
        protected DockNode GLinkNode0;
        protected DockNode GLinkNode1;
        protected DockNode GLinkNode2;
        protected DockNode GLinkNode3;
        protected Polyline tmpLine;
        protected System.Windows.Shapes.Path tmpLinkPath;
        protected CtrlMultiSelector GMultiSelector;
        protected DescEditor GDescEditor;

        /// <summary>
        /// 当鼠标选择连接点的时候临时拖出来的连接线
        /// </summary>
        protected LinkBase tmpLink;

        /// <summary>
        /// 流程图编号
        /// </summary>
        public string Id
        {
            get;
            set;
        }

        public BaseWorkModel() {
            Id = Guid.NewGuid().ToString();
            registerMouseEvent();
            init();
            commandBindings();
        }

        /// <summary>
        /// 注册鼠标事件
        /// </summary>
        protected void registerMouseEvent()
        {
            this.DragEnter += canvasDragEnter;
            this.DragOver += canvasDragOver;
            this.DragLeave += canvasDragLeave;
            this.Drop += canvasDrop;
            this.MouseMove += canvasMouseMove;
            this.PreviewMouseDown += canvasPreviewMouseDown;
            this.MouseDown += canvasMouseDown;
        }
        protected void init()
        {
            GCtrlLine = new CtrlLine();
            GCtrlLine.Visibility = Visibility.Hidden;
            this.Children.Add(GCtrlLine);

            GCtrlShape = new CtrlShape();
            GCtrlShape.Visibility = Visibility.Hidden;
            this.Children.Add(GCtrlShape);

            GMoveShape = new Rectangle();
            GMoveShape.Visibility = Visibility.Hidden;
            GMoveShape.Stroke = new SolidColorBrush(Colors.Lime);
            GMoveShape.StrokeDashArray = new DoubleCollection() { 3, 3 };
            this.Children.Add(GMoveShape);

            GLinkNode0 = new DockNode();
            GLinkNode0.Visibility = Visibility.Hidden;
            this.Children.Add(GLinkNode0);

            GLinkNode1 = new DockNode();
            GLinkNode1.Visibility = Visibility.Hidden;
            this.Children.Add(GLinkNode1);

            GLinkNode2 = new DockNode();
            GLinkNode2.Visibility = Visibility.Hidden;
            this.Children.Add(GLinkNode2);

            GLinkNode3 = new DockNode();
            GLinkNode3.Visibility = Visibility.Hidden;
            this.Children.Add(GLinkNode3);

            tmpLine = new Polyline();
            tmpLine.Visibility = Visibility.Hidden;
            tmpLine.Stroke = new SolidColorBrush(Colors.Black);
            tmpLine.StrokeDashArray = new DoubleCollection() { 3, 3 };
            this.Children.Add(tmpLine);

            tmpLinkPath = new System.Windows.Shapes.Path();
            tmpLinkPath.Visibility = Visibility.Hidden;
            tmpLinkPath.Stroke = new SolidColorBrush(Colors.Black);
            tmpLinkPath.StrokeDashArray = new DoubleCollection() { 3, 3 };
            this.Children.Add(tmpLinkPath);

            GMultiSelector = new CtrlMultiSelector();
            this.Children.Add((UserControl)GMultiSelector);

            GDescEditor = new DescEditor();
            this.Children.Add((UserControl)GDescEditor);

            AllShapes.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(AllShapes_CollectionChanged);
            GlobalMouseHook.evtGlobalMouseUp += new delGlobalMouseUp(GlobalMouseHook_evtGlobalMouseUp);
            GCtrlShape.evtSizeChanged += new delSizeChanged(GCtrlShape_evtSizeChanged);
            GCtrlShape.evtPrepareToChangePosition += new delPrepareToChangePosition(GCtrlShape_evtPrepareToChangePosition);
            GCtrlShape.evtPositionChanged += new delPositionChanged(GCtrlShape_evtPositionChanged);
            GCtrlLine.evtTerminalPointMoved += new delTerminalPointMoved(GCtrlLine_evtTerminalPointMoved);
                      

            //确定几个顶层控件
            Canvas.SetZIndex(GMoveShape, int.MaxValue - 5);
            Canvas.SetZIndex(GMultiSelector, int.MaxValue - 10);
            Canvas.SetZIndex(GLinkNode0, int.MaxValue - 20);
            Canvas.SetZIndex(GLinkNode1, int.MaxValue - 20);
            Canvas.SetZIndex(GLinkNode2, int.MaxValue - 20);
            Canvas.SetZIndex(GLinkNode3, int.MaxValue - 20);
            Canvas.SetZIndex(tmpLine, int.MaxValue - 30);
            Canvas.SetZIndex(tmpLinkPath, int.MaxValue - 30);
            Canvas.SetZIndex(GCtrlShape, int.MaxValue - 40);
            Canvas.SetZIndex(GCtrlLine, int.MaxValue - 45);
            Canvas.SetZIndex(GDescEditor, int.MaxValue - 50);
            //Clipboard.Clear();
        }

        private void commandBindings()
        {
            this.CommandBindings.Add(new CommandBinding(ApplicationCommands.Delete, Delete_Executed, Delete_Enabled));
        }

        #region Delete Command 

        public virtual void Delete_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            for (int i = AllShapes.Count - 1; i >= 0; i--)
            {
                if (AllShapes[i].IsMultiSelected || AllShapes[i].IsSelected)
                {
                    this.Children.Remove((UserControl)AllShapes[i]);
                    AllShapes.Remove(AllShapes[i]);
                }
            }

            GCtrlLine.Visibility = Visibility.Hidden;
            GCtrlShape.Visibility = Visibility.Hidden;
            GLinkNode0.Visibility = Visibility.Hidden;
            GLinkNode1.Visibility = Visibility.Hidden;
            tmpLine.Visibility = Visibility.Hidden;
        }


        public virtual void Delete_Enabled(object sender, CanExecuteRoutedEventArgs e)
        {
            int cnt = 0;
            for (int i = AllShapes.Count - 1; i >= 0; i--)
            {
                if (AllShapes[i].IsMultiSelected || AllShapes[i].IsSelected)
                {
                    this.Children.Remove((UserControl)AllShapes[i]);
                    AllShapes.Remove(AllShapes[i]);
                    cnt = cnt + 1;
                }
            }
            e.CanExecute = cnt > 0;
           

        }
        #endregion DeleleCommand

        #region 复制命令
        public virtual void Copy_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            //todo
        }
        
      

        public virtual void LoadFromXmlFile(BaseWorkModel workModel, string fileName)
        {
            //todo
        }

        public virtual void Copy_Enabled(object sender, CanExecuteRoutedEventArgs e)
        {
            //todo
            e.CanExecute = true;
        }
        #endregion 复制命令
        #region Paste Command
        public virtual void Paste_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            //todo
        }

        public virtual void Paste_Enabled(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;

        }
        #endregion Paste Command

        protected virtual void GCtrlLine_evtTerminalPointMoved(Point point, int seq)
        {
            for (int i = 0; i < AllShapes.Count; i++)
            {
                AllShapes[i].AcceptPointMove(point, seq);
            }
        }

        protected virtual void GCtrlShape_evtPrepareToChangePosition(bool flag)
        {
            if (flag)
            {
                for (int i = 0; i < AllShapes.Count; i++)
                {
                    if (AllShapes[i].IsSelected || AllShapes[i].IsMultiSelected)
                    {
                        IFlowChartBase icb = (IFlowChartBase)AllShapes[i].Clone();
                        CloneShapes.Add(icb);
                        this.Children.Add((UserControl)icb);
                    }
                }
            }
            else
            {
                for (int i = 0; i < CloneShapes.Count; i++)
                {
                    this.Children.Remove((UserControl)CloneShapes[i]);
                    CloneShapes[i] = null;
                    CloneShapes.Clear();
                }
            }
        }


        protected virtual void GCtrlShape_evtPositionChanged(double left, double top)
        {

        }

       protected virtual void GCtrlShape_evtSizeChanged(double difLeft, double difTop, double difWidth, double difHeight)
        {
            for (int i = 0; i < AllShapes.Count; i++)
            {
                if (AllShapes[i].IsSelected || AllShapes[i].IsMultiSelected)
                {
                    AllShapes[i].ChangePositionAndSize(difLeft, difTop, difWidth, difHeight);
                }
            }
        }
        protected virtual void AllShapes_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove && e.OldItems[0] is ShapeBase)
            {
                for (int i = 0; i < AllShapes.Count; i++)
                {
                    if (AllShapes[i] is LinkBase)
                    {
                        if (((LinkBase)AllShapes[i]).StartPnt.RelatedShapeId == ((IFlowChartBase)e.OldItems[0]).Id)
                        {
                            ((LinkBase)AllShapes[i]).StartPnt.DockedFlag = false;
                            ((LinkBase)AllShapes[i]).StartPnt.RelatedShapeId = "";
                        }

                        if (((LinkBase)AllShapes[i]).EndPnt.RelatedShapeId == ((IFlowChartBase)e.OldItems[0]).Id)
                        {
                            ((LinkBase)AllShapes[i]).EndPnt.DockedFlag = false;
                            ((LinkBase)AllShapes[i]).EndPnt.RelatedShapeId = "";
                        }
                    }

                }
            }
            if (BelongToModelItem != null)
            {
                if (ParentWorkModel != null)
                {
                    for (int j = 0; j < ParentWorkModel.ModelItems.Count; j++)
                    {
                        if (ParentWorkModel.ModelItems[j] is ShapeBase)
                        {
                            if (ParentWorkModel.ModelItems[j].CloneSourceId == BelongToModelItem.CloneSourceId)
                            {
                                ShapeBase sb = ParentWorkModel.ModelItems[j] as ShapeBase;
                                sb.ChildWorkModel = this;
                            }
                        }
                    }
                }

            }
           
        }

        /// <summary>
        /// 处理两个连接线之间的
        /// </summary>
        protected virtual void DealWithLinkNodeEvent()
        {
            if (IsLinkNodeDown)
            {
                IsLinkNodeDown = false;
                if (dragStartPoint.HasValue && dragEndPoint.HasValue)
                {
                    tmpLink = new InterfaceInteractionLink((Point)dragStartPoint, (Point)dragEndPoint);
                    if (linkStartRelatedShapeId != string.Empty)
                    {
                        tmpLink.StartPnt.RelatedShapeId = linkStartRelatedShapeId;
                        tmpLink.StartPnt.RelatedType = startLinkNodeType;
                        tmpLink.StartPnt.DockedFlag = true;
                    }
                    if (linkEndRelatedShapeId != string.Empty)
                    {
                        tmpLink.EndPnt.RelatedShapeId = linkEndRelatedShapeId;
                        tmpLink.EndPnt.RelatedType = endLinkNodeType;
                        tmpLink.EndPnt.DockedFlag = true;
                    }
                    AddShapes(tmpLink);
                    linkStartRelatedShapeId = "";
                    linkEndRelatedShapeId = "";
                    tmpLink = null;
                }
            }
        }


        protected virtual void GlobalMouseHook_evtGlobalMouseUp(GlobalMouseArgs e)
        {
            DealWithLinkNodeEvent();
            #region 临时连接线
            if (this.tmpLinkPath.Visibility == Visibility.Visible)
            {
                if (GCtrlLine.Visibility == Visibility.Visible)
                {
                    //GCtrlLine.CreateShapePoints(tmpLine.Points);
                }

                this.tmpLinkPath.Visibility = Visibility.Hidden;
            }
            #endregion


            for (int i = 0; i < CloneShapes.Count; i++)
            {
                for (int j = 0; j < AllShapes.Count; j++)
                {
                    if (AllShapes[j].Id == CloneShapes[i].CloneSourceId)
                    {
                        if (AllShapes[j] is LinkBase)
                        {
                            if (((LinkBase)AllShapes[j]).StartPnt.DockedFlag || ((LinkBase)AllShapes[j]).EndPnt.DockedFlag)
                            {
                                continue;
                            }
                        }

                        AllShapes[j].MoveToClone(CloneShapes[i]);
                    }
                }
            }

            for (int i = 0; i < CloneShapes.Count; i++)
            {
                this.Children.Remove((System.Windows.Controls.UserControl)CloneShapes[i]);
                CloneShapes[i] = null;
            }

            CloneShapes.Clear();         

            #region 多选框
            if (GMultiSelector.Visibility == Visibility.Visible)
            {
                for (int i = 0; i < AllShapes.Count; i++)
                {
                    if (AllShapes[i].Range.Count < 4)
                    {
                        continue;
                    }

                    if (AllShapes[i].Range[0].X > GMultiSelector.Margin.Left &&
                        AllShapes[i].Range[0].Y > GMultiSelector.Margin.Top &&
                        AllShapes[i].Range[2].X < GMultiSelector.Margin.Left + GMultiSelector.Width &&
                        AllShapes[i].Range[2].Y < GMultiSelector.Margin.Top + GMultiSelector.Height)
                    {
                        AllShapes[i].IsSelected = false;
                        AllShapes[i].IsMultiSelected = true;
                    }
                }

                GMultiSelector.Visibility = Visibility.Hidden;
            }
            #endregion
            DisplayGlobalCtrlShape();
        }

        /// <summary>
        /// 显示控制框
        /// </summary>
        /// <param name="shape"></param>
        protected virtual void DisplayGlobalCtrlShape()
        {
            GCtrlLine.Visibility = Visibility.Hidden;
            GCtrlShape.Visibility = Visibility.Hidden;

            for (int i = 0; i < AllShapes.Count; i++)
            {
                if (AllShapes[i].IsSelected && AllShapes[i] is ShapeBase)
                {
                    GCtrlShape.Width = AllShapes[i].Width;
                    GCtrlShape.Height = AllShapes[i].Height;
                    GCtrlShape.Margin = AllShapes[i].Margin;
                    GCtrlShape.Visibility = Visibility.Visible;
                    break;
                }
                else if (AllShapes[i].IsSelected && AllShapes[i] is LinkBase)
                {
                    GCtrlLine.Width = ((LinkBase)AllShapes[i]).Width;
                    GCtrlLine.Height = ((LinkBase)AllShapes[i]).Height;
                    GCtrlLine.Margin = ((LinkBase)AllShapes[i]).Margin;
                    GCtrlLine.Visibility = Visibility.Visible;
                    GCtrlLine.BindLinkLine((LinkBase)AllShapes[i]);
                    GCtrlLine.Offset = AllShapes[i].Offset;
                }
                else if (AllShapes[i].IsMultiSelected)
                {
                    break;
                }
            }

            //if (flag == 0) //非单选情况
            {
                int cnt = 0;
                int pos = 0;
                double minx = int.MaxValue;
                double maxx = int.MinValue;
                double miny = int.MaxValue;
                double maxy = int.MinValue;

                for (int i = 0; i < AllShapes.Count; i++)
                {
                    if (AllShapes[i].Range.Count < 4)
                    {
                        continue;
                    }

                    if (AllShapes[i].IsMultiSelected)
                    {
                        minx = minx > AllShapes[i].Range[0].X ? AllShapes[i].Range[0].X : minx;
                        miny = miny > AllShapes[i].Range[0].Y ? AllShapes[i].Range[0].Y : miny;
                        maxx = maxx < AllShapes[i].Range[2].X ? AllShapes[i].Range[2].X : maxx;
                        maxy = maxy < AllShapes[i].Range[2].Y ? AllShapes[i].Range[2].Y : maxy;
                        pos = i;
                        cnt++;
                    }
                }

                if (cnt > 1)
                {
                    GCtrlShape.Width = maxx - minx + 20;
                    GCtrlShape.Height = maxy - miny + 20;
                    GCtrlShape.Margin = new Thickness(minx - 10, miny - 10, 0, 0);
                    GCtrlShape.Visibility = Visibility.Visible;
                }
                else if (cnt == 1)
                {
                    AllShapes[pos].IsMultiSelected = false;
                    AllShapes[pos].IsSelected = true;
                }
            }

            //设置GMoveShape参数
            if (GCtrlShape.Visibility == Visibility.Visible)
            {
                //13为CtrlNode的尺寸
                GMoveShape.Margin = new Thickness(GCtrlShape.Margin.Left + 6.5, GCtrlShape.Margin.Top + 6.5, 0, 0);
                GMoveShape.Height = GCtrlShape.Height - 13;
                GMoveShape.Width = GCtrlShape.Width - 13;
            }
            GMoveShape.Visibility = Visibility.Hidden;
        }


        #region 鼠标事件
        /// <summary>
        /// 拖动时生成的形状
        /// </summary>
        protected IFlowChartBase DragIfcb = null;
        protected virtual void canvasDragEnter(object sender, System.Windows.DragEventArgs e)
        {
            if (null == DragIfcb)
            {
                switch ((FlowChartTypes)(e.Data.GetData(typeof(FlowChartTypes))))
                {                    
                    default:
                        break;
                }

                if (null != DragIfcb)
                {
                    DragIfcb.ChangePositionAndSize(e.GetPosition(this).X - DragIfcb.Width / 2 - DragIfcb.Margin.Left,
                                                    e.GetPosition(this).Y - DragIfcb.Height / 2 - DragIfcb.Margin.Top, 0, 0);
                    this.Children.Add((System.Windows.Controls.UserControl)DragIfcb);

                    if (DragIfcb is ShapeBase)
                    {
                        ((ShapeBase)DragIfcb).evtDocketLinkNode += new delDockedLinkNode(FlowChartEditor_evtDocketLinkNode);
                    }
                }
            }
        }

        protected virtual void canvasDragOver(object sender, System.Windows.DragEventArgs e)
        {
            if (null != DragIfcb)
            {
                #region 控制鼠标移动为整数点

                int x = (int)Math.Truncate(e.GetPosition(this).X);
                int y = (int)Math.Truncate(e.GetPosition(this).Y);
                if (x % 2 == 1)
                {
                    x += 1;
                }
                if (y % 2 == 1)
                {
                    y += 1;
                }
                Point point = new Point(x, y);

                #endregion

                DragIfcb.ChangePositionAndSize(point.X - DragIfcb.Width / 2 - DragIfcb.Margin.Left,
                                                    point.Y - DragIfcb.Height / 2 - DragIfcb.Margin.Top, 0, 0);

                DealShapeMoving(DragIfcb);
            }
        }

        protected void canvasDragLeave(object sender, System.Windows.DragEventArgs e)
        {
            if (e.OriginalSource == this)
            {
                return;
            }

            if (null != DragIfcb)
            {
                this.Children.Remove((System.Windows.Controls.UserControl)DragIfcb);
                DragIfcb = null;
            }
        }

        protected virtual void canvasDrop(object sender, System.Windows.DragEventArgs e)
        {
            if (null != DragIfcb)
            {
                this.Children.Remove((System.Windows.Controls.UserControl)DragIfcb);
                AddShapes(DragIfcb);
                DragIfcb = null;
            }
        }
        protected virtual void canvasMouseMove(object sender, MouseEventArgs e)
        {
            if (Mouse.LeftButton != MouseButtonState.Pressed)
            {
                dragStartPoint = null;
                dragEndPoint = null;
                if (IsLinkNodeDown)
                {
                    GCtrlLine.Visibility = Visibility.Hidden;
                    IsLinkNodeDown = false;
                }
                return;
            }

            //鼠标移动一定偏移量后，才确认为开始移动
            if (!IsStartMove)
            {
                if (Math.Abs(e.GetPosition(this).X - Offset.X) > 10 || Math.Abs(e.GetPosition(this).Y - Offset.Y) > 10)
                {
                    IsStartMove = true;
                               
                }
                return;
            }
          


            #region 控制鼠标移动为整数点

            int x = (int)Math.Truncate(e.GetPosition(this).X);
            int y = (int)Math.Truncate(e.GetPosition(this).Y);
            if (x % 2 == 1)
            {
                x += 1;
            }
            if (y % 2 == 1)
            {
                y += 1;
            }
            Point point = new Point(x, y);

            #endregion

            #region 连接线绘制
            if (IsLinkNodeDown)
            {
                dragEndPoint = point;       
                Geometry lineGeometry=createGeometry((Point) dragStartPoint, (Point) dragEndPoint);
                FlowChartEditor_evtRefreshTmpLine(lineGeometry);
                return;
            }
            #endregion 连接线绘制

            if (GMultiSelector.Visibility == Visibility.Visible) //多选框控制
            {
                Point p = e.GetPosition(this);
                GMultiSelector.Width = Math.Abs(p.X - Offset.X);
                GMultiSelector.Height = Math.Abs(p.Y - Offset.Y);
                GMultiSelector.Margin = new Thickness(p.X > Offset.X ? Offset.X : p.X, p.Y > Offset.Y ? Offset.Y : p.Y, 0, 0);
            }
            else if (CloneShapes.Count > 0)
            {
                //临时形状移动
                for (int i = 0; i < CloneShapes.Count; i++)
                {

                    CloneShapes[i].ChangePositionAndSize(point.X + CloneShapes[i].Offset.X - Offset.X - CloneShapes[i].Margin.Left,
                                                        point.Y + CloneShapes[i].Offset.Y - Offset.Y - CloneShapes[i].Margin.Top, 0, 0);
                }

                //如果临时形状数量为1，则需要判断是否停靠到联系点
                if (CloneShapes.Count == 1)
                {
                    DealShapeMoving(CloneShapes[0]);

                    //临时形状数量==1，且为ShapeBase时，显示移动框
                    if (CloneShapes[0] is ShapeBase)
                    {
                        GMoveShape.Visibility = Visibility.Visible;
                        GMoveShape.Margin = new Thickness(point.X + GCtrlShape.Margin.Left - Offset.X + 6.5,
                            point.Y + GCtrlShape.Margin.Top - Offset.Y + 6.5, 0, 0);
                    }
                }
                else  //临时形状数量大于1，则只移动，并控制显示移动框
                {
                    GMoveShape.Visibility = Visibility.Visible;
                    GMoveShape.Margin = new Thickness(point.X + GCtrlShape.Margin.Left - Offset.X + 6.5,
                        point.Y + GCtrlShape.Margin.Top - Offset.Y + 6.5, 0, 0);
                }
            }
            else
            {
                GCtrlShape.AcceptMouseMove(e);
                if (GCtrlLine.Visibility == Visibility.Visible)
                {
                    GCtrlLine.AcceptMouseMove(MouseDockedLinkNode, point);
                }
            }
        }
        

        protected virtual void canvasPreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            Offset = e.GetPosition(this);
            dragStartPoint = e.GetPosition(this);
            IsStartMove = false;
        }

        protected virtual void canvasMouseDown(object sender, MouseButtonEventArgs e)
        {

            if (e.Source == this)
            {
                for (int i = 0; i < AllShapes.Count; i++)
                {
                    AllShapes[i].IsSelected = false;
                    AllShapes[i].IsMultiSelected = false;
                }
                GCtrlShape.Visibility = Visibility.Hidden;

                Offset = e.GetPosition(this);
                GMultiSelector.Margin = new Thickness(Offset.X, Offset.Y, 0, 0);
                GMultiSelector.Width = 0;
                GMultiSelector.Height = 0;
                GMultiSelector.Visibility = Visibility.Visible;

                GDescEditor.Visibility = Visibility.Hidden;
            }

            if(IsLinkNodeDown){
                //第一步,令控制线可见
                dragStartPoint = e.GetPosition(this);               
            }            

        }
        #endregion

        #region 创建连接线形状
        protected virtual Geometry createGeometry(Point startPoint, Point endPoint)
        {
            StreamGeometry geometry = new StreamGeometry();
            geometry.FillRule = FillRule.EvenOdd;

            using (StreamGeometryContext ctx = geometry.Open())
            {
                ctx.BeginFigure(startPoint, false, false);
                ctx.LineTo(endPoint, true, false);
            }
            geometry.Freeze();
            return geometry;
        }
        #endregion

        /// <summary>
        /// 添加形状
        /// </summary>
        /// <param name="ifcb"></param>
        protected virtual void AddShapes(IFlowChartBase ifcb)
        {
            for (int i = 0; i < AllShapes.Count; i++)
            {
                AllShapes[i].IsSelected = false;
                AllShapes[i].IsMultiSelected = false;
            }

            ifcb.IsSelected = true;
            ifcb.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(ifcb_PropertyChanged);
            ifcb.evtMouseClick += new delMouseClick(ifcb_evtMouseClick);
            ifcb.evtMouseDoubleClick += new delMouseDoubleClick(ifcb_evtMouseDoubleClick);

            if (ifcb is ShapeBase)
            {  
                ((ShapeBase)ifcb).evtDocketLinkNode += new delDockedLinkNode(FlowChartEditor_evtDocketLinkNode);
                ((ShapeBase)ifcb).evtMouseDownOnShapeBaseLinkNode += new delMouseDownOnShapeLinkNode(FlowChartEditor_evtMouseDownOnLinkNode);
                ((ShapeBase)ifcb).evtMouseEnterOnShapeBaseLinkNode += new delMouseEnterOnShapeLinkNode(FlowChartEditor_evtMouseEnterOnLinkNode);
                
            }

            if (ifcb is LinkBase)
            {
                ((LinkBase)ifcb).evtRefreshTmpLine += new delRefreshTmpLine(FlowChartEditor_evtRefreshTmpLine);              
                ifcb.MoveToClone((IFlowChartBase)ifcb.Clone());
            }

            AllShapes.Add(ifcb);

            this.Children.Add((UserControl)ifcb);

            ifcb.IsSelected = true;
            ifcb.ChangePositionAndSize(0, 0, 0, 0);
        }

      

        public void MenuItem_Click_NewFile(object sender, RoutedEventArgs e)
        {
            CreateNewFlowChart(System.Guid.NewGuid().ToString());
        }

        /// <summary>
        /// 创建新流程图
        /// </summary>
        /// <param name="id"></param>
        public void CreateNewFlowChart(string newFlowChartId)
        {
            this.Id = newFlowChartId;

            for (int i = 0; i < AllShapes.Count; i++)
            {
                this.Children.Remove((UserControl)AllShapes[i]);
            }

            AllShapes.Clear();
        }

        protected void FlowChartEditor_evtRefreshTmpLine(Geometry geometry)
        {
            tmpLinkPath.Visibility = Visibility.Visible;
            tmpLinkPath.Data = geometry;
        }


        protected void ifcb_evtMouseDoubleClick(IFlowChartBase ifcb, string description)
        {
            if (null != evtSetShapeDetail)
            {
                evtSetShapeDetail(ifcb.GetType().ToString(), ifcb.Id);
            }
            else
            {
                GCtrlLine.Visibility = Visibility.Hidden;
                GCtrlShape.Visibility = Visibility.Hidden;
                ifcb.IsSelected = false;
                this.GDescEditor.BindFlowChart(ifcb);
                this.GDescEditor.Visibility = Visibility.Visible;
                this.GDescEditor.Focus();
            }
          
        }

        protected void FlowChartEditor_evtDocketLinkNode(DockedLinkNodeArgs e)
        {
            MouseDockedLinkNode = e;
            //如果是形状移动，则最多有4个联系点，需要再补充
            if (0 == e.Id)
            {
                if (e.Flag)
                {
                    GLinkNode0.Margin = new Thickness(e.DocketLinkNode.Position.X - 6.5 + e.DockedShape.Margin.Left,
                    e.DocketLinkNode.Position.Y - 6.5 + e.DockedShape.Margin.Top, 0, 0);
                    GLinkNode0.Visibility = Visibility.Visible;
                }
                else
                {
                    GLinkNode0.Visibility = Visibility.Hidden;
                }
            }
            if (1 == e.Id)
            {
                if (e.Flag)
                {
                    GLinkNode1.Margin = new Thickness(e.DocketLinkNode.Position.X - 6.5 + e.DockedShape.Margin.Left,
                    e.DocketLinkNode.Position.Y - 6.5 + e.DockedShape.Margin.Top, 0, 0);
                    GLinkNode1.Visibility = Visibility.Visible;
                }
                else
                {
                    GLinkNode1.Visibility = Visibility.Hidden;
                }
            }
            if (2 == e.Id)
            {
                if (e.Flag)
                {
                    GLinkNode2.Margin = new Thickness(e.DocketLinkNode.Position.X - 6.5 + e.DockedShape.Margin.Left,
                    e.DocketLinkNode.Position.Y - 6.5 + e.DockedShape.Margin.Top, 0, 0);
                    GLinkNode2.Visibility = Visibility.Visible;
                }
                else
                {
                    GLinkNode2.Visibility = Visibility.Hidden;
                }
            }
            if (3 == e.Id)
            {
                if (e.Flag)
                {
                    GLinkNode3.Margin = new Thickness(e.DocketLinkNode.Position.X - 6.5 + e.DockedShape.Margin.Left,
                    e.DocketLinkNode.Position.Y - 6.5 + e.DockedShape.Margin.Top, 0, 0);
                    GLinkNode3.Visibility = Visibility.Visible;
                }
                else
                {
                    GLinkNode3.Visibility = Visibility.Hidden;
                }
            }
        }

        protected virtual void FlowChartEditor_evtMouseDownOnLinkNode(LinkNodeArgs linkNodeArgs)
        {
            IsLinkNodeDown = true;           

        }

        protected virtual void FlowChartEditor_evtMouseEnterOnLinkNode(LinkNodeArgs linkNodeArgs)
        {
            if (IsLinkNodeDown)
            {
                linkEndRelatedShapeId = linkNodeArgs.DockedShape.Id;
                endLinkNodeType = linkNodeArgs.DocketLinkNode.LinkNodeType;
            }
            else
            {
                linkStartRelatedShapeId = linkNodeArgs.DockedShape.Id;
                startLinkNodeType = linkNodeArgs.DocketLinkNode.LinkNodeType;
            }
        }

       protected virtual void ifcb_evtOpenSubModelArea(ShapeBase modelItem)
        {
             //todo
              MessageBox.Show("当前模型元素不支持递归建模!");          

        }

       protected virtual void ifcb_evtMouseClick(IFlowChartBase ifcb)
        {
            int cnt = 0;
            for (int i = 0; i < AllShapes.Count; i++)
            {
                if (AllShapes[i].IsSelected || AllShapes[i].IsMultiSelected)
                {
                    IFlowChartBase icb = (IFlowChartBase)AllShapes[i].Clone();
                    if (icb is ShapeBase)
                    {
                        ((ShapeBase)icb).evtDocketLinkNode += new delDockedLinkNode(FlowChartEditor_evtDocketLinkNode);
                        ///如果子流程为剖面子流程
                        ((ShapeBase)icb).evtOpenSubModelArea += new delOpenSubModelArea(ifcb_evtOpenSubModelArea);
                    }
                    CloneShapes.Add(icb);
                    this.Children.Add((UserControl)icb);
                    cnt++;
                }
            }
        }

        protected void ifcb_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch ((ChangedPropertys)Enum.Parse(typeof(ChangedPropertys), e.PropertyName))
            {
                case ChangedPropertys.SelectTrue:
                    for (int i = 0; i < AllShapes.Count; i++)
                    {
                        if (AllShapes[i].Id != ((IFlowChartBase)sender).Id)
                        {
                            AllShapes[i].IsSelected = false;
                            AllShapes[i].IsMultiSelected = false;
                        }
                    }

                    DisplayGlobalCtrlShape();

                    if (null != evtShapeSelected)
                    {
                        evtShapeSelected((IFlowChartBase)sender);
                    }

                    break;
                case ChangedPropertys.SelectFalse:
                    break;
                case ChangedPropertys.Size:
                    break;
                case ChangedPropertys.Position:
                    //如果移动为流程形状，则需要重新布置连接线
                    if (sender is ShapeBase)
                    {
                        for (int i = 0; i < AllShapes.Count; i++)
                        {
                            if (AllShapes[i] is LinkBase)
                            {
                                ((LinkBase)AllShapes[i]).MoveToDockedShape((IFlowChartBase)sender);
                            }
                        }
                    }
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 处理形状移动
        /// </summary>
        /// <param name="ifcb"></param>
        protected virtual void DealShapeMoving(IFlowChartBase ifcb)
        {
            if (ifcb is ShapeBase)
            {
                for (int i = 0; i < AllShapes.Count; i++)
                {
                    if (AllShapes[i] is LinkBase)
                    {
                        AllShapes[i].AcceptShapeMove(ifcb);
                    }
                }
            }
            else if (ifcb is LinkBase)
            {
                for (int i = 0; i < AllShapes.Count; i++)
                {
                    if (AllShapes[i] is ShapeBase)
                    {
                        AllShapes[i].AcceptShapeMove(ifcb);
                    }
                }

            }
        }

          


        #region 打开文件
        public virtual void Open_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.Filter = "Designer Files (*.xml)|*.xml|All Files (*.*)|*.*";

            if (openFile.ShowDialog() == true)
            {
               LoadXml(this,openFile.FileName);
            }        

        }

        #endregion

        #region 保存文件
        public virtual void Save_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            SaveFileDialog saveFile = new SaveFileDialog();
            saveFile.Filter = "Files (*.xml)|*.xml|All Files (*.*)|*.*";
            if (saveFile.ShowDialog() == true)
            {
                try
                {
                    Save2XmlFile(this, saveFile.FileName);                    
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.StackTrace, ex.Message, MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

        }
        #endregion

        protected virtual IFlowChartBase getFlowChartBase(string modelName)
        {
            IFlowChartBase ifcb = null;
            // todo 
            return ifcb;
        }


        #region 数据持久化到xml页面部分

        protected virtual void Save2XmlFile(BaseWorkModel baseModel, string fileName)
        {
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.AppendChild(xmlDocument.CreateXmlDeclaration("1.0", "utf-8", "yes"));
            XmlElement xmlRoot = xmlDocument.CreateElement(string.Empty, "BaseModel", string.Empty);
            SerializeModelItems(baseModel, xmlDocument, xmlRoot);
            xmlDocument.AppendChild(xmlRoot);
            xmlDocument.Save(fileName);
        }

        private void SerializeModelItems(BaseWorkModel baseModel, XmlDocument xmlDocument, XmlElement xmlElement)
        {
            IEnumerable<ShapeBase> designerItems = baseModel.AllShapes.OfType<ShapeBase>();
            IEnumerable<LinkBase> connections = baseModel.AllShapes.OfType<LinkBase>();
            XmlElement designerItemsXml = xmlDocument.CreateElement(string.Empty, "DesignerItems", string.Empty);
            foreach (ShapeBase shapeBase in designerItems)
            {
                Type type = shapeBase.GetType();
                XmlElement designerItemXml = xmlDocument.CreateElement(string.Empty, type.Name, string.Empty);               
                foreach (string propName in shapeBase.SerializeAttributes) {
                    XmlElement propXml = xmlDocument.CreateElement(string.Empty, propName, string.Empty);
                    propXml.InnerText = type.GetProperty(propName).GetValue(shapeBase, null).ToString();
                    designerItemXml.AppendChild(propXml);
                }
                designerItemsXml.AppendChild(designerItemXml);
            }
            XmlElement connectionItemsXml = xmlDocument.CreateElement(string.Empty, "Connections", string.Empty);            
            foreach (LinkBase linkBase in connections) /// connections 还是包含了所有LinkBase和ShapeBase图形，没有达到过滤的目的，OfType作用是什么？
            {
                Type type = linkBase.GetType();
                XmlElement connectionXml = xmlDocument.CreateElement(string.Empty, type.Name, string.Empty);
                foreach (string propName in linkBase.SerializeAttributes)
                {
                    XmlElement propXml = xmlDocument.CreateElement(string.Empty, propName, string.Empty);
                    propXml.InnerText = type.GetProperty(propName).GetValue(linkBase, null).ToString();
                    connectionXml.AppendChild(propXml);
                }              
                connectionItemsXml.AppendChild(connectionXml);
            }
            xmlElement.AppendChild(designerItemsXml);
            xmlElement.AppendChild(connectionItemsXml);
        }

        private void LoadXml(BaseWorkModel baseModel, string fileName)
        {
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(fileName);
            XmlElement xmlRoot = xmlDocument.DocumentElement;
            LoadChildNode(baseModel, xmlRoot);

        }

        private void LoadChildNode(BaseWorkModel baseModel, XmlElement xmlElement)
        {
            XmlNodeList xmlNodeList = xmlElement.ChildNodes;//根结点,包括DesignerItems和Connections

            foreach (XmlElement xe in xmlNodeList)   //遍历模块节点
            {
                if (xe.Name == "DesignerItems")
                {
                    XmlNodeList designerItemNodeList = xe.ChildNodes;
                    foreach (XmlElement designeritemXe in designerItemNodeList)
                    {
                        Assembly assembly = Assembly.GetExecutingAssembly();//获取当前程序集
                        ShapeBase obj = assembly.CreateInstance("ModelingToolsAppWithMVVM.Common.ChartCommon." + designeritemXe.Name) as ShapeBase;
                        Type type = obj.GetType();
                        var props = type.GetProperties();
                        foreach (var prop in props)
                        {
                            XmlNodeList attributeItemNodeList = designeritemXe.ChildNodes;
                            foreach (XmlElement attributeXe in attributeItemNodeList)
                            {
                                if (attributeXe.Name == prop.Name)
                                {
                                    prop.SetValue(obj, Convert.ChangeType(attributeXe.InnerText, prop.PropertyType));
                                }
                            }                           
                        }
                        baseModel.AddShapes(obj);                       
                    }
                }
                else if (xe.Name == "Connections")
                {
                    XmlNodeList connectionItemNodeList = xe.ChildNodes;
                    foreach (XmlElement connectionXe in connectionItemNodeList)
                    {
                        Assembly assembly = Assembly.GetExecutingAssembly();//获取当前程序集
                        LinkBase obj = assembly.CreateInstance("ModelingToolsAppWithMVVM.Common.ChartCommon." + connectionXe.Name) as LinkBase;
                        Type type = obj.GetType();
                        var props = type.GetProperties();
                        foreach (var prop in props)
                        {
                            XmlNodeList attributeItemNodeList = connectionXe.ChildNodes;
                            foreach (XmlElement attributeXe in attributeItemNodeList)
                            {
                                if (attributeXe.Name == prop.Name)
                                {
                                    prop.SetValue(obj, Convert.ChangeType(attributeXe.InnerText, prop.PropertyType));
                                }
                            }
                        }
                        if (obj != null)
                        {
                            obj.CreateShape();
                            baseModel.AddShapes(obj);
                        }         

                    }
                }

            }
        }

        #endregion

    }
}
