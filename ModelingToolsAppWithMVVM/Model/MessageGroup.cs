using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelingToolsAppWithMVVM.Model
{
    /// <summary>
    /// 报文分组
    /// </summary>
    public class MessageGroup
    {

        public MessageGroup()
        {
            groupName = "";
            messageList = new List<Message>();
        }

        public MessageGroup(string groupName,List<Message> msgList){
            GroupName=groupName;
            MessageList=msgList;
        }
        
        private string groupName;

        public string GroupName
        {
            get { return groupName; }
            set { groupName = value; }
        }
        private List<Message> messageList;

        public List<Message> MessageList
        {
            get { return messageList; }
            set { messageList = value; }
        }


        private string selectedMsgName;

        public string SelectedMsgName {
            get
            {
                return selectedMsgName;
            }
            set
            {
                selectedMsgName = value;
            }
        }

        /// <summary>
        /// 添加一个报文
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public bool AddMessage(Message msg)
        {
            bool isAdd = false;
            if (MessageList != null)
            {
                bool isExist=false;
                for (int i = 0; i < MessageList.Count; i++)
                {
                    if (MessageList[i].Name == msg.Name)
                    {
                        isExist = true;
                    }
                }
                if (!isExist)
                {
                    MessageList.Add(msg);
                   // MessageListCollectionChanged();
                    isAdd = true;
                }
            }
            return isAdd;
        }

        private void MessageListCollectionChanged()
        {
            List<Message> tmpMsgList = new List<Message>();
            for (int i = 0; i < MessageList.Count; i++)
            {
                tmpMsgList.Add(MessageList[i]);
            }
            MessageList = tmpMsgList;   
        }

        /// <summary>
        /// 删除一个报文
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public bool DeleteMessage(Message msg)
        {
            bool isDelete = false;

            if (MessageList != null)
            {
                for (int i = 0; i < MessageList.Count; i++)
                {
                    if (MessageList[i].Name == msg.Name)
                    {
                        MessageList.RemoveAt(i);
                        isDelete = true;
                        break;
                    }
                }
            }

            return isDelete;
        }

    }
}
