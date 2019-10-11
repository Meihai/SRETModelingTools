using GalaSoft.MvvmLight;
using ModelingToolsAppWithMVVM.Model;
using System.Collections.Generic;
using System.Windows.Input;
using ModelingToolsAppWithMVVM.View;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Windows.Controls;
using System.Windows;

namespace ModelingToolsAppWithMVVM.ViewModel
{
    public delegate void delRefreshDataContext();
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MessageManagerViewModel : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the MessageManagerViewModel class.
        /// </summary>
        public MessageManagerViewModel()
        {
            init();

        }

        public event delRefreshDataContext evtRefreshDataContext;

        private void init()
        {
            messageManager = new MessageManagerModel();
            AddSwitchViewEvents();
            _view1 = new MessageGroupUserControlView();
            if (messageManager != null && messageManager.MessageGroups != null && messageManager.MessageGroups.Count>0)
            {
                messageManager.MessageGroups[0].IsSelected = true;
                if(messageManager.MessageGroups[0].MessageGroup.MessageList!=null &&
                    messageManager.MessageGroups[0].MessageGroup.MessageList.Count> 0)
                {
                    messageManager.MessageGroups[0].MessageGroup.SelectedMsgName
                        = messageManager.MessageGroups[0].MessageGroup.MessageList[0].Name;
                }
                MessageGroupViewModel insMgVM=new MessageGroupViewModel(messageManager.MessageGroups[0].MessageGroup);
                insMgVM.evtAddMessage += evtAddMessage;
                insMgVM.evtDeleteMessage += evtDeleteMessage;
                insMgVM.evtAddGroup += evtAddGroup;
                insMgVM.evtDeleteGroup += evtDeleteGroup;
                ((UserControl)_view1).DataContext = insMgVM;

            }            

            _view2 = new MessageUserControlView();
            ((UserControl)_view2).DataContext = new MessageViewModel();

            CurrentView = _view1;
        }

        private MessageManagerModel messageManager;

        public MessageManagerModel MessageManager
        {
            get { return messageManager; }
            set { 
                  messageManager = value;
                  AddSwitchViewEvents();
                  RaisePropertyChanged(() => MessageManager);                 
            }
        }

        private void AddSwitchViewEvents(){
            if(null!=messageManager){
                List<MessageGroupTreeModel> groupTreeModels = messageManager.MessageGroups;
                for (int i = 0; i < groupTreeModels.Count; i++)
                {
                    groupTreeModels[i].evtViewSwitch += SwitchView;
                    groupTreeModels[i].Refresh();
                    if (groupTreeModels[i].IsSelected)
                    {
                        var dataContext = ((UserControl)CurrentView).DataContext;
                        ((UserControl)CurrentView).DataContext = null;
                        ((UserControl)CurrentView).DataContext = dataContext;
                    }
                    List<TreeViewItemModel> messageTreeModels = groupTreeModels[i].Children;
                    for (int j = 0; j < messageTreeModels.Count; j++)
                    {
                        messageTreeModels[j].evtViewSwitch += SwitchView;
                        if (messageTreeModels[j].IsSelected)
                        {
                             //重新赋值DataContext
                            var dataContext = ((UserControl)CurrentView).DataContext;
                            ((UserControl)CurrentView).DataContext = null;
                            ((UserControl)CurrentView).DataContext = dataContext;
                        }
                    }
                }
            }
        }


        private object _currentView;
        private object _view1;
        private object _view2;         

        public object CurrentView
        {
             get { return _currentView; }
             set
             {
                 _currentView = value;
                 RaisePropertyChanged(() => CurrentView);
                
             }
        }

        private void SwitchView(Object sender)
        {
            if (sender is MessageGroupTreeModel)
            {           
                MessageGroupViewModel insMgVM = new MessageGroupViewModel(((MessageGroupTreeModel)sender).MessageGroup);
                insMgVM.evtAddMessage += evtAddMessage;
                insMgVM.evtDeleteMessage += evtDeleteMessage;
                insMgVM.evtAddGroup += evtAddGroup;
                insMgVM.evtDeleteGroup += evtDeleteGroup;
                _view1 = new MessageGroupUserControlView();
                ((UserControl)_view1).DataContext = insMgVM;
                CurrentView = _view1;
            }
            else if (sender is MessageTreeModel)
            {               
                MessageViewModel insMsgVM = new MessageViewModel(((MessageTreeModel)sender).Message);
                _view2 = new MessageUserControlView();
                ((UserControl)_view2).DataContext = insMsgVM;
                CurrentView = _view2;
            }
            else
            {
                CurrentView = null;
            }
        }   
    

      
        /// <summary>
        /// 添加报文到报文分组中
        /// </summary>
        /// <param name="groupName"></param>
        /// <param name="msg"></param>
        private void evtAddMessage(string groupName, Message msg)
        {
            bool isAdd=messageManager.AddMessageToGroup(groupName, msg);
            if (!isAdd)
            {
                string msgStr = "添加消息【" + msg.Name + "】到组【" + groupName + "】中失败";
                MessageBox.Show(msgStr);
            }

            AddSwitchViewEvents();
            if (null != evtRefreshDataContext)
            {
                evtRefreshDataContext();
            }
          
        }

        private void evtDeleteMessage(string groupName, Message msg)
        {
            bool isDelete=messageManager.DeleteMessageFromGroup(groupName, msg);
            if (!isDelete)
            {
                string msgStr = "从组【" + groupName + "】中删除消息【" + msg.Name + "】失败";
                MessageBox.Show(msgStr);
            }
            AddSwitchViewEvents();
            if (null != evtRefreshDataContext)
            {
                evtRefreshDataContext();
            }
        }

        private void evtAddGroup(MessageGroup mGroup)
        {
            MessageGroupTreeModel groupTreeModel = new MessageGroupTreeModel(mGroup);
            bool isAdd=messageManager.AddMessageGroup(groupTreeModel);
            if (!isAdd)
            {
                string msgStr="添加组【"+mGroup.GroupName+"】失败";
                MessageBox.Show(msgStr);
            }
            AddSwitchViewEvents();
            if (null != evtRefreshDataContext)
            {
                evtRefreshDataContext();
            }
        }

        private void evtDeleteGroup(MessageGroup mGroup)
        {
            bool isDelete=messageManager.DeleteMessageGroup(mGroup.GroupName);
            if (!isDelete)
            {
                string msgStr = "删除组【" + mGroup.GroupName + "】失败";
                MessageBox.Show(msgStr);
            }
            AddSwitchViewEvents();
            if (null != evtRefreshDataContext)
            {
                evtRefreshDataContext();
            }
        }
   
     
 
       
    }
}