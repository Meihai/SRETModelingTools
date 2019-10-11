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
using System.Collections.ObjectModel;

namespace ModelingToolsApp.UserControls
{
    /// <summary>
    /// 路径搜索
    /// </summary>
    public static class RoadSearch
    {
        /// <summary>
        /// 构成临时连接线的点集
        /// </summary>
        static private PointCollection TmpLinePnts = new PointCollection();

        static private ObservableCollection<IFlowChartBase> _allShapes = new ObservableCollection<IFlowChartBase>();
        /// <summary>
        /// 流程图中所有的形状的范围
        /// </summary>
        static public ObservableCollection<IFlowChartBase> AllShapes
        {
            set { _allShapes = value; }
            private get { return _allShapes; }
        }

        /// <summary>
        /// 开始路径搜索
        /// </summary>
        /// <param name="start0"></param>
        /// <param name="end0"></param>
        /// <param name="start1"></param>
        /// <param name="end1"></param>
        static public PointCollection StartRoadSearch(Point start0, Point end0, Point start1, Point end1)
        {
            TmpLinePnts.Clear();

            if (start0.X != start1.X || start0.Y != start1.Y)
            {
                TmpLinePnts.Add(start0);
            }

            SearchRoad(start1, end1, 0);

            if (end0.X != end1.X || end0.Y != end1.Y)
            {
                TmpLinePnts.Add(end0);
            }


            ClearInvalidPoint();

            return TmpLinePnts;
        }


        static private void ClearInvalidPoint()
        {
            //清理同一直线上的中间点
            for (int i = 0; i < TmpLinePnts.Count - 2; i++)
            {
                if ((TmpLinePnts[i].X == TmpLinePnts[i + 1].X && TmpLinePnts[i + 1].X == TmpLinePnts[i + 2].X) ||
                    (TmpLinePnts[i].Y == TmpLinePnts[i + 1].Y && TmpLinePnts[i + 1].Y == TmpLinePnts[i + 2].Y))
                {
                    TmpLinePnts[i + 1] = TmpLinePnts[i];
                }
            }

            //清理重复点
            for (int i = TmpLinePnts.Count - 1; i > 0; i--)
            {
                if (TmpLinePnts[i].X == TmpLinePnts[i - 1].X && TmpLinePnts[i].Y == TmpLinePnts[i - 1].Y)
                {
                    TmpLinePnts.RemoveAt(i);
                }
            }
        }


