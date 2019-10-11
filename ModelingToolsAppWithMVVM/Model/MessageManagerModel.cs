using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using ModelingToolsAppWithMVVM.Test;

namespace ModelingToolsAppWithMVVM.Model
{
    public class MessageManagerModel:ObservableObject
    {

        public MessageManagerModel() {
            initMessageGroups();
        }

        private void initMessageGroups()
        {
            List<MessageGroupTreeModel> groupTreeModels = new List<MessageGroupTreeModel>();
            MessageGroup sendGroup = MessageTestUtil.GetInitSendMessageGroup();
            MessageGroup recvGroup = MessageTestUtil.GetInitRecvMessageGroup();
            MessageGroupTreeModel sendGroupTreeModel = new MessageGroupTreeModel(sendGroup);
            MessageGroupTreeModel recvGroupTreeModel = new MessageGroupTreeModel(recvGroup);
            groupTreeModels.Add(sendGroupTreeModel);
            groupTreeModels.Add(recvGroupTreeModel);
            MessageGroups = groupTreeModels;
        }

        /// <summary>
        /// 添加一个报文分组
        /// </summary>
        /// <param name="mGroup">报文分组</param>
        public bool AddMessageGroup(MessageGroupTreeModel mGroup)
        {
            bool isAdd = false;
            if (MessageGroups != null)
            {
                bool isRepeat=false;
                for (int i = 0; i < MessageGroups.Count; i++)
                {
                    if (MessageGroups[i].MessageGroup.GroupName == mGroup.MessageGroup.GroupName)
                    {
                        isRepeat = true;
                        break;
                    }
                }
                if (!isRepeat)
                {
                    MessageGroups.Add(mGroup);
                    MsgGroupCollectionChanged();
                    isAdd = true;                    
                }
            }
            return isAdd;
        }

        private void MsgGroupCollectionChanged()
        {
            List<MessageGroupTreeModel> groupTreeModels = new List<MessageGroupTreeModel>();
            for (int i = 0; i < MessageGroups.Count; i++)
            {
                groupTreeModels.Add(MessageGroups[i]);
            }
            MessageGroups = groupTreeModels;
        }

        /// <summary>
        /// 删除一个报文分组
        /// </summary>
        /// <param name="groupName"></param>
        /// <returns></returns>
        public bool DeleteMessageGroup(string groupName)
        {
            bool isDelete = false;
            if (MessageGroups != null)
            {
                for (int i = 0; i < MessageGroups.Count; i++)
                {
                    if (MessageGroups[i].MessageGroup.GroupName == groupName)
                    {
                        MessageGroups.RemoveAt(i);
                        isDelete = true;
                        MsgGroupCollectionChanged();
                        break;
                    }
                }
            }

            return isDelete;
        }
        /// <summary>
        ///  给报文分组添加一个报文
        /// </summary>
        /// <param name="groupName"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public bool AddMessageToGroup(string groupName,Message message)
        {
            bool isAdd = false;
            if (MessageGroups != null)
            {
                for (int i = 0; i < MessageGroups.Count; i++)
                {
                    if (MessageGroups[i].MessageGroup.GroupName == groupName)
                    {
                        isAdd = MessageGroups[i].MessageGroup.AddMessage(message);
                        MsgGroupCollectionChanged();
                        break;
                    }
                }
            }
            return isAdd;
        }


        /// <summary>
        /// 从报文分组中删除一个报文
        /// </summary>
        /// <param name="groupName"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public bool DeleteMessageFromGroup(string groupName, Message message)
        {
            bool isDelete = false;
            if (MessageGroups != null)
            {
                for (int i = 0; i < MessageGroups.Count; i++)
                {
                    if (MessageGroups[i].MessageGroup.GroupName == groupName)
                    {
                        isDelete = MessageGroups[i].MessageGroup.DeleteMessage(message);
                        MsgGroupCollectionChanged();
                        break;
                    }
                }
            }
            return isDelete;
        }
       
     

        private List<MessageGroupTreeModel> messageGroups;

        public List<MessageGroupTreeModel> MessageGroups
        {
            get { return messageGroups; }
            set { messageGroups = value;
                  RaisePropertyChanged(() => MessageGroups);
            }
        }

    


    }
}
