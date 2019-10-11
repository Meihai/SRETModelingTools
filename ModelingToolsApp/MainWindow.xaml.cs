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
using System.Collections.ObjectModel;
using System.Data;
using System.Collections;
using System.ComponentModel;
using System.Xml;
using System.Xml.Linq;
using System.Windows.Markup;
using System.Globalization;
using System.IO;
using ModelingToolsApp.UserControls;
using Microsoft.Win32;

namespace ModelingToolsApp
{
    public delegate void delSetShapeDetail(string shapeType, string shapeId);
    public delegate void delSaveFlowChart(string flowChartId, DataTable flowChartRelations, byte[] flowChartData);
    public delegate void delOpenFlowChart();
    public delegate void delNewFlowChart();
    public delegate void delEditFlowChart();
    public delegate void delDeleteRoute();
    public delegate void delAfterLoadData();
    public delegate void delShapeSelected(string shapeId);

    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
     

        /// <summary>
        /// 判断鼠标是否开始移动
        /// </summary>
        bool IsStartMove = false;

        /// <summary>
        /// 鼠标在viewer上按下左键时的位置
        /// </summary>
        Point Offset = new Point(0, 0);

        /// <summary>
        /// 全局鼠标事件监听
        /// </summary>
        GlobalMouseHook GlobalMouseListener = new GlobalMouseHook();

        /// <summary>
        /// 鼠标停靠在某个形状上的联系点
        /// </summary>
        private DockedLinkNodeArgs MouseDockedLinkNode = new DockedLinkNodeArgs();

        /// <summary>
        /// 所有已经添加的形状
        /// </summary>
        private ObservableCollection<IFlowChartBase> AllShapes = new ObservableCollection<IFlowChartBase>();

        private List<IFlowChartBase> CloneShapes = new List<IFlowChartBase>();

        /// <summary>
        /// 设置流程详细参数,"properties"必须包含key为desc的项，其值为字符串，在流程图中显示
        /// </summary>
        public event delSetShapeDetail evtSetShapeDetail;

        /// <summary>
        /// 保存流程图。FlowChartRelations流程图中包含的加工步骤；FlowChartData为构成流程图展示效果的二进制数据
        /// </summary>
        public event delSaveFlowChart evtSaveFlowChart;

        /// <summary>
        /// 新建流程图
        /// </summary>
        public event delNewFlowChart evtNewFlowChart;

        /// <summary>
        /// 打开流程图
        /// </summary>
        public event delOpenFlowChart evtOpenFlowChart;

        /// <summary>
        /// 编辑流程图
        /// </summary>
        public event delEditFlowChart evtEditFlowChart;

        /// <summary>
        /// 流程被选中
        /// </summary>
        public event delShapeSelected evtShapeSelected;

        /// <summary>
        /// 删除流程图
        /// </summary>
        public event delDeleteRoute evtDeleteRoute;

        
        /// <summary>
        /// 流程图编号
        /// </summary>
        private string FlowChartId = "";


     
        private ArrayList linkBaseArray;


        public MainWindow()
        {
            try
            {
                InitializeComponent();
                AllShapes.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(AllShapes_CollectionChanged);
                GlobalMouseHook.evtGlobalMouseUp += new delGlobalMouseUp(GlobalMouseHook_evtGlobalMouseUp);
                GCtrlShape.evtSizeChanged += new delSizeChanged(GCtrlShape_evtSizeChanged);
                GCtrlShape.evtPrepareToChangePosition += new delPrepareToChangePosition(GCtrlShape_evtPrepareToChangePosition);
                GCtrlShape.evtPositionChanged += new delPositionChanged(GCtrlShape_evtPositionChanged);
                GCtrlLine.evtTerminalPointMoved += new delTerminalPointMoved(GCtrlLine_evtTerminalPointMoved);
               // menuMain.evtMainMenuClick += new delMainMenuClick(menuMain_evtMainMenuClick);
                evtShapeSelected += new delShapeSelected(ShapeSelected);;

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
                this.CommandBindings.Add(new CommandBinding(ApplicationCommands.Delete, Delete_Executed, Delete_Enabled));
                this.CommandBindings.Add(new CommandBinding(ApplicationCommands.Copy, Copy_Executed, Copy_Enabled));
                this.CommandBindings.Add(new CommandBinding(ApplicationCommands.Paste,Paste_Executed,Paste_Enabled));
                this.CommandBindings.Add(new CommandBinding(ApplicationCommands.Open, Open_Executed));
                this.CommandBindings.Add(new CommandBinding(ApplicationCommands.Save, Save_Executed));
                Clipboard.Clear();
            }
            catch (Exception ex)
            {
                /**
                 * 1.异常消息
                 * 2.异常模块名称
                 * 3.异常方法名称
                 * 4.异常行号
                 */
                String str = "";
                str += ex.Message + "\n";//异常消息
                str += ex.StackTrace + "\n";//提示出错位置，不会定位到方法内部去
                str += ex.ToString() + "\n";//将方法内部和外部所有出错的位置提示出来
                System.Windows.MessageBox.Show(str);
            }
        }


        public 
        void SearchRoad_evtRoadSearchedFinished(PointCollection path)
        {
            if (null == path || 0 == path.Count)
            {
                return;
            }
            tmpLine.Points = path;
            this.tmpLine.Visibility = Visibility.Visible;
        }

        void AllShapes_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
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

