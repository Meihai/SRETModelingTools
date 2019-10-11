using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelingToolsAppWithMVVM.Common
{
    /// <summary>
    /// 概率事件
    /// </summary>
    public class ProbabilityEvent
    {
        private string name;    

        private double probability;


        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public double Probability
        {
            get { return probability; }
            set { probability = value; }
        }
    }
}
