using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelingToolsAppWithMVVM.Common.ServiceModel
{
    public class ConstraintCondition
    {
        public ConstraintCondition() { 
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

        private string description;

        public string Description
        {
            get { return description; }
            set { description = value; }
        }


        private string dataSourceId;

        public string DataSourceId
        {
            get { return dataSourceId; }
            set { dataSourceId = value; }
        }


        private string dataAtrribute;

        public string DataAtrribute
        {
            get { return dataAtrribute; }
            set { dataAtrribute = value; }
        }

        private ComparisonMode comparisonMode;

        public ComparisonMode ComparisonMode
        {
            get { return comparisonMode; }
            set { comparisonMode = value; }
        }


        private string expectValue;

        public string ExpectValue
        {
            get { return expectValue; }
            set { expectValue = value; }
        }

        private ValueTypes expectValueType;

        public ValueTypes ExpectValueType
        {
            get { return expectValueType; }
            set { expectValueType = value; }
        }



        

        
    }
}
