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
    public class LinkArc : LinkBase
    {
        public LinkArc()
            : base()
        {
            LinkLineType = LinkLineTypes.Arc;
            LineType = LineTypes.ShortDashes;
        }

        public LinkArc(LineTypes lineType)
            : base()
        {
            LinkLineType = LinkLineTypes.Arc;
            LineType = lineType;
        }

        public override void SetDescTextPosition()
        {
            double x = (StartPnt.Position.X + EndPnt.Position.X) / 2;
            double y = (StartPnt.Position.Y + EndPnt.Position.Y) / 2;

            double width = 0;
            double height = 0;
            CalcDescTextSize(out width, out height);

            txtDescription.Margin = new Thickness(x - width / 4, y - height / 2, 0, 0);
        }

        public override void CreateShape()
        {
            StreamGeometry geometry = new StreamGeometry();
            geometry.FillRule = FillRule.EvenOdd;

            using (StreamGeometryContext ctx = geometry.Open())
            {
                ctx.BeginFigure(StartPnt.Position, false, false);

                //其圆周用于绘制角度的椭圆的半径(半宽度和高度的一半)
                Size arcSize = new Size(30, 30);
                //指定曲线的椭圆的旋转角度
                Double rotationAngle = 0;
                //若要绘制大于180度的弧线,则为true，否则为false
                bool isLargeArc = true;
                //顺时针还是逆时针
                SweepDirection sweepDirection = SweepDirection.Counterclockwise;
                //若要在使用 Pen 呈现线段时使线段描边，则为 true；否则为 false。
                bool isStroked = true;
                //在用 Pen 描边时，如果要将此线段与前一条线段之间的联接视为角，则为 true；否则为 false
                bool isSmoothJoin = false;
                ctx.ArcTo(EndPnt.Position, arcSize,rotationAngle,isLargeArc, sweepDirection,isStroked,isSmoothJoin);
            }

            geometry.Freeze();
            setPathLinkLineType();
            this.pathLink.Stroke = LineBrush;
            this.pathLink.Data = geometry;           
            this.pathRange.Data = geometry;

            CalcShapeRange();
            SetDescTextPosition();

           // CreateTerminalShape();
        }

        private void setPathLinkLineType()
        {
            if (LineType == LineTypes.ShortDashes)
            {
                this.pathLink.StrokeDashArray.Clear();
                this.pathLink.StrokeDashArray.Add(3);
                this.pathLink.StrokeDashArray.Add(3);
            }
            else if (LineType == LineTypes.LongDashes)
            {
                this.pathLink.StrokeDashArray.Clear();
                this.pathLink.StrokeDashArray.Add(3);
                this.pathLink.StrokeDashArray.Add(3);
            }
            else
            {
                this.pathLink.StrokeDashArray.Clear();
            }
        }

        public override void CalcShapeRange()
        {
            Range.Clear();

            double minx = ShapePnt[0].X;
            double maxx = ShapePnt[0].X;
            double miny = ShapePnt[0].Y;
            double maxy = ShapePnt[0].Y;

            for (int i = 1; i < ShapePnt.Count; i++)
            {
                minx = ShapePnt[i].X < minx ? ShapePnt[i].X : minx;
                maxx = ShapePnt[i].X > maxx ? ShapePnt[i].X : maxx;
                miny = ShapePnt[i].Y < miny ? ShapePnt[i].Y : miny;
                maxy = ShapePnt[i].Y > maxy ? ShapePnt[i].Y : maxy;
            }

            Range.Add(new Point(minx + Margin.Left, miny + Margin.Top));
            Range.Add(new Point(maxx + Margin.Left, miny + Margin.Top));
            Range.Add(new Point(maxx + Margin.Left, maxy + Margin.Top));
            Range.Add(new Point(minx + Margin.Left, maxy + Margin.Top));
        }

        public override void InitializeShapePnt()
        {
            StartPnt = new LineTerminalPoint(new Point(6, 6));
            EndPnt = new LineTerminalPoint(new Point(Width - 6, Height - 6));

            ShapePnt.Clear();
            ShapePnt.Add(new Point(6, 6));
            ShapePnt.Add(new Point(Width - 6, Height - 6));
        }

        public override object Clone()
        {
            LinkStraight s = new LinkStraight();
            s.Margin = this.Margin;
            s.Width = this.Width;
            s.Height = this.Height;
            s.Description = this.Description;
            s.ShapePnt = this.ShapePnt;
            s.Offset = new Point(this.Margin.Left, this.Margin.Top);
            s.CloneSourceId = this.Id;
            s.StartPnt = this.StartPnt;
            s.EndPnt = this.EndPnt;
            s.CreateShape();
            return s;
        }

        public override bool GetDescriptionPosition(out Point position)
        {
            double x = (StartPnt.Position.X + EndPnt.Position.X) / 2;
            double y = (StartPnt.Position.Y + EndPnt.Position.Y) / 2;

            position = new Point(x + Margin.Left, y + Margin.Top);
            return true;
        }

        protected override void CreateTerminalShape()
        {
            StreamGeometry geometry = new StreamGeometry();
            geometry.FillRule = FillRule.EvenOdd;


            using (StreamGeometryContext ctx = geometry.Open())
            {
                ctx.BeginFigure(EndPnt.Position, true, true);
                ctx.LineTo(new Point(EndPnt.Position.X + 4, EndPnt.Position.Y - 10), true, false);
                ctx.LineTo(new Point(EndPnt.Position.X - 4, EndPnt.Position.Y - 10), true, false);
            }

            geometry.Freeze();
            this.pathEnd.Data = geometry;
            this.pathEnd.Fill = LineBrush;



            double r = Math.Atan((StartPnt.Position.X - EndPnt.Position.X) / (StartPnt.Position.Y - EndPnt.Position.Y) * -1);
            double d = 0;
            if (StartPnt.Position.Y < EndPnt.Position.Y)
            {
                d = r * 180 / Math.PI;
            }
            else if (StartPnt.Position.Y > EndPnt.Position.Y)
            {
                d = r * 180 / Math.PI + 180;
            }
            else if (StartPnt.Position.Y == EndPnt.Position.Y)
            {
                if (StartPnt.Position.X >= EndPnt.Position.X)
                {
                    d = 90;
                }
                if (StartPnt.Position.X < EndPnt.Position.X)
                {
                    d = 270;
                }
            }

            Transform t = new RotateTransform(d, EndPnt.Position.X, EndPnt.Position.Y);
            this.pathEnd.RenderTransform = t;
        }

        public override LineCtrlPoint GetLineCtrlPoints()
        {
            LineCtrlPoint lcp = new LineCtrlPoint(StartPnt.Position, LinePointTypes.StartPoint, CtrlNodeTypes.START);

            lcp.Append(EndPnt.Position, LinePointTypes.EndPoint, CtrlNodeTypes.END);
            return lcp;
        }

        public override void CreateTmpLine(CtrlNode ctrlNode, Point position)
        {
            StreamGeometry geometry = new StreamGeometry();
            geometry.FillRule = FillRule.EvenOdd;

            using (StreamGeometryContext ctx = geometry.Open())
            {
                if (ctrlNode.LinePointType == LinePointTypes.StartPoint)
                {
                    ctx.BeginFigure(new Point(position.X + Margin.Left, position.Y + Margin.Top), false, false);
                }
                else
                {
                    ctx.BeginFigure(new Point(StartPnt.Position.X + Margin.Left, StartPnt.Position.Y + Margin.Top), false, false);
                }

                if (ctrlNode.LinePointType == LinePointTypes.EndPoint)
                {
                    ctx.LineTo(new Point(position.X + Margin.Left, position.Y + Margin.Top), true, false);
                }
                else
                {
                    ctx.LineTo(new Point(EndPnt.Position.X + Margin.Left, EndPnt.Position.Y + Margin.Top), true, false);
                }
            }
            geometry.Freeze();

            OnRefreshTmpLine(geometry);
        }

        public override void FinishTmpLine(LineTerminalPoint start, LineTerminalPoint end)
        {
            StartPnt = start;
            EndPnt = end;

            ShapePnt.Clear();
            ShapePnt.Add(StartPnt.Position);
            ShapePnt.Add(EndPnt.Position);

            CreateShape();
        }

        public override void MoveToDockedShape(IFlowChartBase ifcb)
        {
            base.MoveToDockedShape(ifcb);

            if (StartPnt.DockedFlag && StartPnt.RelatedShapeId == ifcb.Id)
            {
                for (int i = 0; i < ifcb.LinkNodes.Count; i++)
                {
                    if (ifcb.LinkNodes[i].LinkNodeType == StartPnt.RelatedType)
                    {

                        StartPnt.Position = new Point(ifcb.LinkNodes[i].Position.X + ifcb.Margin.Left - this.Margin.Left,
                                                        ifcb.LinkNodes[i].Position.Y + ifcb.Margin.Top - this.Margin.Top);
                        break;
                    }
                }
            }

            if (EndPnt.DockedFlag && EndPnt.RelatedShapeId == ifcb.Id)
            {
                for (int i = 0; i < ifcb.LinkNodes.Count; i++)
                {
                    if (ifcb.LinkNodes[i].LinkNodeType == EndPnt.RelatedType)
                    {
                        EndPnt.Position = new Point(ifcb.LinkNodes[i].Position.X + ifcb.Margin.Left - this.Margin.Left,
                                                        ifcb.LinkNodes[i].Position.Y + ifcb.Margin.Top - this.Margin.Top);
                        break;
                    }
                }
            }

            CreateShape();
        }

        public override void MoveToClone(IFlowChartBase cloneIfcb)
        {
            this.StartPnt.Position = ((LinkBase)cloneIfcb).StartPnt.Position;
            this.StartPnt.DockedFlag = ((LinkBase)cloneIfcb).StartPnt.DockedFlag;
            this.StartPnt.RelatedShapeId = ((LinkBase)cloneIfcb).StartPnt.RelatedShapeId;
            this.StartPnt.RelatedType = ((LinkBase)cloneIfcb).StartPnt.RelatedType;

            this.EndPnt.Position = ((LinkBase)cloneIfcb).EndPnt.Position;
            this.EndPnt.DockedFlag = ((LinkBase)cloneIfcb).EndPnt.DockedFlag;
            this.EndPnt.RelatedShapeId = ((LinkBase)cloneIfcb).EndPnt.RelatedShapeId;
            this.EndPnt.RelatedType = ((LinkBase)cloneIfcb).EndPnt.RelatedType;

            ChangePositionAndSize(cloneIfcb.Margin.Left - Margin.Left, cloneIfcb.Margin.Top - Margin.Top, 0, 0);
        }

        public override FlowChartTypes FlowChartType
        {
            get { return FlowChartTypes.LinkStraight; }
        }


        

    }
}
