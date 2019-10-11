using ModelingToolsAppWithMVVM.Common.ChartCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelingToolsAppWithMVVM.Common.ServiceModel
{
    public class LinkProbabilityTransferSM
    {
        public LinkProbabilityTransferSM() {
            Id = Guid.NewGuid().ToString();
            Name = "";
            Type = FlowChartTypes.ShapeProbTransfer;

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
        private double probability;

        public double Probability
        {
            get { return probability; }
            set { probability = value; }
        }

        private FlowChartTypes type;

        public FlowChartTypes Type
        {
            get { return type; }
            set { type = value; }
        }

    }
}
