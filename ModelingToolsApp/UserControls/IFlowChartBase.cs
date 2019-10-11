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
    public delegate void delMouseDoubleClick(IFlowChartBase ifcb, string description);
    public delegate void delMouseClick(IFlowChartBase ifcb);
   
    //public delegate void delMovePoints(IFlowChartBase ifcb);

    public interface IFlowChartBase : ICloneable, INotifyPropertyChanged
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
        /// 是否设置了流程属性
        /// </summary>
        bool IsSetProperty
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
        /// 转换为可保存的二进制数据
        /// </summary>
        /// <returns></returns>
        byte[] ToBytes();

        /// <summary>
        /// 由二进制数据生成流程形状
        /// </summary>
        /// <returns></returns>
        IFlowChartBase FromBytes(byte[] data, int index, out int length);
        /// <summary>
        


    }
}