        /// <summary>
        /// 路径搜索
        /// </summary>
        /// <param name="start"></param>
        /// <param name="direction">上一步计算时优先考虑的方向，本次计算时，需优先考虑另外的方向</param>
        static private void SearchRoad(Point start, Point endPoint, int direction)
        {
            TmpLinePnts.Add(start);

            if (TmpLinePnts.Count > 50)   //已经出现问题，直连
            {
                TmpLinePnts.Add(new Point(endPoint.X, TmpLinePnts[50].Y));
                TmpLinePnts.Add(new Point(endPoint.X, endPoint.Y));
                //linkline.Points = tmpLinePnts;
                return;
            }

            Point p = new Point(start.X, start.Y);
            int directX = endPoint.X >= p.X ? 1 : -1;
            int directY = endPoint.Y >= p.Y ? 1 : -1;

            int dx = (int)Math.Abs(p.X - endPoint.X);
            int dy = (int)Math.Abs(p.Y - endPoint.Y);

            int xstop = 0;
            int ystop = 0;

            int directionEx = 0;

            if (p.X == endPoint.X && p.Y == endPoint.Y)
            {
                return;
            }

            while (p.X != endPoint.X || p.Y != endPoint.Y)
            {
                if (direction == 1)
                {
                    goto PRIORITY_Y;
                }
                else if (direction == 2)
                {
                    goto PRIORITY_X;
                }

            PRIORITY_X:
                if (xstop == 0)  //x轴扩展
                {
                    if (dx > 20) //x轴方向还没有靠近终点
                    {
                        p.X += 20 * directX;

                        if (PointInShapes(p))
                        {
                            p.X -= 20 * directX;
                            xstop = 1; //x方向已经受阻

                            if (p.X != start.X) //x方向已经存在位移 ，记录该点
                            {
                                break;
                            }
                            else
                            {
                                continue;  //x方向无位移 
                            }
                        }

                        dx = (int)Math.Abs(p.X - endPoint.X);
                        continue;
                    }
                    else if (0 < dx && dx <= 20)  // x轴方向已经靠近终点，但还存在距离
                    {
                        p.X = endPoint.X;
                        dx = 0;
                        break;
                    }
                }

            PRIORITY_Y:
                if (ystop == 0)  //y轴扩展
                {
                    if (dy > 20) //y轴方向还没有靠近终点
                    {
                        p.Y += 20 * directY;

                        if (PointInShapes(p))
                        {
                            p.Y -= 20 * directY;
                            ystop = 1;  //y方向已经受阻

                            if (p.Y != start.Y) //已经存在位移
                            {
                                break;
                            }
                            else
                            {
                                continue;   //y方向无位移 
                            }
                        }

                        dy = (int)Math.Abs(p.Y - endPoint.Y);
                        continue;
                    }
                    else if (0 < dy && dy <= 20)  // y轴方向已经靠近终点，但还存在距离
                    {
                        p.Y = endPoint.Y;
                        dy = 0;
                        break;
                    }
                }

                if (dx < 20 && dy < 20 && xstop == 0 && ystop == 0)   //两方向都没有扩展，且在20以内，到达目标
                {
                    TmpLinePnts.Add(new Point(p.X, endPoint.Y));
                    TmpLinePnts.Add(endPoint);
                    return;
                }
                else
                {
                    //if (dy < 20 && xstop == 1)  //在x方向受阻，y方向已经和终点靠近。需要改变y坐标，然后看是否可以穿过目前的形状
                    if (xstop == 1)  //在x方向受阻， 需要改变y坐标，然后看是否可以穿过目前的形状
                    {
                        if (PointInShapes(endPoint))
                        {
                            p.X = endPoint.X;
                            //p.Y = endPoint.Y;
                            break;
                        }


                        int f = 1;
                        int dis = 5;
                        while (xstop == 1)
                        {
                            p.Y += dis * f;
                            Point tmp = new Point(p.X + 20 * directX, p.Y);
                            xstop = PointInShapes(tmp) ? 1 : 0;

                            f = f * -1;
                            dis += 5;
                        }

                        TmpLinePnts.Add(new Point(p.X, p.Y));  //得到可以避开形状的点

                        //p.X = p.X + 20 * directX; //下一个点位避开形状后的点

                        directionEx = 2;

                        break;
                    }


                    //if (dx<20 && ystop == 1)  //在y方向受阻，x方向已经和终点靠近。需要改变x坐标，然后看是否可以穿过目前的形状
                    if (ystop == 1)    //在y方向受阻， 需要改变x坐标，然后看是否可以穿过目前的形状
                    {
                        if (PointInShapes(endPoint))
                        {
                            //p.X = endPoint.X;
                            p.Y = endPoint.Y;
                            break;
                        }

                        int f = 1;
                        int dis = 5;
                        while (ystop == 1)
                        {
                            p.X += dis * f;
                            Point tmp = new Point(p.X, p.Y + 20 * directY);
                            ystop = PointInShapes(tmp) ? 1 : 0;

                            f = f * -1;
                            dis += 5;
                        }

                        TmpLinePnts.Add(new Point(p.X, p.Y));//得到可以避开形状的点

                        //p.Y = p.Y + 20 * directY; //下一个点位避开形状后的点

                        directionEx = 1;

                        break;
                    }
                }
            }

            SearchRoad(p, endPoint, directionEx);

        }


        /// <summary>
        /// 判断点是否在某个形状内
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        static private bool PointInShapes(Point p)
        {
            bool res = false;

            for (int i = 0; i < AllShapes.Count; i++)
            {
                if (AllShapes[i] is ShapeBase)
                {
                    if (PointIsInPolygon(p, ((ShapeBase)AllShapes[i]).Range))
                    {
                        res = true;
                        break;
                    }
                }
            }

            return res;
        }



        /// <summary>
        /// 判断点是否在一个面内
        /// </summary>
        /// <param name="p"></param>
        /// <param name="points"></param>
        /// <returns></returns>
        static private bool PointIsInPolygon(Point p, PointCollection points)
        {
            int counter = 0;
            int i = 0;
            double xinters;
            Point p1;
            Point p2;
            int pcount = points.Count;
            p1 = points[0];
            for (i = 1; i <= pcount; i++)
            {
                p2 = points[i % pcount];
                if (p.Y > Math.Min(p1.Y, p2.Y))
                {
                    if (p.Y <= Math.Max(p1.Y, p2.Y))
                    {
                        if (p.X <= Math.Max(p1.X, p2.X))
                        {
                            if (p1.Y != p2.Y)
                            {
                                xinters = (int)(((float)(p.Y - p1.Y) * (p2.X - p1.X)) / ((float)(p2.Y - p1.Y)) + p1.X);
                                if (p1.X == p2.X || p.X <= xinters)
                                    counter++;
                            }
                        }
                    }
                }
                p1 = p2;
            }
            if (counter % 2 == 0)
                return false;
            else
                return true;
        }
    }
}
