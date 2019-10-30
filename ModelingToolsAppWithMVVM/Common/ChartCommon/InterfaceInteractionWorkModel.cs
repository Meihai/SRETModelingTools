using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using System.Xml;

namespace ModelingToolsAppWithMVVM.Common.ChartCommon
{
    public class InterfaceInteractionWorkModel:BaseWorkModel
    {

        public InterfaceInteractionWorkModel()
            : base()
        {
            this.CommandBindings.Add(new CommandBinding(ApplicationCommands.Delete, Delete_Executed, Delete_Enabled));           

        }

        protected override void canvasDragEnter(object sender, System.Windows.DragEventArgs e)
        {
            if (null == DragIfcb)
            {
                switch ((FlowChartTypes)(e.Data.GetData(typeof(FlowChartTypes))))
                {
                    case FlowChartTypes.InterfaceInteractionLink: {
                        DragIfcb = new InterfaceInteractionLink();
                        DragIfcb.CreateShape();
                        break;
                    }
                    case FlowChartTypes.InterfaceInteractionObject:
                    {
                        DragIfcb = new InterfaceInteractionObject();
                        DragIfcb.CreateShape();
                        break;
                    }
                    case FlowChartTypes.InterfaceTestedObject:
                    {
                        DragIfcb = new InterfaceInteractionTestedObject();
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



        #region 数据持久化到xml页面部分

        protected override void Save2XmlFile(BaseWorkModel baseModel, string fileName)
        {
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.AppendChild(xmlDocument.CreateXmlDeclaration("1.0", "utf-8", "yes"));
            XmlElement xmlRoot = xmlDocument.CreateElement(string.Empty, "InterfaceInteractionModel", string.Empty);
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
                        if (shapeBase.ChildWorkModel != null)
                        {
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
                                if (attributeXe.Name == prop.Name) {
                                    object objValue = ConvertHelper.ChangeType(attributeXe.InnerText, prop.PropertyType);
                                    prop.SetValue(obj, objValue);
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
                                    if (prop.Name == "ShapePnt")
                                    {
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
