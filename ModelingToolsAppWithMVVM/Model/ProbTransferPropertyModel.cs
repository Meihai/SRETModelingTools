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
    public class ProbTransferPropertyModel:ObservableObject
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
                    LinkProbabilityTransferSM insSM =(LinkProbabilityTransferSM) SmModel.PropertyModel;
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
        private LinkProbTransfer smModel;

        public LinkProbTransfer SmModel
        {
            get { return smModel; }
            set { smModel = value; }
        }


        public ProbTransferPropertyModel()
        {
            id = Guid.NewGuid().ToString();
            name = "";
            type = FlowChartTypes.ShapeProbTransfer;
        }

        public ProbTransferPropertyModel(LinkProbTransfer probTransferModel) {
            id = ((LinkProbabilityTransferSM)probTransferModel.PropertyModel).Id;
            name = ((LinkProbabilityTransferSM)probTransferModel.PropertyModel).Name;
            type = ((LinkProbabilityTransferSM)probTransferModel.PropertyModel).Type;
            SmModel = probTransferModel;
        }

    }
}
