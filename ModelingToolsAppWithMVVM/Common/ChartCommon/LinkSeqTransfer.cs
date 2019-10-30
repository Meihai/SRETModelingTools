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
using ModelingToolsAppWithMVVM.Common.ServiceModel;

namespace ModelingToolsAppWithMVVM.Common.ChartCommon
{
   
    [System.ComponentModel.DesignTimeVisible(false)]
    public class LinkSeqTransfer : LinkBase
    {
        /// <summary>
        /// 控制点
        /// </summary>
        private LineCtrlPoint LineCtrlPnt;

        public LinkSeqTransfer()
            : base()
        {
            LinkLineType = LinkLineTypes.Broken;
            LineType = LineTypes.Solid;
            FlowChartType=FlowChartTypes.ShapeSeqTransfer;
        }

        public LinkSeqTransfer(LineTypes lineType)
            : base()
        {
            LinkLineType = LinkLineTypes.Broken;
            LineType = lineType;
            FlowChartType = FlowChartTypes.ShapeSeqTransfer;
        }

        public LinkSeqTransfer(Point startPoint, Point endPoint)
            : base()
        {
            StartPnt = new LineTerminalPoint(startPoint);
            EndPnt = new LineTerminalPoint(endPoint);
            ShapePnt.Clear();
            ShapePnt.Add(startPoint);
            ShapePnt.Add(endPoint);
            LinkLineType = LinkLineTypes.Broken;
            LineType = LineTypes.Solid;
            toPropertyModel();
        }


        public override bool GetDescriptionPosition(out Point position)
        {
            double len = 0;
            double half = 0;

            double x = 0;
            double y = 0;

            double width = 0;
            double height = 0;
            CalcDescTextSize(out width, out height);

            //计算连接线总长
            for (int i = 0; i < ShapePnt.Count - 1; i++)
            {
                len += Math.Abs(ShapePnt[i].X - ShapePnt[i + 1].X) + Math.Abs(ShapePnt[i].Y - ShapePnt[i + 1].Y);
            }
            half = len / 2.0;
            len = 0;

            int pos = 0;
            for (int i = 1; i < ShapePnt.Count && len < half; i++)
            {
                len += Math.Abs(ShapePnt[i - 1].X - ShapePnt[i].X) + Math.Abs(ShapePnt[i - 1].Y - ShapePnt[i].Y);
                pos = i;
            }

            double dif = len - half;
            if (ShapePnt[pos].Y == ShapePnt[pos - 1].Y)
            {
                x = ShapePnt[pos].X + dif * (ShapePnt[pos].X < ShapePnt[pos - 1].X ? 1 : -1);
                y = ShapePnt[pos].Y;
            }
            else
            {
                y = ShapePnt[pos].Y + dif * (ShapePnt[pos].Y < ShapePnt[pos - 1].Y ? 1 : -1);
                x = ShapePnt[pos].X;
            }

            position = new Point(x + Margin.Left, y + Margin.Top);
            return true;
        }

