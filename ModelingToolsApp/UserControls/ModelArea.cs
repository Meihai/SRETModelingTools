using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Xml;
using System.Collections.ObjectModel;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Data;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.ComponentModel;
using System.Collections;

using System.Xml.Linq;
using System.Globalization;
using ModelingToolsApp.UserControls;
using Microsoft.Win32;

namespace ModelingToolsApp.UserControls
{

    public partial class ModelArea : Canvas
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
        /// 当前工作区中所有已经添加的形状
        /// </summary>
        private ObservableCollection<IFlowChartBase> AllShapes = new ObservableCollection<IFlowChartBase>();

        /// <summary>
        /// 当前工作区所拥有的建模元素
        /// </summary>
        public ObservableCollection<IFlowChartBase> ModelItems
        {
            get { return AllShapes; }
            set { AllShapes = value;
                  //接收到数据后的进一步处理
                  // todo
                }
        }

        /// <summary>
        /// 当前工作区所属的父级建模工作区
        /// </summary>
        public ModelArea ParentModelArea
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


        private List<IFlowChartBase> CloneShapes = new List<IFlowChartBase>();

        private CtrlLine GCtrlLine;
        private CtrlShape GCtrlShape;
        private Rectangle GMoveShape;
        private DockNode GLinkNode0;
        private DockNode GLinkNode1;
        private DockNode GLinkNode2;
        private DockNode GLinkNode3;
        private Polyline tmpLine;
        private System.Windows.Shapes.Path tmpLinkPath;
        private CtrlMultiSelector GMultiSelector;
        private DescEditor GDescEditor;
        
        public ModelArea() {
           
            registerMouseEvent();
            init();

        }

        /// <summary>
        /// 注册鼠标事件
        /// </summary>
        private void registerMouseEvent() {
            this.DragEnter += canvasDragEnter;
            this.DragOver += canvasDragOver;
            this.DragLeave += canvasDragLeave;
            this.Drop += canvasDrop;
            this.MouseMove += canvasMouseMove;
            this.PreviewMouseDown += canvasPreviewMouseDown;
            this.MouseDown += canvasMouseDown;
        } 
        private void init() { 
             GCtrlLine=new CtrlLine();
             GCtrlLine.Visibility=Visibility.Hidden;
             this.Children.Add(GCtrlLine);

             GCtrlShape=new CtrlShape();
             GCtrlShape.Visibility=Visibility.Hidden;
             this.Children.Add(GCtrlShape);

             GMoveShape=new Rectangle();
             GMoveShape.Visibility=Visibility.Hidden;
             GMoveShape.Stroke=new SolidColorBrush(Colors.Lime);
             GMoveShape.StrokeDashArray=new DoubleCollection(){3,3};
             this.Children.Add(GMoveShape);
            
             GLinkNode0=new DockNode();
             GLinkNode0.Visibility=Visibility.Hidden;
             this.Children.Add(GLinkNode0);
             
             GLinkNode1=new DockNode();
             GLinkNode1.Visibility=Visibility.Hidden;
             this.Children.Add(GLinkNode1);

             GLinkNode2=new DockNode();
             GLinkNode2.Visibility=Visibility.Hidden;
             this.Children.Add(GLinkNode2);

             GLinkNode3=new DockNode();
             GLinkNode3.Visibility=Visibility.Hidden;
             this.Children.Add(GLinkNode3);
                                      
             tmpLine=new Polyline();
             tmpLine.Visibility=Visibility.Hidden;
             tmpLine.Stroke=new SolidColorBrush(Colors.Black);
             tmpLine.StrokeDashArray=new DoubleCollection(){3,3};
             this.Children.Add(tmpLine);

             tmpLinkPath=new System.Windows.Shapes.Path();
             tmpLinkPath.Visibility=Visibility.Hidden;
             tmpLinkPath.Stroke=new SolidColorBrush(Colors.Black);
             tmpLinkPath.StrokeDashArray=new DoubleCollection(){3,3};
             this.Children.Add(tmpLinkPath);
             
             GMultiSelector=new CtrlMultiSelector();
             this.Children.Add((UserControl)GMultiSelector);

             GDescEditor=new DescEditor();
             this.Children.Add((UserControl)GDescEditor);

             AllShapes.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(AllShapes_CollectionChanged);
             GlobalMouseHook.evtGlobalMouseUp += new delGlobalMouseUp(GlobalMouseHook_evtGlobalMouseUp);
             GCtrlShape.evtSizeChanged += new delSizeChanged(GCtrlShape_evtSizeChanged);
             GCtrlShape.evtPrepareToChangePosition += new delPrepareToChangePosition(GCtrlShape_evtPrepareToChangePosition);
             GCtrlShape.evtPositionChanged += new delPositionChanged(GCtrlShape_evtPositionChanged);
             GCtrlLine.evtTerminalPointMoved += new delTerminalPointMoved(GCtrlLine_evtTerminalPointMoved);
             // menuMain.evtMainMenuClick += new delMainMenuClick(menuMain_evtMainMenuClick);
                       

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
            
             Clipboard.Clear();
        }


        #region Delete Command

        public void Delete_Executed(object sender, ExecutedRoutedEventArgs e)
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


