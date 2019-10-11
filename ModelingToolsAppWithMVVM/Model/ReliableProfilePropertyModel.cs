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
    public class ReliableProfilePropertyModel:ObservableObject
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
                    ReliableProfileSM insSM = (ReliableProfileSM)SmModel.PropertyModel;
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
        private ShapeReliableProfile smModel;

        public ShapeReliableProfile SmModel
        {
            get { return smModel; }
            set { smModel = value; }
        }

        public ReliableProfilePropertyModel()
        {
            id = Guid.NewGuid().ToString();
            Name = "";
            Type = FlowChartTypes.ShapeReliableProfile;
        }

        public ReliableProfilePropertyModel(ShapeReliableProfile profileModel)
        {
            Id = ((ReliableProfileSM)profileModel.PropertyModel).Id;
            Name = ((ReliableProfileSM)profileModel.PropertyModel).Name;
            Type = ((ReliableProfileSM)profileModel.PropertyModel).Type;
            SmModel = profileModel;
        }
    }
}