        public override object Clone()
        {
            LinkSeqTransfer s = new LinkSeqTransfer();
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

        public override void SetDescTextPosition()
        {
            double len = 0;
            double half = 0;

            double x = 0;
            double y = 0;


            double width = 0;
            double height = 0;
            CalcDescTextSize(out width, out height);

            //计算连接线总长
            for (int i = 0; i < ShapePnt.Count - 1; i++)
            {
                len += Math.Abs(ShapePnt[i].X - ShapePnt[i + 1].X) + Math.Abs(ShapePnt[i].Y - ShapePnt[i + 1].Y);
            }
            half = len / 2.0;
            len = 0;

            int pos = 0;
            for (int i = 1; i < ShapePnt.Count && len < half; i++)
            {
                len += Math.Abs(ShapePnt[i - 1].X - ShapePnt[i].X) + Math.Abs(ShapePnt[i - 1].Y - ShapePnt[i].Y);
                pos = i;
            }

            double dif = len - half;
            if (ShapePnt[pos].Y == ShapePnt[pos - 1].Y)
            {
                x = ShapePnt[pos].X + dif * (ShapePnt[pos].X < ShapePnt[pos - 1].X ? 1 : -1);
                y = ShapePnt[pos].Y;
            }
            else
            {
                y = ShapePnt[pos].Y + dif * (ShapePnt[pos].Y < ShapePnt[pos - 1].Y ? 1 : -1);
                x = ShapePnt[pos].X;
            }

            txtDescription.Margin = new Thickness(x - width / 4, y - txtDescription.FontSize / 2, 0, 0);
        }

        public override void CreateShape()
        {
            StreamGeometry geometry = new StreamGeometry();
            geometry.FillRule = FillRule.EvenOdd;

            using (StreamGeometryContext ctx = geometry.Open())
            {
                ctx.BeginFigure(ShapePnt[0], false, false);

                for (int i = 1; i < ShapePnt.Count; i++)
                {
                    ctx.LineTo(ShapePnt[i], true, false);
                }
            }

            geometry.Freeze();
            setPathLinkLineType();
            this.pathLink.Data = geometry;
            this.pathRange.Data = geometry;
            CalcShapeRange();
            SetDescTextPosition();
            CreateTerminalShape();
           
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

            for (int i = 0; i < ShapePnt.Count; i++)
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

        protected override void CreateTerminalShape()
        {
            StreamGeometry geometry = new StreamGeometry();
            geometry.FillRule = FillRule.EvenOdd;
            using (StreamGeometryContext ctx = geometry.Open())
            {
                ctx.BeginFigure(EndPnt.Position, true, true);
                //添加一个末端箭头
                ctx.LineTo(new Point(EndPnt.Position.X + 4, EndPnt.Position.Y - 10), true, false);
                ctx.LineTo(new Point(EndPnt.Position.X - 4, EndPnt.Position.Y - 10), true, false);
            }

            geometry.Freeze();
            this.pathEnd.Data = geometry;
            this.pathEnd.Data = geometry;


            //转换箭头的形状        
            int i = ShapePnt.Count - 1;
            int j = i - 1;    


            double r = Math.Atan((ShapePnt[j].X - ShapePnt[i].X) / (ShapePnt[j].Y - ShapePnt[i].Y) * -1);
            double d = 0;
            if (ShapePnt[j].Y < ShapePnt[i].Y)
            {
                d = r * 180 / Math.PI;
            }
            else if (ShapePnt[j].Y > ShapePnt[i].Y)
            {
                d = r * 180 / Math.PI + 180;
            }
            else if (ShapePnt[j].Y == ShapePnt[i].Y)
            {
                if (ShapePnt[j].X >= ShapePnt[i].X)
                {
                    d = 90;
                }
                if (ShapePnt[j].X < ShapePnt[i].X)
                {
                    d = 270;
                }
            }

            Transform t = new RotateTransform(d, EndPnt.Position.X, EndPnt.Position.Y);
            this.pathEnd.RenderTransform = t;          
        }

      
        // 定义获取线上的控制点测试
        public override LineCtrlPoint GetLineCtrlPoints()
        {
            //起点位置
            LineCtrlPnt = new LineCtrlPoint(ShapePnt[0], LinePointTypes.StartPoint, CtrlNodeTypes.START);
            for (int i = 0; i < ShapePnt.Count - 1; i++)
            {
                LineCtrlPnt.Append((ShapePnt[i].X + ShapePnt[i + 1].X) / 2.0, (ShapePnt[i].Y + ShapePnt[i + 1].Y) / 2.0, LinePointTypes.MiddlePoint, CtrlNodeTypes.ALL);

                if (i + 1 == ShapePnt.Count - 1)
                {
                    LineCtrlPnt.Append(ShapePnt[i + 1].X, ShapePnt[i + 1].Y, LinePointTypes.EndPoint, CtrlNodeTypes.END);
                }
                else
                {
                    LineCtrlPnt.Append(ShapePnt[i + 1].X, ShapePnt[i + 1].Y, LinePointTypes.BreakPoint, CtrlNodeTypes.ALL);
                }
            }

            return LineCtrlPnt;
        }

        /// <summary>
        /// 移动过程中是否插入新点
        /// </summary>
        bool IsInsertedPrev = false;
        bool IsInsertedNext = false;
    
        /// <summary>
        /// 当前移动的控制点
        /// </summary>
        LineCtrlPoint CurPnt = null;
        public override void CreateTmpLine(CtrlNode ctrlNode, Point position)
        {
            StreamGeometry geometry = new StreamGeometry();
            geometry.FillRule = FillRule.EvenOdd;            
                     
            {
                IsManualSetted = true;
                //找到当前移动的控制点在连接线所有控制点中的位置
                if (null == CurPnt || CurPnt.Id != ctrlNode.Id)
                {
                    CurPnt = LineCtrlPnt;
                    while (null != CurPnt && CurPnt.Id != ctrlNode.Id)
                    {
                        CurPnt = CurPnt.Next;
                    }
                }

                #region 起始点移动

                if ((ctrlNode.CtrlNodeType == CtrlNodeTypes.START || ctrlNode.CtrlNodeType == CtrlNodeTypes.END))
                {
                    CurPnt.Position = new Point(position.X, position.Y);
                }
                #endregion

                #region 中间点移动
                if (CurPnt.PointTypes==LinePointTypes.MiddlePoint)
                {
                    if (!IsInsertedPrev) //开始拖动，未插入新点
                    {
                        //中点移动，会变成转折点,更新位置
                        CurPnt.PointTypes = LinePointTypes.BreakPoint;
                        CurPnt.Position = new Point(position.X, position.Y);
                        //再插入两个中点坐标,方便进行下一次拖动
                        LineCtrlPoint lcp1 = new LineCtrlPoint(new Point((CurPnt.Prev.Position.X + position.X) / 2.0, (CurPnt.Prev.Position.Y+position.Y) / 2.0),
                                   LinePointTypes.MiddlePoint, CtrlNodeTypes.ALL);
                        LineCtrlPoint lcp2 = new LineCtrlPoint(new Point((CurPnt.Next.Position.X + position.X) / 2.0, (CurPnt.Next.Position.Y + position.Y) / 2.0),
                                 LinePointTypes.MiddlePoint, CtrlNodeTypes.ALL);
                        CurPnt.Insert(lcp1, 0);
                        CurPnt.Insert(lcp2, 1);
                        IsInsertedPrev = true;                        
                    }
                    else  //已经有新插入点，则直接移动插入点的位置
                    {
                        if (CurPnt.Prev.Prev != null && CurPnt.Next.Next != null)
                        {
                            CurPnt.Prev.Position = new Point((CurPnt.Prev.Prev.Position.X + position.X) / 2.0, (CurPnt.Prev.Prev.Position.Y + position.Y) / 2.0);
                            CurPnt.Next.Position = new Point((CurPnt.Next.Next.Position.X + position.X) / 2.0, (CurPnt.Next.Next.Position.Y + position.Y) / 2.0);
                        }
                       
                       // CurPnt.Position = new Point(position.X, position.Y);
                    }
                }
                #endregion             

                #region 拐点移动
                else if (CurPnt.PointTypes == LinePointTypes.BreakPoint)
                {
                    //如果拐点跟前后两个拐点或者断点在同一条直线上，则去掉中间两个中点
                    //P1(x1,y1) P2(x2,y2) P3(x3,y3) 判断斜率是否相等
                    //CurPnt.Prev.Prev,CurPnt.Next.Next,position,
                    double slopeResult = Math.Abs((position.Y - CurPnt.Prev.Prev.Position.Y) * (CurPnt.Next.Next.Position.X - CurPnt.Prev.Prev.Position.X)
                        - (CurPnt.Next.Next.Position.Y - CurPnt.Prev.Prev.Position.Y) * (position.X - CurPnt.Prev.Prev.Position.X));
                    if (slopeResult <= 1e-2)
                    {
                        CurPnt.Delete(0);
                        CurPnt.Delete(1);
                      //  CurPnt.PointTypes = LinePointTypes.MiddlePoint;
                       // CurPnt.Position=new Point((CurPnt.Prev.Position.X+CurPnt.Next.Position.X)/2.0,(CurPnt.Prev.Position.Y+CurPnt.Next.Position.Y)/2.0);
                    }
                    else {
                        //更新前后两个拐点的位置
                        CurPnt.Prev.Position = new Point((CurPnt.Prev.Prev.Position.X + position.X) / 2.0, (CurPnt.Prev.Prev.Position.Y + position.Y) / 2.0);
                        CurPnt.Next.Position = new Point((CurPnt.Next.Next.Position.X + position.X) / 2.0, (CurPnt.Next.Next.Position.Y + position.Y) / 2.0);
                         
                    }
                            
                }
                #endregion
                CurPnt.Position = new Point(position.X, position.Y);    
                LineCtrlPoint tmp = LineCtrlPnt;
                using (StreamGeometryContext ctx = geometry.Open())
                {
                    ctx.BeginFigure(new Point(tmp.Position.X + Margin.Left, tmp.Position.Y + Margin.Top), false, false);
                    tmp = tmp.Next;
                    while (null != tmp)
                    {
                        if (tmp.PointTypes != LinePointTypes.MiddlePoint)
                        {
                            ctx.LineTo(new Point(tmp.Position.X + Margin.Left, tmp.Position.Y + Margin.Top), true, false);
                        }

                        tmp = tmp.Next;
                    }
                }
            }

            geometry.Freeze();
            OnRefreshTmpLine(geometry);
        }


        /// <summary>
        /// 根据控制点设置形状点
        /// </summary>
        private void SetShapePntByCtrlPnt()
        {
           
            LineCtrlPoint tmp = LineCtrlPnt;
            if (tmp != null)
            {
                ShapePnt.Clear();
            }
            while (null != tmp)
            {
                if (null != tmp.Prev && null != tmp.Next && tmp.PointTypes == LinePointTypes.BreakPoint)
                {
                    if ((tmp.Prev.Position.X == tmp.Position.X && tmp.Position.X == tmp.Next.Position.X) ||
                        (tmp.Prev.Position.Y == tmp.Position.Y && tmp.Position.Y == tmp.Next.Position.Y))
                    {
                        //可过滤的拐点
                    }
                    else
                    {
                        ShapePnt.Add(tmp.Position);
                    }
                }
                else if (tmp.PointTypes == LinePointTypes.MiddlePoint)
                {
                    //可过滤的中点
                }
                else
                {
                    ShapePnt.Add(tmp.Position);
                }
                tmp = tmp.Next;
            }
        }


        public override void FinishTmpLine(LineTerminalPoint start, LineTerminalPoint end)
        {
            StartPnt = start;
            EndPnt = end;
            SetShapePntByCtrlPnt();
            IsInsertedPrev = false;
            IsInsertedNext = false;
            CurPnt = null;
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

            //起点
            Point start0 = new Point(StartPnt.Position.X + Margin.Left, StartPnt.Position.Y + Margin.Top);          
            //终点
            Point end0 = new Point(EndPnt.Position.X + Margin.Left, EndPnt.Position.Y + Margin.Top);            
            //路径转换为形状点        
            ShapePnt.Clear();
            SetShapePntByCtrlPnt();
            ShapePnt[0]=new Point(start0.X - this.Margin.Left, start0.Y - this.Margin.Top);
            ShapePnt[ShapePnt.Count-1]=new Point(end0.X - this.Margin.Left, end0.Y - this.Margin.Top);           
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

            //起点
            Point start0 = new Point(StartPnt.Position.X + Margin.Left, StartPnt.Position.Y + Margin.Top);
            //如果起点停靠在形状上，则起点后的第一点位置也固定
            Point start1 = new Point(StartPnt.Position.X + Margin.Left, StartPnt.Position.Y + Margin.Top);
            //终点
            Point end0 = new Point(EndPnt.Position.X + Margin.Left, EndPnt.Position.Y + Margin.Top);
            //如果终点停靠在形状上，则终点前的第一点位置也固定
            Point end1 = new Point(EndPnt.Position.X + Margin.Left, EndPnt.Position.Y + Margin.Top);

            SetShapePntByCtrlPnt();      
            CreateShape();
        }

      

        private LinkSeqTransferSM propertyModel;

        public void toPropertyModel()
        {
            LinkSeqTransferSM seqTransferSM = new LinkSeqTransferSM();
            seqTransferSM.Id = Id;
            seqTransferSM.Name = Description;
            seqTransferSM.Type = FlowChartType;
            propertyModel = seqTransferSM;
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
                propertyModel = (LinkSeqTransferSM)value;
                Description = propertyModel.Name;
            }
        }

        internal override void SetSerializeAttributes()
        {
            SerializeAttributes = new List<string> { "Left", "Top", "Width", "Height", "Id",
                "FlowChartType", "Description", "CloneSourceId" ,"IsManualSetted",
                "StartPnt_Position_X","StartPnt_Position_Y",
                "StartPnt_DockedFlag","StartPnt_RelatedShapeId","StartPnt_RelatedType",
                "EndPnt_Position_X","EndPnt_Position_Y",
                "EndPnt_DockedFlag","EndPnt_RelatedShapeId","EndPnt_RelatedType","ShapePnt"};
            //还需要加ShapePnt,ShapePnt为点的集合，复杂属性
        }



    }
}
