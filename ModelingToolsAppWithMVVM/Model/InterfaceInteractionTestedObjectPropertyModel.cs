using GalaSoft.MvvmLight;
using ModelingToolsAppWithMVVM.Common.ChartCommon;
using ModelingToolsAppWithMVVM.Common.ServiceModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelingToolsAppWithMVVM.Model
{
    public class InterfaceInteractionTestedObjectPropertyModel:ObservableObject
    {
        private string id;

        public string Id
        {
            get { return id; }
            set { id = value;
            RaisePropertyChanged(() => Id);
            }
        }
        private string name;

        public string Name
        {
            get { return name; }
            set { name = value;
                  RaisePropertyChanged(() => Name);
            }
        }
        private FlowChartTypes type;

        public FlowChartTypes Type
        {
            get { return type; }
            set { type = value;
                  RaisePropertyChanged(() => Type);
            }
        }

        public InterfaceInteractionTestedObjectPropertyModel() {
            id = Guid.NewGuid().ToString();
            name = "被测对象";
            type = FlowChartTypes.InterfaceTestedObject;
        }


        public InterfaceInteractionTestedObjectPropertyModel(IITestedObjectSM iiTestedObjectSM)
        {
            Id = iiTestedObjectSM.Id;
            Name = iiTestedObjectSM.Name;
            Type = iiTestedObjectSM.Type;
        }

    }
}