        public void Delete_Enabled(object sender, CanExecuteRoutedEventArgs e)
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
            e.CanExecute = cnt>0;
        }
        #endregion

        #region Copy Command
        public void Copy_Executed(object sender, ExecutedRoutedEventArgs e)
        {
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
                    }
                    else if (AllShapes[i] is LinkBase)
                    {
                        linkBaseList.Add((LinkBase)AllShapes[i]);
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

        private void Save2XmlFile(ModelArea modelArea, string fileName)
        {
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.AppendChild(xmlDocument.CreateXmlDeclaration("1.0", "utf-8", "yes"));
            XmlElement xmlRoot = xmlDocument.CreateElement(string.Empty, "ModelArea", string.Empty);
            SerializeModelItems(modelArea, xmlDocument, xmlRoot);
            xmlDocument.AppendChild(xmlRoot);
            xmlDocument.Save(fileName);
        }

        private void SerializeModelItems(ModelArea modelArea, XmlDocument xmlDocument, XmlElement xmlElement)
        {   
            
            IEnumerable<ShapeBase> designerItems = modelArea.AllShapes.OfType<ShapeBase>();
            IEnumerable<LinkBase> connections = modelArea.AllShapes.OfType<LinkBase>();
            XmlElement designerItemsXml=xmlDocument.CreateElement(string.Empty,"DesignerItems",string.Empty);
            foreach(ShapeBase shapeBase in designerItems){
                XmlElement designerItemXml=xmlDocument.CreateElement(string.Empty,"DesignerItem",string.Empty);
                if(shapeBase.ChildModelArea!=null){
                    XmlElement childModelAreaXml=xmlDocument.CreateElement(string.Empty,"ChildModelArea",string.Empty);
                    SerializeModelItems(shapeBase.ChildModelArea,xmlDocument,childModelAreaXml);
                    designerItemXml.AppendChild(childModelAreaXml);
                }
                XmlElement leftXml=xmlDocument.CreateElement(string.Empty,"Left",string.Empty);
                leftXml.InnerText=(shapeBase.Margin.Left.ToString());
                designerItemXml.AppendChild(leftXml);

                XmlElement topXml=xmlDocument.CreateElement(string.Empty,"Top",string.Empty);
                topXml.InnerText=(shapeBase.Margin.Top.ToString());
                designerItemXml.AppendChild(topXml);

                XmlElement widthXml=xmlDocument.CreateElement(string.Empty,"Width",string.Empty);
                widthXml.InnerText=(shapeBase.Width.ToString());
                designerItemXml.AppendChild(widthXml);

                XmlElement heightXml=xmlDocument.CreateElement(string.Empty,"Height",string.Empty);
                heightXml.InnerText=(shapeBase.Height.ToString());
                designerItemXml.AppendChild(heightXml);

                XmlElement IdXml=xmlDocument.CreateElement(string.Empty,"ID",string.Empty);
                IdXml.InnerText = (shapeBase.Id.ToString());
                designerItemXml.AppendChild(IdXml);

                XmlElement zIndexXml=xmlDocument.CreateElement(string.Empty,"zIndex",string.Empty);
                zIndexXml.InnerText = (Canvas.GetZIndex(shapeBase).ToString());
                designerItemXml.AppendChild(zIndexXml);

                XmlElement typeXml=xmlDocument.CreateElement(string.Empty,"type",string.Empty);
                typeXml.InnerText = (shapeBase.FlowChartType.ToString());
                designerItemXml.AppendChild(typeXml);

                XmlElement descriptionXml=xmlDocument.CreateElement(string.Empty,"description",string.Empty);
                descriptionXml.InnerText = (shapeBase.Description);
                designerItemXml.AppendChild(descriptionXml);
              
                XmlElement cloneSourceIdXml=xmlDocument.CreateElement(string.Empty,"cloneSourceId",string.Empty);
                cloneSourceIdXml.InnerText = (shapeBase.CloneSourceId);
                designerItemXml.AppendChild(cloneSourceIdXml);
                
                designerItemsXml.AppendChild(designerItemXml);
            }
            XmlElement connectionItemsXml = xmlDocument.CreateElement(string.Empty, "Connections", string.Empty);
            foreach (LinkBase linkBase in connections) {
                XmlElement connectionXml = xmlDocument.CreateElement(string.Empty, "Connection", string.Empty);

                XmlElement IdXml=xmlDocument.CreateElement(string.Empty,"ID",string.Empty);
                IdXml.InnerText = (linkBase.Id);
                connectionXml.AppendChild(IdXml);

                XmlElement leftXml=xmlDocument.CreateElement(string.Empty,"Left",string.Empty);
                leftXml.InnerText = (linkBase.Margin.Left.ToString());
                connectionXml.AppendChild(leftXml);

                XmlElement topXml=xmlDocument.CreateElement(string.Empty,"Top",string.Empty);
                topXml.InnerText = (linkBase.Margin.Top.ToString());
                connectionXml.AppendChild(topXml);

                XmlElement widthXml=xmlDocument.CreateElement(string.Empty,"Width",string.Empty);
                widthXml.InnerText = (linkBase.Width.ToString());
                connectionXml.AppendChild(widthXml);

                XmlElement heightXml=xmlDocument.CreateElement(string.Empty,"Height",string.Empty);
                heightXml.InnerText = (linkBase.Height.ToString());
                connectionXml.AppendChild(heightXml);             

                XmlElement typeXml=xmlDocument.CreateElement(string.Empty,"type",string.Empty);
                typeXml.InnerText = (linkBase.FlowChartType.ToString());
                connectionXml.AppendChild(typeXml);

                XmlElement IsManualSettedXml=xmlDocument.CreateElement(string.Empty,"IsManualSetted",string.Empty);
                IsManualSettedXml.InnerText = (linkBase.IsManualSetted.ToString());
                connectionXml.AppendChild(IsManualSettedXml);

                XmlElement descriptionXml=xmlDocument.CreateElement(string.Empty,"description",string.Empty);
                descriptionXml.InnerText = (linkBase.Description);
                connectionXml.AppendChild(descriptionXml);
              
                XmlElement cloneSourceIdXml=xmlDocument.CreateElement(string.Empty,"cloneSourceId",string.Empty);
                cloneSourceIdXml.InnerText = (linkBase.CloneSourceId);
                connectionXml.AppendChild(cloneSourceIdXml);
                //起始点坐标序列化
                XmlElement StartPnt_Position_X_Xml=xmlDocument.CreateElement(string.Empty,"StartPnt.Position.X",string.Empty);
                StartPnt_Position_X_Xml.InnerText = (linkBase.StartPnt.Position.X.ToString());
                connectionXml.AppendChild(StartPnt_Position_X_Xml);

                XmlElement StartPnt_Position_Y_Xml=xmlDocument.CreateElement(string.Empty,"StartPnt.Position.Y",string.Empty);
                StartPnt_Position_Y_Xml.InnerText = (linkBase.StartPnt.Position.Y.ToString());
                connectionXml.AppendChild(StartPnt_Position_Y_Xml);

                XmlElement StartPnt_DockedFlag_Xml=xmlDocument.CreateElement(string.Empty,"StartPnt.DockedFlag",string.Empty);
                StartPnt_DockedFlag_Xml.InnerText = (linkBase.StartPnt.DockedFlag.ToString());
                connectionXml.AppendChild(StartPnt_DockedFlag_Xml);

                XmlElement StartPnt_RelatedShapeId_Xml=xmlDocument.CreateElement(string.Empty,"StartPnt.RelatedShapeId",string.Empty);
                StartPnt_RelatedShapeId_Xml.InnerText = (linkBase.StartPnt.RelatedShapeId);
                connectionXml.AppendChild(StartPnt_RelatedShapeId_Xml);

                XmlElement StartPnt_RelatedType_Xml=xmlDocument.CreateElement(string.Empty,"StartPnt.RelatedType",string.Empty);
                StartPnt_RelatedType_Xml.InnerText = (linkBase.StartPnt.RelatedType.ToString());
                connectionXml.AppendChild(StartPnt_RelatedType_Xml);

                //终点坐标序列化
                XmlElement EndPnt_Position_X_Xml=xmlDocument.CreateElement(string.Empty,"EndPnt.Position.X",string.Empty);
                EndPnt_Position_X_Xml.InnerText = (linkBase.EndPnt.Position.X.ToString());
                connectionXml.AppendChild(EndPnt_Position_X_Xml);

                XmlElement EndPnt_Position_Y_Xml=xmlDocument.CreateElement(string.Empty,"EndPnt.Position.Y",string.Empty);
                EndPnt_Position_Y_Xml.InnerText = (linkBase.EndPnt.Position.Y.ToString());
                connectionXml.AppendChild(EndPnt_Position_Y_Xml);

                XmlElement EndPnt_DockedFlag_Xml=xmlDocument.CreateElement(string.Empty,"EndPnt.DockedFlag",string.Empty);
                EndPnt_DockedFlag_Xml.InnerText = (linkBase.EndPnt.DockedFlag.ToString());
                connectionXml.AppendChild(EndPnt_DockedFlag_Xml);

                XmlElement EndPnt_RelatedShapeId_Xml=xmlDocument.CreateElement(string.Empty,"EndPnt.RelatedShapeId",string.Empty);
                EndPnt_RelatedShapeId_Xml.InnerText = (linkBase.EndPnt.RelatedShapeId);
                connectionXml.AppendChild(EndPnt_RelatedShapeId_Xml);

                XmlElement EndPnt_RelatedType_Xml=xmlDocument.CreateElement(string.Empty,"EndPnt.RelatedType",string.Empty);
                EndPnt_RelatedType_Xml.InnerText = (linkBase.EndPnt.RelatedType.ToString());
                connectionXml.AppendChild(EndPnt_RelatedType_Xml);

                XmlElement ShapePnt_Count_Xml=xmlDocument.CreateElement(string.Empty,"ShapePnt.Count",string.Empty);
                ShapePnt_Count_Xml.InnerText = (linkBase.ShapePnt.Count.ToString());
                connectionXml.AppendChild(ShapePnt_Count_Xml);

                XmlElement ShapePnt_Positions_Xml=xmlDocument.CreateElement(string.Empty,"ShapePnt.Positions",string.Empty);
                foreach(Point position in linkBase.ShapePnt){
                    XmlElement positionXml=xmlDocument.CreateElement(string.Empty,"Position",string.Empty);
                    XmlElement positionXXml=xmlDocument.CreateElement(string.Empty,"X",string.Empty);
                    XmlElement positionYXml=xmlDocument.CreateElement(string.Empty,"Y",string.Empty);
                    positionXXml.InnerText = position.X.ToString();
                    positionYXml.InnerText = position.Y.ToString();
                    positionXml.AppendChild(positionXXml);
                    positionXml.AppendChild(positionYXml);
                    ShapePnt_Positions_Xml.AppendChild(positionXml);
                }
                connectionXml.AppendChild(ShapePnt_Positions_Xml);
                connectionItemsXml.AppendChild(connectionXml);              
            }
            xmlElement.AppendChild(designerItemsXml);
            xmlElement.AppendChild(connectionItemsXml);           
        }

        private void LoadXml(ModelArea modelArea, string fileName)
        {
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(fileName);
            XmlElement xmlRoot = xmlDocument.DocumentElement;
            LoadChildNode(modelArea, xmlRoot);

        }

        private void LoadChildNode(ModelArea modelArea, XmlElement xmlElement)
        {
            XmlNodeList xmlNodeList = xmlElement.ChildNodes;//根结点,包括DesignerItems和Connections

            foreach (XmlElement xe in xmlNodeList)   //遍历模块节点
            {
                if (xe.Name == "DesignerItems")
                {
                    XmlNodeList designerItemNodeList = xe.ChildNodes;
                    foreach (XmlElement designeritemXe in designerItemNodeList)
                    {
                        XmlNodeList attributeItemNodeList = designeritemXe.ChildNodes;
                        double sbleft=0;
                        double sbTop=0;
                        double sbWidth=100;
                        double sbHeight=40; 
                        string sbId="";
                        int sbzIndex=0;                         
                        string sbDescription="";
                        string sbcloneSourceId="";
                        ShapeBase sbTemp = null;
                        ModelArea sbModelArea = null;
                        
                        foreach (XmlElement attributeXe in attributeItemNodeList)
                        {
                            if (attributeXe.Name == "ChildModelArea")
                            {
                                sbModelArea = new ModelArea();
                                LoadChildNode(sbModelArea, attributeXe);
                            }
                            else if (attributeXe.Name == "Left")
                            {
                                sbleft = Double.Parse(attributeXe.InnerText);
                            }
                            else if (attributeXe.Name == "Top")
                            {
                                sbTop = Double.Parse(attributeXe.InnerText);
                            }
                            else if (attributeXe.Name == "Width")
                            {
                                sbWidth = Double.Parse(attributeXe.InnerText);
                            }
                            else if (attributeXe.Name == "Height")
                            {
                                sbHeight = Double.Parse(attributeXe.InnerText);
                            }
                            else if (attributeXe.Name == "ID")
                            {
                                sbId = attributeXe.InnerText;
                            }
                            else if (attributeXe.Name == "zIndex")
                            {
                                sbzIndex = int.Parse(attributeXe.InnerText);
                            }
                            else if (attributeXe.Name == "type")
                            {
                                sbTemp = (ShapeBase)getFlowChartBase(attributeXe.InnerText);
                                
                            }
                            else if (attributeXe.Name == "description")
                            {
                                sbDescription = attributeXe.InnerText;
                            }
                            else if (attributeXe.Name == "cloneSourceId")
                            {
                                sbcloneSourceId = attributeXe.InnerText;
                            }
                        }
                        if (sbTemp != null)
                        {
                             sbTemp.Width = sbWidth;
                             sbTemp.Height = sbHeight;
                             sbTemp.Description = sbDescription;
                             sbTemp.CloneSourceId = sbcloneSourceId;            
                             sbTemp.Margin = new Thickness(sbleft, sbTop, 0, 0);
                             sbTemp.ChildModelArea = sbModelArea;
                             sbTemp.Id = sbId;
                             //添加到AllShape中
                             modelArea.AddShapes(sbTemp);
                       }                        

                    }
                }
                else if (xe.Name == "Connections") {
                    XmlNodeList connectionItemNodeList = xe.ChildNodes;
                    foreach (XmlElement connectionXe in connectionItemNodeList)
                    {
                        LinkBase connection = getLinkBaseFromXml(connectionXe);
                        //添加连线到形状中
                        if (connection != null)
                        {
                            connection.CreateShape();
                            modelArea.AddShapes(connection);
                            
                        }
                      
                    }
                }
              
            }
        }


        private LinkBase getLinkBaseFromXml(XmlElement connectionXml)
        {
            XmlNodeList attributeItemNodeList = connectionXml.ChildNodes;
            double sbleft = 0;
            double sbTop = 0;
            double sbWidth = 100;
            double sbHeight = 40;
            string sbId = "";           
            string sbDescription = "";
            string sbcloneSourceId = "";

            double sbStartPnt_Position_X = 0;
            double sbStartPnt_Position_Y = 0;
            bool sbStartPnt_DockedFlag=false;
            string sbStartPnt_RelatedShapeId = "";
            string sbStartPnt_RelatedType = "";
            double sbEndPnt_Position_X = 0;
            double sbEndPnt_Position_Y = 0;
            bool sbEndPnt_DockedFlag = false;
            string sbEndPnt_RelatedShapeId = "";
            string sbEndPnt_RelatedType = "";
            int sbShapePnt_Count=0;
            PointCollection sbShapePnt = new PointCollection();

            LinkBase sbTemp = null;            

            foreach (XmlElement attributeXe in attributeItemNodeList)
            {               
                if (attributeXe.Name == "Left")
                {
                    sbleft = Double.Parse(attributeXe.InnerText);
                }
                else if (attributeXe.Name == "Top")
                {
                    sbTop = Double.Parse(attributeXe.InnerText);
                }
                else if (attributeXe.Name == "Width")
                {
                    sbWidth = Double.Parse(attributeXe.InnerText);
                }
                else if (attributeXe.Name == "Height")
                {
                    sbHeight = Double.Parse(attributeXe.InnerText);
                }
                else if (attributeXe.Name == "ID")
                {
                    sbId = attributeXe.InnerText;
                }
                
                else if (attributeXe.Name == "type")
                {
                    sbTemp = (LinkBase)getFlowChartBase(attributeXe.InnerText);

                }
                else if (attributeXe.Name == "description")
                {
                    sbDescription = attributeXe.InnerText;
                }
                else if (attributeXe.Name == "cloneSourceId")
                {
                    sbcloneSourceId = attributeXe.InnerText;
                }
                else if (attributeXe.Name == "StartPnt.Position.X") {
                    sbStartPnt_Position_X = Double.Parse(attributeXe.InnerText);
                }
                else if (attributeXe.Name == "StartPnt.Position.Y")
                {
                    sbStartPnt_Position_Y = Double.Parse(attributeXe.InnerText);
                }
                else if (attributeXe.Name == "StartPnt.DockedFlag")
                {
                    sbStartPnt_DockedFlag = bool.Parse(attributeXe.InnerText);
                }
                else if (attributeXe.Name == "StartPnt.RelatedShapeId")
                {
                    sbStartPnt_RelatedShapeId = attributeXe.InnerText;
                }
                else if (attributeXe.Name == "StartPnt.RelatedType")
                {
                    sbStartPnt_RelatedType = attributeXe.InnerText;
                }

                else if (attributeXe.Name == "EndPnt.Position.X")
                {
                    sbEndPnt_Position_X = Double.Parse(attributeXe.InnerText);
                }
                else if (attributeXe.Name == "EndPnt.Position.Y")
                {
                    sbEndPnt_Position_Y = Double.Parse(attributeXe.InnerText);
                }
                else if (attributeXe.Name == "EndPnt.DockedFlag")
                {
                    sbEndPnt_DockedFlag = bool.Parse(attributeXe.InnerText);
                }
                else if (attributeXe.Name == "EndPnt.RelatedShapeId")
                {
                    sbEndPnt_RelatedShapeId = attributeXe.InnerText;
                }
                else if (attributeXe.Name == "EndPnt.RelatedType")
                {
                    sbEndPnt_RelatedType = attributeXe.InnerText;
                }
                else if (attributeXe.Name == "ShapePnt.Count")
                {
                    sbShapePnt_Count = int.Parse(attributeXe.InnerText);
                }
                else if (attributeXe.Name == "ShapePnt.Positions")
                {
                    XmlNodeList positionNodeList = attributeXe.ChildNodes;
                    foreach (XmlElement positionXe in positionNodeList)
                    {
                        XmlNodeList positionInfoNodeList = positionXe.ChildNodes;
                        double positionx = 0;
                        double positiony = 0;
                        foreach (XmlElement positionInfoXe in positionInfoNodeList)
                        {
                         
                            if (positionInfoXe.Name == "X") {
                                positionx = Double.Parse(positionInfoXe.InnerText);

                            }
                            else if (positionInfoXe.Name == "Y")
                            {
                                positiony = Double.Parse(positionInfoXe.InnerText);
                            }
                        }
                        sbShapePnt.Add(new Point(positionx, positiony));
                    }
                }

            }
            if (sbTemp != null)
            {
                sbTemp.Id = sbId;
                sbTemp.Width = sbWidth;
                sbTemp.Height = sbHeight;
                sbTemp.Description = sbDescription;
                sbTemp.CloneSourceId = sbcloneSourceId;
                sbTemp.Margin = new Thickness(sbleft, sbTop, 0, 0);
                sbTemp.StartPnt = new LineTerminalPoint(new Point(sbStartPnt_Position_X,sbStartPnt_Position_Y));
                sbTemp.StartPnt.DockedFlag = sbStartPnt_DockedFlag;
                sbTemp.StartPnt.RelatedShapeId = sbStartPnt_RelatedShapeId;
                sbTemp.StartPnt.RelatedType = (LinkNodeTypes)Enum.Parse(typeof(LinkNodeTypes), sbStartPnt_RelatedType);

                sbTemp.EndPnt = new LineTerminalPoint(new Point(sbEndPnt_Position_X, sbEndPnt_Position_Y));
                sbTemp.EndPnt.DockedFlag = sbEndPnt_DockedFlag;
                sbTemp.EndPnt.RelatedShapeId = sbEndPnt_RelatedShapeId;
                sbTemp.EndPnt.RelatedType = (LinkNodeTypes)Enum.Parse(typeof(LinkNodeTypes), sbEndPnt_RelatedType);
                sbTemp.ShapePnt = sbShapePnt;
           
            }
            return sbTemp;         
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
                                                  new XElement("type", item.FlowChartType),
                                                  new XElement("description", item.Description),
                                                  new XElement("cloneSourceId", item.CloneSourceId),
                                                  new XElement("Content", contentXaml)
                                              )
                                   );

            return serializedItems;
        }



        private ShapeBase DeserializeShapeItem(XElement itemXML, Guid id, double OffsetX, double OffsetY)
        {
            FlowChartTypes flowChartType = (FlowChartTypes)Enum.Parse(typeof(FlowChartTypes), itemXML.Element("type").Value);
            ShapeBase item = createShapeItem(flowChartType);
            item.Width = Double.Parse(itemXML.Element("Width").Value, CultureInfo.InvariantCulture);
            item.Height = Double.Parse(itemXML.Element("Height").Value, CultureInfo.InvariantCulture);
            item.Description = itemXML.Element("description").Value;
            item.CloneSourceId = itemXML.Element("cloneSourceId").Value;
            double left = Double.Parse(itemXML.Element("Left").Value, CultureInfo.InvariantCulture) + OffsetX;
            double top = Double.Parse(itemXML.Element("Top").Value, CultureInfo.InvariantCulture) + OffsetY;
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
                                      new XElement("type", connection.FlowChartType),
                                      new XElement("IsManualSetted", connection.IsManualSetted),
                                      new XElement("description", connection.Description),
                                      new XElement("cloneSourceId", connection.CloneSourceId),
                                      new XElement("StartPnt.Position.X", connection.StartPnt.Position.X),
                                      new XElement("StartPnt.Position.Y", connection.StartPnt.Position.Y),
                                      new XElement("StartPnt.DockedFlag", connection.StartPnt.DockedFlag),
                                      new XElement("StartPnt.RelatedShapeId", connection.StartPnt.RelatedShapeId),
                                      new XElement("StartPnt.RelatedType", connection.StartPnt.RelatedType),
                               //
                                      new XElement("EndPnt.Position.X", connection.EndPnt.Position.X),
                                      new XElement("EndPnt.Position.Y", connection.EndPnt.Position.Y),
                                      new XElement("EndPnt.DockedFlag", connection.EndPnt.DockedFlag),
                                      new XElement("EndPnt.RelatedShapeId", connection.EndPnt.RelatedShapeId),
                                      new XElement("EndPnt.RelatedType", connection.EndPnt.RelatedType),
                               //-----
                                      new XElement("ShapePnt.Count", connection.ShapePnt.Count),
                                      new XElement("ShapePnt.Positions",
                                             from position in connection.ShapePnt
                                             select new XElement("Position",
                                                     new XElement("X", position.X),
                                                     new XElement("Y", position.Y)
                                                     )
                                              )
                                     )
                                  );



            return serializedConnections;
        }


        private ShapeBase createShapeItem(FlowChartTypes flowChartType)
        {
            ShapeBase shapeBase = null;
            switch (flowChartType)
            {
                case FlowChartTypes.ShapeEntrance:
                    {
                        shapeBase = new ShapeEntrance();
                        break;
                    }
                case FlowChartTypes.ShapeExit:
                    {
                        shapeBase = new ShapeExit();
                        break;
                    }
                case FlowChartTypes.ShapeTerminal:
                    {
                        shapeBase = new ShapeTerminal();
                        break;
                    }
                case FlowChartTypes.ShapeReliableProfile:
                    {
                        shapeBase = new ShapeReliableProfile();
                        break;
                    }
                case FlowChartTypes.ShapeOperation:
                    {
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
                        //linkBase = new LinkBroken(LineTypes.Solid);
                        linkBase=new LinkSeqTransfer(LineTypes.Solid);
                        break;
                    }
                case FlowChartTypes.LinkBroken:
                    {
                        linkBase = new LinkBroken(LineTypes.Solid);
                        break;
                    }
                case FlowChartTypes.LinkStraight:
                    {
                        linkBase = new LinkStraight(LineTypes.ShortDashes);
                        break;
                    }
                default:
                    MessageBox.Show("不支持的连线类型");
                    break;
            }
            if (linkBase != null)
            {
                linkBase.CreateShape();
            }
            return linkBase;
        }

        public void Copy_Enabled(object sender, CanExecuteRoutedEventArgs e)
        {
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


        public void Paste_Executed(object sender, ExecutedRoutedEventArgs e)
        {

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
                item.IsManualSetted = Boolean.Parse(connectionXML.Element("IsManualSetted").Value);
                item.StartPnt.Position = new Point(Double.Parse(connectionXML.Element("StartPnt.Position.X").Value),
                    Double.Parse(connectionXML.Element("StartPnt.Position.Y").Value));
                item.StartPnt.DockedFlag = Boolean.Parse(connectionXML.Element("StartPnt.DockedFlag").Value);
                item.StartPnt.RelatedShapeId = connectionXML.Element("StartPnt.RelatedShapeId").Value;
                item.StartPnt.RelatedType = (LinkNodeTypes)Enum.Parse(typeof(LinkNodeTypes), connectionXML.Element("StartPnt.RelatedType").Value);
                //EndPnt
                item.EndPnt.Position = new Point(Double.Parse(connectionXML.Element("EndPnt.Position.X").Value),
                    Double.Parse(connectionXML.Element("EndPnt.Position.Y").Value));
                item.EndPnt.DockedFlag = Boolean.Parse(connectionXML.Element("EndPnt.DockedFlag").Value);
                item.EndPnt.RelatedShapeId = connectionXML.Element("EndPnt.RelatedShapeId").Value;
                item.EndPnt.RelatedType = (LinkNodeTypes)Enum.Parse(typeof(LinkNodeTypes), connectionXML.Element("EndPnt.RelatedType").Value);
                item.ShapePnt.Clear();
                IEnumerable<XElement> shapePntsXML = connectionXML.Elements("ShapePnt.Positions").Elements("Position");
                foreach (XElement shapePntXML in shapePntsXML)
                {
                    item.ShapePnt.Add(new Point(Double.Parse(shapePntXML.Element("X").Value), Double.Parse(shapePntXML.Element("Y").Value)));
                }
                AddShapes(item);

            }

            // update paste offset
            root.Attribute("OffsetX").Value = (offsetX + 10).ToString();
            root.Attribute("OffsetY").Value = (offsetY + 10).ToString();
            Clipboard.Clear();
            Clipboard.SetData(DataFormats.Xaml, root);




        }

        public void Paste_Enabled(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = Clipboard.ContainsData(DataFormats.Xaml);

        }
        #endregion

        void GCtrlLine_evtTerminalPointMoved(Point point, int seq)
        {
            for (int i = 0; i < AllShapes.Count; i++)
            {
                AllShapes[i].AcceptPointMove(point, seq);
            }
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


        void GCtrlShape_evtPositionChanged(double left, double top)
        {

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
            if (BelongToModelItem != null)
            {
               if (ParentModelArea != null) {
                    for (int j = 0; j < ParentModelArea.ModelItems.Count; j++) {
                        if (ParentModelArea.ModelItems[j] is ShapeBase) {
                            if (ParentModelArea.ModelItems[j].CloneSourceId== BelongToModelItem.CloneSourceId) {
                                ShapeBase sb = ParentModelArea.ModelItems[j] as ShapeBase;
                                sb.ChildModelArea = this;                              
                            } 
                        }
                    }
                }
                
            }

            RoadSearch.AllShapes = AllShapes;
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
        IFlowChartBase DragIfcb = null;
        private void canvasDragEnter(object sender, System.Windows.DragEventArgs e)
         {
            if (null == DragIfcb)
            {
                switch ((FlowChartTypes)(e.Data.GetData(typeof(FlowChartTypes))))
                {
                   
                    case FlowChartTypes.ShapeSeqTransfer: {
                        //DragIfcb = new LinkBroken(LineTypes.Solid);
                        DragIfcb = new LinkSeqTransfer(LineTypes.Solid);
                        DragIfcb.CreateShape();
                        break;
                    }
                    case FlowChartTypes.ShapeProbTransfer:
                        {
                            DragIfcb = new LinkStraight(LineTypes.ShortDashes);
                            DragIfcb.CreateShape();
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

        private void canvasDragOver(object sender, System.Windows.DragEventArgs e)
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

        private void canvasDragLeave(object sender, System.Windows.DragEventArgs e)
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

        private void canvasDrop(object sender, System.Windows.DragEventArgs e)
        {
            if (null != DragIfcb)
            {
                this.Children.Remove((System.Windows.Controls.UserControl)DragIfcb);
                AddShapes(DragIfcb);
                DragIfcb = null;
            }
        }
        private void canvasMouseMove(object sender, MouseEventArgs e)
        {
            if (Mouse.LeftButton != MouseButtonState.Pressed)
            {
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

        private void canvasPreviewMouseDown(object sender, MouseButtonEventArgs e){        
             Offset = e.GetPosition(this);
             IsStartMove = false;       
        }
        private void canvasMouseDown(object sender, MouseButtonEventArgs e)
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
            
            this.Children.Add((UserControl)ifcb);            
           
            ifcb.IsSelected = true;
            ifcb.ChangePositionAndSize(0, 0, 0, 0);
        }

        public void MenuItem_Click_ColorSelection(object sender, RoutedEventArgs e)
        {
            linkBaseArray = new ArrayList();
            for (int i = 0; i < AllShapes.Count; i++)
            {
                if (AllShapes[i].IsSelected || AllShapes[i].IsMultiSelected)
                {
                    if (AllShapes[i] is LinkBase)
                    {
                        linkBaseArray.Add(i);
                    }
                }
            }
            if (linkBaseArray.Count == 0)
            {
                MessageBox.Show("未选择连接线");
                return;
            }

            ColorSelectionWindow colorWindow = new ColorSelectionWindow();

            Brush brush = new SolidColorBrush(Colors.Black);

            colorWindow.ExSelectedColor = (Color)ColorConverter.ConvertFromString(brush.ToString());
            colorWindow.ColorChangedEvent += evtColorChangeEvent;
            colorWindow.ShowDialog();

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
            this.FlowChartId = newFlowChartId;

            for (int i = 0; i < AllShapes.Count; i++)
            {
                this.Children.Remove((UserControl)AllShapes[i]);
            }

            AllShapes.Clear();
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
                int j = (int)linkBaseArray[i];
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

        void ifcb_evtOpenSubModelArea(ShapeBase modelItem)
        {
            if (modelItem.FlowChartType == FlowChartTypes.ShapeReliableProfile)
            {
                ModelingSubWindowTemplate subModelingWindow = new ModelingSubWindowTemplate();
                if (modelItem.ChildModelArea!=null)
                {
                    ObservableCollection<IFlowChartBase> ModelItemsTemp=modelItem.ChildModelArea.ModelItems;

                    for (int k = 0; k < ModelItemsTemp.Count; k++) {
                        
                        modelItem.ChildModelArea.Children.Remove((UserControl)ModelItemsTemp[k]);
                       
                        subModelingWindow.viewer.AddShapes(ModelItemsTemp[k]);
                    }

                    //重新设置viewer引用
                    for (int kk = 0; kk < ModelItems.Count; kk++)
                    {   
                        if(ModelItems[kk].Id== modelItem.CloneSourceId){
                            ((ShapeBase)ModelItems[kk]).ChildModelArea= subModelingWindow.viewer;
                        }
                    }
                       
                }
                subModelingWindow.viewer.BelongToModelItem = modelItem;
                subModelingWindow.viewer.ParentModelArea = this;
              
              
                subModelingWindow.ShowDialog();
            }
            else {
                MessageBox.Show("当前模型元素不支持递归建模!");
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
                        ///如果子流程为剖面子流程
                        ((ShapeBase)icb).evtOpenSubModelArea += new delOpenSubModelArea(ifcb_evtOpenSubModelArea);
                    }
                    CloneShapes.Add(icb);
                    this.Children.Add((UserControl)icb);
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
            Save2XmlFile(this, "test190714.xml");
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


        #region 打开文件
        public void Open_Executed(object sender, ExecutedRoutedEventArgs e)
        {

            //XElement root = LoadSerializedDataFromFile();
            //if (root == null)
            //    return;

            OpenFileDialog openFile = new OpenFileDialog();
            openFile.Filter = "Designer Files (*.xml)|*.xml|All Files (*.*)|*.*";

            if (openFile.ShowDialog() == true)
            {
                try
                {
                    for (int i = 0; i < AllShapes.Count; i++)
                    {
                        this.Children.Remove((UserControl)AllShapes[i]);
                    }

                    AllShapes.Clear();
                    LoadXml(this, openFile.FileName);
                    //return XElement.Load(openFile.FileName);
                }catch (Exception ex)
                {
                    MessageBox.Show(ex.StackTrace, ex.Message, MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

            

          
           
           
            //IEnumerable<XElement> itemsXML = root.Elements("DesignerItems").Elements("DesignerItem");

            //foreach (XElement itemXML in itemsXML)
            //{
            //    Guid newID = Guid.NewGuid();
            //    ShapeBase item = DeserializeShapeItem(itemXML, newID, 0, 0);
            //    AddShapes(item);

            //}

            ////创建连接
            //IEnumerable<XElement> connectionsXML = root.Elements("Connections").Elements("Connection");
            //foreach (XElement connectionXML in connectionsXML)
            //{
            //    FlowChartTypes flowChartType = (FlowChartTypes)Enum.Parse(typeof(FlowChartTypes), connectionXML.Element("type").Value);
            //    LinkBase item = createLinkItem(flowChartType);
            //    item.Width = Double.Parse(connectionXML.Element("Width").Value, CultureInfo.InvariantCulture);
            //    item.Height = Double.Parse(connectionXML.Element("Height").Value, CultureInfo.InvariantCulture);
            //    item.Description = connectionXML.Element("description").Value;
            //    item.CloneSourceId = connectionXML.Element("cloneSourceId").Value;
            //    double left = Double.Parse(connectionXML.Element("Left").Value, CultureInfo.InvariantCulture);
            //    double top = Double.Parse(connectionXML.Element("Top").Value, CultureInfo.InvariantCulture);
            //    item.Margin = new Thickness(left, top, 0, 0);
            //    item.IsManualSetted = Boolean.Parse(connectionXML.Element("IsManualSetted").Value);
            //    item.StartPnt.Position = new Point(Double.Parse(connectionXML.Element("StartPnt.Position.X").Value),
            //        Double.Parse(connectionXML.Element("StartPnt.Position.Y").Value));
            //    item.StartPnt.DockedFlag = Boolean.Parse(connectionXML.Element("StartPnt.DockedFlag").Value);
            //    item.StartPnt.RelatedShapeId = connectionXML.Element("StartPnt.RelatedShapeId").Value;
            //    item.StartPnt.RelatedType = (LinkNodeTypes)Enum.Parse(typeof(LinkNodeTypes), connectionXML.Element("StartPnt.RelatedType").Value);
            //    //EndPnt
            //    item.EndPnt.Position = new Point(Double.Parse(connectionXML.Element("EndPnt.Position.X").Value),
            //        Double.Parse(connectionXML.Element("EndPnt.Position.Y").Value));
            //    item.EndPnt.DockedFlag = Boolean.Parse(connectionXML.Element("EndPnt.DockedFlag").Value);
            //    item.EndPnt.RelatedShapeId = connectionXML.Element("EndPnt.RelatedShapeId").Value;
            //    item.EndPnt.RelatedType = (LinkNodeTypes)Enum.Parse(typeof(LinkNodeTypes), connectionXML.Element("EndPnt.RelatedType").Value);
            //    item.ShapePnt.Clear();
            //    IEnumerable<XElement> shapePntsXML = connectionXML.Elements("ShapePnt.Positions").Elements("Position");
            //    foreach (XElement shapePntXML in shapePntsXML)
            //    {
            //        item.ShapePnt.Add(new Point(Double.Parse(shapePntXML.Element("X").Value), Double.Parse(shapePntXML.Element("Y").Value)));
            //    }
            //    AddShapes(item);
            //}

        }

        #endregion

        #region 保存文件
        public void Save_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            //IEnumerable<ShapeBase> designerItems = this.AllShapes.OfType<ShapeBase>();
            //IEnumerable<LinkBase> connections = this.AllShapes.OfType<LinkBase>();

            //XElement designerItemsXML = SerializeShapeItems(designerItems);
            //XElement connectionsXML = SerializeConnections(connections);

            //XElement root = new XElement("Root");
            //root.Add(designerItemsXML);
            //root.Add(connectionsXML);

            //SaveFile(root);
           
            SaveFileDialog saveFile = new SaveFileDialog();
            saveFile.Filter = "Files (*.xml)|*.xml|All Files (*.*)|*.*";
            if (saveFile.ShowDialog() == true)
            {
                try
                {
                    Save2XmlFile(this, saveFile.FileName);
                    //xElement.Save(saveFile.FileName);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.StackTrace, ex.Message, MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            
        }
        #endregion

        private IFlowChartBase getFlowChartBase(string modelName)
        {
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
                       // ifcb = new LinkBroken(LineTypes.Solid);
                        ifcb=new LinkSeqTransfer(LineTypes.Solid);
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
                case "LinkBroken":
                    {
                       ifcb = new LinkBroken(LineTypes.Solid);
                        break;
                    }
                case "LinkStraight":
                    {
                        ifcb = new LinkStraight(LineTypes.ShortDashes);
                        break;
                    }
                case "LinkSeqTransfer":
                    {
                        ifcb = new LinkSeqTransfer(LineTypes.Solid);
                        break;
                    }

                default:
                    break;
            }
            return ifcb;
        }
              
        
    }
}
