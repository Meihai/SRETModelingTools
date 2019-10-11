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

namespace ModelingToolsApp.UserControls
{
    [System.ComponentModel.DesignTimeVisible(false)]
    public class ShapeReliableProfile : ShapeBase
    {
        public ShapeReliableProfile()
        {
            Description = "包";
        }

        public override void CreateShape()
        {
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
            this.pathShape.Fill = new SolidColorBrush(Colors.LightGray);
            this.pathShape.Data = geometry;

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
            ShapeBase s = new ShapeReliableProfile();
            s.Margin = new Thickness(this.Margin.Left, this.Margin.Top, 0, 0);
            s.Width = this.Width;
            s.Height = this.Height;
            s.Description = this.Description;
            s.CloneSourceId = this.Id;
            s.Offset = new Point(this.Margin.Left, this.Margin.Top);
            s.CreateShape();
            //添加建模子窗口
            s.ChildModelArea = this.ChildModelArea;
            

            return s;
        }


        public override FlowChartTypes FlowChartType
        {
            get { return FlowChartTypes.ShapeReliableProfile; }
        }
    }
}
