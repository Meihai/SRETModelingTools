using ModelingToolsAppWithMVVM.Common.ServiceModel;
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
    [System.ComponentModel.DesignTimeVisible(false)]
    public class ShapeEntrance : ShapeBase
    {
        public ShapeEntrance()
        {
            Description = "";
            toPropertyModel();
        }

        public override void CreateShape()
        {
            /*
            StreamGeometry geometry = new StreamGeometry();           

            using (StreamGeometryContext ctx = geometry.Open())
            {
                ctx.BeginFigure(new Point(CtrlNodeSize, CtrlNodeSize), true, true);                
                ctx.LineTo(new Point(this.Width - CtrlNodeSize, CtrlNodeSize), true, false);
                ctx.LineTo(new Point(this.Width - CtrlNodeSize, this.Height - CtrlNodeSize), true, false);
                ctx.LineTo(new Point(CtrlNodeSize, this.Height - CtrlNodeSize), true, false);
            }
            geometry.Freeze();
            */

            this.Width = 70;
            this.Height = 70;          

            //Rect rect = new Rect(30, 30, 30, 30);
            EllipseGeometry ellipseGeometry = new EllipseGeometry(new Point(this.Width/2.0,this.Height/2.0),25,25);
           
            this.pathShape.Fill =new SolidColorBrush(Colors.Black);
            this.pathShape.Data = ellipseGeometry;

            this.pathShapeEx.Data = this.pathShape.Data;
            RepositionLinkNode();
        }
               

          

        public override void RepositionLinkNode()
        {   
            double radius=Math.Min(Width,Height)/2.0;
            lLinkNode.Center = new Point(CtrlNodeSize+Width/2.0-radius, Height / 2.0);//左边的控制点
            lLinkNode.Margin = new Thickness(CtrlNodeSize + 0.46*Width  - radius, 0, 0, 0);

            tLinkNode.Center = new Point(Width / 2.0, CtrlNodeSize+Height/2.0-radius);//上边的控制点
            tLinkNode.Margin = new Thickness(0, CtrlNodeSize + 0.46*Height - radius,0,0);

            rLinkNode.Center = new Point(Width - CtrlNodeSize-(Width/2.0-radius), Height / 2.0);//右边的控制点
            rLinkNode.HorizontalAlignment = HorizontalAlignment.Left;
            rLinkNode.Margin = new Thickness(Width - CtrlNodeSize - (0.65*Width- radius), 0, 0, 0);

            bLinkNode.Center = new Point(Width / 2.0, Height - CtrlNodeSize-(Height/2.0-radius));//下边的控制点
            bLinkNode.VerticalAlignment = VerticalAlignment.Top;
            bLinkNode.Margin = new Thickness(0, Height - CtrlNodeSize - (0.65*Height - radius),0,0);
            /*
            cLinkNode.Height = Height;
            cLinkNode.Width = Width;
            cLinkNode.Center = new Point(Width / 2.0, Height / 2.0);
            */


        }


        public override object Clone()
        {
            ShapeBase s = new ShapeEntrance();
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
            get { return FlowChartTypes.ShapeEntrance; }
        }

        private EntranceSM propertyModel;


        public void toPropertyModel()
        {
            EntranceSM entranceSM = new EntranceSM();
            entranceSM.Id = Id;
            entranceSM.Name = Description;
            entranceSM.Type = FlowChartType;
            propertyModel = entranceSM;
        }

        public override object PropertyModel
        {
            get
            {
                toPropertyModel();
                return propertyModel;
            }
            set
            {
                propertyModel = (EntranceSM)value;
                Description = propertyModel.Name;
            }
        }


        internal override void SetSerializeAttributes()
        {
            SerializeAttributes = new List<string> { "Left", "Top", "Width", "Height", "Id", "ZIndex", "FlowChartType", "Description", "CloneSourceId" };
        }
    }
}
