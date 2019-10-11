using ModelingToolsAppWithMVVM.Common.ChartCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelingToolsAppWithMVVM.Common.ServiceModel
{
    /// <summary>
    /// 接口交互测试对象
    /// </summary>
    public class IITestedObjectSM
    {
        public IITestedObjectSM()
        {

        }

        private string id;
        public string Id
        {
            get { return id; }
            set { id = value; }
        }
        private string name;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        private FlowChartTypes type;

        public FlowChartTypes Type
        {
            get { return type; }
            set { type = value; }
        }
    }
}
