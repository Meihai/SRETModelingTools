using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelingToolsAppWithMVVM.Common;
using ModelingToolsAppWithMVVM.Model;

namespace ModelingToolsAppWithMVVM.Test
{
    /// <summary>
    /// 消息测试数据生成工具类
    /// </summary>
    public static class MessageTestUtil
    {
        public static List<Message> GetInitSendMessageList()
        {
            List<Message> messageList = new List<Message>();
            //添加多个发送报文
            Message tempMessage = getSendMessage1();
            messageList.Add(tempMessage);
            Message alertMessage = getSendMessage2();
            messageList.Add(alertMessage);
            return messageList;
        }

        public static List<Message> GetInitRecvMessageList()
        {
            List<Message> messageList = new List<Message>();
            //添加多个接收报文
            Message tempMessage = getRecvMessage1();
            messageList.Add(tempMessage);
            Message alertMessage = getRecvMessage2();
            messageList.Add(alertMessage);
            return messageList;
        }

        public static MessageGroup GetInitSendMessageGroup() {
            MessageGroup messageGroup = new MessageGroup();
            messageGroup.GroupName = "发送报文";
            messageGroup.MessageList = GetInitSendMessageList();
            return messageGroup;
        }

        public static MessageGroup GetInitRecvMessageGroup() {
            MessageGroup recvMessageGroup = new MessageGroup();
            recvMessageGroup.GroupName = "接收报文";
            recvMessageGroup.MessageList = GetInitRecvMessageList();
            return recvMessageGroup;
        }

        /// <summary>
        /// 添加温度采集发送报文
        /// </summary>
        /// <returns></returns>
        public static Message getSendMessage1() {
            #region 添加发送报文1
            Message message1 = new Message();

            //----------------------------
            //      id = guid.newguid().tostring();
            //name = id;
            //startbit = 0;
            //length = 1;
            //initvalue = "";
            //description = "";
            //scale = 0;
            //unit = "";
            //isbit = false;
            //bitorder = 0;
            //dividedby = 1;
            //offset = 0;
            //iscrc = false;
            //valuetype = valuetypes.hexstring;
            //RelatedValueId = "";
            //InjectedMode = InjectedModeTypes.InjectedToEachOther;
            //IsUsedForParser = false;
            //-----------------------------
            #region 含义字段添加
            //设置第一个字段含义
            DataMeaning dataMeaning1 = new DataMeaning();
            dataMeaning1.Name = "采集地址";
            dataMeaning1.StartBit = 0;
            dataMeaning1.InitValue = "01";
            // dataMeaning1.IsUsedForParser = true;

            //设置第二个字段
            DataMeaning dataMeaning2 = new DataMeaning();
            dataMeaning2.Name = "功能码";
            dataMeaning2.StartBit = 1;
            dataMeaning2.InitValue = "04";
            //dataMeaning2.IsUsedForParser = true;

            //设置第三个字段
            DataMeaning dataMeaning3 = new DataMeaning();
            dataMeaning3.Name = "温度寄存器开始地址";
            dataMeaning3.StartBit = 2;
            dataMeaning3.Length = 2;
            dataMeaning3.InitValue = "0008";

            //设置第四个字段
            DataMeaning dataMeaning4 = new DataMeaning();
            dataMeaning4.Name = "采集寄存器个数";
            dataMeaning4.StartBit = 4;
            dataMeaning4.Length = 2;
            dataMeaning4.InitValue = "0001";

            //设置第五个字段
            DataMeaning dataMeaning5 = new DataMeaning();
            dataMeaning5.Name = "十六位CRC校验码";
            dataMeaning5.StartBit = 6;
            dataMeaning5.Length = 2;
            dataMeaning5.InitValue = "55AA";
            dataMeaning5.IsCrc = true;
            #endregion //含义字段添加
            message1.Add(dataMeaning1);
            message1.Add(dataMeaning2);
            message1.Add(dataMeaning3);
            message1.Add(dataMeaning4);
            message1.Add(dataMeaning5);
            message1.Name = "温度采集报文";
            message1.Type = "发送报文";
            message1.Description = "通过Modbus协议从温度传感器中采集温度数据";
            #endregion //添加报文1
            return message1;

        }


