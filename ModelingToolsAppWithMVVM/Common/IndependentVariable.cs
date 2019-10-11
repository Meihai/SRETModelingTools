using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelingToolsAppWithMVVM.Common
{
    /// <summary>
    /// 自变量类
    /// </summary>
    public class IndependentVariable
    {
        private string symbol;

      
        private double value;

       
        private DomainDefinition domainRange;


        public string Symbol
        {
            get { return symbol; }
            set { symbol = value; }
        }

        public double Value
        {
            get { return this.value; }
            set { this.value = value; }
        }

        public DomainDefinition DomainRange
        {
            get { return domainRange; }
            set { domainRange = value; }
        }


    }
}
