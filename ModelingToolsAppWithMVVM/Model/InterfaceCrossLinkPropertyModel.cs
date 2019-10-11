using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelingToolsAppWithMVVM.Common.ChartCommon;
using ModelingToolsAppWithMVVM.Common;
using ModelingToolsAppWithMVVM.Common.ServiceModel;

namespace ModelingToolsAppWithMVVM.Model
{
    public class InterfaceCrossLinkPropertyModel : ObservableObject
    {

        public InterfaceCrossLinkPropertyModel() {
            id = Guid.NewGuid().ToString();
            name = "连接线"+RandomStringBuilder.Create(4);
            type = FlowChartTypes.InterfaceInteractionLink;
        }

        public InterfaceCrossLinkPropertyModel(InterfaceInteractionLink iiLink
           )
        {
            id = ((IILinkSM)(iiLink.PropertyModel)).Id;
            name = ((IILinkSM)(iiLink.PropertyModel)).Name;
            type = ((IILinkSM)(iiLink.PropertyModel)).Type;
            SmModel = iiLink;
        }

        private InterfaceInteractionLink smModel;

        /// <summary>
        /// 业务模型,浅复制
        /// </summary>
        public InterfaceInteractionLink SmModel
        {
            get { return smModel; }
            set { smModel = value; }
        }



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
            set { name = value;
                  IILinkSM insSM = (IILinkSM)SmModel.PropertyModel;
                  insSM.Name = name;
                  SmModel.PropertyModel=insSM;
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

       
        
    }
}
