using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelingToolsAppWithMVVM.Common;
using System.ComponentModel;

namespace ModelingToolsAppWithMVVM.Model
{
    /// <summary>
    /// 接口配置类
    /// </summary>
    public class InterfaceConfig:ObservableObject
    {
        private string id;
        private string name;       
        private InterfaceTypes type;       
        private ProtocolTypes protocol;      
        private string serialPort;       
        private BaudRateTypes baudRate;      
        private int startBit;      
        private StopBitTypes stopBit;      
        private DataBitTypes dataBit;       
        private DataParityTypes parity;   //奇校验、偶校验、None    
        private string ip;      
        private int port;
        private IOTypes ioType;
        private List<Message> relatedMessages;

              
        public InterfaceConfig()
        {
            Id = Guid.NewGuid().ToString();
            Name = "交互接口示例";
            Type = InterfaceTypes.RS485TransparentConnect;
            Protocol = ProtocolTypes.UDP;
            SerialPort = "COM3";
            BaudRate = BaudRateTypes.b9600;
            StartBit = 1;
            StopBit = StopBitTypes.BIT_1;
            DataBit = DataBitTypes.BIT_8;
            Parity = DataParityTypes.NONE;
            Ip = "127.0.0.1";
            Port = 6001;
            IoType = IOTypes.INOUT;


        }


        public string Id
        {
            get { return id; }
            set {
                id = value;
                RaisePropertyChanged(() => Id);
            }
        }

        public string Name
        {
           
            get { return name; }
            set { name = value;
                  RaisePropertyChanged(() => Name);
            }
        }

        public InterfaceTypes Type
        {
            get { return type; }
            set { type = value;
                  RaisePropertyChanged(() => Type);
            }
        }

        public ProtocolTypes Protocol
        {
            get { return protocol; }
            set {
                  protocol = value;
                  RaisePropertyChanged(() => Protocol);
            }
        }

        public string SerialPort
        {
            get { return serialPort; }
            set { serialPort = value;
                  RaisePropertyChanged(() => SerialPort);
            }
        }

        public BaudRateTypes BaudRate
        {
            get { return baudRate; }
            set { baudRate = value;
                  RaisePropertyChanged(() => BaudRate);
            }
        }

        public int StartBit
        {
            get { return startBit; }
            set { startBit = value;
                  RaisePropertyChanged(() => StartBit);
            }
        }

        public StopBitTypes StopBit
        {
            get { return stopBit; }
            set {
                stopBit = value;
                RaisePropertyChanged(() => StopBit);
            }
        }

        public DataBitTypes DataBit
        {
            get { return dataBit; }
            set { 
                dataBit = value;
                RaisePropertyChanged(() => DataBit);
            }
        }

        public DataParityTypes Parity
        {
            get { return parity; }
            set {
                parity = value;
                RaisePropertyChanged(() => Parity);
            }
        }

        public string Ip
        {
            get { return ip; }
            set { 
                ip = value;
                RaisePropertyChanged(() => Ip);
            }
        }

        public int Port
        {
            get { return port; }
            set { 
                port = value;
                RaisePropertyChanged(() => Port);
            }
        }

        public IOTypes IoType
        {
            get { return ioType; }
            set { 
                ioType = value;
                RaisePropertyChanged(() => IoType);
            }
        }

        public List<Message> RelatedMessages
        {
            get { return relatedMessages; }
            set { 
                relatedMessages = value;
                RaisePropertyChanged(() => RelatedMessages);
            }
        }

       
        /// <summary>
        /// 接口类型字典
        /// </summary>
        public Dictionary<InterfaceTypes, string> InterfaceTypesWithCaptions {
          
            get
            {
                var r = new Dictionary<InterfaceTypes, string>();
                foreach(InterfaceTypes item in Enum.GetValues(typeof(InterfaceTypes))) {
                    r.Add(item, EnumHelper.GetDescription(item));
                }
                return r;   
            }
        }

        /// <summary>
        /// 协议类型字典
        /// </summary>
        public Dictionary<ProtocolTypes, string> ProtocolWithCaption
        {
            get
            {
                var r = new Dictionary<ProtocolTypes, string>();
                foreach (ProtocolTypes item in Enum.GetValues(typeof(ProtocolTypes))) {
                    r.Add(item, EnumHelper.GetDescription(item));
                }
                return r;
            }
        }

        public Dictionary<DataParityTypes, string> DataParityWithCaption
        {
            get
            {
                var r = new Dictionary<DataParityTypes, string>();
                foreach (DataParityTypes item in Enum.GetValues(typeof(DataParityTypes)))
                {
                    r.Add(item, EnumHelper.GetDescription(item));
                }
                return r;
            }
        }

        public Dictionary<BaudRateTypes, string> BaudRateWithCaption
        {
            get
            {
                var r = new Dictionary<BaudRateTypes, string>();
                foreach (BaudRateTypes item in Enum.GetValues(typeof(BaudRateTypes)))
                {
                    r.Add(item, EnumHelper.GetDescription(item));
                }
                return r;
            }
        }

        public Dictionary<StopBitTypes, string> StopBitWithCaption
        {
            get
            {
                var r = new Dictionary<StopBitTypes, string>();
                foreach (StopBitTypes item in Enum.GetValues(typeof(StopBitTypes)))
                {
                    r.Add(item, EnumHelper.GetDescription(item));
                }
                return r;
            }
        }

        public Dictionary<DataBitTypes, string> DataBitWithCaption
        {
            get
            {
                var r = new Dictionary<DataBitTypes, string>();
                foreach (DataBitTypes item in Enum.GetValues(typeof(DataBitTypes)))
                {
                    r.Add(item, EnumHelper.GetDescription(item));
                }
                return r;
            }
        }
       
   }
}