        /// <summary>
        /// 添加开启温度报警发送报文2
        /// </summary>
        /// <returns></returns>
        public static Message getSendMessage2()
        {
            #region 添加温度报警发送报文2
            Message message1 = new Message();

            //----------------------------
            //      id = guid.newguid().tostring();
            //name = id;
            //startbit = 0;
            //length = 1;
            //initvalue = "";
            //description = "";
            //scale = 0;
            //unit = "";
            //isbit = false;
            //bitorder = 0;
            //dividedby = 1;
            //offset = 0;
            //iscrc = false;
            //valuetype = valuetypes.hexstring;
            //RelatedValueId = "";
            //InjectedMode = InjectedModeTypes.InjectedToEachOther;
            //IsUsedForParser = false;
            //-----------------------------
            #region 含义字段添加
            //设置第一个字段含义
            DataMeaning dataMeaning1 = new DataMeaning();
            dataMeaning1.Name = "采集地址";
            dataMeaning1.StartBit = 0;
            dataMeaning1.InitValue = "01";
            // dataMeaning1.IsUsedForParser = true;

            //设置第二个字段
            DataMeaning dataMeaning2 = new DataMeaning();
            dataMeaning2.Name = "功能码";
            dataMeaning2.StartBit = 1;
            dataMeaning2.InitValue = "05";
            //dataMeaning2.IsUsedForParser = true;

            //设置第三个字段
            DataMeaning dataMeaning3 = new DataMeaning();
            dataMeaning3.Name = "温度报警寄存器地址";
            dataMeaning3.StartBit = 2;
            dataMeaning3.Length = 1;
            dataMeaning3.InitValue = "CE";

            //设置第四个字段
            DataMeaning dataMeaning4 = new DataMeaning();
            dataMeaning4.Name = "启动温度报警标志";
            dataMeaning4.StartBit = 3;
            dataMeaning4.Length = 2;
            dataMeaning4.InitValue = "FF00";

            //设置第五个字段
            DataMeaning dataMeaning5 = new DataMeaning();
            dataMeaning5.Name = "十六位CRC校验码";
            dataMeaning5.StartBit = 5;
            dataMeaning5.Length = 2;
            dataMeaning5.InitValue = "55AA";
            dataMeaning5.IsCrc = true;
            #endregion //含义字段添加
            message1.Add(dataMeaning1);
            message1.Add(dataMeaning2);
            message1.Add(dataMeaning3);
            message1.Add(dataMeaning4);
            message1.Add(dataMeaning5);
            message1.Name = "温度报警报文";
            message1.Type = "发送报文";
            #endregion //添加报文1
            return message1;
        }

