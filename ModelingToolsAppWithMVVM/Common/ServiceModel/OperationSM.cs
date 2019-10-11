using ModelingToolsAppWithMVVM.Common.ChartCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelingToolsAppWithMVVM.Common.ServiceModel
{
    /// <summary>
    /// 操作业务模型
    /// </summary>
    public class OperationSM
    {
        public OperationSM() {
            id = Guid.NewGuid().ToString();
            inputs = new List<ValueSM>();
            outputs = new List<ValueSM>();
            preConditions = new List<ConstraintCondition>();
            postConditions = new List<ConstraintCondition>();
        }

        private List<ValueSM> inputs;

        public List<ValueSM> Inputs
        {
            get { return inputs; }
            set { inputs = value; }
        }

        private List<ValueSM> outputs;

        public List<ValueSM> Outputs
        {
            get { return outputs; }
            set { outputs = value; }
        }

        private List<ConstraintCondition> preConditions;
        private List<ConstraintCondition> postConditions;


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
        private RunMethod runMethod;

        public RunMethod RunMethod
        {
            get { return runMethod; }
            set { runMethod = value; }
        }
        private long startTime; //单位为ms

        public long StartTime
        {
            get { return startTime; }
            set { startTime = value; }
        }
        private long period; //单位ms

        public long Period
        {
            get { return period; }
            set { period = value; }
        }


        private FlowChartTypes type;

        public FlowChartTypes Type
        {
            get { return type; }
            set { type = value; }
        }

    }
}
