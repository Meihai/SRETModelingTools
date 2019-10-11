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
    public class ShapeTerminal : ShapeBase
    {
        public ShapeTerminal()
        {
            SetSerializeAttributes();
            Description = "终止";
        }

        public override void CreateShape()
        {
            StreamGeometry geometry = new StreamGeometry();
            geometry.FillRule = FillRule.EvenOdd;
            using (StreamGeometryContext ctx = geometry.Open())
            {
                //ctx.BeginFigure(new Point(CtrlNodeSize, CtrlNodeSize), true, true);
                ctx.BeginFigure(new Point(this.Width/2.0, CtrlNodeSize), true, true);
                ctx.LineTo(new Point(this.Width - CtrlNodeSize - this.Width*0.21, this.Height - CtrlNodeSize), true, false);
                ctx.LineTo(new Point(CtrlNodeSize + this.Width * 0.21, this.Height - CtrlNodeSize), true, false);
            }
            geometry.Freeze();

            this.pathShape.Fill = new SolidColorBrush(Colors.Black);
            this.pathShape.Data = geometry;

            this.pathShapeEx.Data = this.pathShape.Data;
            RepositionLinkNode();
        }

        public override void RepositionLinkNode()
        {              
            //
            lLinkNode.Center = new Point(CtrlNodeSize+Width*0.31, Height / 2.0);
            lLinkNode.Margin = new Thickness(CtrlNodeSize + Width * 0.27,0,0,0);
            //lLinkNode.HorizontalAlignment = HorizontalAlignment.Center;
            tLinkNode.Center = new Point(Width / 2.0, CtrlNodeSize);
           
            rLinkNode.Center = new Point(Width - CtrlNodeSize-Width*0.31, Height / 2.0);
            rLinkNode.HorizontalAlignment = HorizontalAlignment.Left;
            rLinkNode.Margin = new Thickness(CtrlNodeSize + Width * 0.52, 0, 0, 0);

            bLinkNode.Center = new Point(Width / 2.0, Height - CtrlNodeSize);
             
            //cLinkNode.Center = new Point(Width / 2.0, Height / 2.0);


        }


        public override object Clone()
        {
            ShapeBase s = new ShapeTerminal();
            s.Margin = new Thickness(this.Margin.Left, this.Margin.Top, 0, 0);
            s.Width = this.Width;
            s.Height = this.Height;
            s.Description = this.Description;
            s.CloneSourceId = this.Id;
            s.Offset = new Point(this.Margin.Left, this.Margin.Top);
            s.CreateShape();

            return s;
        }

     

        //Height 不需要
        //Width  不需要

        public override FlowChartTypes FlowChartType
        {
            get { return FlowChartTypes.ShapeTerminal; }
        }

        private TerminalSM propertyModel;


        public void toPropertyModel()
        {
            TerminalSM terminalSM = new TerminalSM();
            terminalSM.Id = Id;
            terminalSM.Name = Description;
            terminalSM.Type = FlowChartType;
            propertyModel = terminalSM;
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
                propertyModel = (TerminalSM)value;
                Description = propertyModel.Name;
            }
        }

        internal override void SetSerializeAttributes()
        {
            SerializeAttributes = new List<string> {"Left","Top","Width","Height","Id","ZIndex","FlowChartType","Description","CloneSourceId"};
        }
    }
}
