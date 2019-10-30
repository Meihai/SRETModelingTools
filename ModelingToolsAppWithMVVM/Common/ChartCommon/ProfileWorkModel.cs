using ModelingToolsAppWithMVVM.View;
using ModelingToolsAppWithMVVM.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Xml;

namespace ModelingToolsAppWithMVVM.Common.ChartCommon
{
    /// <summary>
    /// 剖面建模工作区
    /// </summary>
    public class ProfileWorkModel:BaseWorkModel
    {
        
        public ProfileWorkModel()
            : base()
        {
            this.CommandBindings.Add(new CommandBinding(ApplicationCommands.Delete, Delete_Executed, Delete_Enabled));          

        }

        /// <summary>
        /// 连接线类型
        /// </summary>
        private bool isSeqType=true;
        protected override void canvasDragEnter(object sender, System.Windows.DragEventArgs e)
        {
            if (null == DragIfcb)
            {
                switch ((FlowChartTypes)(e.Data.GetData(typeof(FlowChartTypes))))
                {                   
                    case FlowChartTypes.ShapeProbTransfer:
                    {
                        DragIfcb = new LinkProbTransfer();
                        DragIfcb.CreateShape();
                        break;
                    }

                    case FlowChartTypes.ShapeSeqTransfer:
                    {
                        DragIfcb = new LinkSeqTransfer();
                        DragIfcb.CreateShape();
                        break;
                    }

                    case FlowChartTypes.ShapeEntrance:
                    {
                        DragIfcb = new ShapeEntrance();
                        DragIfcb.CreateShape();
                        break;
                    }

                    case FlowChartTypes.ShapeExit:
                    {
                        DragIfcb = new ShapeExit();
                        DragIfcb.CreateShape();
                        break;
                    }

                    case FlowChartTypes.ShapeTerminal:
                    {
                        DragIfcb = new ShapeTerminal();
                        DragIfcb.CreateShape();
                        break;
                    }

                    case FlowChartTypes.ShapeOperation:
                    {
                        DragIfcb = new ShapeOperation();
                        DragIfcb.CreateShape();
                        break;
                    }

                    case FlowChartTypes.ShapeReliableProfile:
                    {
                        DragIfcb = new ShapeReliableProfile();
                        DragIfcb.CreateShape();
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

        #region 处理连接节点事件

        /// <summary>
        /// 重写鼠标松开事件，生成
        /// </summary>
        /// <param name="e"></param>
        protected override void DealWithLinkNodeEvent()
        {
            if (IsLinkNodeDown)
            {
                IsLinkNodeDown = false;
                if (dragStartPoint.HasValue && dragEndPoint.HasValue)
                {
                    if (isSeqType)
                    {
                        tmpLink = new LinkSeqTransfer((Point)dragStartPoint, (Point)dragEndPoint);
                    }
                    else
                    {
                        tmpLink = new LinkProbTransfer((Point)dragStartPoint, (Point)dragEndPoint);
                    }

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

        #endregion 处理连接节点事件


        #region 建模子窗口流程
        protected override void ifcb_evtOpenSubModelArea(ShapeBase modelItem)
        {
            if (modelItem.FlowChartType == FlowChartTypes.ShapeReliableProfile)
            {
                ModelingSubWindowTemplate subModelingWindow = new ModelingSubWindowTemplate();
                subModelingWindow.Title = modelItem.Description + "建模窗口";
                subModelingWindow.DataContext = new ModelingSubWindowViewModel();
                if (modelItem.ChildWorkModel != null)
                {
                    ObservableCollection<IFlowChartBase> ModelItemsTemp = modelItem.ChildWorkModel.ModelItems;
                    for (int k = 0; k < ModelItemsTemp.Count; k++)
                    {
                        modelItem.ChildWorkModel.Children.Remove((UserControl)ModelItemsTemp[k]);
                        /*
                         * 层次关系解析
                         * ModelingSubWindowTemplate ->(DataContext) ModelingSubWindowViewModel 
                         *                           ->(ProfileModelingWorkPanel)ProfileWorkView
                         *                           ->(DataContext)ProfileWorkViewModel
                         *                           ->(WorkPanelView)ProfileWorkModel:BaseModel:Canvas
                         *                           
                         */
                        ProfileWorkView pWorkView = (ProfileWorkView)((ModelingSubWindowViewModel)subModelingWindow.DataContext).ProfileModelingWorkPanel;
                        BaseWorkModel pWorkModel = (BaseWorkModel)((ProfileWorkViewModel)pWorkView.DataContext).WorkPanelView;
                        pWorkModel.AddShapes(ModelItemsTemp[k]); ///同一个引用正确，非同一个引用需重新赋值
                    }

                    //重新设置viewer引用
                    for (int kk = 0; kk < ModelItems.Count; kk++)
                    {
                        if (ModelItems[kk].Id == modelItem.CloneSourceId)
                        {
                            ProfileWorkView pWorkView = (ProfileWorkView)((ModelingSubWindowViewModel)subModelingWindow.DataContext).ProfileModelingWorkPanel;
                            BaseWorkModel pWorkModel = (BaseWorkModel)((ProfileWorkViewModel)pWorkView.DataContext).WorkPanelView;                           
                            ((ShapeBase)ModelItems[kk]).ChildWorkModel = pWorkModel;
                        }
                    }

                }
                ProfileWorkView pWorkView1 = (ProfileWorkView)((ModelingSubWindowViewModel)subModelingWindow.DataContext).ProfileModelingWorkPanel;
                BaseWorkModel pWorkModel1 = (BaseWorkModel)((ProfileWorkViewModel)pWorkView1.DataContext).WorkPanelView;
              
                pWorkModel1.BelongToModelItem = modelItem;
                pWorkModel1.ParentWorkModel = (BaseWorkModel)this;
                subModelingWindow.ShowDialog();
            }
            else
            {
                MessageBox.Show("当前模型元素不支持递归建模!");
            }
        }
        #endregion 建模子窗口流程


        #region 数据持久化到xml页面部分

        protected override void Save2XmlFile(BaseWorkModel baseModel, string fileName)
        {
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.AppendChild(xmlDocument.CreateXmlDeclaration("1.0", "utf-8", "yes"));
            XmlElement xmlRoot = xmlDocument.CreateElement(string.Empty, "ProfileModel", string.Empty);
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
                foreach (string propName in shapeBase.SerializeAttributes)
                {
                    XmlElement propXml = xmlDocument.CreateElement(string.Empty, propName, string.Empty);
                    if (propName == "ChildWorkModel")
                    {   //递归实现
                        if(shapeBase.ChildWorkModel!=null){
                            SerializeModelItems(shapeBase.ChildWorkModel, xmlDocument, propXml);
                        }                       
                    }
                    else
                    {                       
                        propXml.InnerText = type.GetProperty(propName).GetValue(shapeBase, null).ToString();                                         
                    }
                    designerItemXml.AppendChild(propXml);  //将最后的xml加入到designerItemXml中
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

      
        protected override void LoadXml(BaseWorkModel baseModel, string fileName)
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
                                    if (attributeXe.Name == "ChildWorkModel")
                                    {
                                        ProfileWorkModel pWorkModel = new ProfileWorkModel();
                                        LoadChildNode(pWorkModel, attributeXe);
                                        prop.SetValue(obj, pWorkModel);
                                    }else {
                                        object objValue = ConvertHelper.ChangeType(attributeXe.InnerText, prop.PropertyType);
                                        prop.SetValue(obj, objValue);
                                    }
                                    break;                                    
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
                                    if (prop.Name == "ShapePnt") {
                                        PointCollection pc = PointCollection.Parse(attributeXe.InnerText);
                                        prop.SetValue(obj, pc);
                                    }
                                    else
                                    {
                                        prop.SetValue(obj, ConvertHelper.ChangeType(attributeXe.InnerText, prop.PropertyType));
                                    }
                                    break;
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

        #endregion 数据持久化到xml部分


    }
}
