using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ModelingToolsAppWithMVVM.Common.ChartCommon
{
    public class InterfaceInteractionWorkModel:BaseWorkModel
    {

        public InterfaceInteractionWorkModel()
            : base()
        {
            this.CommandBindings.Add(new CommandBinding(ApplicationCommands.Delete, Delete_Executed, Delete_Enabled));           

        }


        protected override void canvasDragEnter(object sender, System.Windows.DragEventArgs e)
        {
            if (null == DragIfcb)
            {
                switch ((FlowChartTypes)(e.Data.GetData(typeof(FlowChartTypes))))
                {
                    case FlowChartTypes.InterfaceInteractionLink: {
                        DragIfcb = new InterfaceInteractionLink();
                        DragIfcb.CreateShape();
                        break;
                    }
                    case FlowChartTypes.InterfaceInteractionObject:
                    {
                        DragIfcb = new InterfaceInteractionObject();
                        DragIfcb.CreateShape();
                        break;
                    }
                    case FlowChartTypes.InterfaceTestedObject:
                    {
                        DragIfcb = new InterfaceInteractionTestedObject();
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

    }
}
