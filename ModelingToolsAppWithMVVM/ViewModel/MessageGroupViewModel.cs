using GalaSoft.MvvmLight;
using ModelingToolsAppWithMVVM.Model;
using ModelingToolsAppWithMVVM.Common;
using GalaSoft.MvvmLight.CommandWpf;
using System.Windows;
using System.Collections.Generic;
using ModelingToolsAppWithMVVM.View;
using GalaSoft.MvvmLight.Ioc;

namespace ModelingToolsAppWithMVVM.ViewModel
{
   /// <summary>
   /// 添加增加消息委托事件
   /// </summary>
   /// <param name="message"></param>
    public delegate void delAddMessage(string groupName,Message message);
    public delegate void delDeleteMessage(string groupName,Message message);
    public delegate void delAddGroup(MessageGroup mGroup);
    public delegate void delDeleteGroup(MessageGroup mGroup);

    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MessageGroupViewModel : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the MessageGroupViewModel class.
        /// </summary>
        [PreferredConstructorAttribute]
        public MessageGroupViewModel()
        {
            MessageGroup = new MessageGroup();
        }


        public MessageGroupViewModel(MessageGroup msgGroup)
        {
            MessageGroup = msgGroup;
        }
        

        public event delAddMessage evtAddMessage;
        public event delDeleteMessage evtDeleteMessage;
        public event delAddGroup evtAddGroup;
        public event delDeleteGroup evtDeleteGroup;


        private MessageGroup _messageGroup;
        public MessageGroup MessageGroup
        {
            get { return _messageGroup; }
            set { _messageGroup = value;
                  RaisePropertyChanged(() => MessageGroup);
            }
        }

        public int MessageCount
        {
            get {
                    if (null != _messageGroup && null!=_messageGroup.MessageList)
                    {
                        return _messageGroup.MessageList.Count;
                    }
                    else
                   {
                       return 0;
                   }
                }
        }

        public string GroupName
        {
            get {
                if (null != _messageGroup) {
                    return _messageGroup.GroupName;
                }
                else
                {
                    return "";
                }
            }
            set
            {
                if (null != _messageGroup)
                {
                    _messageGroup.GroupName = value;
                    RaisePropertyChanged(() => GroupName);
                }
            }
        }

       
        public string SelectedMsgName
        {
            get { return _messageGroup.SelectedMsgName; }
            set
            {
                _messageGroup.SelectedMsgName = value;
                RaisePropertyChanged(() => SelectedMsgName);

            }
        }

        /// <summary>
        /// 报文列表字典
        /// </summary>
        public Dictionary<string, string> MsgsWithCaptions
        {
            get
            {
                var r = new Dictionary<string, string>();
                if (MessageGroup != null)
                {
                    foreach (Message item in MessageGroup.MessageList)
                    {
                        r.Add(item.Name, item.Name);
                    }
                }
               
                return r;
            }
        }



        private RelayCommand _addMessage;
        private RelayCommand _delMessage;
        private RelayCommand _addGroup;
        private RelayCommand _delGroup;


        /// <summary>
        /// 添加报文
        /// </summary>
        public RelayCommand AddMessage
        {
            get
            {
                if (_addMessage == null)
                {
                    return new RelayCommand(() => CmdAddMessageExecute());
                }
                return _addMessage;
            }
            set
            {
                _addMessage = value;
            }
        }

        private void CmdAddMessageExecute()
        {
           // MessageBox.Show("暂未实现");          
            Message insMessage = new Message();
            insMessage.Type = GroupName;
            MessageView messageWindow = new MessageView();
            messageWindow.DataContext = new MessageViewModel(insMessage);
            messageWindow.evtMsgSave += evtMsgSave;
            messageWindow.ShowDialog();           
        }


        private void evtMsgSave(object sender,Message msg)
        {
              if (null != evtAddMessage)
              {
                  
                  evtAddMessage(GroupName, msg);
              }           
        }


        /// <summary>
        /// 删除报文
        /// </summary>
        public RelayCommand DelMessage
        {
            get
            {
                if (_delMessage == null)
                {
                    return new RelayCommand(() => CmdDelMessageExecute());
                }
                return _delMessage;
            }
            set
            {
                _delMessage = value;
            }
        }

        private void CmdDelMessageExecute()
        {
            if (null != evtDeleteMessage)
            {
                if (SelectedMsgName == null)
                {
                    MessageBox.Show("未选择要删除报文");
                    return;
                }
               Message insMessage=new Message();
               insMessage.Name=SelectedMsgName;
               evtDeleteMessage(GroupName, insMessage);
            }
        }


        /// <summary>
        /// 添加报文分组
        /// </summary>
        public RelayCommand AddGroup
        {
            get
            {
                if (_addGroup == null)
                {
                    return new RelayCommand(() => CmdAddGroupExecute());
                }
                return _addGroup;
            }
            set
            {
                _addGroup = value;
            }
        }

        private void CmdAddGroupExecute()
        {
            MessageGroupAddView msgGroupAddView=new MessageGroupAddView();
            
            msgGroupAddView.evtMessageGroupChange+=EvtMessageGroupChange;
            msgGroupAddView.ShowDialog();
           
        }


        private void EvtMessageGroupChange(object sender,MessageGroup msgGroup){
            if (null != evtAddGroup)
            {
                evtAddGroup(msgGroup);
            }
        }
        /// <summary>
        /// 删除报文分组
        /// </summary>
        public RelayCommand DelGroup
        {
            get
            {
                if (_delGroup == null)
                {
                    return new RelayCommand(() => CmdDelGroupExecute());
                }
                return _delGroup;
            }
            set
            {
                _delGroup = value;
            }
        }

        private void CmdDelGroupExecute()
        {
           // MessageBox.Show("暂未实现");
            if (null != evtDeleteGroup)
            {
                if (GroupName == null)
                {
                    MessageBox.Show("未选择报文组");
                    return;
                }
                evtDeleteGroup(MessageGroup);
            }

        }


    }
}