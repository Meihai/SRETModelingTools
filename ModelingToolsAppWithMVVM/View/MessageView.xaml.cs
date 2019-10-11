using System.Windows;
using System.Collections.ObjectModel;
using ModelingToolsAppWithMVVM.Common;
using ModelingToolsAppWithMVVM.Test;
using ModelingToolsAppWithMVVM.Model;
using ModelingToolsAppWithMVVM.ViewModel;
namespace ModelingToolsAppWithMVVM.View
{
    /// <summary>
    /// Description for MessageView.
    /// </summary>
    public partial class MessageView : Window
    {
        /// <summary>
        /// Initializes a new instance of the MessageView class.
        /// </summary>
        public MessageView()
        {
            InitializeComponent();
            this.DataContext = new MessageViewModel(new Message());
           
        }

        /// <summary>
        /// 消息变化改变事件
        /// </summary>
        public event MessageChangeEventHandler evtMsgSave;

        private void Click_SaveMessage(object sender, RoutedEventArgs e)
        {
            if (null != evtMsgSave)
            {
                MessageViewModel msgVM=(MessageViewModel)this.DataContext;
                evtMsgSave(this,msgVM.Message);
            }
            this.Close();
        }
        
      

        private void Click_CancelMessage(object sender, RoutedEventArgs e)
        {
            this.Close();

        }      
       

    }

    public delegate void MessageChangeEventHandler(object sender,Message msg);
}