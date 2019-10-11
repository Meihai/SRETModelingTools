using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelingToolsAppWithMVVM.Common.ChartCommon
{
    public delegate void delSetShapeDetail(string shapeType, string shapeId);
    public delegate void delOpenFlowChart();
    public delegate void delNewFlowChart();
    public delegate void delEditFlowChart();
    public delegate void delDeleteRoute();
    public delegate void delAfterLoadData();  

    public delegate void delShapeSelected(IFlowChartBase flowChartBase);
    public class ChartDelegateDefines
    {
       
    }
}
