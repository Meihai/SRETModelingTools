using ModelingToolsAppWithMVVM.Common.ServiceModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace ModelingToolsAppWithMVVM.Common.ChartCommon
{
    [System.ComponentModel.DesignTimeVisible(false)]
    public class InterfaceInteractionObject : ShapeBase
    {
        public InterfaceInteractionObject()
        {
            Description = "交联对象";
            toPropertyModel();
        }

        public override void CreateShape()
        {
 
            this.Width = 70;
            this.Height = 70;

            //Rect rect = new Rect(30, 30, 30, 30);
            EllipseGeometry ellipseGeometry = new EllipseGeometry(new Point(this.Width / 2.0, this.Height / 2.0), 25, 25);

            this.pathShape.Fill = new SolidColorBrush(Colors.White);
            this.pathShape.Data = ellipseGeometry;

            this.pathShapeEx.Data = this.pathShape.Data;
            RepositionLinkNode();

        }




        public override void RepositionLinkNode()
        {
            double radius = Math.Min(Width, Height) / 2.0;
            lLinkNode.Center = new Point(CtrlNodeSize + Width / 2.0 - radius, Height / 2.0);//左边的控制点
            tLinkNode.Center = new Point(Width / 2.0, CtrlNodeSize + Height / 2.0 - radius);//上边的控制点
            rLinkNode.Center = new Point(Width - CtrlNodeSize - (Width / 2.0 - radius), Height / 2.0);//右边的控制点
            bLinkNode.Center = new Point(Width / 2.0, Height - CtrlNodeSize - (Height / 2.0 - radius));//下边的控制点

            //cLinkNode.Center = new Point(Width / 2.0, Height / 2.0);
        }


        public override object Clone()
        {
            ShapeBase s = new InterfaceInteractionObject();
            s.Margin = new Thickness(this.Margin.Left, this.Margin.Top, 0, 0);
            s.Width = this.Width;
            s.Height = this.Height;
            s.Description = this.Description;
            s.CloneSourceId = this.Id;
            s.Offset = new Point(this.Margin.Left, this.Margin.Top);
            s.CreateShape();

            return s;
        }


        public override FlowChartTypes FlowChartType
        {
            get { return FlowChartTypes.InterfaceInteractionObject; }
        }

        private IIObjectSM propertyModel;

        private void toPropertyModel()
        {
            IIObjectSM iiObjectSM = new IIObjectSM();
            iiObjectSM.Id = Id;
            iiObjectSM.Name = Description;
            iiObjectSM.Type = FlowChartType;
            propertyModel = iiObjectSM;
        }

        public override object PropertyModel
        {
            get {

                toPropertyModel();
                return propertyModel;
            }
            set { 
                propertyModel = (IIObjectSM)value;
                Description = propertyModel.Name;
            }
        }


        internal override void SetSerializeAttributes()
        {
            SerializeAttributes = new List<string> { "Left", "Top", "Width", "Height", "Id", "ZIndex", "FlowChartType", "Description", "CloneSourceId" };
        }
    }
}
