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
    /// <summary>
    /// 入口属性模型
    /// </summary>
    public  class EntrancePropertyModel: ObservableObject
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
                    EntranceSM insSM = (EntranceSM)SmModel.PropertyModel;
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

        private ShapeEntrance smModel;

        public ShapeEntrance SmModel
        {
            get { return smModel; }
            set { smModel = value; }
        }


        public EntrancePropertyModel() {
            id = Guid.NewGuid().ToString();
            type = FlowChartTypes.ShapeEntrance;
            name = "";
        }

        public EntrancePropertyModel(ShapeEntrance entranceModel)
        {
            Id = ((EntranceSM)entranceModel.PropertyModel).Id;
            Name = ((EntranceSM)entranceModel.PropertyModel).Name;
            Type = ((EntranceSM)entranceModel.PropertyModel).Type;
            SmModel = entranceModel;
        }
    }
}
