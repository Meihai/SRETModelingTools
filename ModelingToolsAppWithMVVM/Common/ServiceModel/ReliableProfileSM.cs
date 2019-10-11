using ModelingToolsAppWithMVVM.Common.ChartCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelingToolsAppWithMVVM.Common.ServiceModel
{
    public class ReliableProfileSM
    {
        public ReliableProfileSM()
        {
            Id = Guid.NewGuid().ToString();
            Name = "";
            Type = FlowChartTypes.ShapeReliableProfile;
        }

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
            set { name = value; }
        }

        private FlowChartTypes type;

        public FlowChartTypes Type
        {
            get { return type; }
            set { type = value; }
        }


        private List<EntranceSM> entranceSMList;

        public List<EntranceSM> EntranceSMList
        {
            get { return entranceSMList; }
            set { entranceSMList = value; }
        }

        private List<ExitSM> exitSMList;

        public List<ExitSM> ExitSMList
        {
            get { return exitSMList; }
            set { exitSMList = value; }
        }
        private List<TerminalSM> terminalSMList;

        public List<TerminalSM> TerminalSMList
        {
            get { return terminalSMList; }
            set { terminalSMList = value; }
        }
        private List<OperationSM> operationSMList;

        public List<OperationSM> OperationSMList
        {
            get { return operationSMList; }
            set { operationSMList = value; }
        }
        private List<ReliableProfileSM> reliableProfileSMList;

        public List<ReliableProfileSM> ReliableProfileSMList
        {
            get { return reliableProfileSMList; }
            set { reliableProfileSMList = value; }
        }
        private List<LinkSeqTransferSM> linkSeqTransferSMList;

        public List<LinkSeqTransferSM> LinkSeqTransferSMList
        {
            get { return linkSeqTransferSMList; }
            set { linkSeqTransferSMList = value; }
        }

        private List<LinkProbabilityTransferSM> linkProbabilityTransferSMList;

        public List<LinkProbabilityTransferSM> LinkProbabilityTransferSMList
        {
            get { return linkProbabilityTransferSMList; }
            set { linkProbabilityTransferSMList = value; }
        }
        

    }
}