            RoadSearch.AllShapes = AllShapes;
        }

        void GCtrlLine_evtTerminalPointMoved(Point point, int seq)
        {
            for (int i = 0; i < AllShapes.Count; i++)
            {
                AllShapes[i].AcceptPointMove(point, seq);
            }
        }

        void GCtrlShape_evtPositionChanged(double left, double top)
        {

        }

        void GCtrlShape_evtPrepareToChangePosition(bool flag)
        {
            if (flag)
            {
                for (int i = 0; i < AllShapes.Count; i++)
                {
                    if (AllShapes[i].IsSelected || AllShapes[i].IsMultiSelected)
                    {
                        IFlowChartBase icb = (IFlowChartBase)AllShapes[i].Clone();
                        CloneShapes.Add(icb);
                        viewer.Children.Add((UserControl)icb);
                    }
                }
            }
            else
            {
                for (int i = 0; i < CloneShapes.Count; i++)
                {
                    viewer.Children.Remove((UserControl)CloneShapes[i]);
                    CloneShapes[i] = null;
                    CloneShapes.Clear();
                }
            }
        }

        void GCtrlShape_evtSizeChanged(double difLeft, double difTop, double difWidth, double difHeight)
        {
            for (int i = 0; i < AllShapes.Count; i++)
            {
                if (AllShapes[i].IsSelected || AllShapes[i].IsMultiSelected)
                {
                    AllShapes[i].ChangePositionAndSize(difLeft, difTop, difWidth, difHeight);
                }
            }
        }

        void GlobalMouseHook_evtGlobalMouseUp(GlobalMouseArgs e)
        {
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
                viewer.Children.Remove((System.Windows.Controls.UserControl)CloneShapes[i]);
                CloneShapes[i] = null;
            }

            CloneShapes.Clear();

            #region 多选框
            if (GMultiSelector.Visibility == Visibility.Visible)
            {
                //int cnt = 0;
                //int pos = 0;
                //double minx = int.MaxValue;
                //double maxx = int.MinValue;
                //double miny = int.MaxValue;
                //double maxy = int.MinValue;

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
                        //minx = minx > AllShapes[i].Range[0].X ? AllShapes[i].Range[0].X : minx;
                        //miny = miny > AllShapes[i].Range[0].Y ? AllShapes[i].Range[0].Y : miny;
                        //maxx = maxx < AllShapes[i].Range[2].X ? AllShapes[i].Range[2].X : maxx;
                        //maxy = maxy < AllShapes[i].Range[2].Y ? AllShapes[i].Range[2].Y : maxy;

                        AllShapes[i].IsSelected = false;
                        AllShapes[i].IsMultiSelected = true;
                        //pos = i;
                        //cnt++;
                    }
                }

                GMultiSelector.Visibility = Visibility.Hidden;
            }
            #endregion

            DisplayGlobalCtrlShape();
        }

        private void viewer_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (Mouse.LeftButton != MouseButtonState.Pressed)
            {
                return;
            }

            //鼠标移动一定偏移量后，才确认为开始移动
            if (!IsStartMove)
            {
                if (Math.Abs(e.GetPosition(viewer).X - Offset.X) > 10 || Math.Abs(e.GetPosition(viewer).Y - Offset.Y) > 10)
                {
                    IsStartMove = true;
                }
                return;
            }

            #region 控制鼠标移动为整数点

            int x = (int)Math.Truncate(e.GetPosition(viewer).X);
            int y = (int)Math.Truncate(e.GetPosition(viewer).Y);
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


            if (GMultiSelector.Visibility == Visibility.Visible) //多选框控制
            {
                Point p = e.GetPosition(viewer);
                GMultiSelector.Width = Math.Abs(p.X - Offset.X);
                GMultiSelector.Height = Math.Abs(p.Y - Offset.Y);
                GMultiSelector.Margin = new Thickness(p.X > Offset.X ? Offset.X : p.X, p.Y > Offset.Y ? Offset.Y : p.Y, 0, 0);
            }
            else if (CloneShapes.Count > 0)
            {
                //临时形状移动
                for (int i = 0; i < CloneShapes.Count; i++)
                {
                    //CloneShapes[i].ChangePositionAndSize(e.GetPosition(viewer).X + CloneShapes[i].Offset.X - Offset.X - CloneShapes[i].Margin.Left,
                    //    e.GetPosition(viewer).Y + CloneShapes[i].Offset.Y - Offset.Y - CloneShapes[i].Margin.Top, 0, 0);
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

        private void viewer_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.Source == viewer)
            {
                for (int i = 0; i < AllShapes.Count; i++)
                {
                    AllShapes[i].IsSelected = false;
                    AllShapes[i].IsMultiSelected = false;
                }
                GCtrlShape.Visibility = Visibility.Hidden;

                Offset = e.GetPosition(viewer);
                GMultiSelector.Margin = new Thickness(Offset.X, Offset.Y, 0, 0);
                GMultiSelector.Width = 0;
                GMultiSelector.Height = 0;
                GMultiSelector.Visibility = Visibility.Visible;

                GDescEditor.Visibility = Visibility.Hidden;
            }
        }

        private void ScrollViewer_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            //backviewer.Width = e.NewSize.Width;
          //  backviewer.Height = e.NewSize.Height;
            viewer.Width = e.NewSize.Width;
            viewer.Height = e.NewSize.Height;
        }


        private void ShapeSelected(String shapeId)
        {
            DataTable table = new DataTable("test");
            table.Columns.Add("属性", typeof(String));
            table.Columns.Add("值", typeof(String));
           
            for (int i = 0; i < AllShapes.Count; i++)
            {
                if (AllShapes[i].Id == shapeId)
                {
                    table.Rows.Add("名称", AllShapes[i].Description);
                    table.Rows.Add("高度",AllShapes[i].Height);
                    table.Rows.Add("宽度",AllShapes[i].Width);
                    DisplayShapeProperties(table);                    
                }
            }
        }

        #region 鼠标事件

        /// <summary>
        /// 拖动时生成的形状
        /// </summary>
        IFlowChartBase DragIfcb = null;

        int test = 0;
        private void viewer_DragEnter(object sender, System.Windows.DragEventArgs e)
        {
            if (null == DragIfcb)
            {
                switch ((FlowChartTypes)(e.Data.GetData(typeof(FlowChartTypes))))
                {
                    case FlowChartTypes.ShapeProcess:
                        DragIfcb = new ShapeProcess();
                        break;
                    case FlowChartTypes.ShapeJudge:
                        DragIfcb = new ShapeJudge();
                        break;
                    case FlowChartTypes.ShapeSeqTransfer: {
                        DragIfcb = new LinkBroken(LineTypes.Solid);
                        break;
                    }
                    case FlowChartTypes.ShapeProbTransfer:
                        {
                            DragIfcb = new LinkStraight(LineTypes.ShortDashes);
                            break;
                        }
                    case FlowChartTypes.ShapeEntrance: {
                        DragIfcb = new ShapeEntrance();
                        break;
                    }
                    case FlowChartTypes.ShapeExit: {
                        DragIfcb = new ShapeExit();
                        break;
                    }
                    case FlowChartTypes.ShapeTerminal: {
                        DragIfcb = new ShapeTerminal();
                        break;
                    }
                    case FlowChartTypes.ShapeReliableProfile: {
                        DragIfcb = new ShapeReliableProfile();
                        break;
                    }
                    case FlowChartTypes.ShapeOperation: {
                        DragIfcb = new ShapeOperation();
                        break;
                    }

                    default:
                        break;
                }

                if (null != DragIfcb)
                {
                    DragIfcb.ChangePositionAndSize(e.GetPosition(viewer).X - DragIfcb.Width / 2 - DragIfcb.Margin.Left,
                                                    e.GetPosition(viewer).Y - DragIfcb.Height / 2 - DragIfcb.Margin.Top, 0, 0);
                    viewer.Children.Add((System.Windows.Controls.UserControl)DragIfcb);

                    if (DragIfcb is ShapeBase)
                    {
                        ((ShapeBase)DragIfcb).evtDocketLinkNode += new delDockedLinkNode(FlowChartEditor_evtDocketLinkNode);
                    }
                }
            }
        }

        private void viewer_DragOver(object sender, System.Windows.DragEventArgs e)
        {
            if (null != DragIfcb)
            {
                #region 控制鼠标移动为整数点

                int x = (int)Math.Truncate(e.GetPosition(viewer).X);
                int y = (int)Math.Truncate(e.GetPosition(viewer).Y);
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

        private void viewer_DragLeave(object sender, System.Windows.DragEventArgs e)
        {
            if (e.OriginalSource == viewer)
            {
                return;
            }

            if (null != DragIfcb)
            {
                viewer.Children.Remove((System.Windows.Controls.UserControl)DragIfcb);
                DragIfcb = null;
            }
        }

        private void viewer_Drop(object sender, System.Windows.DragEventArgs e)
        {
            if (null != DragIfcb)
            {
                viewer.Children.Remove((System.Windows.Controls.UserControl)DragIfcb);
                AddShapes(DragIfcb);
                DragIfcb = null;
            }
        }

        #endregion


        /// <summary>
        /// 添加形状
        /// </summary>
        /// <param name="ifcb"></param>
        private void AddShapes(IFlowChartBase ifcb)
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
            {   /*
                ContextMenu cm = new ContextMenu();
                MenuItem mi = new MenuItem();
                mi.Header = "设置线条颜色";                
                mi.DataContext = ifcb;
                cm.Items.Add(mi);
                ((ShapeBase)ifcb).ContextMenu = cm;
               */
             
                ((ShapeBase)ifcb).evtDocketLinkNode += new delDockedLinkNode(FlowChartEditor_evtDocketLinkNode);
            }

            if (ifcb is LinkBase)
            {
                ((LinkBase)ifcb).evtRefreshTmpLine += new delRefreshTmpLine(FlowChartEditor_evtRefreshTmpLine);
               /*
                ContextMenu cm = new ContextMenu();
                MenuItem mi = new MenuItem();
                mi.Header = "设置线条颜色";
                mi.Click += colorSelectWindowClick;
                mi.DataContext = ifcb;
                cm.Items.Add(mi);
                ((LinkBase)ifcb).ContextMenu = cm;
                */
                //为实现加入后调整路径
                ifcb.MoveToClone((IFlowChartBase)ifcb.Clone());
            }

            AllShapes.Add(ifcb);
            viewer.Children.Add((UserControl)ifcb);
            ifcb.IsSelected = true;
            ifcb.ChangePositionAndSize(0, 0, 0, 0);
        }

        private void MenuItem_Click_NewFile(object sender, RoutedEventArgs e) {
            MenuNew();
        }
       
      

        private void MenuItem_Click_ColorSelection(object sender, RoutedEventArgs e) {
            linkBaseArray = new ArrayList();            
            for (int i = 0; i < AllShapes.Count; i++)
            {
                if (AllShapes[i].IsSelected || AllShapes[i].IsMultiSelected) {
                    if(AllShapes[i] is LinkBase){
                        linkBaseArray.Add(i);
                    }                    
                }               
            }
            if (linkBaseArray.Count == 0) { 
                MessageBox.Show("未选择连接线");
                return;
            }

            ColorSelectionWindow colorWindow = new ColorSelectionWindow();
            
            Brush  brush = new SolidColorBrush(Colors.Black);
            
            colorWindow.ExSelectedColor = (Color)ColorConverter.ConvertFromString(brush.ToString());
            colorWindow.ColorChangedEvent += evtColorChangeEvent;
            colorWindow.ShowDialog();

        }

        void FlowChartEditor_evtRefreshTmpLine(Geometry geometry)
        {
            tmpLinkPath.Visibility = Visibility.Visible;
            tmpLinkPath.Data = geometry;
        }

      

        private void evtColorChangeEvent(object sender, Color newColor)
        {
            for (int i = 0; i < linkBaseArray.Count; i++)
            {
                    int j=(int)linkBaseArray[i];
                    LinkBase linkBase = AllShapes[j] as LinkBase;                   
                    linkBase.LineBrush = new SolidColorBrush(newColor);                
                 
                
            }
          

        }
         
      
      

        void ifcb_evtMouseDoubleClick(IFlowChartBase ifcb, string description)
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

        void FlowChartEditor_evtDocketLinkNode(DockedLinkNodeArgs e)
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

        void ifcb_evtMouseClick(IFlowChartBase ifcb)
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
                    }
                    CloneShapes.Add(icb);
                    viewer.Children.Add((UserControl)icb);
                    cnt++;
                }
            }
        }

        void ifcb_PropertyChanged(object sender, PropertyChangedEventArgs e)
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
                        evtShapeSelected(((IFlowChartBase)sender).Id);
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
        /// 显示控制框
        /// </summary>
        /// <param name="shape"></param>
        private void DisplayGlobalCtrlShape()
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
                    //GCtrlShape.Width = AllShapes[pos].Width;
                    //GCtrlShape.Height = AllShapes[pos].Height;
                    //GCtrlShape.Margin = AllShapes[pos].Margin;
                    //GCtrlShape.Visibility = Visibility.Visible;
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

        private void viewer_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            Offset = e.GetPosition(viewer);
            IsStartMove = false;
        }


        #region 菜单按钮事件

        void menuMain_evtMainMenuClick(string menuType)
        {
            if ("new" == menuType)
            {
                MenuNew();
            }
            else if ("open" == menuType)
            {
                MenuOpen();
            }
            else if ("save" == menuType)
            {
                MenuSave();
            }
            else if ("print" == menuType)
            {
                MenuPrint();
            }
            else if ("edit" == menuType)
            {
                MenuEdit();
            }
          
            else if ("delete" == menuType)
            {
                MenuDelete();
            }
          
            else if ("deleteroute" == menuType)
            {
                MenuDeleteRoute();
            }


            //修改控制框位置
            int cnt = 0;
            int pos = 0;
            double minx = int.MaxValue;
            double maxx = int.MinValue;
            double miny = int.MaxValue;
            double maxy = int.MinValue;

            for (int i = 0; i < AllShapes.Count; i++)
            {
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
            if (cnt > 0)
            {
                GCtrlShape.Width = maxx - minx + 20;
                GCtrlShape.Height = maxy - miny + 20;
                GCtrlShape.Margin = new Thickness(minx - 10, miny - 10, 0, 0);
                GCtrlShape.Visibility = Visibility.Visible;
            }
        }

        /// <summary>
        /// 获得所有处于多选状态的形状中的最小位置形状的序号
        /// </summary>
        /// <param name="minxShapePos"></param>
        /// <param name="minyShapePos"></param>
        private bool GetMinPositionShape(out int minxShapePos, out int minyShapePos)
        {
            double minx = int.MaxValue;
            double miny = int.MaxValue;
            minxShapePos = 0;
            minyShapePos = 0;
            int totalShape = 0;
            for (int i = 0; i < AllShapes.Count; i++)
            {
                if (AllShapes[i].IsMultiSelected)
                {
                    if (minx > AllShapes[i].Margin.Left)
                    {
                        minx = AllShapes[i].Margin.Left;
                        minxShapePos = i;
                    }

                    if (miny > AllShapes[i].Margin.Top)
                    {
                        miny = AllShapes[i].Margin.Top;
                        minyShapePos = i;
                    }

                    totalShape++;
                }
            }
            return totalShape > 0;
        }




        /// <summary>
        /// 新建
        /// </summary>
        public void MenuNew()
        {
            if (null != evtNewFlowChart)
            {
                evtNewFlowChart();
            }
            else
            {
                CreateNewFlowChart(System.Guid.NewGuid().ToString());
            }
        }

        /// <summary>
        /// 打开
        /// </summary>
        public void MenuOpen()
        {   
            OpenFlowChart();
        }

        /// <summary>
        /// 保存
        /// </summary>
        public void MenuSave()
        {
            SaveFlowChart();
        }

        /// <summary>
        /// 删除当前流程
        /// </summary>
        public void MenuDeleteRoute()
        {
            if (null != evtDeleteRoute)
            {
                evtDeleteRoute();
            }
        }

        /// <summary>
        /// 打印
        /// </summary>
        public void MenuPrint()
        {
            MessageBox.Show("MenuPrint");
        }

        public void MenuEdit()
        {
            EditFlowChart();
        }

    
          

        /// <summary>
        /// 删除
        /// </summary>
        public void MenuDelete()
        {
            for (int i = AllShapes.Count - 1; i >= 0; i--)
            {
                if (AllShapes[i].IsMultiSelected || AllShapes[i].IsSelected)
                {
                    viewer.Children.Remove((UserControl)AllShapes[i]);
                    AllShapes.Remove(AllShapes[i]);
                }
            }

            //GCtrlLine.Visibility = Visibility.Hidden;
            GCtrlShape.Visibility = Visibility.Hidden;
            GLinkNode0.Visibility = Visibility.Hidden;
            GLinkNode1.Visibility = Visibility.Hidden;
            tmpLine.Visibility = Visibility.Hidden;
        }

        #endregion


        /// <summary>
        /// 处理形状移动
        /// </summary>
        /// <param name="ifcb"></param>
        private void DealShapeMoving(IFlowChartBase ifcb)
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
     

        /// <summary>
        /// 保存全部流程关系的表
        /// </summary>
        DataTable AllFlowRelation = new DataTable();

        /// <summary>
        /// 初始化保存全部流程关系的表
        /// </summary>
        private void InitAllFlowRelation()
        {
            AllFlowRelation.Rows.Clear();
            AllFlowRelation.Columns.Clear();
            AllFlowRelation.Columns.Add("from_id");
            AllFlowRelation.Columns.Add("relation_id");
            AllFlowRelation.Columns.Add("to_id");
        }

        /// <summary>
        /// 添加一个流程到保存全部流程关系的表
        /// </summary>
        /// <param name="fromId"></param>
        /// <param name="fromDesc"></param>
        /// <param name="judgeId"></param>
        /// <param name="judgeDesc"></param>
        /// <param name="relationId"></param>
        /// <param name="relationDesc"></param>
        /// <param name="toId"></param>
        /// <param name="toDesc"></param>
        private void AddAFlow(string fromId, string relationId, string toId)
        {
            DataRow row = AllFlowRelation.NewRow();
            row[0] = fromId;
            row[1] = relationId;
            row[2] = toId;
            AllFlowRelation.Rows.Add(row);
        }

        /// <summary>
        /// 获得整个流程图中的流程
        /// </summary>
        private void GetFlow()
        {
            for (int i = 0; i < AllShapes.Count; i++)
            {
                if (AllShapes[i] is ShapeProcess)
                {
                    for (int j = 0; j < AllShapes.Count; j++)
                    {
                        if (AllShapes[j] is LinkBase)
                        {
                            if (((LinkBase)AllShapes[j]).StartPnt.DockedFlag &&
                                ((LinkBase)AllShapes[j]).StartPnt.RelatedShapeId == AllShapes[i].Id)
                            {
                                for (int k = 0; k < AllShapes.Count; k++)
                                {
                                    if (((LinkBase)AllShapes[j]).EndPnt.DockedFlag &&
                                        ((LinkBase)AllShapes[j]).EndPnt.RelatedShapeId == AllShapes[k].Id)
                                    {
                                        if (AllShapes[k] is ShapeProcess)
                                        {
                                            AddAFlow(AllShapes[i].Id, AllShapes[j].Id, AllShapes[k].Id);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }


        private byte[] GetFlowDatas()
        {
            byte[] data = new byte[AllShapes.Count * 1024];
            int idx = 0;

            for (int i = 0; i < AllShapes.Count; i++)
            {
                byte[] buf = AllShapes[i].ToBytes();
                Array.Copy(buf, 0, data, idx, buf.Length);
                idx += buf.Length;
            }

            byte[] flows = new byte[idx];
            Array.Copy(data, flows, idx);
            return flows;
        }



        private void SaveFlowChart()
        {
            InitAllFlowRelation();
            GetFlow();

            byte[] data = GetFlowDatas();

            if (null != evtSaveFlowChart)
            {
                evtSaveFlowChart(this.FlowChartId, AllFlowRelation, data);
            }
            else
            {
                Microsoft.Win32.SaveFileDialog sfd = new Microsoft.Win32.SaveFileDialog();
                sfd.Filter = "FlowChart documents (.flcd)|*.flcd";
                if (sfd.ShowDialog() == true)
                {
                    string file = sfd.FileName;
                    using (System.IO.FileStream fs = new System.IO.FileStream(file, System.IO.FileMode.Create))
                    {
                        fs.Write(data, 0, data.Length);
                    }
                }
            }
        }

        private void OpenFlowChart()
        {
            if (null != evtOpenFlowChart)
            {
                evtOpenFlowChart();
            }
            else
            {
                Microsoft.Win32.OpenFileDialog ofd = new Microsoft.Win32.OpenFileDialog();
                ofd.Filter = "FlowChart documents (.flcd)|*.flcd";
                ofd.Multiselect = false;
                if (ofd.ShowDialog() == true)
                {
                    string file = ofd.FileName;
                    using (System.IO.FileStream fs = new System.IO.FileStream(file, System.IO.FileMode.Open))
                    {
                        byte[] data = new byte[fs.Length];
                        fs.Read(data, 0, (int)fs.Length);
                        LoadFlowChartData(Guid.NewGuid().ToString(), data);
                    }
                }
            }
        }


        private void EditFlowChart()
        {
            if (null != evtEditFlowChart)
            {
                evtEditFlowChart();
            }
        }


        /// <summary>
        /// 从二进制数据加载流程图
        /// </summary>
        /// <param name="data"></param>
        public void LoadFlowChartData(string flowChartId, byte[] data)
        {
            this.FlowChartId = flowChartId;

            for (int i = 0; i < AllShapes.Count; i++)
            {
                viewer.Children.Remove((UserControl)AllShapes[i]);
            }
            AllShapes.Clear();
            /*
            if (null != data && data.Length > 0)
            {
                int idx = 0;
                int lenght = 0;
              

                while (idx < data.Length)
                {
                    foreach (string item in Enum.GetNames(typeof(FlowChartTypes)))
                    {   
                        
                        IFlowChartBase ifcb = getFlowChartBase(item);
                        ifcb = ifcb.FromBytes(data, idx, out lenght);
                        if (null != ifcb)
                        {
                            AddShapes(ifcb);
                            ifcb.IsSetProperty = true;
                            break;
                        }
                    }
                    idx += lenght;
                }
             
            }*/
        }

        private IFlowChartBase getFlowChartBase(string modelName) {
            IFlowChartBase ifcb = null;
            switch (modelName)
            {
                case "ShapeProcess":
                    ifcb = new ShapeProcess();
                    break;
                case "ShapeJudge":
                    ifcb = new ShapeJudge();
                    break;
                case "ShapeSeqTransfer":
                    {
                        ifcb = new LinkBroken(LineTypes.Solid);
                        break;
                    }
                case "ShapeProbTransfer":
                    {
                        ifcb = new LinkStraight(LineTypes.ShortDashes);
                        break;
                    }
                case "ShapeEntrance":
                    {
                        ifcb = new ShapeEntrance();
                        break;
                    }
                case "ShapeExit":
                    {
                        ifcb = new ShapeExit();
                        break;
                    }
                case "ShapeTerminal":
                    {
                        ifcb = new ShapeTerminal();
                        break;
                    }
                case "ShapeReliableProfile":
                    {
                        ifcb = new ShapeReliableProfile();
                        break;
                    }
                case "ShapeOperation":
                    {
                        ifcb = new ShapeOperation();
                        break;
                    }

                default:
                    break;
            }
            return ifcb;
        }

        /// <summary>
        /// 创建新流程图
        /// </summary>
        /// <param name="id"></param>
        public void CreateNewFlowChart(string newFlowChartId)
        {
            this.FlowChartId = newFlowChartId;

            for (int i = 0; i < AllShapes.Count; i++)
            {
                viewer.Children.Remove((UserControl)AllShapes[i]);
            }

            AllShapes.Clear();
        }



        /// <summary>
        /// 设置流程描述
        /// </summary>
        /// <param name="shapeId"></param>
        /// <param name="description"></param>
        public void SetFlowChartDescription(string shapeId, string description)
        {
            for (int i = 0; i < AllShapes.Count; i++)
            {
                if (AllShapes[i].Id == shapeId)
                {
                    AllShapes[i].Description = description;
                    AllShapes[i].IsSetProperty = true;
                }
            }
        }

         #region Delete Command

        private void Delete_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            MenuDelete();
        }

        private void Delete_Enabled(object sender, CanExecuteRoutedEventArgs e)
        {
            int cnt = 0;
            for (int i = AllShapes.Count - 1; i >= 0; i--)
            {

                if (AllShapes[i].IsMultiSelected || AllShapes[i].IsSelected)
                {
                    viewer.Children.Remove((UserControl)AllShapes[i]);
                    AllShapes.Remove(AllShapes[i]);
                    cnt = cnt + 1;
                }
            }
            e.CanExecute = cnt > 0;
        }
        #endregion

        #region Copy Command
        private void Copy_Executed(object sender,ExecutedRoutedEventArgs e){
            CopyCurrentSelection();
        }

        private void CopyCurrentSelection()
        {
            List<ShapeBase> shapeBaseList = new List<ShapeBase>();
            List<LinkBase> linkBaseList = new List<LinkBase>();
            for (int i = 0; i < AllShapes.Count; i++)
            {
                if (AllShapes[i].IsSelected || AllShapes[i].IsMultiSelected)
                {
                    if (AllShapes[i] is ShapeBase)
                    {
                        shapeBaseList.Add((ShapeBase)AllShapes[i]);
                    }else if (AllShapes[i] is LinkBase) {
                        linkBaseList.Add((LinkBase) AllShapes[i]);
                    }
                }
            }           
            IEnumerable<ShapeBase> selectedDesignerItems =
               shapeBaseList.OfType<ShapeBase>();
            IEnumerable<LinkBase> selectedConnections = linkBaseList.OfType<LinkBase>();

            XElement designerItemsXML = SerializeShapeItems(selectedDesignerItems);
            XElement connectionsXML = SerializeConnections(selectedConnections);
            XElement root = new XElement("Root");
            root.Add(designerItemsXML);
            root.Add(connectionsXML);
            root.Add(new XAttribute("OffsetX", 10));
            root.Add(new XAttribute("OffsetY", 10));
            Clipboard.Clear();
            Clipboard.SetData(DataFormats.Xaml, root);
        }

        private XElement SerializeShapeItems(IEnumerable<ShapeBase> designerItems)
        {
            XElement serializedItems = new XElement("DesignerItems",
                                       from item in designerItems
                                       let contentXaml = XamlWriter.Save(((ShapeBase)item).Content)
                                       select new XElement("DesignerItem",
                                                  new XElement("Left", item.Margin.Left),
                                                  new XElement("Top", item.Margin.Top),
                                                  new XElement("Width", item.Width),
                                                  new XElement("Height", item.Height),
                                                  new XElement("ID", item.Id),
                                                  new XElement("zIndex", Canvas.GetZIndex(item)),
                                                  new XElement("type",item.FlowChartType),
                                                  new XElement("description",item.Description),
                                                  new XElement("cloneSourceId",item.CloneSourceId),
                                                  new XElement("Content", contentXaml)
                                              )
                                   );

            return serializedItems;
        }

        private ShapeBase DeserializeShapeItem(XElement itemXML, Guid id, double OffsetX, double OffsetY)
        {
            FlowChartTypes flowChartType =(FlowChartTypes)Enum.Parse(typeof(FlowChartTypes),itemXML.Element("type").Value);
            ShapeBase item = createShapeItem(flowChartType);
            item.Width = Double.Parse(itemXML.Element("Width").Value, CultureInfo.InvariantCulture);
            item.Height = Double.Parse(itemXML.Element("Height").Value, CultureInfo.InvariantCulture);
            item.Description = itemXML.Element("description").Value;
            item.CloneSourceId = itemXML.Element("cloneSourceId").Value;
            double left= Double.Parse(itemXML.Element("Left").Value, CultureInfo.InvariantCulture) + OffsetX;
            double top=Double.Parse(itemXML.Element("Top").Value, CultureInfo.InvariantCulture) + OffsetY;
            item.Margin = new Thickness(left, top, 0, 0);
          //  Canvas.SetZIndex(item, Int32.Parse(itemXML.Element("zIndex").Value));
            //Object content = XamlReader.Load(XmlReader.Create(new StringReader(itemXML.Element("Content").Value)));
           // item.Content = content;
            return item;
        }

        private XElement SerializeConnections(IEnumerable<LinkBase> Connections)
        {
            var serializedConnections = new XElement("Connections",
                           from connection in Connections
                           select new XElement("Connection",
                                      new XElement("Id", connection.Id),
                                      new XElement("Left", connection.Margin.Left),
                                      new XElement("Top", connection.Margin.Top),
                                      new XElement("Width", connection.Width),
                                      new XElement("Height", connection.Height),
                                      new XElement("type",connection.FlowChartType),
                                      new XElement("IsManualSetted", connection.IsManualSetted),
                                      new XElement("description",connection.Description),
                                      new XElement("cloneSourceId",connection.CloneSourceId),
                                      new XElement("StartPnt.Position.X", connection.StartPnt.Position.X),
                                      new XElement("StartPnt.Position.Y",connection.StartPnt.Position.Y),
                                      new XElement("StartPnt.DockedFlag",connection.StartPnt.DockedFlag),
                                      new XElement("StartPnt.RelatedShapeId", connection.StartPnt.RelatedShapeId),
                                      new XElement("StartPnt.RelatedType", connection.StartPnt.RelatedType),
                                      //
                                      new XElement("EndPnt.Position.X", connection.EndPnt.Position.X),
                                      new XElement("EndPnt.Position.Y",connection.EndPnt.Position.Y),
                                      new XElement("EndPnt.DockedFlag",connection.EndPnt.DockedFlag),
                                      new XElement("EndPnt.RelatedShapeId", connection.EndPnt.RelatedShapeId),
                                      new XElement("EndPnt.RelatedType", connection.EndPnt.RelatedType),
                                      //-----
                                      new XElement("ShapePnt.Count",connection.ShapePnt.Count),
                                      new XElement("ShapePnt.Positions",
                                             from position in connection.ShapePnt
                                             select new XElement("Position",
                                                     new XElement("X",position.X),
                                                     new XElement("Y",position.Y)
                                                     )
                                              )                             
                                     )
                                  );     
                                                
                                                 
                                               
            return serializedConnections;
        }


        private ShapeBase createShapeItem(FlowChartTypes flowChartType) {
            ShapeBase shapeBase = null;
            switch (flowChartType)
            {
                case FlowChartTypes.ShapeEntrance : { 
                    shapeBase=new ShapeEntrance();
                    break;
                }           
                case FlowChartTypes.ShapeExit: {
                     shapeBase = new ShapeExit();
                     break;
                }
                case FlowChartTypes.ShapeTerminal:{
                     shapeBase = new ShapeTerminal();
                     break;
                }
                case FlowChartTypes.ShapeReliableProfile:{
                     shapeBase = new ShapeReliableProfile();
                     break;
                }
                case FlowChartTypes.ShapeOperation:{
                     shapeBase = new ShapeOperation();
                     break;
                }       
                   
                default:
                    MessageBox.Show("不支持的图形类型");
                    break;
            }
            return shapeBase;
        }

        private LinkBase createLinkItem(FlowChartTypes flowChartType)
        {
            LinkBase linkBase = null;
            switch (flowChartType)
            {
                case FlowChartTypes.ShapeProbTransfer:
                    {
                        linkBase = new LinkStraight(LineTypes.ShortDashes);
                        break;
                    }
                case FlowChartTypes.ShapeSeqTransfer:
                    {
                        linkBase=new LinkBroken(LineTypes.Solid);
                        break;
                    }    
                case FlowChartTypes.LinkBroken:{
                      linkBase=new LinkBroken(LineTypes.Solid);
                        break;
                }
                case FlowChartTypes.LinkStraight:{
                     linkBase = new LinkStraight(LineTypes.ShortDashes);
                        break;
                }    
                default:
                    MessageBox.Show("不支持的连线类型");
                    break;
            }
            return linkBase;
        }

        private void Copy_Enabled(object sender,CanExecuteRoutedEventArgs e){
            int cnt = 0;
            for (int i = AllShapes.Count - 1; i >= 0; i--)
            {

                if (AllShapes[i].IsMultiSelected || AllShapes[i].IsSelected)
                {
                   cnt = cnt + 1;
                }
            }
            e.CanExecute = cnt > 0;
        }
        #endregion
        #region Paste Command

       
        private void Paste_Executed(object sender,ExecutedRoutedEventArgs e){

            XElement root = LoadSerializedDataFromClipBoard();

            if (root == null)
                return;

            // create DesignerItems
       
            IEnumerable<XElement> itemsXML = root.Elements("DesignerItems").Elements("DesignerItem");
            double offsetX = Double.Parse(root.Attribute("OffsetX").Value, CultureInfo.InvariantCulture);
            double offsetY = Double.Parse(root.Attribute("OffsetY").Value, CultureInfo.InvariantCulture);

            foreach (XElement itemXML in itemsXML)
            {
                Guid newID = Guid.NewGuid();              
                ShapeBase item = DeserializeShapeItem(itemXML, newID, offsetX, offsetY);
                AddShapes(item);       
                          
            }                
   
            //创建连接
            IEnumerable<XElement> connectionsXML = root.Elements("Connections").Elements("Connection");
            foreach (XElement connectionXML in connectionsXML)
            {
                FlowChartTypes flowChartType = (FlowChartTypes)Enum.Parse(typeof(FlowChartTypes), connectionXML.Element("type").Value);
                LinkBase item = createLinkItem(flowChartType);
                item.Width = Double.Parse(connectionXML.Element("Width").Value, CultureInfo.InvariantCulture);
                item.Height = Double.Parse(connectionXML.Element("Height").Value, CultureInfo.InvariantCulture);
                item.Description = connectionXML.Element("description").Value;
                item.CloneSourceId = connectionXML.Element("cloneSourceId").Value;
                double left = Double.Parse(connectionXML.Element("Left").Value, CultureInfo.InvariantCulture) + offsetX;
                double top = Double.Parse(connectionXML.Element("Top").Value, CultureInfo.InvariantCulture) + offsetY;
                item.Margin = new Thickness(left, top, 0, 0);
                item.IsManualSetted=Boolean.Parse(connectionXML.Element("IsManualSetted").Value);
                item.StartPnt.Position=new Point(Double.Parse(connectionXML.Element("StartPnt.Position.X").Value),
                    Double.Parse(connectionXML.Element("StartPnt.Position.Y").Value));
                item.StartPnt.DockedFlag=Boolean.Parse(connectionXML.Element("StartPnt.DockedFlag").Value);
                item.StartPnt.RelatedShapeId=connectionXML.Element("StartPnt.RelatedShapeId").Value;
                item.StartPnt.RelatedType=(LinkNodeTypes) Enum.Parse(typeof(LinkNodeTypes),connectionXML.Element("StartPnt.RelatedType").Value);
                //EndPnt
                item.EndPnt.Position=new Point(Double.Parse(connectionXML.Element("EndPnt.Position.X").Value),
                    Double.Parse(connectionXML.Element("EndPnt.Position.Y").Value));
                item.EndPnt.DockedFlag=Boolean.Parse(connectionXML.Element("EndPnt.DockedFlag").Value);
                item.EndPnt.RelatedShapeId=connectionXML.Element("EndPnt.RelatedShapeId").Value;
                item.EndPnt.RelatedType=(LinkNodeTypes) Enum.Parse(typeof(LinkNodeTypes),connectionXML.Element("EndPnt.RelatedType").Value);
                item.ShapePnt.Clear();
                IEnumerable<XElement> shapePntsXML=connectionXML.Elements("ShapePnt.Positions").Elements("Position");
                foreach(XElement shapePntXML in shapePntsXML){
                    item.ShapePnt.Add(new Point(Double.Parse(shapePntXML.Element("X").Value),Double.Parse(shapePntXML.Element("Y").Value)));
                }
                AddShapes(item);                      
      
            }

            // update paste offset
            root.Attribute("OffsetX").Value = (offsetX + 10).ToString();
            root.Attribute("OffsetY").Value = (offsetY + 10).ToString();
            Clipboard.Clear();
            Clipboard.SetData(DataFormats.Xaml, root);        
          
          
             

        }

        private void Paste_Enabled(object sender,CanExecuteRoutedEventArgs e){
            e.CanExecute = Clipboard.ContainsData(DataFormats.Xaml);

        }
        #endregion

        #region 打开文件
        private void Open_Executed(object sender,ExecutedRoutedEventArgs e){
            XElement root = LoadSerializedDataFromFile();
            if (root == null)
                return;

            for (int i = 0; i < AllShapes.Count; i++)
            {
                viewer.Children.Remove((UserControl)AllShapes[i]);
            }

            AllShapes.Clear();
             
   
            IEnumerable<XElement> itemsXML = root.Elements("DesignerItems").Elements("DesignerItem");           

            foreach (XElement itemXML in itemsXML)
            {
                Guid newID = Guid.NewGuid();              
                ShapeBase item = DeserializeShapeItem(itemXML, newID, 0, 0);
                AddShapes(item);       
                          
            }                
   
            //创建连接
            IEnumerable<XElement> connectionsXML = root.Elements("Connections").Elements("Connection");
            foreach (XElement connectionXML in connectionsXML)
            {
                FlowChartTypes flowChartType = (FlowChartTypes)Enum.Parse(typeof(FlowChartTypes), connectionXML.Element("type").Value);
                LinkBase item = createLinkItem(flowChartType);
                item.Width = Double.Parse(connectionXML.Element("Width").Value, CultureInfo.InvariantCulture);
                item.Height = Double.Parse(connectionXML.Element("Height").Value, CultureInfo.InvariantCulture);
                item.Description = connectionXML.Element("description").Value;
                item.CloneSourceId = connectionXML.Element("cloneSourceId").Value;
                double left = Double.Parse(connectionXML.Element("Left").Value, CultureInfo.InvariantCulture);
                double top = Double.Parse(connectionXML.Element("Top").Value, CultureInfo.InvariantCulture);
                item.Margin = new Thickness(left, top, 0, 0);
                item.IsManualSetted=Boolean.Parse(connectionXML.Element("IsManualSetted").Value);
                item.StartPnt.Position=new Point(Double.Parse(connectionXML.Element("StartPnt.Position.X").Value),
                    Double.Parse(connectionXML.Element("StartPnt.Position.Y").Value));
                item.StartPnt.DockedFlag=Boolean.Parse(connectionXML.Element("StartPnt.DockedFlag").Value);
                item.StartPnt.RelatedShapeId=connectionXML.Element("StartPnt.RelatedShapeId").Value;
                item.StartPnt.RelatedType=(LinkNodeTypes) Enum.Parse(typeof(LinkNodeTypes),connectionXML.Element("StartPnt.RelatedType").Value);
                //EndPnt
                item.EndPnt.Position=new Point(Double.Parse(connectionXML.Element("EndPnt.Position.X").Value),
                    Double.Parse(connectionXML.Element("EndPnt.Position.Y").Value));
                item.EndPnt.DockedFlag=Boolean.Parse(connectionXML.Element("EndPnt.DockedFlag").Value);
                item.EndPnt.RelatedShapeId=connectionXML.Element("EndPnt.RelatedShapeId").Value;
                item.EndPnt.RelatedType=(LinkNodeTypes) Enum.Parse(typeof(LinkNodeTypes),connectionXML.Element("EndPnt.RelatedType").Value);
                item.ShapePnt.Clear();
                IEnumerable<XElement> shapePntsXML=connectionXML.Elements("ShapePnt.Positions").Elements("Position");
                foreach(XElement shapePntXML in shapePntsXML){
                    item.ShapePnt.Add(new Point(Double.Parse(shapePntXML.Element("X").Value),Double.Parse(shapePntXML.Element("Y").Value)));
                }
                AddShapes(item);   
            }
          
        }
           
        #endregion

        #region 保存文件
        private void Save_Executed(object sender,ExecutedRoutedEventArgs e){
            IEnumerable<ShapeBase> designerItems = this.AllShapes.OfType<ShapeBase>();
            IEnumerable<LinkBase> connections = this.AllShapes.OfType<LinkBase>();

            XElement designerItemsXML = SerializeShapeItems(designerItems);
            XElement connectionsXML = SerializeConnections(connections);

            XElement root = new XElement("Root");
            root.Add(designerItemsXML);
            root.Add(connectionsXML);

            SaveFile(root);
        }
        #endregion

       #region Helper Methods

         private XElement LoadSerializedDataFromClipBoard()
        {
            if (Clipboard.ContainsData(DataFormats.Xaml))
            {
                String clipboardData = Clipboard.GetData(DataFormats.Xaml) as String;

                if (String.IsNullOrEmpty(clipboardData))
                    return null;
                try
                {
                    return XElement.Load(new StringReader(clipboardData));
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.StackTrace, e.Message, MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

            return null;
        }

        private XElement LoadSerializedDataFromFile()
        {
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.Filter = "Designer Files (*.xml)|*.xml|All Files (*.*)|*.*";

            if (openFile.ShowDialog() == true)
            {
                try
                {
                    return XElement.Load(openFile.FileName);
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.StackTrace, e.Message, MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

            return null;
        }

        void SaveFile(XElement xElement)
        {
            SaveFileDialog saveFile = new SaveFileDialog();
            saveFile.Filter = "Files (*.xml)|*.xml|All Files (*.*)|*.*";
            if (saveFile.ShowDialog() == true)
            {
                try
                {
                    xElement.Save(saveFile.FileName);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.StackTrace, ex.Message, MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
       #endregion

        /// <summary>
        /// 显示流程详细属性
        /// </summary>
        /// <param name="properties"></param>
        public void DisplayShapeProperties(DataTable dt)
        {
            gridProperty.Children.Clear();
            gridProperty.RowDefinitions.Clear();

            if (null == dt)
            {
                return;
            }

            int total = dt.Rows.Count;
            int cnt = 0;
            int rowNo = 0;

            foreach (DataRow dr in dt.Rows)
            {
                cnt++;

                RowDefinition row = new RowDefinition();
                row.Height = new GridLength(25);
                gridProperty.RowDefinitions.Add(row);

                Label lblName = new Label();
                lblName.Content = Convert.ToString(dr[0].ToString());
                lblName.HorizontalContentAlignment = HorizontalAlignment.Center;
                lblName.HorizontalAlignment = HorizontalAlignment.Stretch;
                lblName.BorderBrush = new SolidColorBrush(Colors.Black);
                lblName.Height = 25;
                lblName.BorderThickness = (cnt == total) ? new Thickness(0.05, 0.05, 0.05, 0.1) : new Thickness(0.05);
                gridProperty.Children.Add(lblName);
                lblName.SetValue(Grid.RowProperty, rowNo);
                lblName.SetValue(Grid.ColumnProperty, 0);

                Label lblValue = new Label();
                lblValue.Content = Convert.ToString(dr[1].ToString());
                lblValue.HorizontalContentAlignment = HorizontalAlignment.Center;
                lblValue.HorizontalAlignment = HorizontalAlignment.Stretch;
                lblValue.BorderBrush = new SolidColorBrush(Colors.Black);
                lblValue.Height = 25;
                lblValue.BorderThickness = (cnt == total) ? new Thickness(0.05, 0.05, 0.05, 0.1) : new Thickness(0.05);
                gridProperty.Children.Add(lblValue);
                lblValue.SetValue(Grid.RowProperty, rowNo);
                lblValue.SetValue(Grid.ColumnProperty, 1);
                rowNo++;
            }
        }

      
        #region 当前选择的模型
        public static readonly DependencyProperty CurrentSelectedModelProperty = DependencyProperty.Register(
                    "CurrentSelectedModel",
                    typeof(String),
                    typeof(ModelingWorkSpace),
                    new PropertyMetadata("Model"));

        public String CurrentSelectedModel
        {
            get { return (String)GetValue(CurrentSelectedModelProperty); }
            set { 
                SetValue(CurrentSelectedModelProperty, value);
               
                if(PropertyChanged!=null){
                  
                    PropertyChanged(this,new PropertyChangedEventArgs("CurrentSelectedModel"));
                }
           
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion
    }
}
