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
    public class LinkBroken : LinkBase
    {
        private LineCtrlPoint LineCtrlPnt;

        /// <summary>
        /// 搜寻到的路径
        /// </summary>
        private PointCollection SearchedPath = new PointCollection();

        public LinkBroken()
            : base()
        {
            LinkLineType = LinkLineTypes.Broken;
            LineType=LineTypes.Solid;
        }

        public LinkBroken(LineTypes lineType) : base() {
            LinkLineType = LinkLineTypes.Broken;
            LineType = lineType;
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
            LinkBroken s = new LinkBroken();
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

            SearchedPath.Clear();
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
           // ShapePnt.Add(new Point(6, 40));
           // ShapePnt.Add(new Point(Width - 6, 40));
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
            double r = 0;
            int i = ShapePnt.Count - 1;
            int j = i - 1;
            if (ShapePnt[i].X == ShapePnt[j].X && ShapePnt[i].Y >= ShapePnt[j].Y)
            {
                r = 0;
            }
            else if (ShapePnt[i].X == ShapePnt[j].X && ShapePnt[i].Y < ShapePnt[j].Y)
            {
                r = 180;
            }
            else if (ShapePnt[i].X >= ShapePnt[j].X && ShapePnt[i].Y == ShapePnt[j].Y)
            {
                r = 270;
            }
            else if (ShapePnt[i].X < ShapePnt[j].X && ShapePnt[i].Y == ShapePnt[j].Y)
            {
                r = 90;
            }

            Transform t = new RotateTransform(r, EndPnt.Position.X, EndPnt.Position.Y);
            this.pathEnd.RenderTransform = t;
        }

        public override LineCtrlPoint GetLineCtrlPoints()
        {
            LineCtrlPnt = new LineCtrlPoint(ShapePnt[0], LinePointTypes.StartPoint, CtrlNodeTypes.START);

            for (int i = 0; i < ShapePnt.Count - 1; i++)
            {
                //不在这里过滤距离小于20的控制点，是为了方便移动移动拐点时的判断。过滤距离小于20的控制点，放到CtrlLine中处理
                if (ShapePnt[i].X == ShapePnt[i + 1].X)// && Math.Abs(ShapePnt[i].Y - ShapePnt[i + 1].Y) > 20)
                {
                    LineCtrlPnt.Append(ShapePnt[i].X, (ShapePnt[i].Y + ShapePnt[i + 1].Y) / 2.0, LinePointTypes.MiddlePoint, CtrlNodeTypes.LEFT);
                   
                }
                else if (ShapePnt[i].Y == ShapePnt[i + 1].Y)// && Math.Abs(ShapePnt[i].X - ShapePnt[i + 1].X) > 20)
                {
                    LineCtrlPnt.Append((ShapePnt[i].X + ShapePnt[i + 1].X) / 2.0, ShapePnt[i].Y, LinePointTypes.MiddlePoint, CtrlNodeTypes.TOP);                   
                }

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
        /// 移动过程中是否插入了转折点
        /// </summary>
        bool IsInsertedPrev = false;
        bool IsInsertedNext = false;
        /// <summary>
        /// 位置关系，0:水平；1:垂直
        /// </summary>
        int LocationFlagPrev = 0;
        int LocationFlagNext = 0;
        /// <summary>
        /// 当前移动的控制点
        /// </summary>
        LineCtrlPoint CurPnt = null;
        public override void CreateTmpLine(CtrlNode ctrlNode, Point position)
        {
            StreamGeometry geometry = new StreamGeometry();
            geometry.FillRule = FillRule.EvenOdd;
            
            
            #region 端点移动

            if ((ctrlNode.CtrlNodeType == CtrlNodeTypes.START || ctrlNode.CtrlNodeType == CtrlNodeTypes.END))
            {
                //起点
                Point start0 = new Point(StartPnt.Position.X + Margin.Left, StartPnt.Position.Y + Margin.Top);
                //如果起点停靠在形状上，则起点后的第一点位置也固定
                Point start1 = new Point(StartPnt.Position.X + Margin.Left, StartPnt.Position.Y + Margin.Top);
                //终点
                Point end0 = new Point(EndPnt.Position.X + Margin.Left, EndPnt.Position.Y + Margin.Top);
                //如果终点停靠在形状上，则终点前的第一点位置也固定
                Point end1 = new Point(EndPnt.Position.X + Margin.Left, EndPnt.Position.Y + Margin.Top);

                //确定变化点的坐标
                if (ctrlNode.CtrlNodeType == CtrlNodeTypes.START)
                {
                    start0 = new Point(position.X + Margin.Left, position.Y + Margin.Top);
                }

                if (ctrlNode.CtrlNodeType == CtrlNodeTypes.END)
                {
                    end0 = new Point(position.X + Margin.Left, position.Y + Margin.Top);
                }

                GetSecondTerminalPoint(start0, end0, out start1, out end1);

                SearchedPath = RoadSearch.StartRoadSearch(start0, end0, start1, end1);

                if (null == SearchedPath || 0 == SearchedPath.Count)
                {
                    return;
                }

                using (StreamGeometryContext ctx = geometry.Open())
                {
                    ctx.BeginFigure(new Point(SearchedPath[0].X, SearchedPath[0].Y), false, false);
                    
                    for (int i = 1; i < SearchedPath.Count; i++)
                    {
                        ctx.LineTo(new Point(SearchedPath[i].X, SearchedPath[i].Y), true, false);
                    }
                }
            }

            #endregion
            else
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

                #region 中间点左右移动
                if (CurPnt.CtrlNodeType == CtrlNodeTypes.LEFT)
                {
                    if (!IsInsertedPrev) //开始拖动，未插入新点
                    {
                        if (CurPnt.Prev.PointTypes == LinePointTypes.StartPoint)
                        {
                            if (StartPnt.DockedFlag)  //起点已经停靠，需插入两个转折点
                            {
                                LineCtrlPoint lcp = new LineCtrlPoint(new Point(position.X, (CurPnt.Prev.Position.Y + position.Y) / 2.0),
                                    LinePointTypes.BreakPoint, CtrlNodeTypes.ALL);

                                LineCtrlPoint cp2 = new LineCtrlPoint(new Point(StartPnt.Position.X, (CurPnt.Prev.Position.Y + position.Y) / 2.0),
                                    LinePointTypes.BreakPoint, CtrlNodeTypes.ALL);

                                CurPnt.Insert(lcp, 0);
                                lcp.Insert(cp2, 0);

                                IsInsertedPrev = true;
                            }
                            else  //起点未停靠，直接移动起点
                            {
                                StartPnt.Position = new Point(position.X, StartPnt.Position.Y);
                            }
                        }

                        if (CurPnt.Next.PointTypes == LinePointTypes.EndPoint)
                        {
                            if (EndPnt.DockedFlag)
                            {
                                LineCtrlPoint lcp = new LineCtrlPoint(new Point(position.X, (CurPnt.Next.Position.Y + position.Y) / 2.0),
                                   LinePointTypes.BreakPoint, CtrlNodeTypes.ALL);

                                LineCtrlPoint cp2 = new LineCtrlPoint(new Point(EndPnt.Position.X, (CurPnt.Next.Position.Y + position.Y) / 2.0),
                                    LinePointTypes.BreakPoint, CtrlNodeTypes.ALL);

                                CurPnt.Insert(lcp, 1);
                                lcp.Insert(cp2, 1);

                                IsInsertedPrev = true;
                            }
                            else
                            {
                                EndPnt.Position = new Point(position.X, EndPnt.Position.Y);
                            }
                        }

                        //中点的前一点和后一点不是端点，则直接移动相连的转折点
                        CurPnt.Prev.Position = new Point(position.X, CurPnt.Prev.Position.Y);
                        CurPnt.Next.Position = new Point(position.X, CurPnt.Next.Position.Y);
                    }
                    else  //已经有新插入点，则直接移动插入点的位置
                    {
                        CurPnt.Prev.Position = new Point(position.X, CurPnt.Prev.Position.Y);
                        CurPnt.Next.Position = new Point(position.X, CurPnt.Next.Position.Y);
                    }
                }

                #endregion

                #region 中间点上下移动
                else if (CurPnt.CtrlNodeType == CtrlNodeTypes.TOP)
                {
                    if (!IsInsertedPrev)
                    {
                        if (CurPnt.Prev.PointTypes == LinePointTypes.StartPoint)
                        {
                            if (StartPnt.DockedFlag)
                            {
                                LineCtrlPoint lcp = new LineCtrlPoint(new Point((CurPnt.Prev.Position.X + position.X) / 2.0, position.Y),
                                    LinePointTypes.BreakPoint, CtrlNodeTypes.ALL);

                                LineCtrlPoint cp2 = new LineCtrlPoint(new Point((CurPnt.Prev.Position.X + position.X) / 2.0, StartPnt.Position.Y),
                                    LinePointTypes.BreakPoint, CtrlNodeTypes.ALL);

                                CurPnt.Insert(lcp, 0);
                                lcp.Insert(cp2, 0);

                                IsInsertedPrev = true;
                            }
                            else
                            {
                                StartPnt.Position = new Point(StartPnt.Position.X, position.Y);
                            }
                        }

                        if (CurPnt.Next.PointTypes == LinePointTypes.EndPoint)
                        {
                            if (EndPnt.DockedFlag)
                            {
                                LineCtrlPoint lcp = new LineCtrlPoint(new Point((CurPnt.Next.Position.X + position.X) / 2.0, position.Y),
                                   LinePointTypes.BreakPoint, CtrlNodeTypes.ALL);

                                LineCtrlPoint cp2 = new LineCtrlPoint(new Point((CurPnt.Next.Position.X + position.X) / 2.0, EndPnt.Position.Y),
                                    LinePointTypes.BreakPoint, CtrlNodeTypes.ALL);

                                CurPnt.Insert(lcp, 1);
                                lcp.Insert(cp2, 1);

                                IsInsertedPrev = true;
                            }
                            else
                            {
                                EndPnt.Position = new Point(EndPnt.Position.X, position.Y);
                            }
                        }

                        CurPnt.Prev.Position = new Point(CurPnt.Prev.Position.X, position.Y);
                        CurPnt.Next.Position = new Point(CurPnt.Next.Position.X, position.Y);
                    }
                    else
                    {
                        CurPnt.Prev.Position = new Point(CurPnt.Prev.Position.X, position.Y);
                        CurPnt.Next.Position = new Point(CurPnt.Next.Position.X, position.Y);
                    }
                }

                #endregion

                #region 拐点移动
                else if (CurPnt.CtrlNodeType == CtrlNodeTypes.ALL)
                {
                    #region 拐点靠近起点

                    if (null != CurPnt.Prev && null != CurPnt.Prev.Prev &&
                        (CurPnt.Prev.Prev.PointTypes == LinePointTypes.StartPoint ||
                        CurPnt.Prev.Prev.Prev.PointTypes == LinePointTypes.StartPoint))
                    {
                        if (!IsInsertedPrev)  //开始拖动拐点，准备插入新点
                        {
                            IsInsertedPrev = true;
                            LineCtrlPoint lcp;

                            if (CurPnt.Position.Y == CurPnt.Prev.Position.Y) //水平
                            {
                                LocationFlagPrev = 0;
                                lcp = new LineCtrlPoint(new Point(CurPnt.Prev.Position.X, position.Y),
                                 LinePointTypes.BreakPoint, CtrlNodeTypes.ALL);
                            }
                            else  //拐点和起点垂直
                            {
                                LocationFlagPrev = 1;
                                lcp = new LineCtrlPoint(new Point(position.X, CurPnt.Prev.Position.Y),
                                 LinePointTypes.BreakPoint, CtrlNodeTypes.ALL);
                            }

                            CurPnt.Insert(lcp, 0);
                            lcp.Prev.PointTypes = LinePointTypes.BreakPoint;
                            lcp.Prev.CtrlNodeType = CtrlNodeTypes.ALL;
                        }
                        else  //已经开始拖动，已经插入新点
                        {
                            if (0 == LocationFlagPrev)
                            {
                                CurPnt.Prev.Position = new Point(CurPnt.Prev.Prev.Position.X, position.Y);
                            }
                            else
                            {
                                CurPnt.Prev.Position = new Point(position.X, CurPnt.Prev.Prev.Position.Y);
                            }
                        }



                        //如果当前点的下一个拐点不是终点，则直接处理
                        if (CurPnt.Next.Next.PointTypes == LinePointTypes.BreakPoint && !IsInsertedNext)
                        {
                            if (CurPnt.Position.Y == CurPnt.Next.Next.Position.Y)
                            {
                                //和下一拐点水平，则下一拐点只上下移动
                                CurPnt.Next.Next.Position = new Point(CurPnt.Next.Next.Position.X, position.Y);
                            }
                            else
                            {
                                //和下一拐点垂直，则下一拐点只水平移动
                                CurPnt.Next.Next.Position = new Point(position.X, CurPnt.Next.Next.Position.Y);
                            }
                        }
                    }

                    #endregion

                    #region 拐点靠近终点

                    if (null != CurPnt.Next && null != CurPnt.Next.Next &&
                        (CurPnt.Next.Next.PointTypes == LinePointTypes.EndPoint ||
                        CurPnt.Next.Next.Next.PointTypes == LinePointTypes.EndPoint))
                    {
                        if (!IsInsertedNext)
                        {
                            IsInsertedNext = true;
                            LineCtrlPoint lcp;
                            if (CurPnt.Position.X == CurPnt.Next.Position.X)
                            {
                                LocationFlagNext = 1;
                                lcp = new LineCtrlPoint(new Point(position.X, CurPnt.Next.Position.Y),
                                 LinePointTypes.BreakPoint, CtrlNodeTypes.ALL);
                            }
                            else
                            {
                                LocationFlagNext = 0;
                                lcp = new LineCtrlPoint(new Point(CurPnt.Next.Position.X, position.Y),
                                 LinePointTypes.BreakPoint, CtrlNodeTypes.ALL);
                            }

                            CurPnt.Insert(lcp, 1);
                            lcp.Next.PointTypes = LinePointTypes.BreakPoint;
                            lcp.Next.CtrlNodeType = CtrlNodeTypes.ALL;

                        }
                        else
                        {
                            if (0 == LocationFlagNext)
                            {
                                CurPnt.Next.Position = new Point(CurPnt.Next.Next.Position.X, position.Y);
                            }
                            else
                            {
                                CurPnt.Next.Position = new Point(position.X, CurPnt.Next.Next.Position.Y);
                            }
                        }
                        
                        //如果当前点的前一个拐点不是起点，直接处理
                        if (CurPnt.Prev.Prev.PointTypes == LinePointTypes.BreakPoint && !IsInsertedPrev)
                        {
                            if (CurPnt.Position.Y == CurPnt.Prev.Prev.Position.Y)
                            {
                                CurPnt.Prev.Prev.Position = new Point(CurPnt.Prev.Prev.Position.X, position.Y);
                            }
                            else
                            {
                                CurPnt.Prev.Prev.Position = new Point(position.X, CurPnt.Prev.Prev.Position.Y);
                            }
                        }
                    }

                    #endregion

                    #region 中间的拐点

                    if (CurPnt.Prev.Prev.PointTypes == LinePointTypes.BreakPoint &&
                        CurPnt.Next.Next.PointTypes == LinePointTypes.BreakPoint &&
                        !IsInsertedPrev && !IsInsertedNext)
                    {
                        if (CurPnt.Position.Y == CurPnt.Prev.Prev.Position.Y)
                        {
                            CurPnt.Prev.Prev.Position = new Point(CurPnt.Prev.Prev.Position.X, position.Y);
                        }
                        else
                        {
                            CurPnt.Prev.Prev.Position = new Point(position.X, CurPnt.Prev.Prev.Position.Y);
                        }

                        if (CurPnt.Position.Y == CurPnt.Next.Next.Position.Y)
                        {
                            CurPnt.Next.Next.Position = new Point(CurPnt.Next.Next.Position.X, position.Y);
                        }
                        else
                        {
                            CurPnt.Next.Next.Position = new Point(position.X, CurPnt.Next.Next.Position.Y);
                        }
                    }

                    #endregion

                }
                #endregion

                //最后修改当前点的坐标
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
            ShapePnt.Clear();
            LineCtrlPoint tmp = LineCtrlPnt;
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

            //路径转换为形状点
            if (null != SearchedPath && SearchedPath.Count > 0)
            {
                ShapePnt.Clear();

                for (int i = 0; i < SearchedPath.Count; i++)
                {
                    if (i > 0)
                    {
                        if (SearchedPath[i].X == SearchedPath[i - 1].X && SearchedPath[i].Y == SearchedPath[i - 1].Y)
                        {
                            continue;
                        }
                    }

                    ShapePnt.Add(new Point(SearchedPath[i].X - this.Margin.Left, SearchedPath[i].Y - this.Margin.Top));
                }
            }
            else
            {
                SetShapePntByCtrlPnt();
            }

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
            //如果起点停靠在形状上，则起点后的第一点位置也固定
            Point start1 = new Point(StartPnt.Position.X + Margin.Left, StartPnt.Position.Y + Margin.Top);
            //终点
            Point end0 = new Point(EndPnt.Position.X + Margin.Left, EndPnt.Position.Y + Margin.Top);
            //如果终点停靠在形状上，则终点前的第一点位置也固定
            Point end1 = new Point(EndPnt.Position.X + Margin.Left, EndPnt.Position.Y + Margin.Top);

            GetSecondTerminalPoint(start0, end0, out start1, out end1);

            SearchedPath = RoadSearch.StartRoadSearch(start0, end0, start1, end1);

            //路径转换为形状点
            if (null != SearchedPath && SearchedPath.Count > 0)
            {
                ShapePnt.Clear();

                for (int i = 0; i < SearchedPath.Count; i++)
                {
                    ShapePnt.Add(new Point(SearchedPath[i].X - this.Margin.Left, SearchedPath[i].Y - this.Margin.Top));
                }
            }

            CreateShape();
        }

        /// <summary>
        /// 获得靠近端点的点坐标
        /// </summary> 
        /// <param name="start1"></param> 
        /// <param name="end1"></param>
        private void GetSecondTerminalPoint(Point start0, Point end0, out Point start1, out Point end1)
        {
            start1 = new Point(start0.X, start0.Y);
            end1 = new Point(end0.X, end0.Y);

            //根据端点的停靠属性，确定靠近端点的点的坐标
            if (StartPnt.DockedFlag)
            {
                switch (StartPnt.RelatedType)
                {
                    case LinkNodeTypes.LEFT:
                        start1 = new Point(start0.X - 20, start0.Y);
                        break;
                    case LinkNodeTypes.TOP:
                        start1 = new Point(start0.X, start0.Y - 20);
                        break;
                    case LinkNodeTypes.RIGHT:
                        start1 = new Point(start0.X + 20, start0.Y);
                        break;
                    case LinkNodeTypes.BOTTOM:
                        start1 = new Point(start0.X, start0.Y + 20);
                        break;
                    case LinkNodeTypes.CENTER:
                        break;
                    case LinkNodeTypes.NULL:
                        break;
                    default:
                        break;
                }
            }

            if (EndPnt.DockedFlag)
            {
                switch (EndPnt.RelatedType)
                {
                    case LinkNodeTypes.LEFT:
                        end1 = new Point(end0.X - 20, end0.Y);
                        break;
                    case LinkNodeTypes.TOP:
                        end1 = new Point(end0.X, end0.Y - 20);
                        break;
                    case LinkNodeTypes.RIGHT:
                        end1 = new Point(end0.X + 20, end0.Y);
                        break;
                    case LinkNodeTypes.BOTTOM:
                        end1 = new Point(end0.X, end0.Y + 20);
                        break;
                    case LinkNodeTypes.CENTER:
                        break;
                    case LinkNodeTypes.NULL:
                        break;
                    default:
                        break;
                }
            }
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

            GetSecondTerminalPoint(start0, end0, out start1, out end1);

            if (!IsManualSetted)
            {
                SearchedPath = RoadSearch.StartRoadSearch(start0, end0, start1, end1);
                //路径转换为形状点
                if (null != SearchedPath && SearchedPath.Count > 0)
                {
                    ShapePnt.Clear();

                    for (int i = 0; i < SearchedPath.Count; i++)
                    {
                        ShapePnt.Add(new Point(SearchedPath[i].X - this.Margin.Left, SearchedPath[i].Y - this.Margin.Top));
                    }
                }

            }

            CreateShape();
        }

        public override FlowChartTypes FlowChartType
        {
            get { return FlowChartTypes.LinkBroken; }
        }


    }
}
