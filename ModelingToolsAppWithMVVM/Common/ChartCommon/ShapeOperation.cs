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
    public class ShapeOperation : ShapeBase
    {
        public ShapeOperation()
        {
            Description = "操作";
        }

        public override void CreateShape()
        {   /*
            StreamGeometry geometry = new StreamGeometry();
            geometry.FillRule = FillRule.EvenOdd;

            using (StreamGeometryContext ctx = geometry.Open())
            {
                ctx.BeginFigure(new Point(CtrlNodeSize, CtrlNodeSize), true, true);
                ctx.LineTo(new Point(this.Width - CtrlNodeSize, CtrlNodeSize), true, false);
                ctx.LineTo(new Point(this.Width - CtrlNodeSize, this.Height - CtrlNodeSize), true, false);
                ctx.LineTo(new Point(CtrlNodeSize, this.Height - CtrlNodeSize), true, false);
            }
            geometry.Freeze();
            */
         
            Rect rect = new Rect(CtrlNodeSize, CtrlNodeSize, Width-12.0, Height-13.0);
            EllipseGeometry ellipseGeometry = new EllipseGeometry(rect);
            this.pathShape.Fill = new SolidColorBrush(Colors.LightGray);
            this.pathShape.Data = ellipseGeometry;

            this.pathShapeEx.Data = this.pathShape.Data;
            RepositionLinkNode();
        }

        public override void RepositionLinkNode()
        {   
            lLinkNode.Center = new Point(CtrlNodeSize, Height / 2.0);
            tLinkNode.Center = new Point(Width / 2.0, CtrlNodeSize);
            rLinkNode.Center = new Point(Width - CtrlNodeSize, Height / 2.0);
            bLinkNode.Center = new Point(Width / 2.0, Height - CtrlNodeSize);
             
            //cLinkNode.Center = new Point(Width / 2.0, Height / 2.0);

        }


        public override object Clone()
        {
            ShapeBase s = new ShapeOperation();
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
            get { return FlowChartTypes.ShapeOperation; }
        }

        private OperationSM propertyModel;


        public void toPropertyModel()
        {
            OperationSM operationSM = new OperationSM();
            operationSM.Id = Id;
            operationSM.Name = Description;
            operationSM.Type = FlowChartType;
            propertyModel = operationSM;
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
                propertyModel = (OperationSM)value;
                Description = propertyModel.Name;
            }
        }

        internal override void SetSerializeAttributes()
        {
            SerializeAttributes = new List<string> { "Left", "Top", "Width", "Height", "Id", "ZIndex", "FlowChartType", "Description", "CloneSourceId" };
        }
    }
}
