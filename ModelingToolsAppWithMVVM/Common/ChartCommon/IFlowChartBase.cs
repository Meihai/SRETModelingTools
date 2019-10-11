using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Xml;

namespace ModelingToolsAppWithMVVM.Common.ChartCommon
{
    public delegate void delMouseDoubleClick(IFlowChartBase ifcb,string description);
    public delegate void delMouseClick(IFlowChartBase ifcb);
    public  interface IFlowChartBase:ICloneable,INotifyPropertyChanged
    {
        /// <summary>
        /// 编号
        /// </summary>
        string Id
        {
            get;
            set;
        }

        /// <summary>
        /// 克隆源的ID
        /// </summary>
        string CloneSourceId
        {
            get;
        }



        /// <summary>
        /// 说明
        /// </summary>
        string Description
        {
            get;
            set;
        }

        /// <summary>
        /// 是否处于选中状态
        /// </summary>
        bool IsSelected
        {
            get;
            set;
        }

        /// <summary>
        /// 是否处于多选状态
        /// </summary>
        bool IsMultiSelected
        {
            get;
            set;
        }

        /// <summary>
        /// 形状的范围，东南西北四个方向的最大值
        /// </summary>
        PointCollection Range
        {
            get;
        }

        /// <summary>
        /// 形状上可以与其它形状关联的点的位置
        /// </summary>
        List<SimpleLinkNode> LinkNodes
        {
            get;
        }


        FlowChartTypes FlowChartType
        {
            get;
        }

        Point Offset
        {
            get;
        }

        Thickness Margin
        {
            get;
            set;
        }

        double Height
        {
            get;
            set;
        }

        double Width
        {
            get;
            set;
        }

        /// <summary>
        /// 在形状上单击鼠标时触发的事件
        /// </summary>
        event delMouseClick evtMouseClick;

        /// <summary>
        /// 在形状上双击击鼠标时触发的事件
        /// </summary>
        event delMouseDoubleClick evtMouseDoubleClick;

        /// <summary>
        /// 创建形状
        /// </summary>
        void CreateShape();

        /// <summary>
        /// 改变位置和尺寸
        /// </summary>
        /// <param name="difLeft"></param>
        /// <param name="difTop"></param>
        /// <param name="difWidth"></param>
        /// <param name="difHeight"></param>
        void ChangePositionAndSize(double difLeft, double difTop, double difWidth, double difHeight);

        /// <summary>
        /// 处理其它形状移动时对本身的影响
        /// </summary>
        /// <param name="ifcb"></param>
        void AcceptShapeMove(IFlowChartBase ifcb);

        /// <summary>
        /// 处理其它形状的控制点在自身上面的移动
        /// </summary>
        /// <param name="point"></param>
        /// <param name="seq"></param>
        void AcceptPointMove(Point point, int seq);

        /// <summary>
        /// 移动到克隆形状的位置
        /// </summary>
        /// <param name="cloneIfcb"></param>
        void MoveToClone(IFlowChartBase cloneIfcb);

        /// <summary>
        /// 获得说明文字的位置
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        bool GetDescriptionPosition(out Point position);

        /// <summary>
        /// 属性模型
        /// </summary>
        object PropertyModel
        {
            get;
            set;
        }

        /// <summary>
        /// 需要序列化的属性
        /// </summary>
        List<string> SerializeAttributes { get; }
              

    }
}
