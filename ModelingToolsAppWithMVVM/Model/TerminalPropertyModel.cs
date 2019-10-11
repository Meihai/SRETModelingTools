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
    public class TerminalPropertyModel:ObservableObject
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

                    TerminalSM insSM = (TerminalSM)SmModel.PropertyModel;
                    insSM.Name = name;
                    SmModel.PropertyModel = insSM;
                }
            }
        }
        private FlowChartTypes type;

        public FlowChartTypes Type
        {
            get { return type; }
            set { type = value; }
        }
        private ShapeTerminal smModel;

        public ShapeTerminal SmModel
        {
            get { return smModel; }
            set { smModel = value; }
        }

        public TerminalPropertyModel()
        {
            id = Guid.NewGuid().ToString();
            name = "";
            type = FlowChartTypes.ShapeTerminal;

        }

        public TerminalPropertyModel(ShapeTerminal terminalModel)
        {
            Id = ((TerminalSM)terminalModel.PropertyModel).Id;
            Name = ((TerminalSM)terminalModel.PropertyModel).Name;
            Type = ((TerminalSM)terminalModel.PropertyModel).Type;
            SmModel = terminalModel;
        }
    }
}
