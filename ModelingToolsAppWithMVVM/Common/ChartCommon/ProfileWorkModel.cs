using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ModelingToolsAppWithMVVM.Common.ChartCommon
{
    /// <summary>
    /// 剖面建模工作区
    /// </summary>
    public class ProfileWorkModel:BaseWorkModel
    {
        
        public ProfileWorkModel()
            : base()
        {
            this.CommandBindings.Add(new CommandBinding(ApplicationCommands.Delete, Delete_Executed, Delete_Enabled));           

        }

        /// <summary>
        /// 连接线类型
        /// </summary>
        private bool isSeqType=true;
        protected override void canvasDragEnter(object sender, System.Windows.DragEventArgs e)
        {
            if (null == DragIfcb)
            {
                switch ((FlowChartTypes)(e.Data.GetData(typeof(FlowChartTypes))))
                {
                   
                    case FlowChartTypes.ShapeProbTransfer:
                    {
                        DragIfcb = new LinkProbTransfer();
                        DragIfcb.CreateShape();
                        break;
                    }

                    case FlowChartTypes.ShapeSeqTransfer:
                    {
                        DragIfcb = new LinkSeqTransfer();
                        DragIfcb.CreateShape();
                        break;
                    }

                    case FlowChartTypes.ShapeEntrance:
                    {
                        DragIfcb = new ShapeEntrance();
                        DragIfcb.CreateShape();
                        break;
                    }

                    case FlowChartTypes.ShapeExit:
                    {
                        DragIfcb = new ShapeExit();
                        DragIfcb.CreateShape();
                        break;
                    }

                    case FlowChartTypes.ShapeTerminal:
                    {
                        DragIfcb = new ShapeTerminal();
                        DragIfcb.CreateShape();
                        break;
                    }

                    case FlowChartTypes.ShapeOperation:
                    {
                        DragIfcb = new ShapeOperation();
                        DragIfcb.CreateShape();
                        break;
                    }

                    case FlowChartTypes.ShapeReliableProfile:
                    {
                        DragIfcb = new ShapeReliableProfile();
                        DragIfcb.CreateShape();
                        break;
                    }                   

                    default:
                        break;
                }

                if (null != DragIfcb)
                {
                    DragIfcb.ChangePositionAndSize(e.GetPosition(this).X - DragIfcb.Width / 2 - DragIfcb.Margin.Left,
                                                    e.GetPosition(this).Y - DragIfcb.Height / 2 - DragIfcb.Margin.Top, 0, 0);
                    this.Children.Add((System.Windows.Controls.UserControl)DragIfcb);

                    if (DragIfcb is ShapeBase)
                    {
                        ((ShapeBase)DragIfcb).evtDocketLinkNode += new delDockedLinkNode(FlowChartEditor_evtDocketLinkNode);
                    }
                }
            }
        }

        /// <summary>
        /// 重写鼠标松开事件，生成
        /// </summary>
        /// <param name="e"></param>
        protected override void DealWithLinkNodeEvent()
        {
            if (IsLinkNodeDown)
            {
                IsLinkNodeDown = false;
                if (dragStartPoint.HasValue && dragEndPoint.HasValue)
                {
                    if (isSeqType)
                    {
                        tmpLink = new LinkSeqTransfer((Point)dragStartPoint, (Point)dragEndPoint);
                    }
                    else
                    {
                        tmpLink = new LinkProbTransfer((Point)dragStartPoint, (Point)dragEndPoint);
                    }

                    if (linkStartRelatedShapeId != string.Empty)
                    {
                        tmpLink.StartPnt.RelatedShapeId = linkStartRelatedShapeId;
                        tmpLink.StartPnt.RelatedType = startLinkNodeType;
                        tmpLink.StartPnt.DockedFlag = true;
                    }
                    if (linkEndRelatedShapeId != string.Empty)
                    {
                        tmpLink.EndPnt.RelatedShapeId = linkEndRelatedShapeId;
                        tmpLink.EndPnt.RelatedType = endLinkNodeType;
                        tmpLink.EndPnt.DockedFlag = true;
                    }
                    AddShapes(tmpLink);
                    linkStartRelatedShapeId = "";
                    linkEndRelatedShapeId = "";
                    tmpLink = null;
                }

            }
        }
        
    }
}
