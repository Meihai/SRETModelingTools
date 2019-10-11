using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelingToolsAppWithMVVM.Common.ChartCommon;
using GalaSoft.MvvmLight;
using ModelingToolsAppWithMVVM.Common;
using ModelingToolsAppWithMVVM.Common.ServiceModel;


namespace ModelingToolsAppWithMVVM.Model
{
    public class InterfaceInteractionObjectModel:ObservableObject
    {
        private string id;

        public string Id
        {
            get { return id; }
            set { 
                id = value;
                RaisePropertyChanged(() => Id);
            }
        }
        private string name;

        public string Name
        {
            get { return name; }
            set { 
                name = value;
                RaisePropertyChanged(() => Name);
            }
        }
        private FlowChartTypes type;

        public FlowChartTypes Type
        {
            get { return type; }
            set {
                type = value;
                RaisePropertyChanged(() => Type);
            }
        }

        public InterfaceInteractionObjectModel() {
            id = Guid.NewGuid().ToString();
            name = "接口交互对象" + RandomStringBuilder.Create(4);
            type = FlowChartTypes.InterfaceInteractionObject;
        }


        public InterfaceInteractionObjectModel(IIObjectSM iiObjectSM)
        {
            id = iiObjectSM.Id;
            name = iiObjectSM.Name;
            type = iiObjectSM.Type;
        }
        
        
    }
}
