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
    public class ExitPropertyModel:ObservableObject
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
                    
                    ExitSM insSM = (ExitSM)SmModel.PropertyModel;
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

        private ShapeExit smModel;

        public ShapeExit SmModel
        {
            get { return smModel; }
            set { smModel = value; }
        }


        public ExitPropertyModel() {
            id = Guid.NewGuid().ToString();
            name = "";
            type = FlowChartTypes.ShapeExit;
        }

        public ExitPropertyModel(ShapeExit shapeExit)
        {
            Id = ((ExitSM)shapeExit.PropertyModel).Id;
            Name=((ExitSM)shapeExit.PropertyModel).Name;
            Type = ((ExitSM)shapeExit.PropertyModel).Type;
            SmModel = shapeExit;
        }

    }
}
