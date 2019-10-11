using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelingToolsAppWithMVVM.Common
{
    public class DomainDefinition
    {
        private ValueTypes type;


        private string minValue;

      
        private string maxValue;



        public ValueTypes Type
        {
            get { return type; }
            set { type = value; }
        }


        public string MinValue
        {
            get { return minValue; }
            set { minValue = value; }
        }

        public string MaxValue
        {
            get { return maxValue; }
            set { maxValue = value; }
        }
    }
}
