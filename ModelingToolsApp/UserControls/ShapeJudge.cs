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
using System.ComponentModel;

namespace ModelingToolsApp.UserControls
{
    [System.ComponentModel.DesignTimeVisible(false)]
    public class ShapeJudge : ShapeBase
    {
        public ShapeJudge()
        {
            txtDescription.Text = "判断条件";
        }

        public override void CreateShape()
        {
            StreamGeometry geometry = new StreamGeometry();
            geometry.FillRule = FillRule.EvenOdd;

            using (StreamGeometryContext ctx = geometry.Open())
            {
                ctx.BeginFigure(new Point(this.Width / 2.0, CtrlNodeSize), true, true);
                ctx.LineTo(new Point(this.Width - CtrlNodeSize, this.Height / 2.0), true, false);
                ctx.LineTo(new Point(this.Width / 2.0, this.Height - CtrlNodeSize), true, false);
                ctx.LineTo(new Point(CtrlNodeSize, this.Height / 2.0), true, false);
            }
            geometry.Freeze();

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
            ShapeBase s = new ShapeJudge();
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
            get { return FlowChartTypes.ShapeJudge; }
        }
    }
}
