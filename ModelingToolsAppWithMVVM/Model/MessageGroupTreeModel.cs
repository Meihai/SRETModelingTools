using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;

namespace ModelingToolsAppWithMVVM.Model
{
    public class MessageGroupTreeModel : TreeViewItemModel
    {
        private MessageGroup _messageGroup;

     
        public MessageGroupTreeModel(MessageGroup messageGroup)
            : base(null, false)
        {
            _messageGroup = messageGroup;
            LoadChildren();
        }

        public MessageGroup MessageGroup
        {
            get { return _messageGroup; }
            set {
                _messageGroup = value;
                RaisePropertyChanged(() => MessageGroup);
                Refresh();
            }
        }

        public string MessageGroupName
        {
            get { return MessageGroup.GroupName; }
        }

        public void Refresh()
        {
            base.Children.Clear();
            LoadChildren();
        }

        protected override void LoadChildren()
        {
            foreach (Message message in _messageGroup.MessageList)
            {
                base.Children.Add(new MessageTreeModel(message,this));
                //如果有委托事件呢?惰性加载需要重新加载委托事件
            }
           
        }
    }
}