        /// <summary>
        /// 添加温度采集的接收报文
        /// </summary>
        /// <returns></returns>
        public static Message getRecvMessage1()
        {
            #region 添加温度采集接收报文1
            Message message1 = new Message();

            //----------------------------
            //      id = guid.newguid().tostring();
            //name = id;
            //startbit = 0;
            //length = 1;
            //initvalue = "";
            //description = "";
            //scale = 0;
            //unit = "";
            //isbit = false;
            //bitorder = 0;
            //dividedby = 1;
            //offset = 0;
            //iscrc = false;
            //valuetype = valuetypes.hexstring;
            //RelatedValueId = "";
            //InjectedMode = InjectedModeTypes.InjectedToEachOther;
            //IsUsedForParser = false;
            //-----------------------------
            #region 含义字段添加
            //设置第一个字段含义
            DataMeaning dataMeaning1 = new DataMeaning();
            dataMeaning1.Name = "采集地址";
            dataMeaning1.StartBit = 0;
            dataMeaning1.InitValue = "01";
            dataMeaning1.IsUsedForParser = true;

            //设置第二个字段
            DataMeaning dataMeaning2 = new DataMeaning();
            dataMeaning2.Name = "功能码";
            dataMeaning2.StartBit = 1;
            dataMeaning2.InitValue = "04";
            dataMeaning2.IsUsedForParser = true;

            //设置第三个字段
            DataMeaning dataMeaning3 = new DataMeaning();
            dataMeaning3.Name = "返回参数个数";
            dataMeaning3.StartBit = 2;
            dataMeaning3.Length = 1;
            dataMeaning3.InitValue = "01";
            dataMeaning1.IsUsedForParser = true;

            //设置第四个字段
            DataMeaning dataMeaning4 = new DataMeaning();
            dataMeaning4.Name = "温度数值";
            dataMeaning4.StartBit = 3;
            dataMeaning4.Length = 2;
            dataMeaning4.InitValue = "0000";
            

            //设置第五个字段
            DataMeaning dataMeaning5 = new DataMeaning();
            dataMeaning5.Name = "十六位CRC校验码";
            dataMeaning5.StartBit = 5;
            dataMeaning5.Length = 2;
            dataMeaning5.InitValue = "55AA";
            dataMeaning5.IsCrc = true;
            #endregion //含义字段添加
            message1.Add(dataMeaning1);
            message1.Add(dataMeaning2);
            message1.Add(dataMeaning3);
            message1.Add(dataMeaning4);
            message1.Add(dataMeaning5);
            message1.Name = "温度采集回应报文";
            message1.Type = "接收报文";
            #endregion //添加报文1
            return message1;
        }

        /// <summary>
        /// 添加温度报警回应报文2
        /// </summary>
        /// <returns></returns>
        public static Message getRecvMessage2()
        {
            #region 添加温度报警回应报文2
            Message message1 = new Message();                   
      
            #region 含义字段添加
            //设置第一个字段含义
            DataMeaning dataMeaning1 = new DataMeaning();
            dataMeaning1.Name = "采集地址";
            dataMeaning1.StartBit = 0;
            dataMeaning1.InitValue = "01";
            dataMeaning1.IsUsedForParser = true;

            //设置第二个字段
            DataMeaning dataMeaning2 = new DataMeaning();
            dataMeaning2.Name = "功能码";
            dataMeaning2.StartBit = 1;
            dataMeaning2.InitValue = "05";
            dataMeaning2.IsUsedForParser = true;

            //设置第三个字段
            DataMeaning dataMeaning3 = new DataMeaning();
            dataMeaning3.Name = "温度报警寄存器地址";
            dataMeaning3.StartBit = 2;
            dataMeaning3.Length = 1;
            dataMeaning3.InitValue = "CE";
            dataMeaning3.IsUsedForParser = true;

            //设置第四个字段
            DataMeaning dataMeaning4 = new DataMeaning();
            dataMeaning4.Name = "启动温度报警标志";
            dataMeaning4.StartBit = 3;
            dataMeaning4.Length = 2;
            dataMeaning4.InitValue = "FF00";
            dataMeaning4.IsUsedForParser = true;

            //设置第五个字段
            DataMeaning dataMeaning5 = new DataMeaning();
            dataMeaning5.Name = "十六位CRC校验码";
            dataMeaning5.StartBit = 5;
            dataMeaning5.Length = 2;
            dataMeaning5.InitValue = "55AA";
            dataMeaning5.IsCrc = true;
            #endregion //含义字段添加
            message1.Add(dataMeaning1);
            message1.Add(dataMeaning2);
            message1.Add(dataMeaning3);
            message1.Add(dataMeaning4);
            message1.Add(dataMeaning5);
            message1.Name = "温度报警接收报文";
            message1.Type = "接收报文";
            #endregion //添加报文1
            return message1;
        }
    }
}
