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
   public  class OperationPropertyModel:ObservableObject
    {
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
            set { 
                name = value;
                if (SmModel != null)
                {
                    OperationSM insSM = (OperationSM)SmModel.PropertyModel;
                    insSM.Name = name;
                    SmModel.PropertyModel = insSM;
                }
                RaisePropertyChanged(() => Name);
            }
        }
        private FlowChartTypes type;

        public FlowChartTypes Type
        {
            get { return type; }
            set { type = value; }
        }

        private ShapeOperation smModel;

        public ShapeOperation SmModel
        {
            get { return smModel; }
            set { smModel = value; }
        }


       public OperationPropertyModel() {
           Id = Guid.NewGuid().ToString();
           Name = "";
           Type = FlowChartTypes.ShapeOperation;
       }

       public OperationPropertyModel(ShapeOperation operationModel)
       {
           Id = ((OperationSM)operationModel.PropertyModel).Id;
           Name = ((OperationSM)operationModel.PropertyModel).Name;
           Type = ((OperationSM)operationModel.PropertyModel).Type;
           SmModel = operationModel;
       }

    }
}
