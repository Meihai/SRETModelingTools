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
    public class SeqTransferPropertyModel:ObservableObject
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
                    LinkSeqTransferSM insSM =(LinkSeqTransferSM) SmModel.PropertyModel;
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
        private LinkSeqTransfer smModel;

        public LinkSeqTransfer SmModel
        {
            get { return smModel; }
            set { smModel = value; }
        }

        public SeqTransferPropertyModel()
        {
            id = Guid.NewGuid().ToString();
            name = "";
            type = FlowChartTypes.ShapeSeqTransfer;
        }

        public SeqTransferPropertyModel(LinkSeqTransfer seqTransferModel)
        {
            id=((LinkSeqTransferSM)seqTransferModel.PropertyModel).Id;
            name=((LinkSeqTransferSM)seqTransferModel.PropertyModel).Name;
            type=((LinkSeqTransferSM)seqTransferModel.PropertyModel).Type;
            SmModel=seqTransferModel;
        }

        
      

    }
}
