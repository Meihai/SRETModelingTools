using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;

namespace ModelingToolsAppWithMVVM.Model
{
   
    public class MessageTreeModel:TreeViewItemModel{

        private Message _message;

    
        public MessageTreeModel(Message message,MessageGroupTreeModel parentMessageGroup)
               :base(parentMessageGroup,false)
        {
              _message=message;
        }


        public Message Message
        {
            get { return _message; }
            set {
                _message = value;
                RaisePropertyChanged(() => Message);
            }
        }

        public string MessageName
        {
            get { return Message.Name; }
        }
    }

        

}
      
