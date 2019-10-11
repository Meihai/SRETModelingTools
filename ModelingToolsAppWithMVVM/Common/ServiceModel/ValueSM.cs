using ModelingToolsAppWithMVVM.Common.ChartCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelingToolsAppWithMVVM.Common.ServiceModel
{
    /// <summary>
    /// 变量业务模型
    /// </summary>
    public class ValueSM
    {
        public ValueSM()
        {
            id = Guid.NewGuid().ToString();
            name = "";
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

        private InjectedModeTypes injectMode;

        public InjectedModeTypes InjectMode
        {
            get { return injectMode; }
            set { injectMode = value; }
        }

        private string relatedMessageId;

        public string RelatedMessageId
        {
            get { return relatedMessageId; }
            set { relatedMessageId = value; }
        }

        private string relatedDataMeaningId;

        public string RelatedDataMeaningId
        {
            get { return relatedDataMeaningId; }
            set { relatedDataMeaningId = value; }
        }

        private ValueTriggerMethod triggerMethod;

        public ValueTriggerMethod TriggerMethod
        {
            get { return triggerMethod; }
            set { triggerMethod = value; }
        }

        private FunctionExpressionParser function;

        public FunctionExpressionParser Function
        {
            get { return function; }
            set { function = value; }
        }

        private double sampleFreq;

        public double SampleFreq
        {
            get { return sampleFreq; }
            set { sampleFreq = value; }
        }
    }
}
